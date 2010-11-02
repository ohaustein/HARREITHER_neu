using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using log4net;

namespace Europlan.Common {
	public class FileUtils {
		private static readonly ILog log = LogManager.GetLogger(typeof(Product));

		public static void SetAccessForEveryone(string filename) {
			try {
				FileInfo fInfo = new FileInfo(filename);
				FileSecurity fSecurity = fInfo.GetAccessControl();
				SecurityIdentifier id = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
				fSecurity.AddAccessRule(new FileSystemAccessRule(id, FileSystemRights.FullControl, AccessControlType.Allow));
				fInfo.SetAccessControl(fSecurity);
			} catch (Exception e) {
				log.Error("Cannot set access for everyone on file '" + filename + "'", e);
			}
		}
	}
}
