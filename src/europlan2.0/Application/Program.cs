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
using System.Reflection;

namespace Europlan.Application {
	
	static class Program {

		private static readonly ILog log = LogManager.GetLogger(typeof(Program));

		public static Guid updateGuid = new Guid();
		public static string updateLocation = null;
		public static string updateBetaLocation = null;
		public static string updatePublicKey = null;

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

			RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\bluesource\\Europlan3.0");
			string defaultLanguage = "de";
			if (key != null) {
				string val = key.GetValue("SetupLanguage") as string;
				if (val != null) {
					if (val.Equals("en", StringComparison.InvariantCultureIgnoreCase)) {
						defaultLanguage = "en";
						Program.updateGuid = new System.Guid("77e24619-79f7-43da-bcb3-df6e1bb86c8a");
						Program.updateLocation = "http://helios.bluesource.at/EuroplanUpdates/grafisch/en";
                        Program.updateBetaLocation = "http://helios.bluesource.at/EuroplanUpdates/grafisch/beta/en";
						Program.updatePublicKey = "<RSAKeyValue><Modulus>vafl6B8Su3p8+ZZlrlPMzv7Yi6Pi9lBWcCH6DMLK/+2cXhFcXR7DjI8tNo6" +
							"diL0RDvIiZDEZ3F2BaP6j8fAaJ9Gj3l+EV7J1YWfXA1ADHWEqRbVoBR9uO+3x8lghZcSvG4whg8xARnu" +
							"+9Y1NqzFmxUc1lEWfEtxm90FLaEOcMf0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValu" +
							"e>";
					}
				}
			}
			if (Program.updateLocation == null) {
				Program.updateGuid = new System.Guid("04e4c63a-a752-4fb3-a8ca-b7be57aed70b");
				Program.updateLocation = "http://helios.bluesource.at/EuroplanUpdates/grafisch/de";
                Program.updateBetaLocation = "http://helios.bluesource.at/EuroplanUpdates/grafisch/beta/de";
				Program.updatePublicKey = "<RSAKeyValue><Modulus>59wJZhijX3EKxFb0XOFOJiQMrWXNfXIuGlCS7PTFW1f64kmV1O/A/BFjg0B" +
					"OqY9lzp8IRVmm2gy3Md04HATIPx0MlpXD2GpJtPiy4BXRlRamdcOBNUr+2WeqR+y5b0Hm2UlI0ADzgrt" +
					"2pinPX0GfK7DaiCBux7vK6ksNlGXbnhc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValu" +
					"e>";
			}

			// using a customized class of SettingsFile which does not consider the assembly version for storing the settings
			SettingsFile.Create();

			SettingsKey settings = SettingsFile.Settings["OptionsForm"];
			string language = settings.GetSetting("Language", defaultLanguage);
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = GetSpecificCulture(Thread.CurrentThread.CurrentUICulture);

			if (IsApplicationAlreadyRunning()) {
				string message = EuroplanRes.General_LaeuftBereitsText;
				string caption = EuroplanRes.General_LaeuftBereitsTitel;
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
			// Preload the configuration
			Configuration config = Configuration.AdminTemplate;
			config = Configuration.UserTemplate;

			startingForm.Close();
			startingForm.Dispose();
            if (Debugger.IsAttached)
            {
                System.Windows.Forms.Application.Run(mainForm);
            }
            else
            {
                try
                {
                    System.Windows.Forms.Application.Run(mainForm);
#if DEBUG
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Catched unhandled exception: " + ex.StackTrace);
#else
			    } catch (Exception) {
#endif
                }
            }
		}

        // A little bit of a hack to get any non neutral culture for the current language, as our language
        // files only conain neutral cultures, but neutral cultures cannot be used for formatting dates for example.
        private static CultureInfo GetSpecificCulture(CultureInfo ci) {
            if (!ci.IsNeutralCulture) {
                return ci;
            }

            // Some hardcoded cultures
            if (new CultureInfo("de").Equals(ci)) {
                return new CultureInfo("de-DE");

            } else if (new CultureInfo("el").Equals(ci)) {
                return new CultureInfo("el-GR");

            } else if (new CultureInfo("en").Equals(ci)) {
                return new CultureInfo("en-US");

            } else if (new CultureInfo("hr").Equals(ci)) {
                return new CultureInfo("hr-HR");

            } else if (new CultureInfo("hu").Equals(ci)) {
                return new CultureInfo("hu-HU");

            } else if (new CultureInfo("it").Equals(ci)) {
                return new CultureInfo("it-IT");

            } else if (new CultureInfo("sk").Equals(ci)) {
                return new CultureInfo("sk-SK");

            } else if (new CultureInfo("sl").Equals(ci)) {
                return new CultureInfo("sl-SI");
            }

            // If we don't have a culture hardcoded, try to find any matching culture
            foreach (CultureInfo c in CultureInfo.GetCultures(CultureTypes.SpecificCultures)) {
                if (c.Parent.Equals(ci) && !c.IsNeutralCulture) {
                    return c;
                }
            }

            // If we don't findy any culture that matches at all we just use the current system culture.
            return Thread.CurrentThread.CurrentCulture;
        }

		static bool IsApplicationAlreadyRunning() {
#if DEBUG
			return false;
#else
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
#endif
		}
	}

}