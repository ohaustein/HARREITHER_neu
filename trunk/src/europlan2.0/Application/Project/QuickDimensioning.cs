using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Europlan.Application {
	
	public class QuickDimensioning : IGuiRepresentation {

		[NonSerialized]
		private static readonly ILog log = LogManager.GetLogger(typeof(QuickDimensioning));

		public QuickDimensioning() {
			InitializeQuickDimensioning();
		}

		private void InitializeQuickDimensioning() {

		}

		public Type AssociatedPanelType {
			get { return typeof(QuickDimensioningPanel); }
		}

		public System.Drawing.Icon AssociatedIcon {
			get { return null; }
		}

	}
	
}
