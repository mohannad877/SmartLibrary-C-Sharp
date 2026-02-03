using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using smartLibraryForC_.Models;
using smartLibraryForC_.Services;
using System.Drawing;

namespace smartLibraryForC_.Views
{
    public partial class ReaderForm : Form
    {
        private Book _book;
        private readonly DatabaseService _databaseService;
        private int _userBookId;
        private int _currentPage = 1;
        private int _totalPages = 0;
        private bool _isPdfiumLoaded = false;

        public ReaderForm(Book book)
        {
            InitializeComponent();
            _book = book;
            _databaseService = new DatabaseService();
            
            this.WindowState = FormWindowState.Maximized;
            ThemeManager.Instance.ApplyTheme(this);
            
            this.Text = $"قراءة: {_book.Title}";
        }

        private void ReaderForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_book.LocalPath) || !File.Exists(_book.LocalPath))
            {
                MessageBox.Show("ملف الكتاب غير موجود!\nيرجى تحميل الكتاب أولاً.", 
                               "خطأ", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Error);
                Close();
                return;
            }

            LoadBook();
        }

        private void LoadBook()
        {
            try
            {
                lblStatus.Text = "جاري تحميل الكتاب...";
                lblStatus.Visible = true;
                btnNext.Enabled = false;
                btnPrev.Enabled = false;

                // محاولة استخدام PdfiumViewer
                if (LoadWithPdfiumViewer())
                {
                    // تم تحميل PdfiumViewer بنجاح
                    return;
                }

                // Pdfium غير متاح، استخدم الطريقة البديلة
                MessageBox.Show("سيتم فتح الكتاب في التطبيق الافتراضي للقراءة.", 
                               "معلومات", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Information);
                
                OpenInDefaultViewer();
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"خطأ: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
                
                // عرض خيار فتح الملف يدوياً
                var result = MessageBox.Show(
                    $"حدث خطأ أثناء تحميل الكتاب: {ex.Message}\n\nهل تريد فتح الملف يدوياً؟",
                    "خطأ",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    OpenInDefaultViewer();
                }
            }
        }

        private bool LoadWithPdfiumViewer()
        {
            try
            {
                // التحقق من وجود ملفات Pdfium
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var pdfiumDll = Path.Combine(baseDirectory, "PdfiumViewer.dll");
                
                if (!File.Exists(pdfiumDll))
                {
                    System.Diagnostics.Debug.WriteLine("PdfiumViewer.dll not found");
                    return false;
                }

                // إنشاء PdfViewer
                var pdfViewer = new PdfiumViewer.PdfViewer
                {
                    Dock = DockStyle.Fill,
                    Location = new System.Drawing.Point(0, 60),
                    Name = "pdfViewer",
                    Size = new System.Drawing.Size(this.Width, this.Height - 100),
                    TabIndex = 0
                };

                this.Controls.Add(pdfViewer);
                pdfViewer.BringToFront();

                // تحميل PDF
                var doc = PdfiumViewer.PdfDocument.Load(_book.LocalPath);
                pdfViewer.Document = doc;
                
                _totalPages = doc.PageCount;
                _currentPage = 1;
                _isPdfiumLoaded = true;

                // تحديث واجهة المستخدم
                lblPageInfo.Text = $"الصفحة {_currentPage} من {_totalPages}";
                lblStatus.Visible = false;
                
                btnNext.Enabled = true;
                btnPrev.Enabled = true;

                // ربط أزرار التنقل
                btnPrev.Click += (s, ev) => 
                {
                    if (_currentPage > 1)
                    {
                        _currentPage--;
                        pdfViewer.Renderer.Page = _currentPage - 1;
                        lblPageInfo.Text = $"الصفحة {_currentPage} من {_totalPages}";
                        UpdateProgress();
                    }
                };

                btnNext.Click += (s, ev) =>
                {
                    if (_currentPage < _totalPages)
                    {
                        _currentPage++;
                        pdfViewer.Renderer.Page = _currentPage - 1;
                        lblPageInfo.Text = $"الصفحة {_currentPage} من {_totalPages}";
                        UpdateProgress();
                    }
                };

                // تحديث تقدم القراءة
                LoadProgress();
                UpdateProgress();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Pdfium loading error: {ex.Message}");
                return false;
            }
        }

        private void OpenInDefaultViewer()
        {
            try
            {
                var fileService = new FileService();
                fileService.OpenFile(_book.LocalPath);
                
                lblStatus.Text = "تم فتح الكتاب في القارئ الافتراضي";
                lblStatus.ForeColor = Color.Green;
                
                // تحديث تقدم القراءة
                LoadProgress();
                UpdateProgress();
                
                // إظهار معلومات التقدم
                panelProgress.Visible = true;
                lblPageInfo.Text = "المسار: " + _book.LocalPath;
                lblProgress.Text = "تم فتح الكتاب بنجاح";
                
                // تفعيل زر الانتقال للصفحة
                btnGoToPage.Enabled = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"فشل فتح الملف: {ex.Message}");
            }
        }

        private void LoadProgress()
        {
            try
            {
                var library = _databaseService.GetUserLibraryBooks();
                var userBook = library.FirstOrDefault(ub => ub.BookId == _book.BookId);
                
                if (userBook != null)
                {
                    _userBookId = userBook.UserBookId;
                    
                    if (userBook.CurrentPage.HasValue && userBook.CurrentPage.Value > 1)
                    {
                        _currentPage = Math.Min(userBook.CurrentPage.Value, _totalPages > 0 ? _totalPages : 1);
                    }
                    
                    if (userBook.ProgressPercentage.HasValue)
                    {
                        lblProgress.Text = $"التقدم: {userBook.ProgressPercentage.Value:F0}%";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading progress: {ex.Message}");
            }
        }

        private void UpdateProgress()
        {
            try
            {
                if (_totalPages > 0)
                {
                    var progress = (_currentPage * 100.0) / _totalPages;
                    lblProgress.Text = $"التقدم: {progress:F0}%";
                    
                    // تحديث قاعدة البيانات
                    if (_userBookId > 0)
                    {
                        _databaseService.UpdateReadingStatus(
                            _userBookId, 
                            ReadingStatus.Reading, 
                            _currentPage, 
                            progress);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"خطأ في تحديث التقدم: {ex.Message}");
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (!_isPdfiumLoaded)
            {
                MessageBox.Show("يرجى استخدام أزرار التنقل في عارض PDF إذا كان متاحاً،\nأو استخدام شريط التمرير في التطبيق الافتراضي.", 
                               "معلومات", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Information);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!_isPdfiumLoaded)
            {
                MessageBox.Show("يرجى استخدام أزرار التنقل في عارض PDF إذا كان متاحاً،\nأو استخدام شريط التمرير في التطبيق الافتراضي.", 
                               "معلومات", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Information);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_book.LocalPath) && File.Exists(_book.LocalPath))
                {
                    var fileService = new FileService();
                    fileService.OpenFile(_book.LocalPath);
                }
                else
                {
                    MessageBox.Show("ملف الكتاب غير موجود!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGoToPage_Click(object sender, EventArgs e)
        {
            if (_totalPages <= 0 && !_isPdfiumLoaded)
            {
                MessageBox.Show("لا يمكن الانتقال للصفحة - لم يتم تحميل الكتاب بالكامل", 
                               "تنبيه", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Warning);
                return;
            }

            using (var inputDialog = new Form())
            {
                inputDialog.Text = "الانتقال لصفحة";
                inputDialog.Size = new System.Drawing.Size(250, 150);
                inputDialog.StartPosition = FormStartPosition.CenterParent;

                var label = new Label
                {
                    Text = "أدخل رقم الصفحة:",
                    Dock = DockStyle.Top,
                    Padding = new Padding(10),
                    Height = 40
                };

                var numericUpDown = new NumericUpDown
                {
                    Minimum = 1,
                    Maximum = _totalPages > 0 ? _totalPages : 9999,
                    Value = _currentPage,
                    Dock = DockStyle.Top,
                    Margin = new Padding(10),
                    Height = 30
                };

                var okButton = new Button
                {
                    Text = "موافق",
                    DialogResult = DialogResult.OK,
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    Margin = new Padding(10)
                };

                var cancelButton = new Button
                {
                    Text = "إلغاء",
                    DialogResult = DialogResult.Cancel,
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    Margin = new Padding(10, 0, 10, 10)
                };

                inputDialog.Controls.Add(label);
                inputDialog.Controls.Add(numericUpDown);
                inputDialog.Controls.Add(okButton);
                inputDialog.Controls.Add(cancelButton);

                if (inputDialog.ShowDialog(this) == DialogResult.OK)
                {
                    _currentPage = (int)numericUpDown.Value;
                    
                    // تحديث واجهة المستخدم
                    lblPageInfo.Text = $"الصفحة {_currentPage} من {_totalPages}";
                    UpdateProgress();
                }
            }
        }

        private void ReaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // حفظ التقدم قبل الإغلاق
                UpdateProgress();
                
                if (_userBookId > 0 && _totalPages > 0)
                {
                    // تحديث الحالة إذا وصل المستخدم لنهاية الكتاب
                    if (_currentPage >= _totalPages)
                    {
                        _databaseService.UpdateReadingStatus(_userBookId, ReadingStatus.Finished, _currentPage, 100);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"خطأ عند إغلاق النموذج: {ex.Message}");
            }
        }
    }
}
