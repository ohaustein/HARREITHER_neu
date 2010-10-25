using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class ModulKlimaDeckeConstructionAkustik : ModulKlimaDeckeConstruction {
		private double rotation = 0;
		private double schienenBreite = 0.1; // meter
		private double schienenAbstand = 0.5; // meter
		private double offset = 0; // meter
		private double randfries = 0.2; // meter

		public ModulKlimaDeckeConstructionAkustik() {
		}

		public double Rotation {
			get { return this.rotation; }
			set { this.rotation = value; }
		}

		public double SchienenBreite {
			get { return this.schienenBreite; }
			set { this.schienenBreite = value; }
		}

		public double SchienenAbstand {
			get { return this.schienenAbstand; }
			set { this.schienenAbstand = value; }
		}

		public double Offset {
			get { return this.offset; }
			set { this.offset = value; }
		}

		public double Randfries {
			get { return this.randfries; }
			set { this.randfries = value; }
		}

		public override void Paint(Graphics g) {
			// TODO
		}

		public override bool HitTest(Point2D planPoint, Point pointInControl) {
			throw new Exception("The method or operation is not implemented.");
		}

		public override void StartDrag(Point2D planPoint, Point pointInControl) {
			throw new Exception("The method or operation is not implemented.");
		}

		public override void MoveDrag(Point2D planPoint, Point pointInControl) {
			throw new Exception("The method or operation is not implemented.");
		}

		public override void EndDrag(Point2D planPoint, Point pointInControl) {
			throw new Exception("The method or operation is not implemented.");
		}

		[XmlIgnore]
		public override Cursor PickCursor {
			get { return null; }
		}
	}
}
