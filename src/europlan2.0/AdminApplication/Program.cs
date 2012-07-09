using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Europlan.Licensing;
using System.IO;
using System.Diagnostics;
using Europlan.Common;

namespace Europlan.AdminApplication {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		[STAThread]
		static void Main() {
			//DATA_PATH = Path.Combine(Application.StartupPath, "data");
#if DEBUG 
			PathUtil.UseCommonAppDataPath = true;
#else
			PathUtil.UseCommonAppDataPath = false;
#endif
			if (!Directory.Exists(PathUtil.DataPath)) {
				string message = "Die benötigten Konfigurationsdateiein wurden nicht gefunden";
				string caption = "Europlan 2.0 Wartungsbereich kann nicht gestartet werden!";
				MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			string lockedPath = Path.Combine(PathUtil.DataPath, "locked");
			if (File.Exists(lockedPath)) {
				StreamReader sr = new StreamReader(new FileStream(lockedPath, FileMode.Open));
				string otherUser = sr.ReadToEnd();
				sr.Close();
				string message;
				if (string.IsNullOrEmpty(otherUser)) {
					message = "Ein anderer Benutzer arbeitet bereits mit dem Europlan 2.0 Wartungsbereich. Bitte warten Sie bis bis dieser Benutzer den Wartungsbereich geschlossen hat!";
				} else {
					message = "Ein anderer Benutzer (" + otherUser + ") arbeitet bereits mit dem Europlan 2.0 Wartungsbereich. Bitte warten Sie bis bis dieser Benutzer den Wartungsbereich geschlossen hat!";
				}
				string caption = "Europlan 2.0 Wartungsbereich kann nicht gestartet werden!";
				MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			StreamWriter sw = new StreamWriter(new FileStream(lockedPath, FileMode.CreateNew));
			sw.Write(Environment.UserName);
			sw.Close();
			/*if (IsApplicationAlreadyRunning()) {
				string message = "Auf diesem Rechner läuft bereits eine Instanz des Europlan 2.0 Wartungsbereiches. Eventuell arbeitet ein anderer Benutzer damit. Bitte warten Sie bis bis dieser Benutzer den Wartungsbereich geschlossen hat!";
				string caption = "Europlan 2.0 Wartungsbereich kann nicht gestartet werden!";
				MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}*/

			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			EncryptionManager.Instance.Key = "<RSAKeyValue><Modulus>16CcCS56V1YIkjht12jOg5G2597D2lv+AwKMAKvueYpT2X5iE42yuKm/DFDhEIeyGckKe+Edz/T4OtpIq+5oTQ==</Modulus><Exponent>AQAB</Exponent><P>9vqsAPi2ljejeDM0Ce0tBG2Y3JsXDskIo9WBLypHiZ8=</P><Q>34DJ/Gk+O82xTY5fEc4ssVNoqSg9azhxAZYna0JuXpM=</Q><DP>lQX78zofMdSwlj6PD85Ec3+N1M7fwtQamoChSkxAjhE=</DP><DQ>Y2Tp7rd69quF8wCipM90rrNyMu/zSHCESPlWtGuJm8M=</DQ><InverseQ>iaTFIP3unJMIlPHKNUf+E1dq2k+77Rs3sZRMkk7ATlg=</InverseQ><D>Tk/8m7EPqcyLTF/GNOfRqSMw5MGZLCMg/ALeyhengug5b/9oYr/9S+RerfCCTM9oyO2dnFEn5QQRLUMD394rWQ==</D></RSAKeyValue>";

			//string appDataPath = Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath);
			/*if (!File.Exists(Path.Combine(PathUtil.DataPath, "BruttoPreise.csv"))) {
				File.Copy(Path.Combine(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Config"), "BruttoPreise.csv"), Path.Combine(PathUtil.DataPath, "BruttoPreise.csv"));
			}
			if (!File.Exists(Path.Combine(PathUtil.DataPath, "global.conf"))) {
				File.Copy(Path.Combine(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Config"), "global.conf"), Path.Combine(PathUtil.DataPath, "global.conf"));
			}*/

			System.Windows.Forms.Application.Run(new MainForm());
			File.Delete(lockedPath);
		}


		/*static bool IsApplicationAlreadyRunning() {
#if DEBUG
			return false;
#else
			string proc = Process.GetCurrentProcess().ProcessName;
			Process[] processes = Process.GetProcessesByName(proc);
			if (processes.Length > 1) {
				// wait as maybe the application got restarted and the process is not killed completely
				System.Threading.Thread.Sleep(2000);
				processes = Process.GetProcessesByName(proc);
				if (processes.Length > 1) {
					return true;
				} else {
					return false;
	}
			} else {
				return false;
			}
#endif
		}*/
	}
}