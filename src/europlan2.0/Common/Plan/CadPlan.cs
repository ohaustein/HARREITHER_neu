using System;
using System.Collections.Generic;
using System.Text;
using WW.Cad.Model;
using WW.Cad.Model.Tables;
using WW.Cad.IO;
using System.Xml.Serialization;

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

		DxfModel model = null;

		public DxfModel LoadModel() {
			if (model == null) {
				if (this.AbsoluteFileName.EndsWith(".dwg", StringComparison.InvariantCultureIgnoreCase)) {
					model = DwgReader.Read(this.AbsoluteFileName);
				} else {
					model = DxfReader.Read(this.AbsoluteFileName);
				}
				foreach (DxfLayer layer in model.Layers) {
					layer.Enabled = !this.DisabledLayers.Contains(layer.Name);
				}
			}
			return model;
		}

		[XmlIgnore]
		public override double Rotation {
			get { return 0.0; }
		}
	}

}
