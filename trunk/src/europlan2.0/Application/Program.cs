using System;
using System.Collections.Generic;
using System.Windows.Forms;
using log4net;
using System.Diagnostics;
using Star.SettingsXpress;
using System.Threading;
using System.Globalization;
using Europlan.Licensing;
using System.IO;
using Microsoft.Win32;
using Europlan.Common;

namespace Europlan.Application {
	
	static class Program {

		private static readonly ILog log = LogManager.GetLogger(typeof(Program));

		public static Guid updateGuid = new Guid();
		public static string updateLocation = null;
		public static string updatePublicKey = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

			ResourcesManager.resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

			StartingForm startingForm = new StartingForm();
			startingForm.Show();
			startingForm.Update();
			log4net.Config.XmlConfigurator.Configure();
			log.Debug("Starting Application");

			RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\bluesource\\Europlan2.0");
			if (key != null) {
				string val = key.GetValue("SetupLanguage") as string;
				if (val != null) {
					if (val.Equals("en", StringComparison.InvariantCultureIgnoreCase)) {
						Program.updateGuid = new System.Guid("d38bcedd-464a-4fc9-8263-fbf6e43a4cae");
						Program.updateLocation = "http://helios.bluesource.at/EuroplanUpdates/en";
						Program.updatePublicKey = "<RSAKeyValue><Modulus>zx6ZJaMzPDozUcY5l0oq4y40M8qAyQobnURZXiVmsWxT5TnYa55yoxiZn9n" +
												  "ftDpsPc0duQTgwVUag1sj9uxzWp3ANn1bQtgohH0tsm1+j4fxA3Y91ba/v7zSfNfa6To1wnNOHNeyy1O" +
												  "40Pt4eXQXnbzmTocs322J+luWZPC3Fbc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValu" +
												  "e>";
					}
				}
			}
			if (Program.updateLocation == null) {
				Program.updateGuid = new System.Guid("1f24573e-033c-448d-8e15-9f396e67e44e");
				Program.updateLocation = "http://helios.bluesource.at/EuroplanUpdates/de";
				Program.updatePublicKey = "<RSAKeyValue><Modulus>vdBsdX09tG4aP1oLqHWHB3N6hHsEx+x0YbavsjRPuxhf1yLgkQFTN1Z26sV" +
										  "EkIKnfQxxvcfcWClR4P9Xurm2dyoeA2z80nexnKuyVQAW3K72Z0kkFKEj/OFeXoEWJi1rPrNXjwaQ3zV" +
										  "hn4WL1GKso8HiQlp34twvAJGx2A6JW7s=</Modulus><Exponent>AQAB</Exponent></RSAKeyValu" +
										  "e>";
			}

			// using a customized class of SettingsFile which does not consider the assembly version for storing the settings
			SettingsFile.Create();

			SettingsKey settings = SettingsFile.Settings["OptionsForm"];
			string language = settings.GetSetting("Language", Thread.CurrentThread.CurrentUICulture.ToString());
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

			if (IsApplicationAlreadyRunning()) {
				string message = ResourcesManager.resources.GetString("AlreadyRunningMessage", Thread.CurrentThread.CurrentUICulture);
				string caption = ResourcesManager.resources.GetString("AlreadyRunningCaption", Thread.CurrentThread.CurrentUICulture);
				MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			MainForm mainForm = new MainForm();

			if (args.Length != 0) {
				string projectFile = "";
				// path containing spaces will lead to fragmentation of filename in args array
				foreach (string part in args) {
					projectFile += part + " ";
				}
				mainForm.ProjectToLoad = projectFile.TrimEnd(); ;
			}
			// Preload the global configuration
			Configuration config = Configuration.UserTemplate;

			startingForm.Close();
			startingForm.Dispose();
			try {
				System.Windows.Forms.Application.Run(mainForm);
				//mainForm.Dispose();
			} catch (Exception ex) {
#if DEBUG
				MessageBox.Show("Catched unhandled exception: " + ex.StackTrace);
#endif
			}			
		}

		static bool IsApplicationAlreadyRunning() {
			string proc = Process.GetCurrentProcess().ProcessName;
			log.Debug("IsApplicationAlreadyRunning - checking for process: " + proc);
			Process[] processes = Process.GetProcessesByName(proc);
			if (processes.Length > 1) {
				log.Error("Process is already running - waiting as app may have been restarted");
				// wait as maybe the application got restarted and the process is not killed completely
				System.Threading.Thread.Sleep(2000);
				processes = Process.GetProcessesByName(proc);
				if (processes.Length > 1) {
					log.Error("Process is already running - waiting did not help");
					return true;
				} else {
					log.Debug("Waiting solved the problem - application can be started");
					return false;
				}
			} else {
				return false;
			}
		}

	}

}