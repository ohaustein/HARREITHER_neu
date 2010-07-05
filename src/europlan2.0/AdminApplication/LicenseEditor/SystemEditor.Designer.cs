namespace Europlan.AdminApplication {
	partial class SystemEditor {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.gbxEditSystem = new System.Windows.Forms.GroupBox();
			this.lblAnnotation = new System.Windows.Forms.Label();
			this.lblHardwareId = new System.Windows.Forms.Label();
			this.txtAnnotation = new System.Windows.Forms.TextBox();
			this.dtpAdded = new System.Windows.Forms.DateTimePicker();
			this.lblAdded = new System.Windows.Forms.Label();
			this.txtHardwareId = new System.Windows.Forms.TextBox();
			this.gbxEditSystem.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbxEditSystem
			// 
			this.gbxEditSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbxEditSystem.Controls.Add(this.lblAnnotation);
			this.gbxEditSystem.Controls.Add(this.lblHardwareId);
			this.gbxEditSystem.Controls.Add(this.txtAnnotation);
			this.gbxEditSystem.Controls.Add(this.dtpAdded);
			this.gbxEditSystem.Controls.Add(this.lblAdded);
			this.gbxEditSystem.Controls.Add(this.txtHardwareId);
			this.gbxEditSystem.Location = new System.Drawing.Point(0, 0);
			this.gbxEditSystem.Name = "gbxEditSystem";
			this.gbxEditSystem.Size = new System.Drawing.Size(407, 98);
			this.gbxEditSystem.TabIndex = 31;
			this.gbxEditSystem.TabStop = false;
			this.gbxEditSystem.Text = "Ausgewählten Rechner bearbeiten";
			// 
			// lblAnnotation
			// 
			this.lblAnnotation.AutoSize = true;
			this.lblAnnotation.Location = new System.Drawing.Point(6, 74);
			this.lblAnnotation.Name = "lblAnnotation";
			this.lblAnnotation.Size = new System.Drawing.Size(64, 13);
			this.lblAnnotation.TabIndex = 30;
			this.lblAnnotation.Text = "Anmerkung:";
			// 
			// lblHardwareId
			// 
			this.lblHardwareId.AutoSize = true;
			this.lblHardwareId.Location = new System.Drawing.Point(6, 48);
			this.lblHardwareId.Name = "lblHardwareId";
			this.lblHardwareId.Size = new System.Drawing.Size(70, 13);
			this.lblHardwareId.TabIndex = 29;
			this.lblHardwareId.Text = "Hardware ID:";
			// 
			// txtAnnotation
			// 
			this.txtAnnotation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtAnnotation.Location = new System.Drawing.Point(96, 71);
			this.txtAnnotation.Name = "txtAnnotation";
			this.txtAnnotation.Size = new System.Drawing.Size(305, 20);
			this.txtAnnotation.TabIndex = 28;
			this.txtAnnotation.TextChanged += new System.EventHandler(this.txtAnnotation_TextChanged);
			// 
			// dtpAdded
			// 
			this.dtpAdded.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dtpAdded.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpAdded.Location = new System.Drawing.Point(96, 19);
			this.dtpAdded.Name = "dtpAdded";
			this.dtpAdded.Size = new System.Drawing.Size(305, 20);
			this.dtpAdded.TabIndex = 27;
			this.dtpAdded.ValueChanged += new System.EventHandler(this.dtpAdded_ValueChanged);
			// 
			// lblAdded
			// 
			this.lblAdded.AutoSize = true;
			this.lblAdded.Location = new System.Drawing.Point(6, 22);
			this.lblAdded.Name = "lblAdded";
			this.lblAdded.Size = new System.Drawing.Size(84, 13);
			this.lblAdded.TabIndex = 26;
			this.lblAdded.Text = "Hinzugefügt am:";
			// 
			// txtHardwareId
			// 
			this.txtHardwareId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtHardwareId.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.txtHardwareId.Location = new System.Drawing.Point(96, 45);
			this.txtHardwareId.Name = "txtHardwareId";
			this.txtHardwareId.Size = new System.Drawing.Size(305, 20);
			this.txtHardwareId.TabIndex = 25;
			this.txtHardwareId.TextChanged += new System.EventHandler(this.txtHardwareId_TextChanged);
			this.txtHardwareId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHardwareId_KeyDown);
			// 
			// SystemEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gbxEditSystem);
			this.Name = "SystemEditor";
			this.Size = new System.Drawing.Size(407, 98);
			this.gbxEditSystem.ResumeLayout(false);
			this.gbxEditSystem.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbxEditSystem;
		private System.Windows.Forms.Label lblAnnotation;
		private System.Windows.Forms.Label lblHardwareId;
		private System.Windows.Forms.TextBox txtAnnotation;
		private System.Windows.Forms.DateTimePicker dtpAdded;
		private System.Windows.Forms.Label lblAdded;
		private System.Windows.Forms.TextBox txtHardwareId;
	}
}
