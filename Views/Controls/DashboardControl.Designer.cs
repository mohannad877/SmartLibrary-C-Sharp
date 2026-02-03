namespace smartLibraryForC_.Views.Controls
{
    partial class DashboardControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelTotal = new System.Windows.Forms.Panel();
            this.lblTotalBooks = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelReading = new System.Windows.Forms.Panel();
            this.lblReading = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelCompleted = new System.Windows.Forms.Panel();
            this.lblCompleted = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panelFavorites = new System.Windows.Forms.Panel();
            this.lblFavorites = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.grpActivity = new System.Windows.Forms.GroupBox();
            this.flowActivity = new System.Windows.Forms.FlowLayoutPanel();

            this.tableLayoutPanel1.SuspendLayout();
            this.panelTotal.SuspendLayout();
            this.panelReading.SuspendLayout();
            this.panelCompleted.SuspendLayout();
            this.panelFavorites.SuspendLayout();
            this.grpActivity.SuspendLayout();
            this.SuspendLayout();

            // 
            // DashboardControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Size = new System.Drawing.Size(800, 600);
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;

            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panelTotal, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelReading, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelCompleted, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelFavorites, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Height = 400;
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(20);

            // 
            // panelTotal
            // 
            this.panelTotal.Controls.Add(this.lblTotalBooks);
            this.panelTotal.Controls.Add(this.label1);
            this.panelTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTotal.Margin = new System.Windows.Forms.Padding(10);
            this.panelTotal.Name = "panelCardTotal";
            this.panelTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTotal.Padding = new System.Windows.Forms.Padding(5);

            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Text = "إجمالي الكتب";

            // 
            // lblTotalBooks
            // 
            this.lblTotalBooks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalBooks.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold);
            this.lblTotalBooks.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblTotalBooks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTotalBooks.Text = "0";

            // 
            // panelReading
            // 
            this.panelReading.Controls.Add(this.lblReading);
            this.panelReading.Controls.Add(this.label3);
            this.panelReading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelReading.Margin = new System.Windows.Forms.Padding(10);
            this.panelReading.Name = "panelCardReading";
            this.panelReading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelReading.Padding = new System.Windows.Forms.Padding(5);

            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(20, 20);
            this.label3.Text = "قيد القراءة";

            // 
            // lblReading
            // 
            this.lblReading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReading.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold);
            this.lblReading.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblReading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblReading.Text = "0";

            // 
            // panelCompleted
            // 
            this.panelCompleted.Controls.Add(this.lblCompleted);
            this.panelCompleted.Controls.Add(this.label5);
            this.panelCompleted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCompleted.Margin = new System.Windows.Forms.Padding(10);
            this.panelCompleted.Name = "panelCardCompleted";
            this.panelCompleted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCompleted.Padding = new System.Windows.Forms.Padding(5);

            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(20, 20);
            this.label5.Text = "مكتملة";

            // 
            // lblCompleted
            // 
            this.lblCompleted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCompleted.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold);
            this.lblCompleted.ForeColor = System.Drawing.Color.Crimson;
            this.lblCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCompleted.Text = "0";

            // 
            // panelFavorites
            // 
            this.panelFavorites.Controls.Add(this.lblFavorites);
            this.panelFavorites.Controls.Add(this.label7);
            this.panelFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFavorites.Margin = new System.Windows.Forms.Padding(10);
            this.panelFavorites.Name = "panelCardFavorites";
            this.panelFavorites.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFavorites.Padding = new System.Windows.Forms.Padding(5);

            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(20, 20);
            this.label7.Text = "المفضلة";

            // 
            // lblFavorites
            // 
            this.lblFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFavorites.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold);
            this.lblFavorites.ForeColor = System.Drawing.Color.Purple;
            this.lblFavorites.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblFavorites.Text = "0";

            // 
            // grpActivity
            // 
            this.grpActivity.Controls.Add(this.flowActivity);
            this.grpActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpActivity.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.grpActivity.Location = new System.Drawing.Point(0, 400);
            this.grpActivity.Padding = new System.Windows.Forms.Padding(20);
            this.grpActivity.Text = "آخر النشاطات";
            this.grpActivity.Name = "grpActivity";

            // 
            // flowActivity
            // 
            this.flowActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowActivity.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowActivity.AutoScroll = true;
            this.flowActivity.Name = "flowActivity";

            // 
            // Controls
            // 
            this.Controls.Add(this.grpActivity);
            this.Controls.Add(this.tableLayoutPanel1);

            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelTotal.ResumeLayout(false);
            this.panelTotal.PerformLayout();
            this.panelReading.ResumeLayout(false);
            this.panelReading.PerformLayout();
            this.panelCompleted.ResumeLayout(false);
            this.panelCompleted.PerformLayout();
            this.panelFavorites.ResumeLayout(false);
            this.panelFavorites.PerformLayout();
            this.grpActivity.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelTotal;
        private System.Windows.Forms.Label lblTotalBooks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelReading;
        private System.Windows.Forms.Label lblReading;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelCompleted;
        private System.Windows.Forms.Label lblCompleted;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panelFavorites;
        private System.Windows.Forms.Label lblFavorites;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox grpActivity;
        private System.Windows.Forms.FlowLayoutPanel flowActivity;
    }
}
