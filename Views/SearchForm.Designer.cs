namespace smartLibraryForC_.Views
{
    partial class SearchForm
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

        private System.Windows.Forms.TableLayoutPanel tableLayoutTop;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.FlowLayoutPanel flowResults;

        private void InitializeComponent()
        {
            this.tableLayoutTop = new System.Windows.Forms.TableLayoutPanel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.flowResults = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutTop
            // 
            this.tableLayoutTop.ColumnCount = 3;
            this.tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutTop.Controls.Add(this.txtSearch, 0, 0);
            this.tableLayoutTop.Controls.Add(this.btnSearch, 1, 0);
            this.tableLayoutTop.Controls.Add(this.lblStatus, 2, 0);
            this.tableLayoutTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutTop.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutTop.Name = "tableLayoutTop";
            this.tableLayoutTop.Padding = new System.Windows.Forms.Padding(20);
            this.tableLayoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutTop.Size = new System.Drawing.Size(800, 80);
            this.tableLayoutTop.TabIndex = 1;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSearch.Location = new System.Drawing.Point(463, 25);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(314, 29);
            this.txtSearch.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(343, 24);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(114, 32);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "بحث";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(23, 30);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 19);
            this.lblStatus.TabIndex = 2;
            // 
            // flowResults
            // 
            this.flowResults.AutoScroll = true;
            this.flowResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowResults.Location = new System.Drawing.Point(0, 80);
            this.flowResults.Name = "flowResults";
            this.flowResults.Padding = new System.Windows.Forms.Padding(20);
            this.flowResults.Size = new System.Drawing.Size(800, 520);
            this.flowResults.TabIndex = 0;
            // 
            // SearchForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.flowResults);
            this.Controls.Add(this.tableLayoutTop);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "SearchForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "البحث عن كتب";
            this.tableLayoutTop.ResumeLayout(false);
            this.tableLayoutTop.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
