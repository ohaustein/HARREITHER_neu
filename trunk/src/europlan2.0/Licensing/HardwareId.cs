using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Management;
using System.Diagnostics;

namespace Europlan.Licensing {
	public class HardwareId {
		private byte[] id = null;

		private static byte[] currentSystemId = null;

		public HardwareId() {
			this.id = GetCurrentSystemId();
		}

		public HardwareId(byte[] id) {
			if (id.Length != 9) {
				throw new Exception("invalid system id");
			}
			this.id = id;
		}

		public HardwareId(string idString) {
			byte[] tmpId = new byte[9];
			string tmp = idString;
			while (tmp[0] == '-') {
				tmp = tmp.Substring(1);
			}
			int i = 0;
			try {
				while (i < 9 && tmp.Length > 0) {
					tmpId[i] = byte.Parse(tmp.Substring(0, 2), NumberStyles.HexNumber);
					i++;
					tmp = tmp.Substring(2);
					while (tmp.Length > 0 && tmp[0] == '-') {
						tmp = tmp.Substring(1);
					}
				}
			} catch (Exception e) {
				throw new Exception("invalid system id", e);
			}
			if (i < 9 || tmp.Length > 0) {
				throw new Exception("invalid system id");
			}
			this.id = tmpId;
		}

		public override string ToString() {
			if (this.id == null) {
				return "000000-000000-000000";
			}
			return this.IdString;
		}

		public string IdString {
			get {
				Debug.Assert(this.id != null);
				string systemId = string.Format("{0:x2}{1:x2}{2:x2}-{3:x2}{4:x2}{5:x2}-{6:x2}{7:x2}{8:x2}", this.id[0], this.id[1], this.id[2], this.id[3], this.id[4], this.id[5], this.id[6], this.id[7], this.id[8]);
				return systemId;
			}
		}

		public byte[] IdData {
			get {
				Debug.Assert(this.id != null);
				return this.id;
			}
		}

		public override bool Equals(object obj) {
			HardwareId other = obj as HardwareId;
			if (other == null) {
				return false;
			}
			if (this.id == null) {
				return other.id == null;
			}
			if (other.id == null) {
				return false;
			}
			return this.id[0] == other.id[0] &&
				this.id[1] == other.id[1] &&
				this.id[2] == other.id[2] &&
				this.id[3] == other.id[3] &&
				this.id[4] == other.id[4] &&
				this.id[5] == other.id[5] &&
				this.id[6] == other.id[6] &&
				this.id[7] == other.id[7] &&
				this.id[8] == other.id[8];
		}

		public override int GetHashCode() {
			if (this.id == null) {
				return 0;
			}
			return this.id.GetHashCode();
		}

		private static string GetCPUId() {
			string cpuInfo = String.Empty;
			string temp = String.Empty;
			ManagementClass mc = new ManagementClass("Win32_Processor");
			ManagementObjectCollection moc = mc.GetInstances();
			foreach (ManagementObject mo in moc) {
				if (cpuInfo == String.Empty) {
					cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
				}
			}
			return cpuInfo;
		}

		private static string GetMotherBoardID() {
			ManagementObjectCollection mbCol = new ManagementClass("Win32_BaseBoard").GetInstances();
			ManagementObjectCollection.ManagementObjectEnumerator mbEnum = mbCol.GetEnumerator();
			mbEnum.MoveNext();
			return ((ManagementObject)(mbEnum.Current)).Properties["SerialNumber"].Value.ToString();
		}

		public static byte[] GetCurrentSystemId() {
			if (currentSystemId == null) {
				string currentSystemIdString = GetCPUId() + GetMotherBoardID();
				byte[] hashedId = EncryptionManager.Instance.HashString(currentSystemIdString);
				int i = 0;
				int j = hashedId.Length;
				while (j > 9) {
					hashedId[i] ^= hashedId[--j];
					if (i == 8) {
						i = 0;
					} else {
						i++;
					}
				}
				byte[] result = new byte[9];
				for (i = 0; i < 9; i++) {
					result[i] = hashedId[i];
				}
				currentSystemId = result;
			}
			return currentSystemId;
		}

		public static string GetCurrentSystemIdString() {
			byte[] hashedId = GetCurrentSystemId();
			string systemId = string.Format("{0:x2}{1:x2}{2:x2}-{3:x2}{4:x2}{5:x2}-{6:x2}{7:x2}{8:x2}", hashedId[0], hashedId[1], hashedId[2], hashedId[3], hashedId[4], hashedId[5], hashedId[6], hashedId[7], hashedId[8]);
			return systemId;
		}
	}
}
