using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using WW.Math;
using System.Drawing;

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

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, WW.Math.Point2D mousePositionInPlan, System.Drawing.Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, mousePositionInPlan, mousePositionInControl);
		}

		public void PaintAfterPlanPannel(System.Drawing.Graphics g, WW.Math.Point2D mousePositionInPlan, System.Drawing.Point mousePositionInControl) {
			if (this.mode == HithermPlannerMode.HPM_ADD_REGISTER && dragStart != null) {
				float x = (float)(this.dragStart.Value.X < mousePositionInPlan.X ? this.dragStart.Value.X : this.dragEnd.X);
				float width = (float)Math.Abs(this.dragStart.Value.X - this.dragEnd.X);
				float y = (float)(this.dragStart.Value.Y < mousePositionInPlan.Y ? this.dragStart.Value.Y : this.dragEnd.Y);
				float height = (float)Math.Abs(this.dragStart.Value.Y - this.dragEnd.Y);
				g.DrawRectangle(Pens.Green, x, y, width, height);
				if (this.newRegister != null) {
					this.newRegister.PaintObject(g, newRegisterWallXOffset, newRegisterWallYOffset, Color.Green);
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
				double x = this.dragStart.Value.X < this.dragEnd.X ? this.dragStart.Value.X : this.dragEnd.X;
				double y = this.dragStart.Value.Y < this.dragEnd.Y ? this.dragStart.Value.Y : this.dragEnd.Y;
				double width = Math.Abs(this.dragStart.Value.X - this.dragEnd.X);
				double height = Math.Abs(this.dragStart.Value.Y - this.dragEnd.Y);
				Nullable<HithermRegister.HithermRegisterTypeEnum> registerType = HithermRegister.GetRegisterTypeForHoehe((int)Math.Floor(height), true);
				if (registerType.HasValue) {
					if (this.newRegister.Register == null) {
						this.newRegister.Register = new HithermRegister();
					}
					this.newRegister.Register.RegisterType = registerType.Value;
					this.newRegister.Register.RegisterBreiteForDrawing = width;
					this.newRegister.Register.GraphPosX = x;
					this.newRegister.Register.GraphPosY = y;
					this.newRegister.Register.GraphWallId = this.newRegisterWall.Id;
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

		private GraphicalWall GetWallForPoint(Point2D planPoint, out double xOffset, out double yOffset) {
			xOffset = 0;
			yOffset = 0;
			if (this.connectedWallPanel == null) {
				return null;
			}
			GraphicalWall pickedWall = null;
			foreach (GraphicalWall wall in this.connectedWallPanel.Room.Walls) {
				pickedWall = wall.GetPickedWall(planPoint, xOffset, 0);
				if (pickedWall != null) {
					yOffset = wall.GetWallYOffset(pickedWall, 0).Value;
					break;
				}
				xOffset += wall.GetWallWidth();
			}
			if (pickedWall == null) {
				xOffset = 0;
				yOffset = 0;
			}
			return pickedWall;
		}

		public HithermPlannerMode Mode {
			get { return this.mode; }
			set { this.mode = value; }
		}
	}
}
