using System;
using System.Collections.Generic;
using System.Text;
using WW.Cad.Model;
using WW.Cad.Model.Tables;
using WW.Cad.IO;
using System.Xml.Serialization;
using WW.Cad.Drawing.GDI;
using WW.Cad.Drawing;
using WW.Math;
using WW.Cad.Base;
using System.Drawing;

namespace Europlan.Common {

	[Serializable()]
	public class CadPlan : Plan {
		private double scale = 1.0;
		private double translationX = 0.0;
		private double translationY = 0.0;
		private GDIGraphics3D gdiGraphics3D;
		private Bounds3D bounds;
		private double defaultHeight = 1000.0;
		private double defaultWidth = 1000.0;
		private double defaultMargin = 5.0;
		private Matrix4D toDefaultSize = Matrix4D.Identity;
		private Matrix4D fromDefaultSize = Matrix4D.Identity;

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

		public DxfModel LoadModel(bool createForExternalUse) {
			if (createForExternalUse) {
				if (this.AbsoluteFileName.EndsWith(".dwg", StringComparison.InvariantCultureIgnoreCase)) {
					return DwgReader.Read(this.AbsoluteFileName);
				} else {
					return DxfReader.Read(this.AbsoluteFileName);
				}
			} else {
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
			}
			return model;
		}

		[XmlIgnore]
		public override double Rotation {
			get { return 0.0; }
		}

		[XmlIgnore]
		public GDIGraphics3D GdiGraphics3D {
			get { return gdiGraphics3D; }
		}

		[XmlIgnore]
		public Matrix4D ToDefaultSize {
			get { return toDefaultSize; }
		}

		[XmlIgnore]
		public Matrix4D FromDefaultSize {
			get { return fromDefaultSize; }
		}

		[XmlIgnore]
		public Bounds3D Bounds {
			get { return bounds; }
		}
		
		internal void InitializeModel(DxfModel model) {
			GraphicsConfig graphicsConfig = new GraphicsConfig();
			graphicsConfig.CorrectColorForBackgroundColor = true;
			gdiGraphics3D = new GDIGraphics3D(graphicsConfig);
			bounds = new Bounds3D();
			gdiGraphics3D.CreateDrawables(model);
			gdiGraphics3D.BoundingBox(bounds);
			defaultWidth = (bounds.Delta.X >= bounds.Delta.Y) ? 1000.0 : 1000.0 * bounds.Delta.X / bounds.Delta.Y;
			defaultHeight = (bounds.Delta.X <= bounds.Delta.Y) ? 1000.0 : 1000.0 * bounds.Delta.Y / bounds.Delta.X;
			this.CalculateToDefaultSizeTransform();
		}

		private Matrix4D CalculateToDefaultSizeTransform() {
			toDefaultSize = DxfUtil.GetScaleTransform(bounds.Corner1, bounds.Corner2, bounds.Center,
				new Point3D(defaultMargin, defaultHeight - 2.0 * defaultMargin, 0.0),
				new Point3D(defaultWidth - 2.0 * defaultMargin, defaultMargin, 0.0),
				new Point3D(defaultWidth / 2.0, defaultHeight / 2.0, 0.0));
			fromDefaultSize = toDefaultSize.GetInverse();
			return toDefaultSize;
		}

	}

}
