using System;

partial class FormException : System.Windows.Forms.Form
{

    protected override void Dispose(bool disposing)
    {
        try
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
        }
        finally
        {
            base.Dispose(disposing);
        }
    }


    private void InitializeComponent()
    {
        components = null;
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormException));
        this.lblMeldung = new System.Windows.Forms.Label();
        this.tblWerte = new System.Windows.Forms.TableLayoutPanel();
        this.pnlWerte = new System.Windows.Forms.Panel();
        this.tblInhalt = new System.Windows.Forms.TableLayoutPanel();
        this.PictureBox1 = new System.Windows.Forms.PictureBox();
        this.pnlUnten = new System.Windows.Forms.TableLayoutPanel();
        this.btnOK = new System.Windows.Forms.Button();
        this.pnlInnerException = new System.Windows.Forms.TableLayoutPanel();
        this.lnkInnerException = new System.Windows.Forms.LinkLabel();
        this.lblInnerException = new System.Windows.Forms.Label();
        this.btnStop = new System.Windows.Forms.Button();
        this.btnKopieren = new System.Windows.Forms.Button();
        this.imgSymbole = new System.Windows.Forms.ImageList();
        this.btnSpeichern = new System.Windows.Forms.Button();
        this.pnlStackTrace = new System.Windows.Forms.TableLayoutPanel();
        this.lblStackTrace = new System.Windows.Forms.Label();
        this.pnlWerte.SuspendLayout();
        this.tblInhalt.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
        this.pnlUnten.SuspendLayout();
        this.pnlInnerException.SuspendLayout();
        this.pnlStackTrace.SuspendLayout();
        this.SuspendLayout();
        // 
        // lblMeldung
        // 
        this.lblMeldung.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
        this.lblMeldung.AutoSize = true;
        this.lblMeldung.Location = new System.Drawing.Point(67, 8);
        this.lblMeldung.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
        this.lblMeldung.Name = "lblMeldung";
        this.lblMeldung.Size = new System.Drawing.Size(522, 15);
        this.lblMeldung.TabIndex = 2;
        this.lblMeldung.Text = "[Meldung]";
        // 
        // tblWerte
        // 
        this.tblWerte.AutoSize = true;
        this.tblWerte.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tblWerte.ColumnCount = 2;
        this.tblWerte.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tblWerte.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tblWerte.Dock = System.Windows.Forms.DockStyle.Top;
        this.tblWerte.Location = new System.Drawing.Point(0, 0);
        this.tblWerte.Margin = new System.Windows.Forms.Padding(0);
        this.tblWerte.Name = "tblWerte";
        this.tblWerte.RowCount = 1;
        this.tblWerte.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tblWerte.Size = new System.Drawing.Size(522, 0);
        this.tblWerte.TabIndex = 3;
        // 
        // pnlWerte
        // 
        this.pnlWerte.AutoScroll = true;
        this.pnlWerte.AutoSize = true;
        this.pnlWerte.Controls.Add(this.tblWerte);
        this.pnlWerte.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlWerte.Location = new System.Drawing.Point(67, 50);
        this.pnlWerte.Name = "pnlWerte";
        this.pnlWerte.Size = new System.Drawing.Size(522, 0);
        this.pnlWerte.TabIndex = 0;
        this.pnlWerte.Visible = false;
        // 
        // tblInhalt
        // 
        this.tblInhalt.AutoSize = true;
        this.tblInhalt.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.tblInhalt.ColumnCount = 2;
        this.tblInhalt.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tblInhalt.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tblInhalt.Controls.Add(this.lblMeldung, 1, 0);
        this.tblInhalt.Controls.Add(this.pnlWerte, 1, 2);
        this.tblInhalt.Controls.Add(this.PictureBox1, 0, 0);
        this.tblInhalt.Controls.Add(this.pnlUnten, 0, 3);
        this.tblInhalt.Controls.Add(this.pnlStackTrace, 1, 1);
        this.tblInhalt.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tblInhalt.Location = new System.Drawing.Point(0, 0);
        this.tblInhalt.Margin = new System.Windows.Forms.Padding(0);
        this.tblInhalt.Name = "tblInhalt";
        this.tblInhalt.RowCount = 4;
        this.tblInhalt.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tblInhalt.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tblInhalt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tblInhalt.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tblInhalt.Size = new System.Drawing.Size(592, 253);
        this.tblInhalt.TabIndex = 5;
        // 
        // PictureBox1
        // 
        this.PictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
        this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
        this.PictureBox1.Location = new System.Drawing.Point(8, 8);
        this.PictureBox1.Margin = new System.Windows.Forms.Padding(8);
        this.PictureBox1.Name = "PictureBox1";
        this.tblInhalt.SetRowSpan(this.PictureBox1, 3);
        this.PictureBox1.Size = new System.Drawing.Size(48, 48);
        this.PictureBox1.TabIndex = 6;
        this.PictureBox1.TabStop = false;
        // 
        // pnlUnten
        // 
        this.pnlUnten.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
        this.pnlUnten.AutoSize = true;
        this.pnlUnten.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.pnlUnten.BackColor = System.Drawing.SystemColors.ControlDark;
        this.pnlUnten.ColumnCount = 6;
        this.tblInhalt.SetColumnSpan(this.pnlUnten, 2);
        this.pnlUnten.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.pnlUnten.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.pnlUnten.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.pnlUnten.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.pnlUnten.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.pnlUnten.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.pnlUnten.Controls.Add(this.btnOK, 5, 0);
        this.pnlUnten.Controls.Add(this.pnlInnerException, 3, 0);
        this.pnlUnten.Controls.Add(this.btnStop, 2, 0);
        this.pnlUnten.Controls.Add(this.btnKopieren, 1, 0);
        this.pnlUnten.Controls.Add(this.btnSpeichern, 0, 0);
        this.pnlUnten.Location = new System.Drawing.Point(0, 207);
        this.pnlUnten.Margin = new System.Windows.Forms.Padding(0);
        this.pnlUnten.Name = "pnlUnten";
        this.pnlUnten.RowCount = 1;
        this.pnlUnten.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.pnlUnten.Size = new System.Drawing.Size(592, 46);
        this.pnlUnten.TabIndex = 5;
        // 
        // btnOK
        // 
        this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
        this.btnOK.AutoSize = true;
        this.btnOK.BackColor = System.Drawing.SystemColors.Control;
        this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.btnOK.Location = new System.Drawing.Point(488, 10);
        this.btnOK.Margin = new System.Windows.Forms.Padding(8);
        this.btnOK.MinimumSize = new System.Drawing.Size(80, 25);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new System.Drawing.Size(96, 25);
        this.btnOK.TabIndex = 3;
        this.btnOK.Text = "OK";
        this.btnOK.UseVisualStyleBackColor = false;
        // 
        // pnlInnerException
        // 
        this.pnlInnerException.AutoSize = true;
        this.pnlInnerException.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.pnlInnerException.ColumnCount = 1;
        this.pnlInnerException.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.pnlInnerException.Controls.Add(this.lnkInnerException, 0, 1);
        this.pnlInnerException.Controls.Add(this.lblInnerException, 0, 0);
        this.pnlInnerException.Location = new System.Drawing.Point(178, 0);
        this.pnlInnerException.Margin = new System.Windows.Forms.Padding(0);
        this.pnlInnerException.Name = "pnlInnerException";
        this.pnlInnerException.RowCount = 2;
        this.pnlInnerException.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.pnlInnerException.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.pnlInnerException.Size = new System.Drawing.Size(99, 46);
        this.pnlInnerException.TabIndex = 7;
        this.pnlInnerException.Visible = false;
        // 
        // lnkInnerException
        // 
        this.lnkInnerException.AutoSize = true;
        this.lnkInnerException.LinkColor = System.Drawing.Color.PowderBlue;
        this.lnkInnerException.Location = new System.Drawing.Point(3, 23);
        this.lnkInnerException.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
        this.lnkInnerException.Name = "lnkInnerException";
        this.lnkInnerException.Size = new System.Drawing.Size(93, 15);
        this.lnkInnerException.TabIndex = 1;
        this.lnkInnerException.TabStop = true;
        this.lnkInnerException.Text = "[InnerException]";
        this.lnkInnerException.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkInnerException_LinkClicked);
        // 
        // lblInnerException
        // 
        this.lblInnerException.AutoSize = true;
        this.lblInnerException.ForeColor = System.Drawing.SystemColors.ControlLight;
        this.lblInnerException.Location = new System.Drawing.Point(3, 8);
        this.lblInnerException.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
        this.lblInnerException.Name = "lblInnerException";
        this.lblInnerException.Size = new System.Drawing.Size(52, 15);
        this.lblInnerException.TabIndex = 0;
        this.lblInnerException.Text = "Ursache:";
        // 
        // btnStop
        // 
        this.btnStop.Anchor = System.Windows.Forms.AnchorStyles.None;
        this.btnStop.BackColor = System.Drawing.Color.IndianRed;
        this.btnStop.ForeColor = System.Drawing.Color.White;
        this.btnStop.Location = new System.Drawing.Point(90, 10);
        this.btnStop.Margin = new System.Windows.Forms.Padding(8);
        this.btnStop.Name = "btnStop";
        this.btnStop.Size = new System.Drawing.Size(80, 25);
        this.btnStop.TabIndex = 8;
        this.btnStop.Text = "Stop";
        this.btnStop.UseVisualStyleBackColor = false;
        this.btnStop.Visible = false;
        // 
        // btnKopieren
        // 
        this.btnKopieren.Anchor = System.Windows.Forms.AnchorStyles.None;
        this.btnKopieren.BackColor = System.Drawing.SystemColors.Control;
        this.btnKopieren.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
        this.btnKopieren.ImageIndex = 0;
        this.btnKopieren.ImageList = this.imgSymbole;
        this.btnKopieren.Location = new System.Drawing.Point(49, 10);
        this.btnKopieren.Margin = new System.Windows.Forms.Padding(8);
        this.btnKopieren.Name = "btnKopieren";
        this.btnKopieren.Size = new System.Drawing.Size(25, 25);
        this.btnKopieren.TabIndex = 9;
        this.btnKopieren.UseVisualStyleBackColor = false;
        // 
        // imgSymbole
        // 
        this.imgSymbole.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgSymbole.ImageStream")));
        this.imgSymbole.TransparentColor = System.Drawing.Color.Fuchsia;
        this.imgSymbole.Images.SetKeyName(0, "SymbolKopieren.bmp");
        this.imgSymbole.Images.SetKeyName(1, "SymbolSpeichern.bmp");
        // 
        // btnSpeichern
        // 
        this.btnSpeichern.Anchor = System.Windows.Forms.AnchorStyles.None;
        this.btnSpeichern.BackColor = System.Drawing.SystemColors.Control;
        this.btnSpeichern.ImageKey = "SymbolSpeichern.bmp";
        this.btnSpeichern.ImageList = this.imgSymbole;
        this.btnSpeichern.Location = new System.Drawing.Point(8, 10);
        this.btnSpeichern.Margin = new System.Windows.Forms.Padding(8);
        this.btnSpeichern.Name = "btnSpeichern";
        this.btnSpeichern.Size = new System.Drawing.Size(25, 25);
        this.btnSpeichern.TabIndex = 10;
        this.btnSpeichern.UseVisualStyleBackColor = false;
        this.btnSpeichern.Click += new System.EventHandler(this.btnSpeichern_Click);
        // 
        // pnlStackTrace
        // 
        this.pnlStackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
        this.pnlStackTrace.AutoScroll = true;
        this.pnlStackTrace.AutoSize = true;
        this.pnlStackTrace.ColumnCount = 1;
        this.pnlStackTrace.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.pnlStackTrace.Controls.Add(this.lblStackTrace, 0, 0);
        this.pnlStackTrace.Location = new System.Drawing.Point(64, 26);
        this.pnlStackTrace.Margin = new System.Windows.Forms.Padding(0);
        this.pnlStackTrace.MaximumSize = new System.Drawing.Size(0, 200);
        this.pnlStackTrace.Name = "pnlStackTrace";
        this.pnlStackTrace.RowCount = 1;
        this.pnlStackTrace.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.pnlStackTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
        this.pnlStackTrace.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
        this.pnlStackTrace.Size = new System.Drawing.Size(528, 21);
        this.pnlStackTrace.TabIndex = 4;
        // 
        // lblStackTrace
        // 
        this.lblStackTrace.AutoSize = true;
        this.lblStackTrace.ForeColor = System.Drawing.SystemColors.ControlDark;
        this.lblStackTrace.Location = new System.Drawing.Point(3, 3);
        this.lblStackTrace.Margin = new System.Windows.Forms.Padding(3);
        this.lblStackTrace.Name = "lblStackTrace";
        this.lblStackTrace.Size = new System.Drawing.Size(72, 15);
        this.lblStackTrace.TabIndex = 5;
        this.lblStackTrace.Text = "[StackTrace]";
        // 
        // FormException
        // 
        this.AcceptButton = this.btnOK;
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnOK;
        this.ClientSize = new System.Drawing.Size(592, 253);
        this.Controls.Add(this.tblInhalt);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "FormException";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Fehler";
        this.pnlWerte.ResumeLayout(false);
        this.pnlWerte.PerformLayout();
        this.tblInhalt.ResumeLayout(false);
        this.tblInhalt.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
        this.pnlUnten.ResumeLayout(false);
        this.pnlUnten.PerformLayout();
        this.pnlInnerException.ResumeLayout(false);
        this.pnlInnerException.PerformLayout();
        this.pnlStackTrace.ResumeLayout(false);
        this.pnlStackTrace.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }
    private System.Windows.Forms.Label lblMeldung;
    private System.Windows.Forms.TableLayoutPanel tblWerte;
    private System.Windows.Forms.Panel pnlWerte;
    private System.Windows.Forms.TableLayoutPanel tblInhalt;
    private System.Windows.Forms.TableLayoutPanel pnlStackTrace;
    private System.Windows.Forms.TableLayoutPanel pnlUnten;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Label lblStackTrace;
    private System.Windows.Forms.PictureBox PictureBox1;
    private System.Windows.Forms.TableLayoutPanel pnlInnerException;
    private System.Windows.Forms.Label lblInnerException;
    private System.Windows.Forms.LinkLabel lnkInnerException;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Button btnKopieren;
    private System.Windows.Forms.ImageList imgSymbole;
    private System.ComponentModel.IContainer components;
    private System.Windows.Forms.Button btnSpeichern;
}
