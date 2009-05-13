namespace Europlan.AdminApplication {
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.lblType = new System.Windows.Forms.Label();
			this.constructionEditor = new Europlan.AdminApplication.ConstructionEditor();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 357);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(667, 50);
			this.panel1.TabIndex = 1;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.lblType);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(667, 32);
			this.panel2.TabIndex = 2;
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
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "ConstructionEditorForm";
			this.Text = "ConstructionEditorForm";
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label lblType;
		private ConstructionEditor constructionEditor;

	}
}