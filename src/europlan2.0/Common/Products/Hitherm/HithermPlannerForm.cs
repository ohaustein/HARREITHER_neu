using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WW.Math;
using WW.Math.Geometry;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class HithermPlannerForm : Form {

		public HithermPlannerForm(HithermProduct product) {
			InitializeComponent();
			this.graphicalWallPanel.Room = product.AssociatedRoom;
			this.btnCreateWalls.Enabled = this.graphicalWallPanel.Room != null && this.graphicalWallPanel.Room.RoomCoordinates != null && this.graphicalWallPanel.Room.RoomCoordinates.Count > 2 && this.graphicalWallPanel.Room.AssociatedPlan != null && this.graphicalWallPanel.Room.AssociatedPlan.Measure.HasValue;
		}

		private void btnCreateWalls_Click(object sender, EventArgs e) {
			this.graphicalWallPanel.Room.Walls.Clear();
			NewWallForm form = new NewWallForm(true, false);
			DialogResult result = form.ShowDialog();
			if (result == DialogResult.OK) {
				double height = form.Height / 100.0;
				double measure = this.graphicalWallPanel.Room.AssociatedPlan.Measure.Value;
				Point2D lastVertex = this.graphicalWallPanel.Room.RoomCoordinates[this.graphicalWallPanel.Room.RoomCoordinates.Count - 1];
				Polygon2D roomCoords = new Polygon2D(this.graphicalWallPanel.Room.RoomCoordinates);
				if (!roomCoords.IsClockwise()) {
					roomCoords.Reverse();
				}
				foreach (Point2D vertex in this.graphicalWallPanel.Room.RoomCoordinates) {
					double length = (lastVertex - vertex).GetLength() / measure;
					GraphicalWall newWall = new GraphicalWall();
					newWall.WallId = form.WallId;
					newWall.PlanStartPoint = lastVertex;
					newWall.PlanEndPoint = vertex;
					newWall.CeilingContour.Add(new Point2D(0, height));
					newWall.CeilingContour.Add(new Point2D(length, height));
					this.graphicalWallPanel.Room.Walls.Add(newWall);
					lastVertex = vertex;
				}
			}
			form.Dispose();
			this.graphicalWallPanel.InvalidateGraphics();
		}

		private void HithermPlannerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["HithermPlannerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
			if (settings.GetSetting("Maximized", false)) {
				this.WindowState = FormWindowState.Maximized;
			} else {
				this.WindowState = FormWindowState.Normal;
			}
		}

		private void HithermPlannerForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["HithermPlannerForm"];
			if (this.WindowState == FormWindowState.Normal) {
				settings.StorePoint("Location", this.Location);
				settings.StoreSize("Size", this.Size);
				settings.StoreSetting("Maximized", false);
			} else if (this.WindowState == FormWindowState.Maximized) {
				settings.StoreSetting("Maximized", true);
			}
			SettingsFile.Update();
		}

	}
}