using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using smartLibraryForC_.Models;
using smartLibraryForC_.Services;

namespace smartLibraryForC_.Views.Controls
{
    public partial class DashboardControl : UserControl
    {
        private readonly DatabaseService _databaseService;

        public DashboardControl()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        public void LoadData()
        {
            try
            {
                var library = _databaseService.GetUserLibraryBooks();
                var logs = _databaseService.GetRecentLogs(5);
                var favorites = _databaseService.GetFavoriteBooks();

                // Stats
                lblTotalBooks.Text = library.Count.ToString();
                lblReading.Text = library.Count(b => b.Status == ReadingStatus.Reading).ToString();
                lblCompleted.Text = library.Count(b => b.Status == ReadingStatus.Finished).ToString();
                lblFavorites.Text = favorites.Count.ToString();

                // Recent Activity
                ShowRecentActivity(logs);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل بيانات اللوحة: " + ex.Message);
            }
        }

        private void ShowRecentActivity(System.Collections.Generic.List<SystemLog> logs)
        {
            flowActivity.Controls.Clear();
            foreach (var log in logs)
            {
                var lbl = new Label
                {
                    Text = $"• {log.Description} ({log.CreatedAt:g})",
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 5),
                    Font = new Font("Segoe UI", 9)
                };
                flowActivity.Controls.Add(lbl);
            }
        }
    }
}
