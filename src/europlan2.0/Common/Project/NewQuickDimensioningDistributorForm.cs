using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class NewQuickDimensioningDistributorForm : Form {

		public NewQuickDimensioningDistributorForm(Project project) {
			InitializeComponent();
			if (project != null) {
				gridQuickDimensioningDistributor.Distributors = project.QuickDimensioning.Distributors;
			}
		}

		private void NewQuickDimensioningDistributorForm_FormClosing(object sender, FormClosingEventArgs e) {
			gridQuickDimensioningDistributor.Cleanup();
		}
	}
}