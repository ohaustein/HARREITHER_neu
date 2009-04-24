using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Europlan.Licensing {
	public class EncryptionManager {
		private static EncryptionManager instance = null;

		public static EncryptionManager Instance {
			get {
				if (instance == null) {
					instance = new EncryptionManager();
				}
				return instance;
			}
		}

		private HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider();
		private Encoding byteConverter = new ASCIIEncoding();

		private RSACryptoServiceProvider provider = null;
		//private = "<RSAKeyValue><Modulus>16CcCS56V1YIkjht12jOg5G2597D2lv+AwKMAKvueYpT2X5iE42yuKm/DFDhEIeyGckKe+Edz/T4OtpIq+5oTQ==</Modulus><Exponent>AQAB</Exponent><P>9vqsAPi2ljejeDM0Ce0tBG2Y3JsXDskIo9WBLypHiZ8=</P><Q>34DJ/Gk+O82xTY5fEc4ssVNoqSg9azhxAZYna0JuXpM=</Q><DP>lQX78zofMdSwlj6PD85Ec3+N1M7fwtQamoChSkxAjhE=</DP><DQ>Y2Tp7rd69quF8wCipM90rrNyMu/zSHCESPlWtGuJm8M=</DQ><InverseQ>iaTFIP3unJMIlPHKNUf+E1dq2k+77Rs3sZRMkk7ATlg=</InverseQ><D>Tk/8m7EPqcyLTF/GNOfRqSMw5MGZLCMg/ALeyhengug5b/9oYr/9S+RerfCCTM9oyO2dnFEn5QQRLUMD394rWQ==</D></RSAKeyValue>";
		//public  = "<RSAKeyValue><Modulus>16CcCS56V1YIkjht12jOg5G2597D2lv+AwKMAKvueYpT2X5iE42yuKm/DFDhEIeyGckKe+Edz/T4OtpIq+5oTQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

		private EncryptionManager() {
		}

		public string Key {
			get {
				if (this.provider == null) {
					return null;
				}
				return this.provider.ToXmlString(!this.PublicOnly);
			}
			set {
				this.provider = new RSACryptoServiceProvider(512);
				this.provider.FromXmlString(value);
			}
		}

		public bool PublicOnly {
			get {
				Debug.Assert(this.provider != null);
				return this.provider.PublicOnly;
			}
		}

		public string CalculateSignature(string text) {
			Debug.Assert(!this.PublicOnly);
			byte[] textContent = this.byteConverter.GetBytes(text);
			byte[] signature = this.provider.SignData(textContent, this.hashAlgorithm);
			string base64Signature = Convert.ToBase64String(signature);
			return base64Signature;
		}

		public bool VerifySignature(String text, String base64signature) {
			Debug.Assert(this.provider != null);
			byte[] textContent = this.byteConverter.GetBytes(text);
			byte[] signature = Convert.FromBase64String(base64signature);
			return this.provider.VerifyData(textContent, this.hashAlgorithm, signature);
		}

		public string HashData(byte[] data) {
			Debug.Assert(this.provider != null);
			return Convert.ToBase64String(this.hashAlgorithm.ComputeHash(data));
		}

		public byte[] HashString(string text) {
			Debug.Assert(this.provider != null);
			byte[] data = this.byteConverter.GetBytes(text);
			return this.hashAlgorithm.ComputeHash(data);
		}
	}
}
