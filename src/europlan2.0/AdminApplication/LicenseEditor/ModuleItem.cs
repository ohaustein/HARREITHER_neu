using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public class ModuleItem : ListViewItem {

		private LicensedModule module;

		public ModuleItem(LicensedModule module) {
			this.module = module;
			this.Text = this.module.Name;
			this.Checked = this.module.Enabled;
		}

		public LicensedModule Module {
			get { return this.module; }
		}
	}
}
