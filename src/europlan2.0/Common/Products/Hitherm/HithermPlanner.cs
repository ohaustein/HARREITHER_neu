using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Europlan.Common {
	public class HithermPlanner : Component, IWallProductPlanner {
		#region IWallProductPlanner Members

		public GraphicalWallPanel ConnectedWallPanel {
			get {
				throw new Exception("The method or operation is not implemented.");
			}
			set {
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public System.Windows.Forms.Cursor CustomCursor {
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, WW.Math.Point2D mousePositionInPlan, System.Drawing.Point mousePositionInControl) {
			throw new Exception("The method or operation is not implemented.");
		}

		public void PaintAfterPlanPannel(System.Drawing.Graphics g, WW.Math.Point2D mousePositionInPlan, System.Drawing.Point mousePositionInControl) {
			throw new Exception("The method or operation is not implemented.");
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			throw new Exception("The method or operation is not implemented.");
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			throw new Exception("The method or operation is not implemented.");
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			throw new Exception("The method or operation is not implemented.");
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			throw new Exception("The method or operation is not implemented.");
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, System.Windows.Forms.MouseButtons button) {
			throw new Exception("The method or operation is not implemented.");
		}

		public bool PlannerKeyPress(System.Windows.Forms.Keys key) {
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
