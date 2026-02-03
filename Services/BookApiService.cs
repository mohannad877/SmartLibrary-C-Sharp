using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using smartLibraryForC_.Models;
using System.Text.RegularExpressions;

namespace smartLibraryForC_.Services
{
    /// <summary>
    /// خدمة الاتصال بواجهة برمجة الكتب (Open Library API)
    /// مع نظام بديل متعدد المصادر للتحميل
    /// </summary>
    public class BookApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://openlibrary.org";
        private const string GutenbergBaseUrl = "https://www.gutenberg.org";

        public BookApiService()
        {
            // Ensure TLS 1.2/1.1/1.0 is used for secure connections
            System.Net.ServicePointManager.SecurityProtocol = 
                System.Net.SecurityProtocolType.Tls12 | 
                System.Net.SecurityProtocolType.Tls11 | 
                System.Net.SecurityProtocolType.Tls;

            // Create HttpClientHandler to enable proxy support (for VPN compatibility)
            var handler = new HttpClientHandler
            {
                UseProxy = true,
                Proxy = System.Net.WebRequest.GetSystemWebProxy(),
                UseDefaultCredentials = true,
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "SmartLibraryApp/1.0 (Contact: developer@smartlibrary.app)");
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }

        /// <summary>
        /// البحث عن الكتب حسب العنوان أو المؤلف
        /// </summary>
        public async Task<List<Book>> SearchBooksAsync(string query, int limit = 20)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("كلمة البحث لا يمكن أن تكون فارغة", nameof(query));
            }

            var formattedQuery = Uri.EscapeDataString(query);
            var url = $"{BaseUrl}/search.json?q={formattedQuery}&limit={limit}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    // إذا فشل Open Library، جرب مصادر بديلة
                    System.Diagnostics.Debug.WriteLine($"Open Library returned: {(int)response.StatusCode} {response.ReasonPhrase}");
                    return await SearchBooksFromAlternativeSources(query, limit);
                }

                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                
                var result = JsonSerializer.Deserialize<OpenLibrarySearchResult>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var books = new List<Book>();

                if (result?.Docs != null)
                {
                    foreach (var doc in result.Docs)
                    {
                        var book = await CreateBookFromDoc(doc, query);
                        books.Add(book);
                    }
                }

                // إذا لم توجد نتائج، جرب المصادر البديلة
                if (books.Count == 0)
                {
                    return await SearchBooksFromAlternativeSources(query, limit);
                }

                return books;
            }
            catch (HttpRequestException httpEx)
            {
                System.Diagnostics.Debug.WriteLine($"Network error searching books: {httpEx.Message}");
                // جرب المصادر البديلة في حالة خطأ الشبكة
                return await SearchBooksFromAlternativeSources(query, limit);
            }
            catch (TaskCanceledException tcEx)
            {
                System.Diagnostics.Debug.WriteLine($"Timeout searching books: {tcEx.Message}");
                return await SearchBooksFromAlternativeSources(query, limit);
            }
        }

        /// <summary>
        /// البحث عن الكتب من مصادر بديلة عند فشل المصدر الرئيسي
        /// </summary>
        private async Task<List<Book>> SearchBooksFromAlternativeSources(string query, int limit)
        {
            var books = new List<Book>();

            // البحث في Project Gutenberg للكتب العربية والعالمية
            await Task.Run(() =>
            {
                var gutenbergBooks = GetGutenbergBooks(query);
                foreach (var book in gutenbergBooks)
                {
                    if (books.Count < limit)
                    {
                        books.Add(book);
                    }
                }
            });

            // إذا لم توجد نتائج من Project Gutenberg، أضف كتباً عامة
            if (books.Count == 0)
            {
                books.AddRange(GetPublicDomainBooks(query));
            }

            return books;
        }

        /// <summary>
        /// الحصول على كتب من Project Gutenberg بروابط PDF مباشرة
        /// </summary>
        private List<Book> GetGutenbergBooks(string query)
        {
            var books = new List<Book>();
            var queryLower = query.ToLower();

            // كتب عربية من Project Gutenberg
            if (queryLower.Contains("ألف ليلة") || queryLower.Contains("الف ليلة") || queryLower.Contains("1001") || queryLower.Contains("alf leyla"))
            {
                books.Add(new Book
                {
                    Title = "ألف ليلة وليلة - المجموعة الكاملة",
                    Author = "مجموعة مؤلفين",
                    Description = "مجموعة من الحكايات الشعبية الشرقية الكلاسيكية",
                    CoverUrl = null,
                    PublishedDate = new DateTime(2020, 1, 1),
                    PageCount = 1200,
                    DownloadUrl = $"{GutenbergBaseUrl}/cache/epub/34377/pg34377.pdf",
                    Isbn = "97834377"
                });
            }

            if ((queryLower.Contains("كليلة") && queryLower.Contains("دمنة")) || queryLower.Contains("kalila wa dimna"))
            {
                books.Add(new Book
                {
                    Title = "كليلة ودمنة",
                    Author = "ابن المقفع",
                    Description = "كتاب عربي قديم يضم حكايات وحكم",
                    CoverUrl = null,
                    PublishedDate = new DateTime(2019, 1, 1),
                    PageCount = 350,
                    DownloadUrl = $"{GutenbergBaseUrl}/cache/epub/22571/pg22571.pdf",
                    Isbn = "97822571"
                });
            }

            // كتب إنجليزية كلاسيكية بروابط PDF مباشرة
            if (queryLower.Contains("alice") || queryLower.Contains("أليس"))
            {
                books.Add(new Book
                {
                    Title = "Alice's Adventures in Wonderland",
                    Author = "Lewis Carroll",
                    Description = "A classic children's novel",
                    CoverUrl = "https://covers.openlibrary.org/b/id/9678273-M.jpg",
                    PublishedDate = new DateTime(1865, 1, 1),
                    PageCount = 200,
                    DownloadUrl = $"{GutenbergBaseUrl}/cache/epub/11/pg11.pdf",
                    Isbn = "97811"
                });
            }

            if (queryLower.Contains("sherlock"))
            {
                books.Add(new Book
                {
                    Title = "The Adventures of Sherlock Holmes",
                    Author = "Arthur Conan Doyle",
                    Description = "A collection of detective stories",
                    CoverUrl = "https://covers.openlibrary.org/b/id/12645123-M.jpg",
                    PublishedDate = new DateTime(1892, 1, 1),
                    PageCount = 350,
                    DownloadUrl = $"{GutenbergBaseUrl}/cache/epub/1661/pg1661.pdf",
                    Isbn = "9781661"
                });
            }

            if (queryLower.Contains("pride") || queryLower.Contains("prejudice"))
            {
                books.Add(new Book
                {
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    Description = "A romantic novel of manners",
                    CoverUrl = "https://covers.openlibrary.org/b/id/12645114-M.jpg",
                    PublishedDate = new DateTime(1813, 1, 1),
                    PageCount = 400,
                    DownloadUrl = $"{GutenbergBaseUrl}/cache/epub/1342/pg1342.pdf",
                    Isbn = "9781342"
                });
            }

            if (queryLower.Contains("moby") || queryLower.Contains("whale"))
            {
                books.Add(new Book
                {
                    Title = "Moby Dick",
                    Author = "Herman Melville",
                    Description = "The story of Captain Ahab's quest",
                    CoverUrl = "https://covers.openlibrary.org/b/id/12645129-M.jpg",
                    PublishedDate = new DateTime(1851, 1, 1),
                    PageCount = 600,
                    DownloadUrl = $"{GutenbergBaseUrl}/cache/epub/2701/pg2701.pdf",
                    Isbn = "9782701"
                });
            }

            if (queryLower.Contains("dracula"))
            {
                books.Add(new Book
                {
                    Title = "Dracula",
                    Author = "Bram Stoker",
                    Description = "The classic vampire novel",
                    CoverUrl = "https://covers.openlibrary.org/b/id/12645117-M.jpg",
                    PublishedDate = new DateTime(1897, 1, 1),
                    PageCount = 400,
                    DownloadUrl = $"{GutenbergBaseUrl}/cache/epub/345/pg345.pdf",
                    Isbn = "978345"
                });
            }

            if (queryLower.Contains("frankenstein"))
            {
                books.Add(new Book
                {
                    Title = "Frankenstein",
                    Author = "Mary Shelley",
                    Description = "The story of Victor Frankenstein",
                    CoverUrl = "https://covers.openlibrary.org/b/id/12645120-M.jpg",
                    PublishedDate = new DateTime(1818, 1, 1),
                    PageCount = 350,
                    DownloadUrl = $"{GutenbergBaseUrl}/cache/epub/84/pg84.pdf",
                    Isbn = "97884"
                });
            }

            if (queryLower.Contains("romeo") && queryLower.Contains("juliet"))
            {
                books.Add(new Book
                {
                    Title = "Romeo and Juliet",
                    Author = "William Shakespeare",
                    Description = "The tragic tale of star-crossed lovers",
                    CoverUrl = "https://covers.openlibrary.org/b/id/12645137-M.jpg",
                    PublishedDate = new DateTime(1597, 1, 1),
                    PageCount = 200,
                    DownloadUrl = $"{GutenbergBaseUrl}/cache/epub/1513/pg1513.pdf",
                    Isbn = "9781513"
                });
            }

            return books;
        }

        /// <summary>
        /// الحصول على كتب domain عام إضافية
        /// </summary>
        private List<Book> GetPublicDomainBooks(string query)
        {
            var books = new List<Book>();
            var queryLower = query.ToLower();

            // أضف المزيد من الكتب هنا عند الحاجة
            
            return books;
        }

        /// <summary>
        /// إنشاء كائن كتاب من مستند Open Library
        /// </summary>
        private async Task<Book> CreateBookFromDoc(OpenLibraryBookDoc doc, string query)
        {
            var book = new Book
            {
                Title = doc.Title ?? "غير معروف",
                Author = doc.AuthorName?.Count > 0 ? string.Join(", ", doc.AuthorName) : "غير معروف",
                Description = doc.FirstSentence?.FirstOrDefault() ?? doc.Subject?.FirstOrDefault() ?? "لا يوجد وصف متاح",
                CoverUrl = doc.CoverI != null ? $"https://covers.openlibrary.org/b/id/{doc.CoverI}-M.jpg" : null,
                PublishedDate = ParseYear(doc.FirstPublishYear),
                PageCount = doc.NumberOfPagesMedian,
                Isbn = doc.Isbn?.FirstOrDefault()
            };

            // إنشاء روابط تحميل بديلة متعددة باستخدام البحث الديناميكي
            book.DownloadUrl = await GetPrimaryDownloadLink(book.Title, doc.Ia, doc.Isbn);

            return book;
        }

        /// <summary>
        /// الحصول على الرابط الأساسي للتحميل (PDF فقط!)
        /// </summary>
        private async Task<string> GetPrimaryDownloadLink(string title, List<string> iaIds, List<string> isbns)
        {
            var sources = await GetDownloadSourcesAsync(title, iaIds, isbns);
            return sources.Count > 0 ? sources[0] : null;
        }

        /// <summary>
        /// الحصول على روابط التحميل السريعة (بدون بحث ديناميكي)
        /// </summary>
        public List<string> GetFastDownloadSources(string title, List<string> iaIds, List<string> isbns = null)
        {
            var sources = new List<string>();
            var titleLower = title?.ToLower() ?? "";

            // 1. روابط ثابتة للكتب الكلاسيكية
            AddHardcodedClassicLinks(titleLower, sources);

            // 2. Internet Archive IDs مباشرة
            if (iaIds != null && iaIds.Count > 0)
            {
                foreach (var iaId in iaIds)
                {
                    if (!string.IsNullOrEmpty(iaId))
                    {
                        sources.Add($"https://archive.org/download/{iaId}/{iaId}.pdf");
                    }
                }
            }

            return sources.Distinct().ToList();
        }

        /// <summary>
        /// الحصول على روابط التحميل مع البحث البديل (async)
        /// </summary>
        public async Task<List<string>> GetDownloadSourcesAsync(string title, List<string> iaIds, List<string> isbns = null)
        {
            var sources = new List<string>();
            var titleLower = title?.ToLower() ?? "";

            // 1. أولاً جرب المصادر الثابتة المعروفة
            AddHardcodedClassicLinks(titleLower, sources);

            // 2. إذا كان هناك Internet Archive ID مباشر
            if (iaIds != null && iaIds.Count > 0)
            {
                foreach (var iaId in iaIds)
                {
                    if (!string.IsNullOrEmpty(iaId))
                    {
                        sources.Add($"https://archive.org/download/{iaId}/{iaId}.pdf");
                    }
                }
            }

            // 3. إذا لم نجد روابط حتى الآن، نبحث ديناميكياً في Internet Archive
            if (sources.Count == 0)
            {
                try
                {
                    var archiveSources = await SearchInternetArchiveForPdf(title);
                    sources.AddRange(archiveSources);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error querying Internet Archive: {ex.Message}");
                }
            }

            // 4. جرب Project Gutenberg بالبحث
            if (sources.Count == 0)
            {
                try
                {
                    var gutenbergSources = await SearchGutenbergForPdf(title);
                    sources.AddRange(gutenbergSources);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error querying Gutenberg: {ex.Message}");
                }
            }

            return sources.Distinct().ToList();
        }

        private void AddHardcodedClassicLinks(string titleLower, List<string> sources)
        {
            // كتب عربية
            if (titleLower.Contains("ألف ليلة") || titleLower.Contains("الف ليلة") || titleLower.Contains("1001"))
            {
                sources.Add($"{GutenbergBaseUrl}/cache/epub/34377/pg34377.pdf");
                sources.Add("https://archive.org/download/1001-nights/1001-nights.pdf");
            }
            
            if (titleLower.Contains("كليلة") && titleLower.Contains("دمنة"))
            {
                sources.Add($"{GutenbergBaseUrl}/cache/epub/22571/pg22571.pdf");
            }
            
            // كتب إنجليزية كلاسيكية
            var classicBooks = new Dictionary<string, string>
            {
                { "alice", "11" },
                { "sherlock", "1661" },
                { "pride", "1342" },
                { "prejudice", "1342" },
                { "moby", "2701" },
                { "whale", "2701" },
                { "dracula", "345" },
                { "frankenstein", "84" },
                { "romeo", "1513" },
                { "juliet", "1513" },
                { "dorian", "174" },
                { "gray", "174" },
                { "hamlet", "1524" },
                { "macbeth", "1539" },
                { "odyssey", "1727" },
                { "iliad", "1728" }
            };
            
            foreach (var keyword in classicBooks.Keys)
            {
                if (titleLower.Contains(keyword))
                {
                    sources.Add($"{GutenbergBaseUrl}/cache/epub/{classicBooks[keyword]}/pg{classicBooks[keyword]}.pdf");
                }
            }
        }

        /// <summary>
        /// البحث في Internet Archive عن ملفات PDF
        /// </summary>
        private async Task<List<string>> SearchInternetArchiveForPdf(string title)
        {
            var sources = new List<string>();
            
            try
            {
                // تنظيف العنوان للبحث
                var cleanTitle = CleanSearchTitle(title);
                
                // بحث مبسط في Internet Archive
                var searchUrl = $"https://archive.org/advancedsearch.php?q={Uri.EscapeDataString(cleanTitle)}+AND+mediatype:(texts)+AND+format:(PDF)&fl[]=identifier&sort[]=downloads+desc&rows=5&output=json";
                
                var response = await _httpClient.GetAsync(searchUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ArchiveSearchResult>(json, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    
                    if (result?.Response?.Docs != null)
                    {
                        foreach (var doc in result.Response.Docs)
                        {
                            if (!string.IsNullOrEmpty(doc.Identifier))
                            {
                                // جرب الصيغ المختلفة
                                sources.Add($"https://archive.org/download/{doc.Identifier}/{doc.Identifier}.pdf");
                                sources.Add($"https://archive.org/download/{doc.Identifier}/{doc.Identifier}_djvu.txt");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Internet Archive search failed: {ex.Message}");
            }
            
            return sources;
        }

        /// <summary>
        /// البحث في Project Gutenberg
        /// </summary>
        private async Task<List<string>> SearchGutenbergForPdf(string title)
        {
            var sources = new List<string>();
            
            try
            {
                var cleanTitle = CleanSearchTitle(title);
                var searchUrl = $"https://www.gutenberg.org/ebooks/search/?query={Uri.EscapeDataString(cleanTitle)}&format=json";
                
                var response = await _httpClient.GetAsync(searchUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    
                    // Project Gutenberg يعيد HTML، لذلك نحتاج لتحليل بسيط
                    if (json.Contains("ebooks"))
                    {
                        // استخراج معرفات الكتب من HTML
                        var matches = System.Text.RegularExpressions.Regex.Matches(
                            json, @"/ebooks/(\d+)");
                        
                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            if (match.Groups.Count > 1)
                            {
                                var bookId = match.Groups[1].Value;
                                sources.Add($"https://www.gutenberg.org/cache/epub/{bookId}/pg{bookId}.pdf");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Gutenberg search failed: {ex.Message}");
            }
            
            return sources;
        }

        /// <summary>
        /// تنظيف العنوان للبحث
        /// </summary>
        private string CleanSearchTitle(string title)
        {
            if (string.IsNullOrEmpty(title)) return "";
            
            // إزالة الكلمات الشائعة غير الضرورية
            var wordsToRemove = new[] { "the", "a", "an", "and", "or", "but", "in", "on", "at", "to", "for", "of", "with", "by" };
            var titleLower = title.ToLower();
            
            foreach (var word in wordsToRemove)
            {
                titleLower = titleLower.Replace($" {word} ", " ");
            }
            
            // إزالة الأحرف الخاصة
            var clean = new string(titleLower
                .Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-')
                .ToArray());
            
            return clean.Trim();
        }

        /// <summary>
        /// البحث عن الكتب حسب المؤلف
        /// </summary>
        public async Task<List<Book>> SearchBooksByAuthorAsync(string authorName, int limit = 20)
        {
            var formattedQuery = Uri.EscapeDataString(authorName);
            var url = $"{BaseUrl}/search.json?author={formattedQuery}&limit={limit}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OpenLibrarySearchResult>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var books = new List<Book>();

            if (result?.Docs != null)
            {
                foreach (var doc in result.Docs)
                {
                    var book = await CreateBookFromDoc(doc, authorName);
                    books.Add(book);
                }
            }

            return books;
        }

        /// <summary>
        /// جلب تفاصيل كتاب محدد
        /// </summary>
        public async Task<Book> GetBookDetailsAsync(string key)
        {
            try
            {
                var url = $"{BaseUrl}/api/books?bibkeys=ISBN:{key}&format=json&jscmd=data";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

                if (data != null && data.TryGetValue($"ISBN:{key}", out var bookData))
                {
                    var bookJson = bookData.GetRawText();
                    var book = JsonSerializer.Deserialize<Book>(bookJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return book;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"خطأ في جلب تفاصيل الكتاب: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// التحقق من صحة رابط التحميل
        /// </summary>
        public async Task<bool> IsDownloadLinkValidAsync(string url)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Head, url))
                using (var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(15)))
                {
                    var response = await _httpClient.SendAsync(request, cts.Token);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                    }

                    // تحقق من نوع المحتوى
                    var contentType = response.Content.Headers.ContentType?.MediaType;
                    return contentType == "application/pdf";
                }

            }
            catch
            {
                return false;
            }
        }

        private DateTime? ParseYear(int? year)
        {
            if (year.HasValue && year.Value > 0)
            {
                return new DateTime(year.Value, 1, 1);
            }
            return null;
        }
    }

    /// <summary>
    /// نموذج نتيجة البحث من Open Library
    /// </summary>
    internal class OpenLibrarySearchResult
    {
        [JsonPropertyName("num_found")]
        public int NumFound { get; set; }
        
        [JsonPropertyName("docs")]
        public List<OpenLibraryBookDoc> Docs { get; set; }
    }

    internal class OpenLibraryBookDoc
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("author_name")]
        public List<string> AuthorName { get; set; }
        
        [JsonPropertyName("first_sentence")]
        public List<string> FirstSentence { get; set; }
        
        [JsonPropertyName("cover_i")]
        public int? CoverI { get; set; }
        
        [JsonPropertyName("first_publish_year")]
        public int? FirstPublishYear { get; set; }
        
        [JsonPropertyName("isbn")]
        public List<string> Isbn { get; set; }
        
        [JsonPropertyName("subject")]
        public List<string> Subject { get; set; }
        
        [JsonPropertyName("number_of_pages_median")]
        public int? NumberOfPagesMedian { get; set; }

        [JsonPropertyName("ia")]
        public List<string> Ia { get; set; }
    }

    /// <summary>
    /// نموذج نتيجة بحث Internet Archive
    /// </summary>
    internal class ArchiveSearchResult
    {
        [JsonPropertyName("response")]
        public ArchiveResponse Response { get; set; }
    }

    internal class ArchiveResponse
    {
        [JsonPropertyName("docs")]
        public List<ArchiveDoc> Docs { get; set; }
    }

    internal class ArchiveDoc
    {
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("mediatype")]
        public string MediaType { get; set; }
    }
}
