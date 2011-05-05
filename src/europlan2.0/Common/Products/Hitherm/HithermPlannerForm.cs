using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WW.Math;
using WW.Math.Geometry;

namespace Europlan.Common {
	public partial class HithermPlannerForm : Form {

		public HithermPlannerForm(HithermProduct product) {
			InitializeComponent();
			this.graphicalWallPanel1.Room = product.AssociatedRoom;
		}

		private void btnCreateWalls_Click(object sender, EventArgs e) {
			if (this.graphicalWallPanel1.Room != null && this.graphicalWallPanel1.Room.RoomCoordinates != null && this.graphicalWallPanel1.Room.RoomCoordinates.Count > 2 && this.graphicalWallPanel1.Room.AssociatedPlan != null && this.graphicalWallPanel1.Room.AssociatedPlan.Measure.HasValue) {
				this.graphicalWallPanel1.Room.Walls.Clear();
				double height = 2.5;
				double measure = this.graphicalWallPanel1.Room.AssociatedPlan.Measure.Value;
				Point2D lastVertex = this.graphicalWallPanel1.Room.RoomCoordinates[this.graphicalWallPanel1.Room.RoomCoordinates.Count - 1];
				Polygon2D roomCoords = new Polygon2D(this.graphicalWallPanel1.Room.RoomCoordinates);
				if (!roomCoords.IsClockwise()) {
					roomCoords.Reverse();
				}
				foreach (Point2D vertex in this.graphicalWallPanel1.Room.RoomCoordinates) {
					double length = (lastVertex - vertex).GetLength() / measure;
					GraphicalWall newWall = new GraphicalWall();
					newWall.PlanStartPoint = lastVertex;
					newWall.PlanEndPoint = vertex;
					newWall.CeilingContour.Add(new Point2D(0, height));
					newWall.CeilingContour.Add(new Point2D(length, height));
					this.graphicalWallPanel1.Room.Walls.Add(newWall);
					lastVertex = vertex;
				}
			}
			this.graphicalWallPanel1.InvalidateGraphics();
		}
	}
}