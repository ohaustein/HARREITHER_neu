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

			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			EncryptionManager.Instance.Key = "<RSAKeyValue><Modulus>16CcCS56V1YIkjht12jOg5G2597D2lv+AwKMAKvueYpT2X5iE42yuKm/DFDhEIeyGckKe+Edz/T4OtpIq+5oTQ==</Modulus><Exponent>AQAB</Exponent><P>9vqsAPi2ljejeDM0Ce0tBG2Y3JsXDskIo9WBLypHiZ8=</P><Q>34DJ/Gk+O82xTY5fEc4ssVNoqSg9azhxAZYna0JuXpM=</Q><DP>lQX78zofMdSwlj6PD85Ec3+N1M7fwtQamoChSkxAjhE=</DP><DQ>Y2Tp7rd69quF8wCipM90rrNyMu/zSHCESPlWtGuJm8M=</DQ><InverseQ>iaTFIP3unJMIlPHKNUf+E1dq2k+77Rs3sZRMkk7ATlg=</InverseQ><D>Tk/8m7EPqcyLTF/GNOfRqSMw5MGZLCMg/ALeyhengug5b/9oYr/9S+RerfCCTM9oyO2dnFEn5QQRLUMD394rWQ==</D></RSAKeyValue>";

			System.Windows.Forms.Application.Run(new MainForm());
			File.Delete(lockedPath);
		}
	}
}