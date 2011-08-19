namespace Europlan.Common {
	partial class NewPlanForm {
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
			this.lblCaption = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.txtName = new System.Windows.Forms.TextBox();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.lblPage = new System.Windows.Forms.Label();
			this.numPage = new Europlan.Common.NumericBox();
			this.lblNumOfPages = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblCaption
			// 
			this.lblCaption.Location = new System.Drawing.Point(12, 9);
			this.lblCaption.Name = "lblCaption";
			this.lblCaption.Size = new System.Drawing.Size(100, 23);
			this.lblCaption.TabIndex = 2;
			this.lblCaption.Text = "Bezeichnung:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(313, 61);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Abbrechen";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(232, 61);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "&OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(118, 6);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(268, 20);
			this.txtName.TabIndex = 0;
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
			// 
			// lblPage
			// 
			this.lblPage.Location = new System.Drawing.Point(12, 35);
			this.lblPage.Name = "lblPage";
			this.lblPage.Size = new System.Drawing.Size(100, 23);
			this.lblPage.TabIndex = 3;
			this.lblPage.Text = "Seite aus PDF:";
			// 
			// numPage
			// 
			this.numPage.EditType = Europlan.Common.NumericBox.NumericEditType.DEFAULT;
			this.numPage.InternalValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numPage.Location = new System.Drawing.Point(118, 32);
			this.numPage.MaxValue = null;
			this.numPage.MinValue = null;
			this.numPage.Name = "numPage";
			this.numPage.Size = new System.Drawing.Size(268, 20);
			this.numPage.TabIndex = 4;
			this.numPage.Text = "1";
			this.numPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// lblNumOfPages
			// 
			this.lblNumOfPages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblNumOfPages.Location = new System.Drawing.Point(12, 61);
			this.lblNumOfPages.Name = "lblNumOfPages";
			this.lblNumOfPages.Size = new System.Drawing.Size(214, 23);
			this.lblNumOfPages.TabIndex = 5;
			// 
			// NewPlanForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(400, 96);
			this.Controls.Add(this.lblNumOfPages);
			this.Controls.Add(this.numPage);
			this.Controls.Add(this.lblPage);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblCaption);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.helpProvider.SetHelpKeyword(this, "html\\euro8ud1.htm");
			this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewPlanForm";
			this.helpProvider.SetShowHelp(this, true);
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Plan importieren";
			this.Load += new System.EventHandler(this.NewPlanForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewPlanForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblCaption;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.HelpProvider helpProvider;
		private System.Windows.Forms.Label lblPage;
		private NumericBox numPage;
		private System.Windows.Forms.Label lblNumOfPages;
	}
}