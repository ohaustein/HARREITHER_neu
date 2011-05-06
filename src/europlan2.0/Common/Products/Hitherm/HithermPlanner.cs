using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using WW.Math;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Europlan.Common {
	public class HithermPlanner : Component, IWallProductPlanner {

		public enum HithermPlannerMode {
			HPM_NONE,
			HPM_ADD_REGISTER
		}

		private GraphicalWallPanel connectedWallPanel;
		private HithermPlannerMode mode = HithermPlannerMode.HPM_NONE;

		private Nullable<Point2D> dragStart = null;
		private Point2D dragEnd = Point2D.Zero;

		private GraphicalHithermRegisterWrapper newRegister = null;
		private GraphicalWall newRegisterWall = null;
		private double newRegisterWallXOffset = 0;
		private double newRegisterWallYOffset = 0;

		private HithermProduct product = null;

		#region IWallProductPlanner Members
		public GraphicalWallPanel ConnectedWallPanel {
			get { return this.connectedWallPanel; }
			set {
				/*if (this.connectedWallPanel != null) {
					this.connectedWallPanel.KeyDown -= new KeyEventHandler(connectedPlanPanel_KeyDown);
				}*/
				this.connectedWallPanel = value;
				/*if (this.connectedWallPanel != null) {
					this.connectedWallPanel.KeyDown += new KeyEventHandler(connectedPlanPanel_KeyDown);
				}*/
			}
		}

		public System.Windows.Forms.Cursor CustomCursor {
			get { return null; }
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, WW.Math.Point2D mousePositionInPlan, System.Drawing.Point mousePositionInControl, double scale) {
			this.PaintAfterPlanPannel(e.Graphics, mousePositionInPlan, mousePositionInControl, scale);
		}

		public void PaintAfterPlanPannel(System.Drawing.Graphics g, WW.Math.Point2D mousePositionInPlan, System.Drawing.Point mousePositionInControl, double scale) {
			if (this.mode == HithermPlannerMode.HPM_ADD_REGISTER && dragStart != null) {
				float x = (float)(this.dragStart.Value.X < mousePositionInPlan.X ? this.dragStart.Value.X : this.dragEnd.X);
				float width = (float)Math.Abs(this.dragStart.Value.X - this.dragEnd.X);
				float y = (float)(this.dragStart.Value.Y < mousePositionInPlan.Y ? this.dragStart.Value.Y : this.dragEnd.Y);
				float height = (float)Math.Abs(this.dragStart.Value.Y - this.dragEnd.Y);
				Brush brush = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Green, Color.Transparent);
				Pen pen = new Pen(brush, (float)(1.0 / scale));
				g.DrawRectangle(pen, x, y, width, height);
				if (this.newRegister != null) {
					this.newRegister.PaintObject(g, newRegisterWallXOffset, newRegisterWallYOffset, Color.Green, scale);
				}
			}
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}
			return false;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}

			return false;
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}

			this.dragStart = planPoint;

			if (this.mode == HithermPlannerMode.HPM_ADD_REGISTER) {
				this.newRegisterWall = this.GetWallForPoint(planPoint, out this.newRegisterWallXOffset, out this.newRegisterWallYOffset);
				if (this.newRegisterWall != null) {
					this.newRegister = new GraphicalHithermRegisterWrapper();
				}
			}

			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}

			this.dragEnd = planPoint;

			if (this.mode == HithermPlannerMode.HPM_ADD_REGISTER && this.dragStart.HasValue && this.newRegister != null) {
				double width = Math.Abs(this.dragStart.Value.X - this.dragEnd.X);
				double height = Math.Abs(this.dragStart.Value.Y - this.dragEnd.Y);
				Nullable<HithermRegister.HithermRegisterTypeEnum> registerType = HithermRegister.GetRegisterTypeForHoehe((int)Math.Floor(height), true);
				if (registerType.HasValue) {
					if (this.newRegister.Register == null) {
						this.newRegister.Register = new HithermRegister();
					}
					this.newRegister.Register.RegisterType = registerType.Value;
					this.newRegister.Register.RegisterBreiteForDrawing = width;
					if (this.newRegister.Register.RegisterBreiteForDrawing > width) {
						this.newRegister.Register = null;
					} else {
						double x = this.dragStart.Value.X < this.dragEnd.X ? this.dragStart.Value.X : this.dragStart.Value.X - this.newRegister.Register.RegisterBreiteForDrawing;
						double y = this.dragStart.Value.Y < this.dragEnd.Y ? this.dragStart.Value.Y : this.dragStart.Value.Y - this.newRegister.Register.RegisterHoehe;
						this.newRegister.Register.GraphPosX = x - this.newRegisterWallXOffset;
						this.newRegister.Register.GraphPosY = y - this.newRegisterWallYOffset;
						this.newRegister.Register.GraphWallId = this.newRegisterWall.Id;
					}
				} else {
					this.newRegister.Register = null;
				}
				return true;
			}
			return false;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}

			if (this.mode == HithermPlannerMode.HPM_ADD_REGISTER && this.dragStart.HasValue && this.newRegister != null) {
				this.newRegister.Register.Wall = this.newRegisterWall.Wall;
				this.newRegisterWall.Registers.Add(this.newRegister);
				HithermCircuit c = new HithermCircuit();
				c.Registers.Add(this.newRegister.Register);
				this.product.PlannedCircuits.Add(c);
			}

			this.newRegister = null;
			this.newRegisterWall = null;

			this.dragStart = null;
			return false;
		}

		public bool PlannerKeyPress(System.Windows.Forms.Keys key) {
			if (this.mode == HithermPlannerMode.HPM_NONE) {
				return false;
			}
			return false;
		}
		#endregion

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HithermProduct Product {
			get { return this.product; }
			set {
				this.product = value;
				foreach (GraphicalWall wall in this.product.AssociatedRoom.Walls) {
					wall.Registers.Clear();
				}
				foreach (HithermCircuit c in product.PlannedCircuits) {
					foreach (HithermRegister r in c.Registers) {
						GraphicalWall wall = this.product.AssociatedRoom.GetWallForId(r.GraphWallId);
						if (wall != null) {
							wall.Registers.Add(new GraphicalHithermRegisterWrapper(r));
						}
					}
				}
				if (this.ConnectedWallPanel != null) {
					if (this.product == null || this.product.AssociatedRoom == null) {
						this.ConnectedWallPanel.Room = null;
					} else {
						this.ConnectedWallPanel.Room = this.product.AssociatedRoom;
						Room room = this.product.AssociatedRoom;
					}
				}
			}
		}

		private GraphicalWall GetWallForPoint(Point2D planPoint, out double xOffset, out double yOffset) {
			xOffset = 0;
			yOffset = 0;
			if (this.product == null || this.product.AssociatedRoom == null) {
				return null;
			}
			GraphicalWall pickedWall = null;
			foreach (GraphicalWall wall in this.product.AssociatedRoom.Walls) {
				pickedWall = wall.GetPickedWall(planPoint, xOffset, 0);
				if (pickedWall != null) {
					yOffset = wall.GetWallYOffset(pickedWall, 0).Value;
					break;
				}
				xOffset += wall.GetWallWidth() * 100;
			}
			if (pickedWall == null) {
				xOffset = 0;
				yOffset = 0;
			}
			return pickedWall;
		}

		/// <summary>
		/// Returns x-offset in m
		/// </summary>
		/// <param name="wall"></param>
		/// <returns></returns>
		private Nullable<double> GetWallXOffset(GraphicalWall wall) {
			if (this.product == null || this.product.AssociatedRoom == null) {
				return null;
			}
			double xOffset = 0;
			foreach (GraphicalWall w in this.product.AssociatedRoom.Walls) {
				if (w.GetWallYOffset(wall, 0).HasValue) { // quick hack to determine if the searched wall is a dachschräge of w
					return xOffset;
				}
				xOffset += w.GetWallWidth();
			}
			return null;
		}

		private Nullable<double> GetWallYOffset(GraphicalWall wall) {
			if (this.product == null || this.product.AssociatedRoom == null) {
				return null;
			}
			Nullable<double> yOffset = null;
			foreach (GraphicalWall w in this.product.AssociatedRoom.Walls) {
				yOffset = w.GetWallYOffset(wall, 0);
				if (yOffset.HasValue) {
					return yOffset;
				}
			}
			return null;
		}

		public HithermPlannerMode Mode {
			get { return this.mode; }
			set { this.mode = value; }
		}
	}
}
