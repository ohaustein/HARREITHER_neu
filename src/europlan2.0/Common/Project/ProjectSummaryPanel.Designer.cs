namespace Europlan.Common {
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
			this.txtEditor = new System.Windows.Forms.TextBox();
			this.txtCreated = new System.Windows.Forms.TextBox();
			this.txtChanged = new System.Windows.Forms.TextBox();
			this.txtNumber = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnNext = new System.Windows.Forms.Button();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.SuspendLayout();
			// 
			// lblProjectName
			// 
			this.lblProjectName.Location = new System.Drawing.Point(3, 66);
			this.lblProjectName.Name = "lblProjectName";
			this.lblProjectName.Size = new System.Drawing.Size(120, 23);
			this.lblProjectName.TabIndex = 0;
			this.lblProjectName.Text = "Bauvorhaben:";
			// 
			// lblContact
			// 
			this.lblContact.Location = new System.Drawing.Point(4, 144);
			this.lblContact.Name = "lblContact";
			this.lblContact.Size = new System.Drawing.Size(120, 23);
			this.lblContact.TabIndex = 1;
			this.lblContact.Text = "Kontaktadresse:";
			// 
			// lblNotes
			// 
			this.lblNotes.Location = new System.Drawing.Point(4, 219);
			this.lblNotes.Name = "lblNotes";
			this.lblNotes.Size = new System.Drawing.Size(120, 23);
			this.lblNotes.TabIndex = 2;
			this.lblNotes.Text = "Bemerkung:";
			// 
			// lblCreated
			// 
			this.lblCreated.Location = new System.Drawing.Point(4, 294);
			this.lblCreated.Name = "lblCreated";
			this.lblCreated.Size = new System.Drawing.Size(120, 23);
			this.lblCreated.TabIndex = 3;
			this.lblCreated.Text = "Erstellungsdatum:";
			// 
			// lblChanged
			// 
			this.lblChanged.Location = new System.Drawing.Point(4, 320);
			this.lblChanged.Name = "lblChanged";
			this.lblChanged.Size = new System.Drawing.Size(120, 23);
			this.lblChanged.TabIndex = 4;
			this.lblChanged.Text = "Letzte Änderung:";
			// 
			// lblEditor
			// 
			this.lblEditor.Location = new System.Drawing.Point(5, 346);
			this.lblEditor.Name = "lblEditor";
			this.lblEditor.Size = new System.Drawing.Size(120, 23);
			this.lblEditor.TabIndex = 5;
			this.lblEditor.Text = "Sachbearbeiter:";
			// 
			// txtProjectName
			// 
			this.txtProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProjectName.Location = new System.Drawing.Point(130, 66);
			this.txtProjectName.Multiline = true;
			this.txtProjectName.Name = "txtProjectName";
			this.txtProjectName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtProjectName.Size = new System.Drawing.Size(583, 69);
			this.txtProjectName.TabIndex = 6;
			this.txtProjectName.TextChanged += new System.EventHandler(this.txtProjectName_TextChanged);
			// 
			// txtContact
			// 
			this.txtContact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtContact.Location = new System.Drawing.Point(131, 141);
			this.txtContact.Multiline = true;
			this.txtContact.Name = "txtContact";
			this.txtContact.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtContact.Size = new System.Drawing.Size(583, 69);
			this.txtContact.TabIndex = 7;
			this.txtContact.TextChanged += new System.EventHandler(this.txtContact_TextChanged);
			// 
			// txtNotes
			// 
			this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNotes.Location = new System.Drawing.Point(131, 216);
			this.txtNotes.Multiline = true;
			this.txtNotes.Name = "txtNotes";
			this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtNotes.Size = new System.Drawing.Size(583, 69);
			this.txtNotes.TabIndex = 8;
			this.txtNotes.TextChanged += new System.EventHandler(this.txtNotes_TextChanged);
			// 
			// txtEditor
			// 
			this.txtEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtEditor.Location = new System.Drawing.Point(131, 343);
			this.txtEditor.Name = "txtEditor";
			this.txtEditor.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtEditor.Size = new System.Drawing.Size(583, 20);
			this.txtEditor.TabIndex = 11;
			this.txtEditor.TextChanged += new System.EventHandler(this.txtEditor_TextChanged);
			// 
			// txtCreated
			// 
			this.txtCreated.Location = new System.Drawing.Point(130, 291);
			this.txtCreated.Name = "txtCreated";
			this.txtCreated.ReadOnly = true;
			this.txtCreated.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtCreated.Size = new System.Drawing.Size(168, 20);
			this.txtCreated.TabIndex = 12;
			// 
			// txtChanged
			// 
			this.txtChanged.Location = new System.Drawing.Point(130, 317);
			this.txtChanged.Name = "txtChanged";
			this.txtChanged.ReadOnly = true;
			this.txtChanged.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtChanged.Size = new System.Drawing.Size(168, 20);
			this.txtChanged.TabIndex = 13;
			// 
			// txtNumber
			// 
			this.txtNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNumber.Location = new System.Drawing.Point(130, 40);
			this.txtNumber.Name = "txtNumber";
			this.txtNumber.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtNumber.Size = new System.Drawing.Size(583, 20);
			this.txtNumber.TabIndex = 15;
			this.txtNumber.TextChanged += new System.EventHandler(this.txtNumber_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 43);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 23);
			this.label1.TabIndex = 14;
			this.label1.Text = "Projektnummer:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(126, 24);
			this.label2.TabIndex = 16;
			this.label2.Text = "Projektdaten";
			// 
			// btnNext
			// 
			this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNext.Location = new System.Drawing.Point(641, 447);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(75, 23);
			this.btnNext.TabIndex = 17;
			this.btnNext.Text = "Weiter";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "europlan.chm";
			// 
			// ProjectSummaryPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtNumber);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtChanged);
			this.Controls.Add(this.txtCreated);
			this.Controls.Add(this.txtEditor);
			this.Controls.Add(this.txtNotes);
			this.Controls.Add(this.txtContact);
			this.Controls.Add(this.txtProjectName);
			this.Controls.Add(this.lblEditor);
			this.Controls.Add(this.lblChanged);
			this.Controls.Add(this.lblCreated);
			this.Controls.Add(this.lblNotes);
			this.Controls.Add(this.lblContact);
			this.Controls.Add(this.lblProjectName);
			this.helpProvider.SetHelpKeyword(this, "html\\euro8p4l.htm");
			this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
			this.Name = "ProjectSummaryPanel";
			this.helpProvider.SetShowHelp(this, true);
			this.Size = new System.Drawing.Size(716, 473);
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
		private System.Windows.Forms.TextBox txtEditor;
		private System.Windows.Forms.TextBox txtCreated;
		private System.Windows.Forms.TextBox txtChanged;
		private System.Windows.Forms.TextBox txtNumber;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.HelpProvider helpProvider;

	}
}
