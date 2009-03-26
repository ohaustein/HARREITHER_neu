using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Licensing {
	public class LicensedSystemTemplate : AbstractLicensedSystem {

		protected DateTime addedDate = DateTime.Now;
		protected string annotation = "";

		public LicensedSystemTemplate() : base() {
		}

		public LicensedSystemTemplate(string id) : base(id) {
		}

		[XmlAttribute("added")]
		public DateTime AddedDate {
			get { return this.addedDate; }
			set {
				this.addedDate = value;
				this.OnChanged();
			}
		}

		[XmlAttribute("annotation")]
		public string Annotation {
			get { return this.annotation; }
			set {
				this.annotation = value;
				this.OnChanged();
			}
		}

		public LicensedSystem CreateSystem() {
			LicensedSystem system = new LicensedSystem(this.Id);
			return system;
		}
	}
}
