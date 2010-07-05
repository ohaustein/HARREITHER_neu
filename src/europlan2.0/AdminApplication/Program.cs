using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Europlan.Licensing;
using System.IO;

namespace Europlan.AdminApplication {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			EncryptionManager.Instance.Key = "<RSAKeyValue><Modulus>16CcCS56V1YIkjht12jOg5G2597D2lv+AwKMAKvueYpT2X5iE42yuKm/DFDhEIeyGckKe+Edz/T4OtpIq+5oTQ==</Modulus><Exponent>AQAB</Exponent><P>9vqsAPi2ljejeDM0Ce0tBG2Y3JsXDskIo9WBLypHiZ8=</P><Q>34DJ/Gk+O82xTY5fEc4ssVNoqSg9azhxAZYna0JuXpM=</Q><DP>lQX78zofMdSwlj6PD85Ec3+N1M7fwtQamoChSkxAjhE=</DP><DQ>Y2Tp7rd69quF8wCipM90rrNyMu/zSHCESPlWtGuJm8M=</DQ><InverseQ>iaTFIP3unJMIlPHKNUf+E1dq2k+77Rs3sZRMkk7ATlg=</InverseQ><D>Tk/8m7EPqcyLTF/GNOfRqSMw5MGZLCMg/ALeyhengug5b/9oYr/9S+RerfCCTM9oyO2dnFEn5QQRLUMD394rWQ==</D></RSAKeyValue>";

			string appDataPath = Path.GetDirectoryName(System.Windows.Forms.Application.CommonAppDataPath);
			if (!File.Exists(Path.Combine(appDataPath, "BruttoPreise.csv"))) {
				File.Copy(Path.Combine(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Config"), "BruttoPreise.csv"), Path.Combine(appDataPath, "BruttoPreise.csv"));
			}
			if (!File.Exists(Path.Combine(appDataPath, "global.conf"))) {
				File.Copy(Path.Combine(Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Config"), "global.conf"), Path.Combine(appDataPath, "global.conf"));
			}

			System.Windows.Forms.Application.Run(new MainForm());
		}
	}
}