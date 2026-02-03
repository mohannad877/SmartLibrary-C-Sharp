using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using smartLibraryForC_.Models;
using smartLibraryForC_.Services;
using System.Threading.Tasks;

namespace smartLibraryForC_.Views
{
    public partial class MainForm : Form
    {
        private readonly WeatherService _weatherService;
        private readonly DatabaseService _databaseService;
        private System.Windows.Forms.Timer _timer;
        private smartLibraryForC_.Views.Controls.DashboardControl _dashboardControl;

        public MainForm()
        {
            InitializeComponent();
            
            _weatherService = new WeatherService();
            _databaseService = new DatabaseService();

            _dashboardControl = new smartLibraryForC_.Views.Controls.DashboardControl();
            _dashboardControl.Dock = DockStyle.Fill;

            InitializeCustomLogic();
            
            // Apply initial theme
            ThemeManager.Instance.ApplyTheme(this);
            this.WindowState = FormWindowState.Maximized;
        }

        private void InitializeCustomLogic()
        {
            // Timer for clock
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += (s, e) => UpdateTime();
            _timer.Start();

            // Load initial data asynchronously without blocking
            // Load initial data asynchronously without blocking
            _ = LoadInitialDataAsync();
            UpdateTime();
            ShowDashboard();
        }

        private async Task LoadInitialDataAsync()
        {
            try
            {
                lblStatus.Text = "جاري تحميل البيانات...";

                var weatherTask = _weatherService.GetCurrentWeatherAsync();
                
                // الانتظار مع timeout
                var completedTask = await Task.WhenAny(weatherTask, Task.Delay(10000));
                
                if (completedTask == weatherTask)
                {
                    var weather = await weatherTask;
                    if (weather != null)
                    {
                        lblTemperature.Text = $"{weather.Temperature:F1}°C";
                        lblWeatherCondition.Text = weather.Condition;
                        _databaseService.SaveWeatherLog(weather);
                    }
                    else
                    {
                        lblTemperature.Text = "--°C";
                        lblWeatherCondition.Text = "غير متاح";
                    }
                }
                else
                {
                    lblTemperature.Text = "--°C";
                    lblWeatherCondition.Text = "خطأ في الاتصال";
                }

                UpdateStatistics();
                LoadRecentBooks();
                if (_dashboardControl != null && _dashboardControl.Visible)
                {
                    _dashboardControl.Invoke((MethodInvoker)(() => _dashboardControl.LoadData()));
                }

                lblStatus.Text = "جاهز";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "خطأ";
                lblStatus.ForeColor = Color.Red;
                System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
                
                // لا نعرض رسالة خطأ هنا لتجنب إزعاج المستخدم
                UpdateStatistics();
                LoadRecentBooks();
            }
        }

        private void UpdateTime()
        {
            try
            {
                lblTime.Text = _weatherService.GetCurrentTime();
                lblDate.Text = $"{_weatherService.GetDayName()}، {_weatherService.GetCurrentDate()}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating time: {ex.Message}");
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                var books = _databaseService.GetUserLibraryBooks();
                lblTotalBooks.Text = books.Count.ToString();
                lblReadingBooks.Text = books.Count(b => b.Status == ReadingStatus.Reading).ToString();
            }
            catch (Exception ex)
            {
                lblTotalBooks.Text = "0";
                lblReadingBooks.Text = "0";
                System.Diagnostics.Debug.WriteLine($"Error updating statistics: {ex.Message}");
            }
        }

        private void LoadRecentBooks()
        {
            try
            {
                // Clear existing items in flow layout
                flowRecentBooks.Controls.Clear();

                var books = _databaseService.GetUserLibraryBooks().Take(5).ToList();
                
                if (books.Count == 0)
                {
                    // Show empty state
                    var emptyPanel = new Panel
                    {
                        Width = 150,
                        Height = 200,
                        Margin = new Padding(10),
                        BackColor = Color.WhiteSmoke,
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    var emptyLabel = new Label
                    {
                        Text = "📚\n\nمكتبتك فارغة\n\nابدأ بالبحث\nلإضافة كتب",
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Segoe UI", 10),
                        ForeColor = Color.Gray
                    };

                    emptyPanel.Controls.Add(emptyLabel);
                    flowRecentBooks.Controls.Add(emptyPanel);
                    return;
                }

                foreach (var userBook in books)
                {
                    var card = CreateBookCard(userBook);
                    flowRecentBooks.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading recent books: {ex.Message}");
            }
        }

        private Control CreateBookCard(UserBook userBook)
        {
            var panel = new Panel
            {
                Width = 140,
                Height = 220,
                Margin = new Padding(10),
                BackColor = ThemeManager.Instance.GetColor("Surface"),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Cover
            var pbCover = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 150,
                SizeMode = PictureBoxSizeMode.Zoom,
                ImageLocation = userBook.Book.CoverUrl
            };
            
            if (string.IsNullOrEmpty(userBook.Book.CoverUrl))
            {
                pbCover.BackColor = Color.LightGray;
                pbCover.Paint += (s, pe) =>
                {
                    var g = pe.Graphics;
                    using (var font = new Font("Segoe UI", 12))
                    {
                        g.DrawString("📖", font, Brushes.DarkGray, new PointF(50, 50));
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
                Text = TruncateText(userBook.Book.Title, 20),
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = ThemeManager.Instance.GetColor("TextPrimary")
            };

            // Progress bar (if reading)
            if (userBook.Status == ReadingStatus.Reading && userBook.ProgressPercentage.HasValue)
            {
                var progressBar = new ProgressBar
                {
                    Dock = DockStyle.Bottom,
                    Height = 5,
                    Value = (int)(userBook.ProgressPercentage.Value),
                    BackColor = Color.LightGray
                };
                panel.Controls.Add(progressBar);
            }

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(pbCover);

            // Add click handler
            panel.Click += (s, e) => OpenBookDetails(userBook);
            foreach(Control c in panel.Controls) 
            {
                c.Click += (s, e) => OpenBookDetails(userBook);
                c.Cursor = Cursors.Hand;
            }

            // Add hover effect
            panel.MouseEnter += (s, e) => 
            {
                panel.BackColor = ThemeManager.Instance.IsDarkMode 
                    ? ThemeManager.Instance.GetColor("Background") 
                    : ColorTranslator.FromHtml("#E3F2FD"); // Light Blue 50
            };
            panel.MouseLeave += (s, e) => 
            {
                panel.BackColor = ThemeManager.Instance.GetColor("Surface");
            };

            return panel;
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (text.Length <= maxLength) return text;
            return text.Substring(0, maxLength - 3) + "...";
        }

        private void OpenBookDetails(UserBook userBook)
        {
            try
            {
                // تحديث بيانات الكتاب من قاعدة البيانات
                var updatedBook = _databaseService.GetBookById(userBook.BookId);
                
                if (updatedBook != null)
                {
                    var detailsForm = new BookDetailsForm(updatedBook);
                    detailsForm.ShowDialog();
                    LoadRecentBooks();
                    UpdateStatistics();
                }
                else
                {
                    MessageBox.Show("تعذر العثور على بيانات الكتاب", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchForm = new SearchForm();
            searchForm.ShowDialog();
            LoadRecentBooks();
            UpdateStatistics();
        }

        private void btnLibrary_Click(object sender, EventArgs e)
        {
            try
            {
                var libraryForm = new LibraryForm();
                libraryForm.ShowDialog();
                LoadRecentBooks();
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReader_Click(object sender, EventArgs e)
        {
            MessageBox.Show("يرجى اختيار كتاب من المكتبة لبدء القراءة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnLibrary_Click(sender, e);
        }

        private void ShowDashboard()
        {
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(_dashboardControl);
            _dashboardControl.LoadData();
            ThemeManager.Instance.ApplyTheme(_dashboardControl); // Apply theme
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ShowDashboard();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // حفظ السجل قبل الخروج
            try
            {
                _databaseService.AddLog(LogActionType.System, "تم إغلاق التطبيق");
            }
            catch { /* تجاهل الأخطاء */ }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                _databaseService.AddLog(LogActionType.System, "تم تشغيل التطبيق");
            }
            catch { /* تجاهل الأخطاء */ }
        }

        private void btnThemeToggle_Click(object sender, EventArgs e)
        {
            ThemeManager.Instance.ToggleTheme();
            ThemeManager.Instance.ApplyTheme(this);
            _dashboardControl.LoadData(); // Refresh dashboard with new colors
        }

        // Hover effects for sidebar buttons
        private void SidebarButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = ColorTranslator.FromHtml("#1976D2"); // Darker blue
            }
        }

        private void SidebarButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.Transparent; // Reset to sidebar color
            }
        }
    }
}
