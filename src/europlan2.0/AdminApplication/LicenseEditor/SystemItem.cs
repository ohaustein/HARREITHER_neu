using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Europlan.Licensing;

namespace Europlan.AdminApplication {
	public class SystemItem : ListViewItem {

		private LicensedSystemTemplate system;
		private ListViewSubItem dateItem;
		private ListViewSubItem annotationItem;

		public SystemItem(LicensedSystemTemplate system) {
			this.system = system;
			this.Text = this.system.Id;
			this.dateItem = new ListViewSubItem(this, this.system.AddedDate.ToShortDateString());
			this.annotationItem = new ListViewSubItem(this, this.system.Annotation);
			this.SubItems.Add(this.dateItem);
			this.SubItems.Add(this.annotationItem);
			this.system.Changed += new EventHandler(system_Changed);
		}

		private void system_Changed(object sender, EventArgs e) {
			this.Text = this.system.Id;
			this.dateItem.Text = this.system.AddedDate.ToShortDateString();
			this.annotationItem.Text = this.system.Annotation;
		}

		public LicensedSystemTemplate LicensedSystem {
			get { return this.system; }
		}
	}
}
