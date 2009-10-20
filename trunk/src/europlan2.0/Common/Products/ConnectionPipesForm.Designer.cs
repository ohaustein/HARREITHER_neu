namespace Europlan.Common {
	partial class ConnectionPipesForm {
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
			this.btnOk = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.connectionPipePanel = new Europlan.Common.ConnectionPipePanel();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(715, 438);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(796, 438);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Abbrechen";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// connectionPipePanel
			// 
			this.connectionPipePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.connectionPipePanel.Location = new System.Drawing.Point(12, 39);
			this.connectionPipePanel.Name = "connectionPipePanel";
			this.connectionPipePanel.ShowPipesThroughProduct = true;
			this.connectionPipePanel.Size = new System.Drawing.Size(859, 367);
			this.connectionPipePanel.TabIndex = 4;
			this.connectionPipePanel.GridContentChanged += new Europlan.Common.GridContentChangedHandler(this.connectionPipePanel_GridContentChanged);
			// 
			// ConnectionPipesForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(883, 473);
			this.Controls.Add(this.connectionPipePanel);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.btnOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ConnectionPipesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Anbindeleitungen";
			this.Load += new System.EventHandler(this.ConnectionPipesForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConnectionPipesForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button button2;
		private ConnectionPipePanel connectionPipePanel;
	}
}