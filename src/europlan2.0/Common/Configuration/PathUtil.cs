using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Europlan.Common {
	public static class PathUtil {

		private static string dataPath = Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath);
		private static bool useCommonAppDataPath = true;

		public static bool UseCommonAppDataPath {
			set {
				if (value != useCommonAppDataPath) {
					dataPath = value ? Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath) :
						Path.Combine(Application.StartupPath, "data");
					useCommonAppDataPath = value;
				}
			}
			get {
				return useCommonAppDataPath;
			}
		}

		public static string DataPath {
			get { return dataPath; }
		}
	}
}
