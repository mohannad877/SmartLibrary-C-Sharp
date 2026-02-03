using System;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using smartLibraryForC_.Models;
using smartLibraryForC_.Services;

namespace smartLibraryForC_.Views
{
    public partial class SearchForm : Form
    {
        private readonly BookApiService _bookApiService;
        private readonly DatabaseService _databaseService;

        public SearchForm()
        {
            InitializeComponent();
            _bookApiService = new BookApiService();
            _databaseService = new DatabaseService();
            
            this.WindowState = FormWindowState.Maximized;
            ThemeManager.Instance.ApplyTheme(this);
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            var query = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(query))
            {
                MessageBox.Show("الرجاء إدخال كلمة للبحث", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                lblStatus.Text = "جاري البحث...";
                lblStatus.Visible = true;
                lblStatus.ForeColor = Color.Blue;
                flowResults.Controls.Clear();
                btnSearch.Enabled = false;
                Cursor = Cursors.WaitCursor;

                var books = await _bookApiService.SearchBooksAsync(query);
                
                if (books.Count == 0)
                {
                    lblStatus.Text = "لم يتم العثور على نتائج";
                    lblStatus.ForeColor = Color.Orange;
                    
                    // عرض رسالة مفيدة
                    var panel = new Panel
                    {
                        Width = 300,
                        Height = 100,
                        Margin = new Padding(10),
                        BackColor = Color.LightYellow,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    
                    var lbl = new Label
                    {
                        Text = "لم يتم العثور على نتائج.\n\nنصائح:\n• جرب كلمة إنجليزية\n• جرب كلمة أبسط\n• تحقق من اتصالك بالإنترنت",
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Segoe UI", 10)
                    };
                    
                    panel.Controls.Add(lbl);
                    flowResults.Controls.Add(panel);
                }
                else
                {
                    lblStatus.Text = $"تم العثور على {books.Count} كتاب";
                    lblStatus.ForeColor = Color.Green;
                    
                    foreach (var book in books)
                    {
                        var card = CreateBookResultCard(book);
                        flowResults.Controls.Add(card);
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                lblStatus.Text = "خطأ في الاتصال";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"حدث خطأ في الاتصال بالشبكة: {httpEx.Message}\nتأكد من اتصالك بالإنترنت.", 
                               "خطأ اتصال", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Error);
            }
            catch (TaskCanceledException tcEx)
            {
                lblStatus.Text = "انتهت المهلة";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show("انتهت مهلة الاتصال. يرجى المحاولة مرة أخرى.", 
                               "خطأ", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "خطأ غير متوقع";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"حدث خطأ غير متوقع: {ex.Message}", 
                               "خطأ", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Error);
            }
            finally
            {
                btnSearch.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private Control CreateBookResultCard(Book book)
        {
            var panel = new Panel
            {
                Width = 280,
                Height = 120,
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Cover (Left side)
            var pbCover = new PictureBox
            {
                Dock = DockStyle.Left,
                Width = 80,
                Height = 118,
                SizeMode = PictureBoxSizeMode.Zoom,
                ImageLocation = book.CoverUrl,
                BorderStyle = BorderStyle.FixedSingle
            };
            
            if (string.IsNullOrEmpty(book.CoverUrl))
            {
                pbCover.BackColor = Color.LightGray;
                // إضافة أيقونة كتاب بديلة
                pbCover.Paint += (s, pe) =>
                {
                    var g = pe.Graphics;
                    using (var font = new Font("Segoe UI", 8))
                    {
                        g.DrawString("لا توجد\nصورة", font, Brushes.Gray, new PointF(15, 40));
                    }
                };
            }
            else
            {
                // تحميل الصورة بشكل غير متزامن
                pbCover.LoadCompleted += (s, ev) =>
                {
                    if (!pbCover.ImageLocation.Contains("covers.openlibrary.org"))
                    {
                        // صورة خارجية، لا حاجة لشيء
                    }
                };
                pbCover.LoadCompleted += (s, ev) =>
                {
                    if (ev.Error != null)
                    {
                        pbCover.BackColor = Color.LightGray;
                        pbCover.ImageLocation = null;
                    }
                };
            }

            // Info (Fill)
            var panelInfo = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
            
            var lblTitle = new Label
            {
                Text = book.Title,
                Dock = DockStyle.Top,
                Height = 35,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoEllipsis = true
            };

            var lblAuthor = new Label
            {
                Text = book.Author,
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoEllipsis = true
            };

            var lblInfo = new Label
            {
                Text = $"سنة: {(book.PublishedDate?.Year.ToString() ?? "غير معروف")} | صفحات: {(book.PageCount?.ToString() ?? "?")}",
                Dock = DockStyle.Top,
                Height = 18,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.LightGray
            };

            var btnAdd = new Button
            {
                Text = "إضافة للمكتبة",
                Dock = DockStyle.Bottom,
                Height = 35,
                BackColor = Color.DodgerBlue,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnAdd.Click += (s, e) => AddBookToLibrary(book);

            panelInfo.Controls.Add(lblInfo);
            panelInfo.Controls.Add(lblAuthor);
            panelInfo.Controls.Add(lblTitle);
            panelInfo.Controls.Add(btnAdd);

            panel.Controls.Add(panelInfo);
            panel.Controls.Add(pbCover);

            // إضافة تأثير التحويم
            panel.MouseEnter += (s, e) => panel.BackColor = Color.LightBlue;
            panel.MouseLeave += (s, e) => panel.BackColor = Color.White;

            return panel;
        }

        private void AddBookToLibrary(Book book)
        {
            try
            {
                // التحقق أولاً إذا كان الكتاب موجوداً في المكتبة
                var allBooks = _databaseService.GetAllBooks();
                var existingBook = allBooks.FirstOrDefault(b => 
                    b.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase) &&
                    b.Author.Equals(book.Author, StringComparison.OrdinalIgnoreCase));

                int bookId;
                
                if (existingBook != null)
                {
                    // الكتاب موجود، استخدمه
                    bookId = existingBook.BookId;
                    
                    // التحقق إذا كان في مكتبة المستخدم
                    var userLibrary = _databaseService.GetUserLibraryBooks();
                    if (userLibrary.Any(ub => ub.BookId == bookId))
                    {
                        MessageBox.Show("الكتاب موجود بالفعل في مكتبتك!", "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    // كتاب جديد، أضفه
                    bookId = _databaseService.AddBook(book);
                }
                
                // أضف إلى مكتبة المستخدم
                _databaseService.AddToUserLibrary(bookId, ReadingStatus.Downloaded);
                
                _databaseService.AddLog(LogActionType.Download, $"تم إضافة الكتاب: {book.Title}", bookId);

                MessageBox.Show($"تم إضافة \"{book.Title}\" إلى مكتبتك بنجاح", 
                               "نجاح", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إضافة الكتاب: {ex.Message}", 
                               "خطأ", 
                               MessageBoxButtons.OK, 
                               MessageBoxIcon.Error);
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
