using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public class ModuleItem : ListViewItem {

		private LicensedModuleTemplate module;

		public ModuleItem(LicensedModuleTemplate module) {
			this.module = module;
			this.Text = this.module.Name;
			this.Checked = this.module.Enabled;
		}

		public LicensedModuleTemplate Module {
			get { return this.module; }
		}
	}
}
