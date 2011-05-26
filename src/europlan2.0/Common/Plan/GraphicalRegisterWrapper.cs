using System;
using System.Collections.Generic;
using System.Text;
using WW.Math.Geometry;
using WW.Math;
using System.Xml.Serialization;

namespace Europlan.Common {
	public abstract class GraphicalRegisterWrapper : IGraphicalWallObject {

		#region IGraphicalWallObject Members

		public abstract void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale, bool export);

		public abstract IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract Polygon2D GetObjectBorders(double xOffset, double yOffset);

		public abstract bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract bool CollisionTest(Polygon2D polygon, double xOffset, double yOffset, bool ignoreBorders);

		public abstract List<Anchor> GetAnchors(double scale);

		public abstract bool StartDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);
		public abstract bool MoveDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);
		public abstract bool EndDrag(Anchor anchor, Point2D planPoint, GraphicalWall owningWall, Room owningRoom, Product owningProduct, bool useSnap);


		public abstract void BackupState();
		public abstract void RevertState();

		public abstract bool CheckValidity(GraphicalWall owningWall, double offsetX, double offsetY);

		public virtual bool IsMoveable {
			get { return true; }
		}
		#endregion

		protected bool error = false;
		[XmlIgnore]
		public bool Error {
			get { return this.error; }
			set { this.error = value; }
		}

		private bool isNew = false;
		[XmlIgnore]
		public bool IsNew {
			get { return this.isNew; }
			set { this.isNew = value; }
		}


		public abstract bool SnapToHelplines(List<double> helplines, bool snapTop, bool snapBottom);
	}
}
