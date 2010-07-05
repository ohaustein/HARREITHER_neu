namespace Europlan.Common {
	partial class ConstructionEditorForm {
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
			this.panelBottom = new System.Windows.Forms.Panel();
			this.panelTop = new System.Windows.Forms.Panel();
			this.lblType = new System.Windows.Forms.Label();
			this.constructionEditor = new Europlan.Common.ConstructionEditor();
			this.panelTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelBottom
			// 
			this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelBottom.Location = new System.Drawing.Point(0, 357);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new System.Drawing.Size(667, 50);
			this.panelBottom.TabIndex = 1;
			this.panelBottom.Visible = false;
			// 
			// panelTop
			// 
			this.panelTop.Controls.Add(this.lblType);
			this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTop.Location = new System.Drawing.Point(0, 0);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new System.Drawing.Size(667, 32);
			this.panelTop.TabIndex = 2;
			// 
			// lblType
			// 
			this.lblType.AutoSize = true;
			this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblType.Location = new System.Drawing.Point(4, 4);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(57, 20);
			this.lblType.TabIndex = 0;
			this.lblType.Text = "label1";
			// 
			// constructionEditor
			// 
			this.constructionEditor.Construction = null;
			this.constructionEditor.DefaultConstructionScope = Europlan.Common.ConstructionScopeEnum.FloorConstruction;
			this.constructionEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.constructionEditor.Location = new System.Drawing.Point(0, 32);
			this.constructionEditor.Name = "constructionEditor";
			this.constructionEditor.Size = new System.Drawing.Size(667, 325);
			this.constructionEditor.TabIndex = 3;
			// 
			// ConstructionEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(667, 407);
			this.Controls.Add(this.constructionEditor);
			this.Controls.Add(this.panelTop);
			this.Controls.Add(this.panelBottom);
			this.Name = "ConstructionEditorForm";
			this.Text = "Konstruktion";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConstructionEditorForm_FormClosing_1);
			this.panelTop.ResumeLayout(false);
			this.panelTop.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.Label lblType;
		private ConstructionEditor constructionEditor;

	}
}