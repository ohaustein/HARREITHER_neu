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
		protected List<PossibleModulLane> possibleLanes = new List<PossibleModulLane>();

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
		public List<PossibleModulLane> PossibleLanes {
			get { return this.possibleLanes; }
		}
	}

}
