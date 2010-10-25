using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using System.Windows.Forms;
using WW.Math.Geometry;

namespace Europlan.Common {
	[XmlInclude(typeof(ModulKlimaDeckeConstructionGlatt))]
	[XmlInclude(typeof(ModulKlimaDeckeConstructionAkustik))]
	public abstract class ModulKlimaDeckeConstruction : IPickableObject, IDragableObject {
		public abstract void Paint(Graphics g);

		#region IPickableObject Members
		public abstract bool HitTest(Point2D planPoint, Point pointInControl);
		#endregion

		#region IDragableObject Members
		public abstract void StartDrag(Point2D planPoint, Point pointInControl);
		public abstract void MoveDrag(Point2D planPoint, Point pointInControl);
		public abstract void EndDrag(Point2D planPoint, Point pointInControl);
		#endregion

		private ModulKlimaDeckePlanner planner;

		[XmlIgnore]
		public ModulKlimaDeckePlanner Planner {
			get { return this.planner; }
			set { this.planner = value; }
		}

		[XmlIgnore]
		public abstract Cursor PickCursor {
			get;
		}

		protected Matrix4D AdditionalTransformation {
			get {
				return this.Planner.ConnectedPlanPanel.PlanTransformation;
			}
		}
	}

	public class PossibleModulRowArea {
		private Polygon2D area;
		private double length;
		private double width;

		public PossibleModulRowArea(Polygon2D area) {
			this.Area = area;
			length = 0;
			width = 0;
		}

		public Polygon2D Area {
			get { return this.area; }
			set {
				this.area = value;
				length = new Segment2D(this.area[0], this.area[1]).GetLength();
				width = new Segment2D(this.area[1], this.area[2]).GetLength();
			}
		}

		[XmlIgnore]
		public double Length {
			get { return this.length; }
		}

		[XmlIgnore]
		public double Width {
			get { return this.width; }
		}
	}

	public class CompareablePair<V> : IComparable where V : IComparable {
		public V value1;
		public V value2;

		public CompareablePair() {
		}

		public CompareablePair(V value1, V value2) {
			this.value1 = value1;
			this.value2 = value2;
		}

		#region IComparable Members
		public int CompareTo(object obj) {
			if (obj is CompareablePair<V>) {
				int result = this.value1.CompareTo((obj as CompareablePair<double>).value1);
				if (result != 0) {
					return result * -1;
				}
				return this.value2.CompareTo((obj as CompareablePair<double>).value2) * -1;
			}
			return 0;
		}
		#endregion
	}
}
