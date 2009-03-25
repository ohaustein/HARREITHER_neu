using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using log4net;

namespace Europlan.Licensing {
	public class LicenseManager {

		private static readonly ILog log = LogManager.GetLogger(typeof(LicenseManager));

		private static LicenseManager instance = null;
		private static readonly string licenseFileName = "license.epl";

		public static LicenseManager Instance {
			get {
				if (instance == null) {
					instance = new LicenseManager();
				}
				return instance;
			}
		}

		private License license = null;
		private string dataDirPath;

		private LicenseManager() {
			if (EncryptionManager.Instance.Key == null) {
				EncryptionManager.Instance.Key = "<RSAKeyValue><Modulus>16CcCS56V1YIkjht12jOg5G2597D2lv+AwKMAKvueYpT2X5iE42yuKm/DFDhEIeyGckKe+Edz/T4OtpIq+5oTQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
			}
			this.dataDirPath = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "data");
			string licensePath = Path.Combine(this.dataDirPath, licenseFileName);
			if (File.Exists(licensePath)) {
				try {
					using (Stream s = new FileStream(licensePath, FileMode.Open)) {
						this.license = License.LoadLicense(s);
					}
				} catch (Exception e) {
					this.license = null;
					log.Error("Cannot read license file", e);
				}
			} else {
				log.Warn("No license found");
			}
		}

		public bool ImportLicense(string filename) {
			if (File.Exists(filename)) {
				try {
					using (Stream s = new FileStream(filename, FileMode.Open)) {
						this.license = License.LoadLicense(s);
					}
					try {
						if (!Directory.Exists(this.dataDirPath)) {
							Directory.CreateDirectory(this.dataDirPath);
						}
						File.Copy(filename, Path.Combine(this.dataDirPath, licenseFileName));
					} catch (Exception e) {
						log.Warn("Cannot copy license to data directory", e);
					}
				} catch (Exception e) {
					log.Error("Cannot read license file", e);
				}
			} else {
				log.Warn("License '" + filename + "' not found");
			}
			return this.LicenseFound;
		}

		public License License {
			get { return this.license; }
		}

		public bool LicenseFound {
			get { return this.license != null; }
		}
	}
}
