namespace InfinniPlatform.QueryDesigner.Forms
{
	partial class QueryExecutorForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            InfinniPlatform.QueryDesigner.Contracts.Implementation.QueryExecutor queryExecutor2 = new InfinniPlatform.QueryDesigner.Contracts.Implementation.QueryExecutor();
            this.queryExecutor1 = new InfinniPlatform.QueryDesigner.Views.QueryExecutor();
            this.SuspendLayout();
            // 
            // queryExecutor1
            // 
            this.queryExecutor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryExecutor1.IndexQueryExecutor = queryExecutor2;
            this.queryExecutor1.Location = new System.Drawing.Point(0, 0);
            this.queryExecutor1.Name = "queryExecutor1";
            this.queryExecutor1.QueryText = "";
            this.queryExecutor1.Size = new System.Drawing.Size(942, 649);
            this.queryExecutor1.TabIndex = 0;
            // 
            // QueryExecutorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 649);
            this.Controls.Add(this.queryExecutor1);
            this.Name = "QueryExecutorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Query executor";
            this.ResumeLayout(false);

		}

		#endregion

        private Views.QueryExecutor queryExecutor1;
	}
}