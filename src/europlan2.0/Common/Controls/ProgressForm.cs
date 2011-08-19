using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class ProgressForm : Form {
		public ProgressForm() {
			InitializeComponent();
		}

		public string Title {
			set {
				this.Text = value;
			}
		}

		public string InfoText {
			set {
				this.label1.Text = value;
			}
		}
	}
}