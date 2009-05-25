using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;

namespace Europlan.Application {
	public partial class MaterialForm : Form {
		public MaterialForm() {
			InitializeComponent();
			this.FormClosed += new FormClosedEventHandler(MaterialForm_FormClosed);
		}

		void MaterialForm_FormClosed(object sender, FormClosedEventArgs e) {
			Configuration.UserTemplate.Save();
		}

		public Nullable<CategoryType> Filter {
			get { return this.megFloor.Filter; }
			set { this.megFloor.Filter = value; }
		}

		private void MaterialForm_FormClosing(object sender, FormClosingEventArgs e) {
			this.megCeiling.Cleanup();
			this.megDistributor.Cleanup();
			this.megFloor.Cleanup();
			this.megGeneral.Cleanup();
			this.megInsulation.Cleanup();
			this.megWall.Cleanup();
		}
	}
}