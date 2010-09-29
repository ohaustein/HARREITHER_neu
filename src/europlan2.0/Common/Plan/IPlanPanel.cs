using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WW.Math;

namespace Europlan.Common {

	public enum PlanMode {
		PM_MOVE,
		PM_PICK_MEASURE,
		PM_PLANNER_CLICK,
		PM_PLANNER_DRAG,
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
		IProductPlanner ProductPlanner {
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

		ColorMode ColorMode {
			get;
		}

		ModifierKey ModifierKey {
			get;
		}

		System.Windows.Forms.Cursor Cursor {
			set;
			get;
		}

		bool SupportsSnap {
			get;
		}

		void AddScale(double addedScale, Nullable<Point2D> center);

		bool UnsavedChanges {
			get;
		}

		void Invalidate();
	}
}
