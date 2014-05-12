using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Europlan.Licensing {
	[XmlRoot("license")]
	public class License : AbstractLicense<LicensedModule, LicensedSystem>, ILicense {

		protected string signature = "";

		protected override LicensedModule NewModule(string moduleName) {
			return new LicensedModule(moduleName);
		}

		[XmlAttribute("signature")]
		public string Signature {
			get { return this.signature; }
			set { this.signature = value; }
		}

		public bool IsSignatureValid {
			get {
#if DEBUG
				return true;
#else
				return EncryptionManager.Instance.VerifySignature(this.LicenseStringForSigning, this.Signature);
#endif
			}
		}

		public override bool IsValid {
			get { return this.IsSignatureValid && this.IsSystemValid && this.IsDateValid; }
		}

		public static License LoadLicense(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(License));
			return (License)serializer.Deserialize(stream);
		}

		public void SaveLicense(Stream stream) {
			XmlSerializer serializer = new XmlSerializer(typeof(License));
			serializer.Serialize(stream, this);
		}

        public License Data {
            get { return this; }
        }
    }

    public class NoLicense : ILicense {

        public bool IsValid {
            get { return false; }
        }

        public bool IsModuleEnabled(string moduleName) {
            return false;
        }

        public License Data {
            get { return null; }
        }
    }
}
