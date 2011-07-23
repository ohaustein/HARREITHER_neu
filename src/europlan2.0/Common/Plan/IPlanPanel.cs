using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math;
using System.Windows.Forms;

namespace Europlan.Common {

	public enum PlanMode {
		PM_MOVE,
		PM_PICK_MEASURE,
		PM_PLANNER_CLICK,
		PM_PLANNER_DRAG,
		PM_SET_DISTRIBUTOR
	}

	public enum ColorMode {
		CM_BLACK_BG,
		CM_WHITE_BG
	}

	public enum ModifierKey {
		MK_NONE,
		MK_SHIFT = 1,
		MK_CTRL = 2,
		MK_ALT = 4
	}

	public interface IPlanPanel {
		IPlanner ProductPlanner {
			get;
			set;
		}

		double PlanScale {
			get;
			set;
		}

		Vector2D PlanTranslation {
			get;
			set;
		}

		PlanMode Mode {
			get;
			set;
		}

		Plan Plan {
			get;
			set;
		}

		Matrix4D PlanTransformation {
			get;
		}

		double ScaleForCalculation {
			get;
		}

		ColorMode ColorMode {
			get;
		}

		ModifierKey ModifierKey {
			get;
		}

		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		System.Windows.Forms.Cursor PlanCursor {
			set;
			get;
		}

		bool SupportsSnap {
			get;
		}

		Matrix4D PlanToControl {
			get;
		}

		Matrix4D ControlToPlan {
			get;
		}

		void AddScale(double addedScale, Nullable<Point2D> center);

		bool UnsavedChanges {
			get;
		}

		void InvalidateGraphics();

		void SetPlanTransformations(double scale, double translationX, double translationY, double rotation);
		void GetPlanTransformations(out double scale, out double translationX, out double translationY, out double rotation);

		event KeyEventHandler KeyDown;
		event KeyEventHandler KeyUp;
	}
}
