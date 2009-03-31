namespace Europlan.Application {
	partial class ProjectSummaryPanel {
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
			this.lblProjectName = new System.Windows.Forms.Label();
			this.lblContact = new System.Windows.Forms.Label();
			this.lblNotes = new System.Windows.Forms.Label();
			this.lblCreated = new System.Windows.Forms.Label();
			this.lblChanged = new System.Windows.Forms.Label();
			this.lblEditor = new System.Windows.Forms.Label();
			this.txtProjectName = new System.Windows.Forms.TextBox();
			this.txtContact = new System.Windows.Forms.TextBox();
			this.txtNotes = new System.Windows.Forms.TextBox();
			this.dateCreated = new System.Windows.Forms.DateTimePicker();
			this.dateChanged = new System.Windows.Forms.DateTimePicker();
			this.txtEditor = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lblProjectName
			// 
			this.lblProjectName.Location = new System.Drawing.Point(3, 0);
			this.lblProjectName.Name = "lblProjectName";
			this.lblProjectName.Size = new System.Drawing.Size(120, 23);
			this.lblProjectName.TabIndex = 0;
			this.lblProjectName.Text = "Bauvorhaben:";
			// 
			// lblContact
			// 
			this.lblContact.Location = new System.Drawing.Point(3, 78);
			this.lblContact.Name = "lblContact";
			this.lblContact.Size = new System.Drawing.Size(120, 23);
			this.lblContact.TabIndex = 1;
			this.lblContact.Text = "Kontaktadresse:";
			// 
			// lblNotes
			// 
			this.lblNotes.Location = new System.Drawing.Point(3, 153);
			this.lblNotes.Name = "lblNotes";
			this.lblNotes.Size = new System.Drawing.Size(120, 23);
			this.lblNotes.TabIndex = 2;
			this.lblNotes.Text = "Bemerkung:";
			// 
			// lblCreated
			// 
			this.lblCreated.Location = new System.Drawing.Point(3, 228);
			this.lblCreated.Name = "lblCreated";
			this.lblCreated.Size = new System.Drawing.Size(120, 23);
			this.lblCreated.TabIndex = 3;
			this.lblCreated.Text = "Erstellungsdatum:";
			// 
			// lblChanged
			// 
			this.lblChanged.Location = new System.Drawing.Point(3, 254);
			this.lblChanged.Name = "lblChanged";
			this.lblChanged.Size = new System.Drawing.Size(120, 23);
			this.lblChanged.TabIndex = 4;
			this.lblChanged.Text = "Letzte Änderung:";
			// 
			// lblEditor
			// 
			this.lblEditor.Location = new System.Drawing.Point(4, 280);
			this.lblEditor.Name = "lblEditor";
			this.lblEditor.Size = new System.Drawing.Size(120, 23);
			this.lblEditor.TabIndex = 5;
			this.lblEditor.Text = "Sachbearbeiter:";
			// 
			// txtProjectName
			// 
			this.txtProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProjectName.Location = new System.Drawing.Point(130, 0);
			this.txtProjectName.Multiline = true;
			this.txtProjectName.Name = "txtProjectName";
			this.txtProjectName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtProjectName.Size = new System.Drawing.Size(442, 69);
			this.txtProjectName.TabIndex = 6;
			this.txtProjectName.TextChanged += new System.EventHandler(this.txtProjectName_TextChanged);
			// 
			// txtContact
			// 
			this.txtContact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtContact.Location = new System.Drawing.Point(130, 75);
			this.txtContact.Multiline = true;
			this.txtContact.Name = "txtContact";
			this.txtContact.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtContact.Size = new System.Drawing.Size(442, 69);
			this.txtContact.TabIndex = 7;
			this.txtContact.TextChanged += new System.EventHandler(this.txtContact_TextChanged);
			// 
			// txtNotes
			// 
			this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNotes.Location = new System.Drawing.Point(130, 150);
			this.txtNotes.Multiline = true;
			this.txtNotes.Name = "txtNotes";
			this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtNotes.Size = new System.Drawing.Size(442, 69);
			this.txtNotes.TabIndex = 8;
			this.txtNotes.TextChanged += new System.EventHandler(this.txtNotes_TextChanged);
			// 
			// dateCreated
			// 
			this.dateCreated.CustomFormat = "";
			this.dateCreated.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateCreated.Location = new System.Drawing.Point(130, 225);
			this.dateCreated.Name = "dateCreated";
			this.dateCreated.Size = new System.Drawing.Size(125, 20);
			this.dateCreated.TabIndex = 9;
			// 
			// dateChanged
			// 
			this.dateChanged.CustomFormat = "";
			this.dateChanged.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateChanged.Location = new System.Drawing.Point(130, 251);
			this.dateChanged.Name = "dateChanged";
			this.dateChanged.Size = new System.Drawing.Size(125, 20);
			this.dateChanged.TabIndex = 10;
			// 
			// txtEditor
			// 
			this.txtEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtEditor.Location = new System.Drawing.Point(130, 277);
			this.txtEditor.Name = "txtEditor";
			this.txtEditor.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtEditor.Size = new System.Drawing.Size(442, 20);
			this.txtEditor.TabIndex = 11;
			this.txtEditor.TextChanged += new System.EventHandler(this.txtEditor_TextChanged);
			// 
			// ProjectSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.txtEditor);
			this.Controls.Add(this.dateChanged);
			this.Controls.Add(this.dateCreated);
			this.Controls.Add(this.txtNotes);
			this.Controls.Add(this.txtContact);
			this.Controls.Add(this.txtProjectName);
			this.Controls.Add(this.lblEditor);
			this.Controls.Add(this.lblChanged);
			this.Controls.Add(this.lblCreated);
			this.Controls.Add(this.lblNotes);
			this.Controls.Add(this.lblContact);
			this.Controls.Add(this.lblProjectName);
			this.Name = "ProjectSummaryPanel";
			this.Size = new System.Drawing.Size(575, 390);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblProjectName;
		private System.Windows.Forms.Label lblContact;
		private System.Windows.Forms.Label lblNotes;
		private System.Windows.Forms.Label lblCreated;
		private System.Windows.Forms.Label lblChanged;
		private System.Windows.Forms.Label lblEditor;
		private System.Windows.Forms.TextBox txtProjectName;
		private System.Windows.Forms.TextBox txtContact;
		private System.Windows.Forms.TextBox txtNotes;
		private System.Windows.Forms.DateTimePicker dateCreated;
		private System.Windows.Forms.DateTimePicker dateChanged;
		private System.Windows.Forms.TextBox txtEditor;

	}
}
