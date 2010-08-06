using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	[Serializable()]
	public class CadPlan : Plan {
		private double scale = 1.0;
		private double translationX = 0.0;
		private double translationY = 0.0;

		private List<string> disabledLayers = new List<string>();

		public double Scale {
			get { return this.scale; }
			set { this.scale = value; }
		}

		public double TranslationX {
			get { return this.translationX; }
			set { this.translationX = value; }
		}

		public double TranslationY {
			get { return this.translationY; }
			set { this.translationY = value; }
		}

		public List<string> DisabledLayers {
			get { return this.disabledLayers; }
		}
	}

}
