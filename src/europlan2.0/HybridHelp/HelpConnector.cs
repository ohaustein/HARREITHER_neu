using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridHelp
{
    public partial class HybridHelpConnector : Component
    {


        [Browsable(true)]
        [Category("Hybrid Help")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible )]
        public String Host { get; set; }


        public HybridHelpConnector()
        {
            InitializeComponent();
        }

        public HybridHelpConnector(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            InitializeComponent();
        }


    }
}
