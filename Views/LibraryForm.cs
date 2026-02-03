using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using smartLibraryForC_.Models;
using smartLibraryForC_.Services;

namespace smartLibraryForC_.Views
{
    public partial class LibraryForm : Form
    {
        private readonly DatabaseService _databaseService;
        private readonly FileService _fileService;

        public LibraryForm()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            _fileService = new FileService();
            
            this.WindowState = FormWindowState.Maximized;
            ThemeManager.Instance.ApplyTheme(this);
            
            LoadLibrary();
        }

        private void LoadLibrary()
        {
            try
            {
                flowBooks.Controls.Clear();
                var books = _databaseService.GetUserLibraryBooks();

                // Filter Favorites
                if (chkFavorites.Checked)
                {
                    books = books.Where(b => b.Book.IsFavorite).ToList();
                }

                if (books.Count == 0)
                {
                    ShowEmptyState();
                    return;
                }

                // فرز الكتب حسب الحالة (قيد القراءة أولاً)
                var sortedBooks = books.OrderByDescending(b => b.Status == ReadingStatus.Reading)
                                       .ThenByDescending(b => b.AddedAt)
                                       .ToList();

                foreach (var userBook in sortedBooks)
                {
                    var card = CreateLibraryBookCard(userBook);
                    flowBooks.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل المكتبة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowEmptyState()
        {
            var emptyPanel = new Panel
            {
                Width = 300,
                Height = 200,
                Dock = DockStyle.Top,
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle
            };

            var emptyLabel = new Label
            {
                Text = "📚\n\nمكتبتك فارغة حالياً:\\n\nيمكنكn• البحث عن كتب جديدة\n• إضافة ملفات PDF محلية",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14),
                ForeColor = Color.Gray
            };

            emptyPanel.Controls.Add(emptyLabel);
            flowBooks.Controls.Add(emptyPanel);
        }

        private Control CreateLibraryBookCard(UserBook userBook)
        {
             var panel = new Panel
            {
                Width = 160,
                Height = 280,
                Margin = new Padding(15),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Cover
            var pbCover = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 180,
                SizeMode = PictureBoxSizeMode.Zoom,
                ImageLocation = userBook.Book.CoverUrl
            };
            
            if (string.IsNullOrEmpty(userBook.Book.CoverUrl))
            {
                pbCover.BackColor = Color.LightGray;
                pbCover.Paint += (s, pe) =>
                {
                    var g = pe.Graphics;
                    using (var font = new Font("Segoe UI", 24))
                    {
                        g.DrawString("📖", font, Brushes.DarkGray, new PointF(60, 70));
                    }
                };
            }
            else
            {
                pbCover.LoadCompleted += (s, ev) =>
                {
                    if (ev.Error != null)
                    {
                        pbCover.BackColor = Color.LightGray;
                        pbCover.ImageLocation = null;
                        pbCover.Invalidate();
                    }
                };
            }

            // Title
            var lblTitle = new Label
            {
                Text = TruncateText(userBook.Book.Title, 25),
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            // Author
            var lblAuthor = new Label
            {
                Text = TruncateText(userBook.Book.Author, 20),
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };

            // Status Badge
            var lblStatus = new Label
            {
                Text = GetStatusText(userBook.Status),
                Dock = DockStyle.Bottom,
                Height = 25,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = GetStatusColor(userBook.Status),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8, FontStyle.Bold)
            };

            // Progress bar if reading
            if (userBook.Status == ReadingStatus.Reading && userBook.ProgressPercentage.HasValue)
            {
                var progressPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 20,
                    Padding = new Padding(10, 5, 10, 5)
                };

                var progressLabel = new Label
                {
                    Text = $"التقدم: {userBook.ProgressPercentage.Value:F0}%",
                    Dock = DockStyle.Top,
                    Height = 15,
                    Font = new Font("Segoe UI", 7),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var progressBar = new ProgressBar
                {
                    Dock = DockStyle.Bottom,
                    Height = 8,
                    Value = (int)userBook.ProgressPercentage.Value,
                    BackColor = Color.LightGray
                };

                progressPanel.Controls.Add(progressBar);
                progressPanel.Controls.Add(progressLabel);
                panel.Controls.Add(progressPanel);
            }

            // Date added
            var lblDate = new Label
            {
                Text = $"أضيف: {userBook.AddedAt:yyyy/MM/dd}",
                Dock = DockStyle.Bottom,
                Height = 15,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 7),
                ForeColor = Color.LightGray
            };

            panel.Controls.Add(lblStatus);
            panel.Controls.Add(lblDate);
            panel.Controls.Add(lblAuthor);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(pbCover);

            // Click to open details
            panel.Click += (s, e) => OpenBookDetails(userBook);
            foreach(Control c in panel.Controls) 
            {
                c.Click += (s, e) => OpenBookDetails(userBook);
                c.Cursor = Cursors.Hand;
            }

            // Hover effect
            panel.MouseEnter += (s, e) => panel.BackColor = Color.LightBlue;
            panel.MouseLeave += (s, e) => panel.BackColor = Color.White;

            return panel;
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (text.Length <= maxLength) return text;
            return text.Substring(0, maxLength - 3) + "...";
        }

        private string GetStatusText(ReadingStatus status) => status switch
        {
            ReadingStatus.Downloaded => "تم التحميل",
            ReadingStatus.Reading => "قيد القراءة",
            ReadingStatus.Finished => "مكتمل",
            _ => "غير معروف"
        };

        private Color GetStatusColor(ReadingStatus status) => status switch
        {
            ReadingStatus.Downloaded => Color.Gray,
            ReadingStatus.Reading => Color.Green,
            ReadingStatus.Finished => Color.Blue,
            _ => Color.Gray
        };

        private void OpenBookDetails(UserBook userBook)
        {
            try
            {
                var updatedBook = _databaseService.GetBookById(userBook.BookId);
                
                if (updatedBook != null)
                {
                    var detailsForm = new BookDetailsForm(updatedBook);
                    detailsForm.ShowDialog();
                    LoadLibrary(); // Refresh status
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddLocal_Click(object sender, EventArgs e)
        {
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "PDF Files|*.pdf|EPUB Files|*.epub|All Files|*.*";
                    openFileDialog.Title = "إضافة كتاب محلي";
                    openFileDialog.Multiselect = false;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var filePath = openFileDialog.FileName;
                        var fileName = System.IO.Path.GetFileName(filePath);
                        
                        // التحقق من أن الملف PDF
                        if (!filePath.ToLower().EndsWith(".pdf"))
                        {
                            MessageBox.Show("الرجاء اختيار ملف PDF فقط", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // نسخ الملف إلى مجلد الكتب
                        var localPath = _fileService.AddLocalFile(filePath);
                        
                        // إنشاء كتاب جديد
                        var book = new Book
                        {
                            Title = System.IO.Path.GetFileNameWithoutExtension(fileName),
                            Author = "مؤلف محلي",
                            Description = "كتاب مضاف محلياً",
                            LocalPath = localPath,
                            PageCount = 0,
                            PublishedDate = DateTime.Now
                        };

                        // حفظ في قاعدة البيانات
                        var bookId = _databaseService.AddBook(book);
                        _databaseService.AddToUserLibrary(bookId, ReadingStatus.Downloaded);
                        _databaseService.AddLog(LogActionType.Download, $"تم إضافة كتاب محلي: {book.Title}", bookId);

                        MessageBox.Show($"تم إضافة الكتاب بنجاح!\nالمسار: {localPath}", 
                                       "نجاح", 
                                       MessageBoxButtons.OK, 
                                       MessageBoxIcon.Information);

                        LoadLibrary();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إضافة الكتاب: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLibrary();
        }

        private void chkFavorites_CheckedChanged(object sender, EventArgs e)
        {
            LoadLibrary();
        }
    }
}
