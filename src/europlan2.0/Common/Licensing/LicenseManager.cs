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

		private event EventHandler licenseChanged;

		public event EventHandler LicenseChanged {
			add { this.licenseChanged += value; }
			remove { this.licenseChanged -= value; }
		}

		private void OnLicenseChanged() {
			if (this.licenseChanged != null) {
				this.licenseChanged(this, EventArgs.Empty);
			}
		}

		private License license = null;
		private string dataDirPath;

		private LicenseManager() {
			if (EncryptionManager.Instance.Key == null) {
				EncryptionManager.Instance.Key = "<RSAKeyValue><Modulus>16CcCS56V1YIkjht12jOg5G2597D2lv+AwKMAKvueYpT2X5iE42yuKm/DFDhEIeyGckKe+Edz/T4OtpIq+5oTQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
			}
			
			this.dataDirPath = Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath);
			//this.dataDirPath = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "data");
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
#if DEBUG
			if (this.license == null) {
				this.license = new License();
				this.license.LicensedTo = "Development License";
				this.license.Header = "";
				this.license.ValidUntil = DateTime.Today.AddYears(1);
				this.license.Systems.Add(new LicensedSystem());
			}
#endif
		}

		public ImportLicenseResultEnum ImportLicense(string filename) {
			if (File.Exists(filename)) {
				bool copied = false;
				License newLicense;
				try {
					using (Stream s = new FileStream(filename, FileMode.Open)) {
						newLicense = License.LoadLicense(s);
					}
					if (!newLicense.IsValid) {
						if (!newLicense.IsSignatureValid) {
							return ImportLicenseResultEnum.LICENSE_SIGNATURE_NOT_VALID;
						} else if (!newLicense.IsSystemValid) {
							return ImportLicenseResultEnum.LICENSE_SYSTEM_NOT_VALID;
						} else if (!newLicense.IsDateValid) {
							return ImportLicenseResultEnum.LICENSE_DATE_NOT_VALID;
						} else {
							return ImportLicenseResultEnum.LICENSE_NOT_VALID;
						}
					}
					try {
						File.Copy(filename, Path.Combine(this.dataDirPath, licenseFileName), true);
						copied = true;
					} catch (Exception e) {
						log.Warn("Cannot copy license to data directory", e);
					}
				} catch (Exception e) {
					log.Error("Cannot read license file", e);
					return ImportLicenseResultEnum.LICENSE_NOT_READABLE;
				}
				if (copied) {
					this.license = newLicense;
					this.OnLicenseChanged();
					return ImportLicenseResultEnum.LICENSE_IMPORTED;
				} else {
					return ImportLicenseResultEnum.LICENSE_TEMPORARY_IMPORTED;
				}
			} else {
				log.Warn("License '" + filename + "' not found");
				return ImportLicenseResultEnum.LICENSE_NOT_FOUND;
			}
		}

		public License License {
			get { return this.license; }
		}

		public bool LicenseFound {
			get { return this.license != null; }
		}

		public bool LicenseFoundAndValid {
			get { return this.license != null && this.license.IsValid; }
		}

		public string LincensePath {
			get { return Path.Combine(this.dataDirPath, LicenseManager.licenseFileName); }
		}
	}

	public enum ImportLicenseResultEnum {
		LICENSE_IMPORTED,
		LICENSE_NOT_FOUND,
		LICENSE_NOT_READABLE,
		LICENSE_DATE_NOT_VALID,
		LICENSE_SIGNATURE_NOT_VALID,
		LICENSE_SYSTEM_NOT_VALID,
		LICENSE_NOT_VALID,
		LICENSE_TEMPORARY_IMPORTED
	}
}
