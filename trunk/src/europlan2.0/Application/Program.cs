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

namespace Europlan.Application {
	
	static class Program {

		private static System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Program));
		private static readonly ILog log = LogManager.GetLogger(typeof(Program));

		public static string updateSubDir = "de";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			
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
						Program.updateSubDir = "en";
					}
				}
			}

			// using a customized class of SettingsFile which does not consider the assembly version for storing the settings
			SettingsFile.Create();

			SettingsKey settings = SettingsFile.Settings["OptionsForm"];
			string language = settings.GetSetting("Language", Thread.CurrentThread.CurrentUICulture.ToString());
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

			if (IsApplicationAlreadyRunning()) {
				string message = resources.GetString("AlreadyRunningMessage", Thread.CurrentThread.CurrentUICulture);
				string caption = resources.GetString("AlreadyRunningCaption", Thread.CurrentThread.CurrentUICulture);
				MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			MainForm mainForm = new MainForm();

			if (args.Length != 0) {
				mainForm.ProjectToLoad = args[0];
			}
			startingForm.Close();
			System.Windows.Forms.Application.Run(mainForm);
			
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