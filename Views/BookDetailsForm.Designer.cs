namespace smartLibraryForC_.Views
{
    partial class BookDetailsForm
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
            this.pbCover = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnAction = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.lblPages = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            
            ((System.ComponentModel.ISupportInitialize)(this.pbCover)).BeginInit();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();

            // 
            // pbCover
            // 
            this.pbCover.Location = new System.Drawing.Point(30, 30);
            this.pbCover.Size = new System.Drawing.Size(200, 300);
            this.pbCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(250, 30);
            this.lblTitle.Size = new System.Drawing.Size(400, 40);
            this.lblTitle.Text = "Book Title";

            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblAuthor.ForeColor = System.Drawing.Color.Gray;
            this.lblAuthor.Location = new System.Drawing.Point(250, 80);
            this.lblAuthor.Text = "Author Name";

            // 
            // panelInfo
            // 
            this.panelInfo.Controls.Add(this.lblYear);
            this.panelInfo.Controls.Add(this.label4);
            this.panelInfo.Controls.Add(this.lblPages);
            this.panelInfo.Controls.Add(this.label2);
            this.panelInfo.Location = new System.Drawing.Point(250, 120);
            this.panelInfo.Size = new System.Drawing.Size(400, 40);

            // 
            // label2 (Pages Label)
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(0, 10);
            this.label2.Text = "عدد الصفحات:";
            
            // 
            // lblPages
            // 
            this.lblPages.AutoSize = true;
            this.lblPages.Location = new System.Drawing.Point(100, 10);
            this.lblPages.Text = "000";

            // 
            // label4 (Year Label)
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(200, 10);
            this.label4.Text = "سنة النشر:";

            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(280, 10);
            this.lblYear.Text = "0000";

            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(250, 180);
            this.txtDescription.Size = new System.Drawing.Size(500, 150);
            this.txtDescription.Multiline = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.ReadOnly = true;
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescription.Text = "Description goes here...";

            // 
            // btnAction
            // 
            this.btnAction.Location = new System.Drawing.Point(250, 350);
            this.btnAction.Size = new System.Drawing.Size(150, 40);
            this.btnAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAction.ForeColor = System.Drawing.Color.White;
            this.btnAction.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnAction.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAction.Text = "تحميل";

            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(420, 350);
            this.btnRemove.Size = new System.Drawing.Size(150, 40);
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.ForeColor = System.Drawing.Color.Red;
            this.btnRemove.Text = "حذف من المكتبة";
            this.btnRemove.Visible = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(250, 400);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 19);
            this.lblStatus.Visible = false;

            // 
            // btnFavorite
            // 
            this.btnFavorite = new System.Windows.Forms.Button();
            this.btnFavorite.Location = new System.Drawing.Point(590, 350);
            this.btnFavorite.Size = new System.Drawing.Size(50, 40);
            this.btnFavorite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFavorite.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.btnFavorite.Text = "♡";
            this.btnFavorite.ForeColor = System.Drawing.Color.Red;
            this.btnFavorite.Click += new System.EventHandler(this.btnFavorite_Click);

            // 
            // BookDetailsForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnFavorite);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAction);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbCover);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "BookDetailsForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "تفاصيل الكتاب";

            ((System.ComponentModel.ISupportInitialize)(this.pbCover)).EndInit();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.PictureBox pbCover;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnFavorite;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label lblPages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblStatus;
    }
}
