using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using QRCoder;

namespace HybridHelp
{
    public partial class HelpCaller : UserControl
    {


        private String HelpUrl;
        private readonly QRCodeGenerator QrGenerator;



        ////[Browsable(true)]
        ////[Category("Hybrid Help")]
        ////[Description("descriptio")] // TODO ! description
        ////[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        ////public HybridHelpConnector HelpCpnnector
        ////{
        ////    get { return _HelpCpnnector; }
        ////    set
        ////    {
        ////        _HelpCpnnector = value; 
        ////        CreateHelpIcon();
        ////    }
        ////}
        ////private HybridHelpConnector _HelpCpnnector;

        [Browsable(true)]
        [Category("Hybrid Help")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public String HelpID
        {
            get { return _HelpID; }
            set
            {
                _HelpID = value;
                CreateHelpIcon();
            }
        }
        private String _HelpID;


        [Browsable(true)]
        [Category("Hybrid Help")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public String HelpHost
        {
            get { return _HelpHost; }
            set
            {
                _HelpHost = value;
                CreateHelpIcon();
            }
        }
        private String _HelpHost;



        public HelpCaller()
        {
            QrGenerator = new QRCodeGenerator();
            InitializeComponent();
        }


        private void CreateHelpIcon()
        {
            HelpUrl = HelpHost + "/" + HelpID;
            var helpIcon = Properties.Resources.HelpIcon128;
            var qrCodeData = QrGenerator.CreateQrCode(HelpUrl, QRCodeGenerator.ECCLevel.H);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, helpIcon, 35, drawQuietZones: false);
            this.picQrCode.Cursor = Cursors.Help;
            this.picQrCode.Image = qrCodeImage;
            this.picQrCode.SizeMode = PictureBoxSizeMode.Zoom;
        }


        private void picQrCode_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(HelpUrl))
                {
                    Process.Start("http://" + HelpUrl);
                }
            }
            catch { }
        }


    }
}
