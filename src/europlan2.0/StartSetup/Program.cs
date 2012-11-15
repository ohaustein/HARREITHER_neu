using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Youseful.Installer.WI;

namespace StartSetup {
	public class Program {

		static void Main(string[] args) {
			try {

				string foundProduct = new string(' ', 39);
				bool found = (Msidll.MsiEnumRelatedProducts("{F33AAB86-9453-478e-A55B-20CAF7047F33}", 0, 0, foundProduct) == 0);
				Process installProcess = new Process();
				installProcess.StartInfo.FileName = "msiexec";
				installProcess.StartInfo.Arguments = "/i \"" + Path.Combine(Application.StartupPath, "setup.msi") + (found ? " /qb+" : ""); //"\" REINSTALL=ALL REINSTALLMODE=vomus" : "\"");
				installProcess.StartInfo.CreateNoWindow = true;

				installProcess.StartInfo.UseShellExecute = true;

				installProcess.Start();
				installProcess.WaitForExit();

				MoveFileEx(Path.Combine(Application.StartupPath, "setup.exe"), null, MOVEFILE_DELAY_UNITL_REBOOT);
				MoveFileEx(Path.Combine(Application.StartupPath, "setup.msi"), null, MOVEFILE_DELAY_UNITL_REBOOT);
				MoveFileEx(Path.Combine(Application.StartupPath, "instmsi.exe"), null, MOVEFILE_DELAY_UNITL_REBOOT);
				MoveFileEx(Application.StartupPath, null, MOVEFILE_DELAY_UNITL_REBOOT);

			} catch (Exception e) {
				MessageBox.Show(e.ToString());
			}
		}

		//private static long MOVEFILE_REPLACE_EXISTING = 0x1;
		//private static long MOVEFILE_COPY_ALLOWED = 0x2;
		private static long MOVEFILE_DELAY_UNITL_REBOOT = 0x4;
		//private static long MOVEFILE_WRITE_THROUGH = 0x8;
		//private static long MOVEFILE_CREATE_HARDLINK = 0x10;
		//private static long MOVEFILE_FAIL_IF_NOT_TRACKABLE = 0x20;

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern long MoveFileEx(string existingFileName, string newFileName, long flags);
	}
}
