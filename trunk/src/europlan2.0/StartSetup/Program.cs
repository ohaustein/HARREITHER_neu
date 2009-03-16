using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace StartSetup {
	public class Program {

		static void Main(string[] args) {
			try {
				Process installProcess = new Process();
				//MessageBox.Show(Application.StartupPath);
				installProcess.StartInfo.FileName = "msiexec";
				installProcess.StartInfo.Arguments = "/i \"" + Path.Combine(Application.StartupPath, "setup.msi") + "\"";
				//MessageBox.Show(installProcess.StartInfo.Arguments);
				installProcess.StartInfo.CreateNoWindow = true;

				installProcess.StartInfo.UseShellExecute = true;

				installProcess.Start();
				installProcess.WaitForExit();

				MoveFileEx(Path.Combine(Application.StartupPath, "setup.exe"), null, MOVEFILE_DELAY_UNITL_REBOOT);
				MoveFileEx(Path.Combine(Application.StartupPath, "tc.msi"), null, MOVEFILE_DELAY_UNITL_REBOOT);
				MoveFileEx(Path.Combine(Application.StartupPath, "instmsi.exe"), null, MOVEFILE_DELAY_UNITL_REBOOT);
				MoveFileEx(Application.StartupPath, null, MOVEFILE_DELAY_UNITL_REBOOT);

			} catch (Exception e) {
				MessageBox.Show(e.ToString());
			}
		}

		private static long MOVEFILE_REPLACE_EXISTING = 0x1;
		private static long MOVEFILE_COPY_ALLOWED = 0x2;
		private static long MOVEFILE_DELAY_UNITL_REBOOT = 0x4;
		private static long MOVEFILE_WRITE_THROUGH = 0x8;
		private static long MOVEFILE_CREATE_HARDLINK = 0x10;
		private static long MOVEFILE_FAIL_IF_NOT_TRACKABLE = 0x20;

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern long MoveFileEx(string existingFileName, string newFileName, long flags);
	}
}
