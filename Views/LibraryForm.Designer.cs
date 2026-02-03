namespace smartLibraryForC_.Views
{
    partial class LibraryForm
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
            this.flowBooks = new System.Windows.Forms.FlowLayoutPanel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnAddLocal = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowBooks
            // 
            this.flowBooks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowBooks.AutoScroll = true;
            this.flowBooks.Padding = new System.Windows.Forms.Padding(20);
            this.flowBooks.Location = new System.Drawing.Point(0, 60);
            this.flowBooks.Name = "flowBooks";
            this.flowBooks.Size = new System.Drawing.Size(800, 540);
            this.flowBooks.TabIndex = 0;
            // 
            // panelButtons
            // 
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Height = 60;
            this.panelButtons.Padding = new System.Windows.Forms.Padding(10);
            // 
            // btnAddLocal
            // 
            this.btnAddLocal.Text = "إضافة كتاب محلي +";
            this.btnAddLocal.Size = new System.Drawing.Size(150, 40);
            this.btnAddLocal.Location = new System.Drawing.Point(10, 10);
            this.btnAddLocal.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnAddLocal.ForeColor = System.Drawing.Color.White;
            this.btnAddLocal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddLocal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddLocal.Click += new System.EventHandler(this.btnAddLocal_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Text = "تحديث ↻";
            this.btnRefresh.Size = new System.Drawing.Size(100, 40);
            this.btnRefresh.Location = new System.Drawing.Point(170, 10);
            this.btnRefresh.BackColor = System.Drawing.Color.LightGreen;
            this.btnRefresh.ForeColor = System.Drawing.Color.Black;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkFavorites
            // 
            this.chkFavorites = new System.Windows.Forms.CheckBox();
            this.chkFavorites.AutoSize = true;
            this.chkFavorites.Location = new System.Drawing.Point(300, 20);
            this.chkFavorites.Name = "chkFavorites";
            this.chkFavorites.Size = new System.Drawing.Size(120, 23);
            this.chkFavorites.Text = "عرض المفضلة فقط";
            this.chkFavorites.ForeColor = System.Drawing.Color.Purple;
            this.chkFavorites.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkFavorites.CheckedChanged += new System.EventHandler(this.chkFavorites_CheckedChanged);
            // 
            // LibraryForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.flowBooks);
            this.Controls.Add(this.panelButtons);
            this.panelButtons.Controls.Add(this.btnAddLocal);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Controls.Add(this.chkFavorites);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "LibraryForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "مكتبتي";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.FlowLayoutPanel flowBooks;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnAddLocal;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox chkFavorites;
    }
}
