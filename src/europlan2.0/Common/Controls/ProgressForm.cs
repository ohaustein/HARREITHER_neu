using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Common {
	public partial class ProgressForm : Form {

		private Semaphore semaphore;

		public ProgressForm(Semaphore semaphore) {
			InitializeComponent();
			this.semaphore = semaphore;
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

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			this.semaphore.Release();
		}
	}
}