namespace smartLibraryForC_.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelWeather = new System.Windows.Forms.Panel();
            this.lblWeatherCondition = new System.Windows.Forms.Label();
            this.lblTemperature = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.btnThemeToggle = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.panelStats = new System.Windows.Forms.Panel();
            this.lblReadingBooks = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotalBooks = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReader = new System.Windows.Forms.Button();
            this.btnLibrary = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.panelUser = new System.Windows.Forms.Panel();
            this.labelUser = new System.Windows.Forms.Label();
            this.flowRecentBooks = new System.Windows.Forms.FlowLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelTop.SuspendLayout();
            this.panelWeather.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelSidebar.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.panelUser.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panelWeather);
            this.panelTop.Controls.Add(this.btnThemeToggle);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(20);
            this.panelTop.Size = new System.Drawing.Size(1000, 80);
            this.panelTop.TabIndex = 1;
            // 
            // panelWeather
            // 
            this.panelWeather.Controls.Add(this.lblWeatherCondition);
            this.panelWeather.Controls.Add(this.lblTemperature);
            this.panelWeather.Controls.Add(this.lblTime);
            this.panelWeather.Controls.Add(this.lblDate);
            this.panelWeather.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelWeather.Location = new System.Drawing.Point(680, 20);
            this.panelWeather.Name = "panelWeather";
            this.panelWeather.Size = new System.Drawing.Size(300, 40);
            this.panelWeather.TabIndex = 0;
            // 
            // lblWeatherCondition
            // 
            this.lblWeatherCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWeatherCondition.Location = new System.Drawing.Point(0, 69);
            this.lblWeatherCondition.Name = "lblWeatherCondition";
            this.lblWeatherCondition.Size = new System.Drawing.Size(300, 23);
            this.lblWeatherCondition.TabIndex = 0;
            this.lblWeatherCondition.Text = "Loading...";
            this.lblWeatherCondition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTemperature
            // 
            this.lblTemperature.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTemperature.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTemperature.Location = new System.Drawing.Point(0, 46);
            this.lblTemperature.Name = "lblTemperature";
            this.lblTemperature.Size = new System.Drawing.Size(300, 23);
            this.lblTemperature.TabIndex = 1;
            this.lblTemperature.Text = "--";
            this.lblTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTime
            // 
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTime.Location = new System.Drawing.Point(0, 23);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(300, 23);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "00:00:00";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDate
            // 
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDate.Location = new System.Drawing.Point(0, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(300, 23);
            this.lblDate.TabIndex = 3;
            this.lblDate.Text = "Date";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnThemeToggle
            // 
            this.btnThemeToggle.AutoSize = true;
            this.btnThemeToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemeToggle.FlatAppearance.BorderSize = 0;
            this.btnThemeToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemeToggle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnThemeToggle.Location = new System.Drawing.Point(200, 20);
            this.btnThemeToggle.Name = "btnThemeToggle";
            this.btnThemeToggle.Size = new System.Drawing.Size(100, 40);
            this.btnThemeToggle.TabIndex = 1;
            this.btnThemeToggle.Text = "☾/☀";
            this.btnThemeToggle.Click += new System.EventHandler(this.btnThemeToggle_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(147, 32);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "المكتبة الذكية";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 80);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelSidebar);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.Panel1MinSize = 250;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowRecentBooks);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(20);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.Size = new System.Drawing.Size(1000, 598);
            this.splitContainer1.SplitterDistance = 806;
            this.splitContainer1.TabIndex = 0;
            // 
            // panelSidebar
            // 
            this.panelSidebar.Controls.Add(this.panelStats);
            this.panelSidebar.Controls.Add(this.btnReader);
            this.panelSidebar.Controls.Add(this.btnLibrary);
            this.panelSidebar.Controls.Add(this.btnSearch);
            this.panelSidebar.Controls.Add(this.btnDashboard);
            this.panelSidebar.Controls.Add(this.panelUser);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Padding = new System.Windows.Forms.Padding(10);
            this.panelSidebar.Size = new System.Drawing.Size(806, 598);
            this.panelSidebar.TabIndex = 0;
            // 
            // panelStats
            // 
            this.panelStats.Controls.Add(this.lblReadingBooks);
            this.panelStats.Controls.Add(this.label3);
            this.panelStats.Controls.Add(this.lblTotalBooks);
            this.panelStats.Controls.Add(this.label1);
            this.panelStats.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStats.Location = new System.Drawing.Point(10, 438);
            this.panelStats.Name = "panelStats";
            this.panelStats.Size = new System.Drawing.Size(786, 150);
            this.panelStats.TabIndex = 0;
            // 
            // lblReadingBooks
            // 
            this.lblReadingBooks.AutoSize = true;
            this.lblReadingBooks.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblReadingBooks.Location = new System.Drawing.Point(120, 45);
            this.lblReadingBooks.Name = "lblReadingBooks";
            this.lblReadingBooks.Size = new System.Drawing.Size(23, 25);
            this.lblReadingBooks.TabIndex = 0;
            this.lblReadingBooks.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(120, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "قيد القراءة:";
            // 
            // lblTotalBooks
            // 
            this.lblTotalBooks.AutoSize = true;
            this.lblTotalBooks.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalBooks.Location = new System.Drawing.Point(20, 45);
            this.lblTotalBooks.Name = "lblTotalBooks";
            this.lblTotalBooks.Size = new System.Drawing.Size(23, 25);
            this.lblTotalBooks.TabIndex = 2;
            this.lblTotalBooks.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "مجموع الكتب:";
            // 
            // btnReader
            // 
            this.btnReader.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReader.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReader.FlatAppearance.BorderSize = 0;
            this.btnReader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReader.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnReader.ForeColor = System.Drawing.Color.White;
            this.btnReader.Location = new System.Drawing.Point(10, 270);
            this.btnReader.Name = "btnReader";
            this.btnReader.Size = new System.Drawing.Size(786, 50);
            this.btnReader.TabIndex = 1;
            this.btnReader.Text = "▶ قراءة كتاب";
            this.btnReader.Click += new System.EventHandler(this.btnReader_Click);
            this.btnReader.MouseEnter += new System.EventHandler(this.SidebarButton_MouseEnter);
            this.btnReader.MouseLeave += new System.EventHandler(this.SidebarButton_MouseLeave);
            // 
            // btnLibrary
            // 
            this.btnLibrary.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLibrary.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLibrary.FlatAppearance.BorderSize = 0;
            this.btnLibrary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLibrary.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnLibrary.ForeColor = System.Drawing.Color.White;
            this.btnLibrary.Location = new System.Drawing.Point(10, 220);
            this.btnLibrary.Name = "btnLibrary";
            this.btnLibrary.Size = new System.Drawing.Size(786, 50);
            this.btnLibrary.TabIndex = 2;
            this.btnLibrary.Text = "■ مكتبتي";
            this.btnLibrary.Click += new System.EventHandler(this.btnLibrary_Click);
            this.btnLibrary.MouseEnter += new System.EventHandler(this.SidebarButton_MouseEnter);
            this.btnLibrary.MouseLeave += new System.EventHandler(this.SidebarButton_MouseLeave);
            // 
            // btnSearch
            // 
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(10, 170);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(786, 50);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "⚫ البحث عن كتب";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnSearch.MouseEnter += new System.EventHandler(this.SidebarButton_MouseEnter);
            this.btnSearch.MouseLeave += new System.EventHandler(this.SidebarButton_MouseLeave);
            // 
            // btnDashboard
            // 
            this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(10, 110);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(786, 60);
            this.btnDashboard.TabIndex = 4;
            this.btnDashboard.Text = "◆ الرئيسية";
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            this.btnDashboard.MouseEnter += new System.EventHandler(this.SidebarButton_MouseEnter);
            this.btnDashboard.MouseLeave += new System.EventHandler(this.SidebarButton_MouseLeave);
            // 
            // panelUser
            // 
            this.panelUser.Controls.Add(this.labelUser);
            this.panelUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUser.Location = new System.Drawing.Point(10, 10);
            this.panelUser.Name = "panelUser";
            this.panelUser.Size = new System.Drawing.Size(786, 100);
            this.panelUser.TabIndex = 5;
            // 
            // labelUser
            // 
            this.labelUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUser.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelUser.Location = new System.Drawing.Point(0, 0);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(786, 100);
            this.labelUser.TabIndex = 0;
            this.labelUser.Text = "مرحباً بك";
            this.labelUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowRecentBooks
            // 
            this.flowRecentBooks.AutoScroll = true;
            this.flowRecentBooks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowRecentBooks.Location = new System.Drawing.Point(20, 20);
            this.flowRecentBooks.Name = "flowRecentBooks";
            this.flowRecentBooks.Size = new System.Drawing.Size(150, 558);
            this.flowRecentBooks.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 678);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1000, 22);
            this.statusStrip1.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "المكتبة الذكية";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelWeather.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelSidebar.ResumeLayout(false);
            this.panelStats.ResumeLayout(false);
            this.panelStats.PerformLayout();
            this.panelUser.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelWeather;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTemperature;
        private System.Windows.Forms.Label lblWeatherCondition;
        private System.Windows.Forms.Button btnThemeToggle;
        
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelUser;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnLibrary;
        private System.Windows.Forms.Button btnReader;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalBooks;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblReadingBooks;
        
        private System.Windows.Forms.FlowLayoutPanel flowRecentBooks;
        
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}
