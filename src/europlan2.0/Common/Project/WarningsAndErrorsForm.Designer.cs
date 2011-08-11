namespace Europlan.Common {
	partial class WarningsAndErrorsForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("fsdafasdfdas");
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("fdas");
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WarningsAndErrorsForm));
			this.btnClose = new System.Windows.Forms.Button();
			this.lstErrors = new System.Windows.Forms.ListView();
			this.columnText = new System.Windows.Forms.ColumnHeader();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(590, 359);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(105, 23);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "&Schlieﬂen";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// lstErrors
			// 
			this.lstErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnText});
			this.lstErrors.FullRowSelect = true;
			this.lstErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lstErrors.HideSelection = false;
			this.lstErrors.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
			this.lstErrors.Location = new System.Drawing.Point(12, 12);
			this.lstErrors.MultiSelect = false;
			this.lstErrors.Name = "lstErrors";
			this.lstErrors.ShowGroups = false;
			this.lstErrors.Size = new System.Drawing.Size(683, 341);
			this.lstErrors.SmallImageList = this.imageList;
			this.lstErrors.TabIndex = 0;
			this.lstErrors.UseCompatibleStateImageBehavior = false;
			this.lstErrors.View = System.Windows.Forms.View.Details;
			// 
			// columnText
			// 
			this.columnText.Text = global::Europlan.Common.EuroplanRes.Material_a834583d_750c_4695_ba45_7e346108551f;
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "error.png");
			this.imageList.Images.SetKeyName(1, "warning.png");
			// 
			// WarningsAndErrorsForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(707, 394);
			this.Controls.Add(this.lstErrors);
			this.Controls.Add(this.btnClose);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WarningsAndErrorsForm";
			this.Text = "Warnungen und Fehler";
			this.Load += new System.EventHandler(this.WarningsAndErrorsForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WarningsAndErrorsForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ListView lstErrors;
		private System.Windows.Forms.ColumnHeader columnText;
		private System.Windows.Forms.ImageList imageList;
	}
}