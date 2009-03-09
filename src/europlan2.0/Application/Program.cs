using System;
using System.Collections.Generic;
using System.Windows.Forms;
using log4net;

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

			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			System.Windows.Forms.Application.Run(new MainForm());
			
		}
	}

}