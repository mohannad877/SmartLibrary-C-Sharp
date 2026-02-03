using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using smartLibraryForC_.Models;
using smartLibraryForC_.Services;
using System.Linq;

namespace smartLibraryForC_.Views
{
    public partial class BookDetailsForm : Form
    {
        private readonly Book _book;
        private readonly DatabaseService _databaseService;
        private readonly FileService _fileService;
        private readonly BookApiService _bookApiService;
        private UserBook _userBook;

        public BookDetailsForm(Book book)
        {
            InitializeComponent();
            _book = book;
            _databaseService = new DatabaseService();
            _fileService = new FileService();
            _bookApiService = new BookApiService();

            this.WindowState = FormWindowState.Maximized;
            ThemeManager.Instance.ApplyTheme(this);
            
            PopulateData();
            LoadUserBookStatus();
        }

        private void PopulateData()
        {
            lblTitle.Text = _book.Title;
            lblAuthor.Text = _book.Author;
            txtDescription.Text = _book.Description ?? "لا يوجد وصف متاح";
            lblPages.Text = _book.PageCount.HasValue ? $"{_book.PageCount} صفحة" : "غير محدد";
            lblYear.Text = _book.PublishedDate.HasValue ? _book.PublishedDate.Value.Year.ToString() : "غير محدد";
            
            if (!string.IsNullOrEmpty(_book.CoverUrl))
            {
                pbCover.ImageLocation = _book.CoverUrl;
            }
            else
            {
                pbCover.BackColor = Color.LightGray;
                pbCover.Image = null;
            }
        }

        private void LoadUserBookStatus()
        {
            // Initial Favorites State
            UpdateFavoriteButton();

            // Check if user has this book
            var library = _databaseService.GetUserLibraryBooks();
            _userBook = library.Find(ub => ub.BookId == _book.BookId);

            if (_userBook != null)
            {
                // Has book
                if (!string.IsNullOrEmpty(_book.LocalPath) && _fileService.FileExists(_book.LocalPath))
                {
                    btnAction.Text = "قراءة";
                    btnAction.BackColor = Color.Green;
                    btnAction.Click -= DownloadBook;
                    btnAction.Click += ReadBook;
                }
                else
                {
                    // In library but file missing/not downloaded
                    btnAction.Text = "تحميل";
                    btnAction.BackColor = Color.DodgerBlue;
                    btnAction.Click -= ReadBook;
                    btnAction.Click += DownloadBook;
                }
                
                btnRemove.Visible = true;
            }
            else
            {
                // Not in library
                btnAction.Text = "إضافة للمكتبة";
                btnAction.BackColor = Color.DodgerBlue;
                btnAction.Click -= ReadBook;
                btnAction.Click += AddToLibrary;
                btnRemove.Visible = false;
            }
        }

        private void UpdateFavoriteButton()
        {
            if (_book.IsFavorite)
            {
                btnFavorite.Text = "♥";
                btnFavorite.ForeColor = Color.Red;
            }
            else
            {
                btnFavorite.Text = "♡";
                btnFavorite.ForeColor = Color.Gray;
            }
        }

        private void AddToLibrary(object sender, EventArgs e)
        {
             // First add to DB if not exists
             if (_book.BookId == 0)
             {
                 _book.BookId = _databaseService.AddBook(_book);
             }
             
             _databaseService.AddToUserLibrary(_book.BookId, ReadingStatus.Downloaded);
             LoadUserBookStatus();
             MessageBox.Show("تمت الإضافة للمكتبة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void DownloadBook(object sender, EventArgs e)
        {
            btnAction.Enabled = false;
            btnAction.Text = "جاري التحميل...";
            lblStatus.Text = "جاري تحضير روابط التحميل...";
            lblStatus.Visible = true;
            lblStatus.ForeColor = Color.Blue;

            try
            {
                // الحصول على روابط التحميل البديلة
                var downloadUrls = await _bookApiService.GetDownloadSourcesAsync(
                    _book.Title, 
                    null, // IA IDs not directly available in book object here, relied on fallback
                    _book.Isbn != null ? new List<string> { _book.Isbn } : null
                );

                if (downloadUrls.Count == 0)
                {
                    throw new Exception("لا توجد روابط تحميل متاحة لهذا الكتاب");
                }

                // إنشاء progress reporter للنص
                var textProgress = new Progress<string>(status =>
                {
                    lblStatus.Text = status;
                    Application.DoEvents();
                });

                // إنشاء progress reporter للنسبة المئوية
                var percentProgress = new Progress<double>(p =>
                {
                    btnAction.Text = $"تحميل {p:F0}%";
                    Application.DoEvents();
                });

                // استخدام نظام التحميل البديل
                var fileName = SanitizeFileName($"{_book.Title}.pdf");
                var path = await _fileService.DownloadBookWithFallbackAsync(downloadUrls, fileName, textProgress);
                
                if (path != null && _fileService.FileExists(path))
                {
                    _book.LocalPath = path;
                    _databaseService.UpdateBookLocalPath(_book.BookId, path);
                    
                    LoadUserBookStatus();
                    lblStatus.Text = "تم التحميل بنجاح!";
                    lblStatus.ForeColor = Color.Green;
                    
                    MessageBox.Show("تم تحميل الكتاب بنجاح!\n" +
                                  $"المسار: {path}\n\n" +
                                  $"الحجم: {_fileService.GetFileSizeString(path)}", 
                                  "نجاح", 
                                  MessageBoxButtons.OK, 
                                  MessageBoxIcon.Information);
                }
                else
                {
                    throw new Exception("فشل التحميل - الملف غير موجود");
                }
            }
            catch (AggregateException aggEx)
            {
                // أخطاء متعددة من جميع المحاولات
                var errorMessages = aggEx.InnerExceptions
                    .Select(ex => ex.Message)
                    .Take(3)
                    .ToList();
                
                var errorText = string.Join("\n", errorMessages);
                ShowDownloadError("فشل التحميل من جميع المصادر:\n" + errorText);
            }
            catch (Exception ex)
            {
                ShowDownloadError($"حدث خطأ أثناء التحميل:\n{ex.Message}");
            }
            finally
            {
                btnAction.Enabled = true;
                LoadUserBookStatus();
            }
        }

        private void ShowDownloadError(string message)
        {
            lblStatus.Text = "فشل التحميل";
            lblStatus.ForeColor = Color.Red;
            
            MessageBox.Show(message + 
                          "\n\nيمكنك:\n" +
                          "1. التحقق من اتصالك بالإنترنت\n" +
                          "2. المحاولة مرة أخرى لاحقاً\n" +
                          "3. البحث عن كتاب آخر", 
                          "خطأ في التحميل", 
                          MessageBoxButtons.OK, 
                          MessageBoxIcon.Error);
        }

        private string SanitizeFileName(string name)
        {
            foreach(char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            
            // إزالة الأحرف العربية من اسم الملف
            if (name.Any(c => c >= 0x0600 && c <= 0x06FF))
            {
                name = "book_" + name.GetHashCode().ToString("X") + ".pdf";
            }
            
            return name;
        }

        private void ReadBook(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_book.LocalPath) || !_fileService.FileExists(_book.LocalPath))
            {
                MessageBox.Show("ملف الكتاب غير موجود. يرجى تحميل الكتاب أولاً.", 
                              "خطأ", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var readerForm = new ReaderForm(_book);
                readerForm.Show();
                
                // تحديث الحالة إلى قيد القراءة
                if (_userBook != null)
                {
                    _databaseService.UpdateReadingStatus(_userBook.UserBookId, ReadingStatus.Reading);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ في فتح القارئ: {ex.Message}", 
                              "خطأ", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Error);
            }
        }
        
        private void btnRemove_Click(object sender, EventArgs e)
        {
             if (_userBook != null)
             {
                 // سؤال المستخدم عن حذف ملف PDF أيضاً
                 var result = MessageBox.Show("هل أنت متأكد من حذف الكتاب من مكتبتك؟\n\nهل تريد حذف ملف PDF من الجهاز أيضاً؟", 
                                             "تأكيد الحذف", 
                                             MessageBoxButtons.YesNoCancel, 
                                             MessageBoxIcon.Question);
                 
                 if (result == DialogResult.Cancel)
                     return;
                     
                 // حذف ملف PDF إذا اختار المستخدم "نعم"
                 if (result == DialogResult.Yes && !string.IsNullOrEmpty(_book.LocalPath))
                 {
                     _fileService.DeleteBookFile(_book.LocalPath);
                 }
                 
                 // حذف من قاعدة البيانات دائماً (نعم أو لا)
                 _databaseService.RemoveFromUserLibrary(_userBook.UserBookId);
                 LoadUserBookStatus();
                 
                 var books = _databaseService.GetUserLibraryBooks();
                 if (books.Count == 0)
                 {
                     lblStatus.Text = "المكتبة فارغة";
                 }
             }
        }


        private void btnFavorite_Click(object sender, EventArgs e)
        {
            _book.IsFavorite = !_book.IsFavorite;
            
            // If the book is not saved in DB yet (search result), save it first
            if (_book.BookId == 0)
            {
                _book.BookId = _databaseService.AddBook(_book);
            }
            
            _databaseService.ToggleFavorite(_book.BookId, _book.IsFavorite);
            UpdateFavoriteButton();
        }

        /// <summary>
        /// تحديث بيانات الكتاب (للاستخدام عند إعادة التحميل)
        /// </summary>
        public void UpdateBook(Book updatedBook)
        {
            _book.Title = updatedBook.Title;
            _book.Author = updatedBook.Author;
            _book.Description = updatedBook.Description;
            _book.CoverUrl = updatedBook.CoverUrl;
            _book.DownloadUrl = updatedBook.DownloadUrl;
            _book.LocalPath = updatedBook.LocalPath;
            
            PopulateData();
        }
    }
}
