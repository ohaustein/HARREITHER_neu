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

		protected double rotation = 0;
		protected List<Polygon2D> schienen = new List<Polygon2D>();
		protected List<PossibleModulRow> possibleRows = new List<PossibleModulRow>();

		public abstract void Paint(Graphics g, ModulKlimaDeckePlanner.KlimaDeckeMode mode);

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

		public double Rotation {
			get { return this.rotation; }
			set {
				this.rotation = value;
				this.RecalculateSchienen();
			}
		}

		[XmlIgnore]
		public double RotationRelativeToPlan {
			get {
				if (this.Planner.ConnectedPlanPanel.Plan is ImagePlan) {
					return this.rotation + (this.Planner.ConnectedPlanPanel.Plan as ImagePlan).Rotation;
				} else if (this.Planner.ConnectedPlanPanel.Plan is CadPlan) {
					return -this.rotation;
				}
				return this.rotation;
			}
			set {
				if (this.Planner.ConnectedPlanPanel.Plan is ImagePlan) {
					this.Rotation = value - (this.Planner.ConnectedPlanPanel.Plan as ImagePlan).Rotation;
				} else if (this.Planner.ConnectedPlanPanel.Plan is CadPlan) {
					this.Rotation = -value;
				} else {
					this.Rotation = value;
				}
			}
		}

		public abstract void RecalculateSchienen();

		[XmlIgnore]
		public List<Polygon2D> Schienen {
			get { return this.schienen; }
		}

		[XmlIgnore]
		public List<PossibleModulRow> PossibleRows {
			get { return this.possibleRows; }
		}
	}

	public class PossibleModulRow {
		private List<PossibleModulRowArea> areas = new List<PossibleModulRowArea>();
		private Line2D borderLeft;
		private Line2D borderRight;

		public PossibleModulRow(Line2D borderLeft, Line2D borderRight) {
			this.borderLeft = borderLeft;
			this.borderRight = borderRight;
		}

		public PossibleModulRow(List<PossibleModulRowArea> areas) {
			this.areas = areas;
			if (this.areas.Count > 0) {
				this.borderLeft = new Line2D(this.areas[0].TopLeft, this.areas[0].TopLeft - this.areas[0].BottomLeft);
				this.borderRight = new Line2D(this.areas[0].TopRight, this.areas[0].TopRight - this.areas[0].BottomRight);
			}
		}

		public List<PossibleModulRowArea> Areas {
			get { return this.areas; }
			set { this.areas = value; }
		}

		[XmlIgnore]
		public Line2D BorderLeft {
			get { return this.borderLeft; }
		}

		[XmlIgnore]
		public Line2D BorderRight {
			get { return this.borderRight; }
		}
	}

	public class PossibleModulRowArea {
		//private Polygon2D area;
		private Point2D topLeft, topRight, bottomRight, bottomLeft;
		private double top, bottom;
		private double length, width;

		public PossibleModulRowArea(/*Polygon2D area*/Point2D topLeft, Point2D bottomLeft, Point2D bottomRight, Point2D topRight, double top, double bottom) {
			//this.area = area;
			this.topLeft = topLeft;
			this.topRight = topRight;
			this.bottomRight = bottomRight;
			this.bottomLeft = bottomLeft;
			this.length = new Segment2D(this.topLeft, this.bottomLeft).GetLength();
			this.width = new Segment2D(this.topLeft, this.topRight).GetLength();
			this.top = top;
			this.bottom = bottom;
		}

		public Polygon2D Area {
			get {
				// return this.area;
				return new Polygon2D(new Point2D[] { this.topLeft, this.bottomLeft, this.bottomRight, this.topRight });
			}
			/*set {
				this.area = value;
				if (this.area.Count != 4) {
					this.area = null;
				}
				if (this.area == null) {
					length = 0;
					width = 0;
					//borderLeft = null;
					//borderRight = null;
				} else {
					length = new Segment2D(this.area[0], this.area[1]).GetLength();
					width = new Segment2D(this.area[1], this.area[2]).GetLength();
					//borderLeft = new Line2D(area[0], area[0] - area[3]);
					//borderRight = new Line2D(area[1], area[1] - area[2]);
				}
			}*/
		}

		[XmlIgnore]
		public double Length {
			get { return this.length; }
		}

		[XmlIgnore]
		public double Width {
			get { return this.width; }
		}

		[XmlIgnore]
		public Point2D TopLeft {
			get { return this.topLeft; }
		}

		[XmlIgnore]
		public Point2D TopRight {
			get { return this.topRight; }
		}

		[XmlIgnore]
		public Point2D BottomRight {
			get { return this.bottomRight; }
		}

		[XmlIgnore]
		public Point2D BottomLeft {
			get { return this.bottomLeft; }
		}

		[XmlIgnore]
		public double Top {
			get { return this.top; }
		}

		[XmlIgnore]
		public double Bottom {
			get { return this.bottom; }
		}

		public bool Fits(double moduleTop, double moduleBottom) {
			return (this.top <= moduleTop && this.bottom >= moduleBottom);
		}

		public Nullable<double> BestStart(double moduleTop, double moduleBottom, bool bottomUp) {
			if (bottomUp) {
				if (moduleBottom <= this.bottom) {
					if (moduleTop >= this.top) {
						return moduleTop;
					} else {
						return null;
					}
				} else {
					if (moduleTop <= this.bottom) {
						return this.BestStart(this.bottom + moduleTop - moduleBottom, this.bottom, bottomUp);
					} else {
						return null;
					}
				}
			} else {
				if (moduleTop >= this.top) {
					if (moduleBottom <= this.bottom) {
						return moduleTop;
					} else {
						return null;
					}
				} else {
					if (moduleBottom >= this.top) {
						return this.BestStart(this.top, this.top + moduleBottom - moduleTop, bottomUp);
					} else {
						return null;
					}
				}
			}
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
					return result;
				}
				return this.value2.CompareTo((obj as CompareablePair<double>).value2);
			}
			return 0;
		}
		#endregion
	}
}
