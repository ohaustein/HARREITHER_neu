using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Europlan.Common {
	
	public class QuickDimensioning : IGuiRepresentation {

		public enum ProductCheckState {
			None = 0,
			Heat = 1,
			Cool = 2,
			HeatAndCool = 3
		}
		
		private ProductCheckState eurovalCheckState = ProductCheckState.None;
		private ProductCheckState concreteActivationCheckState = ProductCheckState.None;
		private ProductCheckState hithermCheckState = ProductCheckState.None;
		private ProductCheckState hithermCompactCheckState = ProductCheckState.None;
		private ProductCheckState modulBodenCheckState = ProductCheckState.None;
		private ProductCheckState modulDeckeCheckState = ProductCheckState.None;
		private float heatFlowTemperature = 35;
		private float coolFlowTemperature = 16;
		private Europlan.Common.EurovalProduct.LayDistance layDistance = Europlan.Common.EurovalProduct.LayDistance.EV20;
		private float ceilingAllocation = 80;

		[NonSerialized]
		private static readonly ILog log = LogManager.GetLogger(typeof(QuickDimensioning));

		public QuickDimensioning() {
			InitializeQuickDimensioning();
		}

		private void InitializeQuickDimensioning() {

		}

		public Type AssociatedPanelType {
			get { return typeof(QuickDimensioningPanel); }
		}

		public System.Drawing.Icon AssociatedIcon {
			get { return null; }
		}

		public ProductCheckState EurovalCheckState {
			get { return eurovalCheckState; }
			set { eurovalCheckState = value; }
		}

		public ProductCheckState ConcreteActivationCheckState {
			get { return concreteActivationCheckState; }
			set { concreteActivationCheckState = value; }
		}

		public ProductCheckState HithermCheckState {
			get { return hithermCheckState; }
			set { hithermCheckState = value; }
		}

		public ProductCheckState HithermCompactCheckState {
			get { return hithermCompactCheckState; }
			set { hithermCompactCheckState = value; }
		}

		public ProductCheckState ModulBodenCheckState {
			get { return modulBodenCheckState; }
			set { modulBodenCheckState = value; }
		}

		public ProductCheckState ModulDeckeCheckState {
			get { return modulDeckeCheckState; }
			set { modulDeckeCheckState = value; }
		}

		public float HeatFlowTemperature {
			get { return heatFlowTemperature; }
			set { heatFlowTemperature = value; }
		}

		public float CoolFlowTemperature {
			get { return coolFlowTemperature; }
			set { coolFlowTemperature = value; }
		}

		public Europlan.Common.EurovalProduct.LayDistance LayDistance {
			get { return layDistance; }
			set { layDistance = value; }
		}

		public float CeilingAllocation {
			get { return ceilingAllocation; }
			set { ceilingAllocation = value; }
		}

	}
	
}
