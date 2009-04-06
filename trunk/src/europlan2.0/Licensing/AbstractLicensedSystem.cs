using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Licensing {
	public class AbstractLicensedSystem {
		private HardwareId id = null;

		public AbstractLicensedSystem() {
			this.id = new HardwareId();
		}

		public AbstractLicensedSystem(string id) {
			this.id = new HardwareId(id);
		}

		protected event EventHandler changed;

		public event EventHandler Changed {
			add { this.changed += value; }
			remove { this.changed -= value; }
		}

		protected void OnChanged() {
			if (this.changed != null) {
				this.changed(this, EventArgs.Empty);
			}
		}

		[XmlAttribute("id")]
		public string Id {
			get { return this.id.IdString; }
			set {
				this.id = new HardwareId(value);
				this.OnChanged();
			}
		}

		public override string ToString() {
			return this.id.IdString;
		}

		public bool MatchesCurrentSystem {
			get { return new HardwareId(true).Equals(this.id); }
		}
	}
}
