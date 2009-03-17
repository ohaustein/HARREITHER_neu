using System;
using System.Collections.Generic;
using System.Windows.Forms;
using log4net;
using System.Diagnostics;
using Star.SettingsXpress;
using System.Threading;
using System.Globalization;

namespace Europlan.Application {
	
	static class Program {

		private static readonly ILog log = LogManager.GetLogger(typeof(Program));
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			log4net.Config.XmlConfigurator.Configure();
			log.Debug("Starting Application");

			// using a customized class of SettingsFile which does not consider the assembly version for storing the settings
			SettingsFile.Create();

			SettingsKey settings = SettingsFile.Settings["OptionsForm"];
			string language = settings.GetSetting("Language", Thread.CurrentThread.CurrentUICulture.ToString());
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

			if (IsApplicationAlreadyRunning()) {
				MessageBox.Show("Es läuft bereits eine Instanz von Europlan auf diesem Rechner.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			System.Windows.Forms.Application.Run(new MainForm());
			
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