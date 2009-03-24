using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Europlan.Licensing {
	public class LicenseKey {
		public static readonly string licenseKeyChars = "123456789ABCDEFGHJKLMNOQRSUVWXYZ";

		public static Random r = null;

		private byte[] key = null;
		private string hashedKey = null;

		public LicenseKey() {
			if (r == null) {
				r = new Random();
			}
			key = new byte[10];
			r.NextBytes(key);
		}

		public LicenseKey(byte[] key) {
			if (key.Length != 10) {
				throw new Exception("invalid license key");
			}
			this.key = key;
		}

		public LicenseKey(string keyString, bool hashed) {
			if (hashed) {
				this.hashedKey = keyString;
			} else {
				byte[] tmpKey = new byte[10];
				string tmp = keyString.ToUpper();
				ushort tmpVal = 0;
				int tmpBits = 0;
				int i = 0;
				while (tmp.Length > 0 && i < 10) {
					int curVal = licenseKeyChars.IndexOf(tmp[0]);
					if (curVal >= 0) {
						tmpVal = (ushort)(tmpVal << 5);
						tmpVal = (ushort)(tmpVal | (ushort)curVal);
						tmpBits += 5;
						while (tmpBits >= 8 && i < 10) {
							tmpKey[i] = (byte)((tmpVal >> (tmpBits - 8)) & 0xff);
							tmpBits -= 8;
							i++;
						}
					} else {
						if (tmp[0] != '-') {
							throw new Exception("invalid license key");
						}
					}
					tmp = tmp.Substring(1);
				}
				this.key = tmpKey;
			}
		}

		public override string ToString() {
			if (this.key != null) {
				return this.KeyString;
			} else {
				return "hashedKey#" + this.hashedKey;
			}
		}

		public string KeyString {
			get {
				Debug.Assert(this.key != null);
				string keyString = "";
				ushort tmp = 0;
				int tmpBits = 0;
				int keyPos = 0;
				for (int i = 0; i < 4; i++) {
					for (int j = 0; j < 4; j++) {
						if (tmpBits < 5) {
							tmp = (ushort)(tmp << 8);
							tmp = (ushort)(tmp | this.key[keyPos]);
							keyPos++;
							tmpBits += 8;
						}
						keyString += licenseKeyChars[(tmp >> (tmpBits - 5)) & 0x1f];
						tmpBits -= 5;
					}
					if (i < 3) {
						keyString += "-";
					}
				}
				return keyString;
			}
		}

		public byte[] KeyData {
			get {
				Debug.Assert(this.key != null);
				return this.key;
			}
		}

		public string HashedKey {
			get {
				Debug.Assert(this.key != null || this.hashedKey != null);
				if (this.key != null) {
					return EncryptionManager.Instance.HashData(this.key);
				} else {
					return this.hashedKey;
				}
			}
		}

		public override bool Equals(object obj) {
			LicenseKey other = obj as LicenseKey;
			if (other == null) {
				return false;
			}
			if (this.key == null && this.hashedKey == null) {
				return other.key == null && other.hashedKey == null;
			}
			if (other.key == null && other.hashedKey == null) {
				return false;
			}
			return this.HashedKey.Equals(other.HashedKey);
		}

		public override int GetHashCode() {
			if (this.key == null && this.hashedKey == null) {
				return 0;
			}
			return this.HashedKey.GetHashCode();
		}
	}
}
