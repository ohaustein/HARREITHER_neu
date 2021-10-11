using System;
using System.Drawing;
using System.Windows.Forms;

public partial class FormException
{

    public static void Anzeigen(Exception ex, String titel = "Fehler", Font schrift = null)
    {
        using (FormException frmException = new FormException(ex, titel, schrift))
        {
            frmException.ShowDialog();
        }
    }

    public static void Anzeigen(Exception ex, Form gebundenAn, String titel = "Fehler", Font schrift = null)
    {
        if (schrift == null)
            schrift = gebundenAn.Font;
        using (FormException frmException = new FormException(ex, titel, schrift))
        {
            frmException.ShowDialog(gebundenAn);
        }
    }

    public static void AnzeigenMitSchatten(Exception ex, Form gebundenAn, String titel = "Fehler")
    {
        using (FormSchatten frmSchatten = new FormSchatten(gebundenAn))
        {
            using (FormException frmException = new FormException(ex, titel, gebundenAn.Font))
            {
                frmException.ShowDialog(frmSchatten);
            }
        }
    }


    public Exception Ex { get; private set; }


    public FormException(Exception ex, String titel = "Fehler", Font schrift = null)
    {
        Shown += FormException_Shown;
        InitializeComponent();
        Ex = ex;
        Text = titel;
        if (schrift != null) Font = schrift;
        EinstellenMeldung();
        EinstellenInnerException();
        EinstellenData();
        EinstellenStackTrace();
#if DEBUG
        SetBounds(Left, Top, 800, Math.Min(400 + ex.Data.Count * 10, 600));
        this.btnStop.Visible = true;
        this.btnKopieren.Visible = false;
#else
            SetBounds(Left, Top, 600, Math.Min(200 + ex.Data.Count * 10, 400));
            this.btnKopieren.Visible = true;
#endif
    }


    private void EinstellenMeldung()
    {
        this.lblMeldung.Font = new Font(Font.FontFamily, 10, FontStyle.Bold);
        this.lblMeldung.Text = Ex.Message;
    }

    private void EinstellenInnerException()
    {
        if (Ex.InnerException != null)
        {
            this.lnkInnerException.Text = Ex.InnerException.Message;
            this.pnlInnerException.Visible = true;
        }
        else
        {
            this.pnlInnerException.Visible = false;
        }
    }

    private void EinstellenData()
    {
        if (Ex.Data.Count > 0)
        {
            this.pnlWerte.Visible = true;
            this.tblWerte.RowCount = Ex.Data.Count;
            foreach (string bezeichnung in Ex.Data.Keys)
            {
                Label neuesLabel = new Label
                {
                    Anchor = AnchorStyles.Left | AnchorStyles.Top,
                    AutoSize = true,
                    Margin = new Padding(3, 3, 3, 0),
                    Text = bezeichnung + ":"
                };
                TextBox neueTextBox = new TextBox
                {
                    Anchor = AnchorStyles.Left | AnchorStyles.Right,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(0, 3, 3, 0),
                    ReadOnly = true,
                    Text = Ex.Data[bezeichnung].ToString()
                };
                this.tblWerte.Controls.Add(neuesLabel);
                this.tblWerte.Controls.Add(neueTextBox);
            }
            this.tblWerte.RowStyles.Clear();
            for (int index = 1; index <= this.tblWerte.RowCount; index++)
            {
                this.tblWerte.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            this.tblWerte.AutoSize = true;
            this.tblWerte.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
    }

    private void EinstellenStackTrace()
    {
#if DEBUG
        this.pnlStackTrace.Visible = true;
        this.lblStackTrace.Text = Ex.StackTrace;
#else
            this.pnlStackTrace.Visible = false;
#endif
    }

    private void AnzeigenInnerException()
    {
        using (FormSchatten schatten = new FormSchatten(this))
        {
            using (FormException formularInnerException = new FormException(Ex.InnerException, schrift: Font))
            {
                formularInnerException.StartPosition = FormStartPosition.Manual;
                formularInnerException.SetBounds(schatten.Left + 20, schatten.Top + 20, formularInnerException.Width, formularInnerException.Height);
                formularInnerException.ShowDialog(schatten);
            }
        }
    }

    //private void SpeichernAlsXml(String dateiname)
    //{
    //    XmlDocument xmlKnoten = new XmlDocument();
    //    xmlKnoten.LoadXml("<Daten/>");
    //    ClassExceptionXml.UebertrageAllgemeineInfo(xmlKnoten.DocumentElement);
    //    XmlElement xmlKnotenException = xmlKnoten.CreateElement("Exception");
    //    xmlKnoten.DocumentElement.AppendChild(xmlKnotenException);
    //    ClassExceptionXml.UebertrageException(Ex, xmlKnotenException);
    //    xmlKnoten.Save(dateiname);
    //}

    private void FormException_Shown(object sender, System.EventArgs e)
    {
        this.btnOK.Focus();
    }

    private void btnSpeichern_Click(object sender, EventArgs e)
    {
        using (FormSchatten schatten = new FormSchatten(this))
        {
            try
            {
                using (SaveFileDialog dlgSpeichern = new SaveFileDialog())
                {
                    dlgSpeichern.DefaultExt = ".exception";
                    dlgSpeichern.Filter = "Exception-Dateien|*.exception|XML-Dateien|*.xml|Alle Dateien|*.*";
                    dlgSpeichern.FileName = DateTime.Now.ToString(@"yyyy\-MM\-dd\-HHmm");
                    dlgSpeichern.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    dlgSpeichern.Title = "Exception speichern unter ...";
                    if (dlgSpeichern.ShowDialog(schatten) == DialogResult.OK)
                    {
                        //     SpeichernAlsXml(dlgSpeichern.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(schatten, ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void btnKopieren_Click(System.Object sender, System.EventArgs e)
    {
        Bitmap bitmap = new Bitmap(this.Width, this.Height);
        this.DrawToBitmap(bitmap, Rectangle.FromLTRB(0, 0, this.Width, this.Height));
        Clipboard.SetImage(Image.FromHbitmap(bitmap.GetHbitmap()));
    }

    private void btnStop_Click(System.Object sender, System.EventArgs e)
    {
        System.Diagnostics.Debugger.Break();
    }

    private void lnkInnerException_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
    {
        AnzeigenInnerException();
    }


    private class FormSchatten : Form
    {

        private System.ComponentModel.IContainer components = null;

        [System.Diagnostics.DebuggerStepThrough()]
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

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            //FormSchattierung
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(100, 100);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSchatten";
            this.Opacity = 0.45;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Schatten";
            this.ResumeLayout(false);
        }

        public FormSchatten(Form gebundenesFormular)
        {
            InitializeComponent();
            {
                SetBounds(gebundenesFormular.Bounds.X, gebundenesFormular.Bounds.Y, gebundenesFormular.Bounds.Width, gebundenesFormular.Bounds.Height);
            }
            Show(gebundenesFormular);
        }

    }

}

public static class FormExceptionExtensionAnzeigen
{

    public static void Anzeigen(this Exception ex, String titel = "Fehler", Font schrift = null)
    {
        FormException.Anzeigen(ex, titel, schrift);
    }

    public static void Anzeigen(this Exception ex, Form gebundenAn, String titel = "Fehler", Font schrift = null)
    {
        FormException.Anzeigen(ex, gebundenAn, titel, schrift);
    }

    public static void AnzeigenMitSchatten(this Exception ex, Form gebundenAn, String titel = "Fehler")
    {
        FormException.Anzeigen(ex, gebundenAn, titel);
    }

}
