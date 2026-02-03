using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace smartLibraryForC_.Services
{
    /// <summary>
    /// خدمة إدارة الملفات المحلية مع دعم التحميل المتعدد المصادر
    /// </summary>
    public class FileService
    {
        private readonly HttpClient _httpClient;
        private readonly string _booksDirectory;

        public FileService()
        {
            // Ensure TLS 1.2 is used for secure connections (required by many modern APIs like archive.org)
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            System.Net.ServicePointManager.DefaultConnectionLimit = 10;

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
                Timeout = TimeSpan.FromMinutes(5) // Increased timeout for larger files
            };
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "SmartLibraryApp/1.0 (Contact: developer@smartlibrary.app)");
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/pdf,application/octet-stream,*/*");

            // مجلد Books في مجلد البيانات المحلي
            _booksDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Books");

            if (!Directory.Exists(_booksDirectory))
            {
                Directory.CreateDirectory(_booksDirectory);
            }
        }

        /// <summary>
        /// تنزيل كتاب مع نظام بديل متعدد المصادر
        /// </summary>
        /// <param name="downloadUrls">قائمة الروابط البديلة للتحميل</param>
        /// <param name="fileName">اسم الملف</param>
        /// <param name="progress">تقدم التحميل</param>
        /// <returns>مسار الملف أو null إذا فشل</returns>
        public async Task<string> DownloadBookWithFallbackAsync(
            List<string> downloadUrls, 
            string fileName, 
            IProgress<string> progress = null)
        {
            if (downloadUrls == null || downloadUrls.Count == 0)
            {
                throw new ArgumentException("لا توجد روابط تحميل متاحة", nameof(downloadUrls));
            }

            // تنظيف اسم الملف
            string safeFileName = SanitizeFileName(fileName);
            if (safeFileName.Length > 100)
            {
                safeFileName = safeFileName.Substring(0, 100) + Path.GetExtension(fileName);
            }

            // جرب كل رابط بالتتابع حتى ينجح أحدها
            var errors = new List<string>();
            
            for (int i = 0; i < downloadUrls.Count; i++)
            {
                var url = downloadUrls[i];
                
                if (progress != null)
                {
                    if (i == 0)
                        progress.Report("جاري محاولة التحميل...");
                    else
                        progress.Report($"المصدر {i} غير متاح، جاري تجربة الرابط التالي...");
                }

                try
                {
                    // التحقق أولاً من صحة الرابط
                    var isValid = await IsValidDownloadUrlAsync(url);
                    if (!isValid)
                    {
                        System.Diagnostics.Debug.WriteLine($"الرابط غير صالح: {url}");
                        errors.Add($"المصدر {i + 1}: الرابط غير صالح");
                        continue;
                    }

                    // محاولة التحميل
                    // Create an adapter to report progress if available
                    IProgress<double> doubleProgress = null;
                    if (progress != null)
                    {
                        doubleProgress = new Progress<double>(p => 
                            progress.Report($"جاري التحميل... {p:F0}%"));
                    }

                    var filePath = await DownloadFileAsync(url, safeFileName, doubleProgress);
                    
                    if (filePath != null && File.Exists(filePath))
                    {
                        // التحقق من أن الملف ليس صفحة خطأ
                        if (IsValidPdfFile(filePath))
                        {
                            if (progress != null)
                            {
                                progress.Report("تم التحميل بنجاح!");
                            }
                            return filePath;
                        }
                        else
                        {
                            // الملف ليس PDF صالح، احذفه وجرب الرابط التالي
                            System.Diagnostics.Debug.WriteLine($"الملف ليس PDF صالح: {filePath}");
                            errors.Add($"المصدر {i + 1}: الملف تالف");
                            
                            try
                            {
                                File.Delete(filePath);
                            }
                            catch { /* تجاهل خطأ الحذف */ }
                        }
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    System.Diagnostics.Debug.WriteLine($"خطأ HTTP من {url}: {httpEx.Message}");
                    errors.Add($"المصدر {i + 1}: خطأ شبكة - {httpEx.Message}");
                }
                catch (TaskCanceledException tcEx)
                {
                    System.Diagnostics.Debug.WriteLine($"انتهت المهلة لـ {url}: {tcEx.Message}");
                    errors.Add($"المصدر {i + 1}: انتهت المهلة");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"خطأ غير متوقع من {url}: {ex.Message}");
                    errors.Add($"المصدر {i + 1}: {ex.Message}");
                }
            }

            // جميع الروابط فشلت
            var errorMessage = "فشل التحميل من جميع المصادر:\n" + string.Join("\n", errors.Take(3));
            if (errors.Count > 3)
            {
                errorMessage += $"\n... و {errors.Count - 3} أخطاء أخرى";
            }
            
            throw new AggregateException("فشل التحميل", errors.Select(e => new Exception(e)));
        }

        /// <summary>
        /// تنزيل كتاب من رابط URL واحد (طريقة قديمة محفوظة للتوافق)
        /// </summary>
        public async Task<string> DownloadBookAsync(string url, string fileName, IProgress<double> progress = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url), "رابط التحميل فارغ");
            }

            // تنظيف اسم الملف
            string safeFileName = SanitizeFileName(fileName);
            if (safeFileName.Length > 100)
            {
                safeFileName = safeFileName.Substring(0, 100) + Path.GetExtension(fileName);
            }

            var filePath = Path.Combine(_booksDirectory, safeFileName);

            // التحقق إذا كان الملف موجوداً
            if (File.Exists(filePath) && IsValidPdfFile(filePath))
            {
                return filePath;
            }

            return await DownloadFileAsync(url, safeFileName, progress);
        }

        /// <summary>
        /// تنزيل ملف من الإنترنت
        /// </summary>
        private async Task<string> DownloadFileAsync(string url, string fileName, IProgress<double> progress = null)
        {
            var filePath = Path.Combine(_booksDirectory, fileName);

            // التحقق من صحة الرابط أولاً
            if (!await IsValidDownloadUrlAsync(url))
            {
                throw new HttpRequestException($"رابط التحميل غير صالح أو لا يستجيب: {url}");
            }

            // تنزيل الملف
            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"فشل التحميل: {(int)response.StatusCode} {response.ReasonPhrase}");
            }

            // التحقق من نوع المحتوى
            var contentType = response.Content.Headers.ContentType?.MediaType;
            if (contentType != null && !contentType.Contains("pdf") && !contentType.Contains("octet-stream"))
            {
                System.Diagnostics.Debug.WriteLine($"تحذير: نوع المحتوى ليس PDF: {contentType}");
            }

            var totalBytes = response.Content.Headers.ContentLength ?? -1L;
            var downloadedBytes = 0L;

            using (var contentStream = await response.Content.ReadAsStreamAsync())
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                var buffer = new byte[8192];
                int bytesRead;

                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                    downloadedBytes += bytesRead;

                    if (totalBytes > 0 && progress != null)
                    {
                        var percentage = (double)downloadedBytes / totalBytes * 100;
                        progress.Report(percentage);
                    }
                }
            }

            return filePath;
        }

        /// <summary>
        /// التحقق من صحة رابط التحميل
        /// </summary>
        private async Task<bool> IsValidDownloadUrlAsync(string url)
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
                    return contentType != null && 
                           (contentType.Contains("pdf") || 
                            contentType.Contains("octet-stream") || 
                            contentType.Contains("application"));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"فحص الرابط فشل: {url}, Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// التحقق من أن الملف هو PDF صالح
        /// </summary>
        private bool IsValidPdfFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return false;
                }

                var fileInfo = new FileInfo(filePath);
                
                // الملف يجب أن يكون أكبر من 1KB
                if (fileInfo.Length < 1024)
                {
                    System.Diagnostics.Debug.WriteLine($"الملف صغير جداً: {fileInfo.Length} bytes");
                    return false;
                }

                // تحقق من توقيع PDF
                using (var stream = File.OpenRead(filePath))
                {
                    var header = new byte[4];
                    var bytesRead = stream.Read(header, 0, 4);
                    
                    if (bytesRead < 4)
                    {
                        return false;
                    }

                    // توقيع PDF هو "%PDF"
                    bool isValidPdf = header[0] == 0x25 && // %
                                      header[1] == 0x50 && // P
                                      header[2] == 0x44 && // D
                                      header[3] == 0x46;   // F
                    
                    if (!isValidPdf)
                    {
                        System.Diagnostics.Debug.WriteLine($"توقيع PDF غير صالح: {header[0]:X2}{header[1]:X2}{header[2]:X2}{header[3]:X2}");
                    }
                    
                    return isValidPdf;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"فحص PDF فشل: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// إضافة ملف محلي إلى المكتبة (للسحب والإفلات)
        /// </summary>
        public string AddLocalFile(string sourcePath)
        {
            try
            {
                if (!File.Exists(sourcePath))
                {
                    throw new FileNotFoundException("الملف غير موجود", sourcePath);
                }

                var fileName = Path.GetFileName(sourcePath);
                var destinationPath = Path.Combine(_booksDirectory, fileName);

                // إذا كان الملف موجوداً، إضافة رقم للتكرار
                var counter = 1;
                var baseFileName = Path.GetFileNameWithoutExtension(sourcePath);
                var extension = Path.GetExtension(sourcePath);

                while (File.Exists(destinationPath))
                {
                    var newFileName = $"{baseFileName}_{counter}{extension}";
                    destinationPath = Path.Combine(_booksDirectory, newFileName);
                    counter++;
                }

                // نسخ الملف
                File.Copy(sourcePath, destinationPath);
                return destinationPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"خطأ في إضافة الملف المحلي: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// حذف ملف كتاب
        /// </summary>
        public bool DeleteBookFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"خطأ في حذف الملف: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// فتح ملف في البرنامج الافتراضي
        /// </summary>
        public void OpenFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var process = new System.Diagnostics.Process
                    {
                        StartInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = filePath,
                            UseShellExecute = true
                        }
                    };
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"خطأ في فتح الملف: {ex.Message}");
            }
        }

        /// <summary>
        /// الحصول على حجم الملف بتنسيق مقروء
        /// </summary>
        public string GetFileSizeString(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return "غير معروف";
            }

            var fileInfo = new FileInfo(filePath);
            return GetFileSizeString(fileInfo.Length);
        }

        /// <summary>
        /// تحويل الحجم بالبايت إلى نص مقروء
        /// </summary>
        public string GetFileSizeString(long bytes)
        {
            string[] sizes = { "بايت", "كيلوبايت", "ميجابايت", "جيجابايت" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// تنظيف اسم الملف من الأحرف غير المسموحة
        /// </summary>
        private string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "book_" + DateTime.Now.Ticks.ToString();
            }

            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var c in invalidChars)
            {
                fileName = fileName.Replace(c, '_');
            }

            // إزالة الأحرف العربية من اسم الملف للـ Windows
            // بدلاً من ذلك، استخدم اسم ملف محايد
            if (fileName.Any(c => c >= 0x0600 && c <= 0x06FF))
            {
                // إذا كان الاسم يحتوي على عربية، استخدم ISBN أو اسم تجريبي
                fileName = "book_" + Path.GetFileNameWithoutExtension(fileName).GetHashCode().ToString("X");
            }

            // إزالة المسافات الزائدة
            fileName = System.Text.RegularExpressions.Regex.Replace(fileName, @"\s+", "_");

            // التأكد من أن الاسم ليس طويلاً جداً
            if (fileName.Length > 80)
            {
                fileName = fileName.Substring(0, 80);
            }

            return fileName;
        }

        /// <summary>
        /// الحصول على قائمة الكتب المحلية
        /// </summary>
        public List<string> GetLocalBooks()
        {
            var books = new List<string>();

            if (Directory.Exists(_booksDirectory))
            {
                foreach (var file in Directory.GetFiles(_booksDirectory, "*.*", SearchOption.TopDirectoryOnly))
                {
                    // تخطي الملفات غير الصالحة
                    if (IsValidPdfFile(file))
                    {
                        books.Add(file);
                    }
                }
            }

            return books;
        }

        /// <summary>
        /// التحقق من وجود ملف
        /// </summary>
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// الحصول على مسار مجلد الكتب
        /// </summary>
        public string GetBooksDirectory()
        {
            return _booksDirectory;
        }
    }
}
