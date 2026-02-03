namespace smartLibraryForC_.Views
{
    partial class ReaderForm
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
            this.lblBookTitle = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnGoToPage = new System.Windows.Forms.Button();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblBookTitle
            // 
            this.lblBookTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBookTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBookTitle.Location = new System.Drawing.Point(0, 0);
            this.lblBookTitle.Name = "lblBookTitle";
            this.lblBookTitle.Size = new System.Drawing.Size(1000, 30);
            this.lblBookTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAuthor
            // 
            this.lblAuthor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAuthor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAuthor.ForeColor = System.Drawing.Color.Gray;
            this.lblAuthor.Location = new System.Drawing.Point(0, 30);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(1000, 25);
            this.lblAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelProgress
            // 
            this.panelProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelProgress.Height = 60;
            this.panelProgress.Padding = new System.Windows.Forms.Padding(10);
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Location = new System.Drawing.Point(400, 15);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(100, 20);
            this.lblPageInfo.Text = "صفحة 1 من 0";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(520, 15);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(80, 20);
            this.lblProgress.Text = "التقدم: 0%";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 670);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1000, 30);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatus.ForeColor = System.Drawing.Color.Blue;
            // 
            // btnPrev
            // 
            this.btnPrev.Text = "الصفحة السابقة";
            this.btnPrev.Size = new System.Drawing.Size(120, 35);
            this.btnPrev.Location = new System.Drawing.Point(200, 10);
            this.btnPrev.BackColor = System.Drawing.Color.LightBlue;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Text = "الصفحة التالية";
            this.btnNext.Size = new System.Drawing.Size(120, 35);
            this.btnNext.Location = new System.Drawing.Point(330, 10);
            this.btnNext.BackColor = System.Drawing.Color.LightBlue;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Text = "فتح في برنامج آخر";
            this.btnOpen.Size = new System.Drawing.Size(130, 35);
            this.btnOpen.Location = new System.Drawing.Point(20, 10);
            this.btnOpen.BackColor = System.Drawing.Color.LightGreen;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnGoToPage
            // 
            this.btnGoToPage.Text = "انتقل لصفحة...";
            this.btnGoToPage.Size = new System.Drawing.Size(110, 35);
            this.btnGoToPage.Location = new System.Drawing.Point(630, 10);
            this.btnGoToPage.BackColor = System.Drawing.Color.LightYellow;
            this.btnGoToPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGoToPage.Click += new System.EventHandler(this.btnGoToPage_Click);
            // 
            // ReaderForm
            // 
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.lblBookTitle);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panelProgress);
            this.panelProgress.Controls.Add(this.btnPrev);
            this.panelProgress.Controls.Add(this.btnNext);
            this.panelProgress.Controls.Add(this.btnOpen);
            this.panelProgress.Controls.Add(this.btnGoToPage);
            this.panelProgress.Controls.Add(this.lblPageInfo);
            this.panelProgress.Controls.Add(this.lblProgress);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "ReaderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "القارئ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Load += new System.EventHandler(this.ReaderForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReaderForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblBookTitle;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnGoToPage;
        private System.Windows.Forms.Panel panelProgress;
    }
}
