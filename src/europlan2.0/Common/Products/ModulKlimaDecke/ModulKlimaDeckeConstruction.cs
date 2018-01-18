using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using System.Windows.Forms;
using WW.Math.Geometry;
using WW.Cad.Model;
using WW.Cad.Model.Tables;

namespace Europlan.Common {
	[XmlInclude(typeof(ModulKlimaDeckeConstructionGlatt))]
	[XmlInclude(typeof(ModulKlimaDeckeConstructionAkustik))]
	[XmlInclude(typeof(ModulKlimaDeckeConstructionKassette))]
	public abstract class ModulKlimaDeckeConstruction : IPickableObject, IDragableObject {

        public class LaneGap
        {
            public LaneGap()
            {
            }

            public LaneGap(int panelPositon, int shift)
            {
                this.PanelPosition = panelPositon;
                this.Shift = shift;
            }

            private int _panelPosition;
            public int PanelPosition { get { return _panelPosition; } set { _panelPosition = value; } }

            private int _shift;
            public int Shift { get { return _shift; } set { _shift = value; } }

            private Polygon2D _visual;

            public Polygon2D Visual
            {
                get { return _visual; }
                set { _visual = value; }
            }
        }

		protected double rotation = 0;
		protected List<Polygon2D> schienen = new List<Polygon2D>();
        protected List<LaneGap> visualGaps = new List<LaneGap>();
		protected List<PossibleModulLane> possibleLanes = new List<PossibleModulLane>();

		public abstract void Paint(Graphics g, ModulKlimaDeckePlanner.KlimaDeckeMode mode, bool drawBeplankung);
		public abstract void PaintDxf(DxfModel model, DxfLayer constructionLayer, DxfLayer beplankungLayer, bool drawBeplankung);

		#region IPickableObject Members
		public abstract bool HitTest(Point2D planPoint, Point pointInControl);
		#endregion

		#region IDragableObject Members
		public abstract void StartDrag(Point2D planPoint, Point pointInControl);
		public abstract void MoveDrag(Point2D planPoint, Point pointInControl);
		public abstract void EndDrag(Point2D planPoint, Point pointInControl);
		#endregion

		private ModulKlimaDeckeProduct product;
		private PlanPanel planPanel;

		[XmlIgnore]
		public ModulKlimaDeckeProduct Product {
			set { this.product = value; }
			get { return this.product; }
		}

		[XmlIgnore]
		public PlanPanel PlanPanel {
			set { this.planPanel = value; }
			get { return this.planPanel; }
		}
		
		[XmlIgnore]
		public abstract Cursor PickCursor {
			get;
		}

		protected Matrix4D AdditionalTransformation {
			get {
				if (/*this.Planner != null && */
					this.Product != null && 
					this.Product.AssociatedRoom != null && 
					this.Product.AssociatedRoom.AssociatedPlan != null &&
					this.Product.AssociatedRoom.AssociatedPlan is CadPlan) {
					return (this.Product.AssociatedRoom.AssociatedPlan as CadPlan).GdiGraphics3D.To2DTransform;
				} else {
					return Matrix4D.Identity;
				}
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
		public virtual double RotationRelativeToPlan {
			get {
				if (this.Product.AssociatedRoom.AssociatedPlan is ImagePlan) {
					return this.rotation + (this.Product.AssociatedRoom.AssociatedPlan as ImagePlan).Rotation;
				} else if (this.Product.AssociatedRoom.AssociatedPlan is CadPlan) {
					return -this.rotation;
				}
				return this.rotation;
			}
			set {
				if (this.Product.AssociatedRoom.AssociatedPlan is ImagePlan) {
					this.Rotation = value - (this.Product.AssociatedRoom.AssociatedPlan as ImagePlan).Rotation;
				} else if (this.Product.AssociatedRoom.AssociatedPlan is CadPlan) {
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
		public abstract List<Point2D> CeilingCoordinates {
			get;
		}

		[XmlIgnore]
		public List<PossibleModulLane> PossibleLanes {
			get { return this.possibleLanes; }
		}

		[XmlIgnore]
		public abstract ModulKlimaDeckeProduct.ModulCeilingConstructionEnum CeilingConstruction {
			get;
		}
	}

}
