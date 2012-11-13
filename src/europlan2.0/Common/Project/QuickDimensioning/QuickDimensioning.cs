using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Xml.Serialization;

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
		private ProductCheckState hithermCompactRoofCheckState = ProductCheckState.None;
		private ProductCheckState modulBodenCheckState = ProductCheckState.None;
		private ProductCheckState modulDeckeCheckState = ProductCheckState.None;
		private float heatFlowTemperature = 35;
		private float coolFlowTemperature = 16;
		private Europlan.Common.EurovalProduct.EurovalLayDistance layDistance = Europlan.Common.EurovalProduct.EurovalLayDistance.EV20;
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

		public ProductCheckState HithermCompactRoofCheckState {
			get { return hithermCompactRoofCheckState; }
			set { hithermCompactRoofCheckState = value; }
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

		public Europlan.Common.EurovalProduct.EurovalLayDistance LayDistance {
			get { return layDistance; }
			set { layDistance = value; }
		}

		public float CeilingAllocation {
			get { return ceilingAllocation; }
			set { ceilingAllocation = value; }
		}

		[XmlIgnore]
		public List<Distributor> Distributors {
		    get {
				List<Distributor> result = new List<Distributor>();
				foreach (Floor floor in Project.Instance.Floors) {
					foreach (Distributor distributor in floor.Distributors) {
						result.Add(distributor);
					}
				}
				return result;
			}
		}

		public List<QuickDimensioningDistributorsReportWrapper> GetQuickDimensioningDistributorsReports() {
			List<QuickDimensioningDistributorsReportWrapper> result = new List<QuickDimensioningDistributorsReportWrapper>();
			Dictionary<string, Distributor> distributorMapping = new Dictionary<string, Distributor>();
			foreach (Distributor distributor in Project.Instance.QuickDimensioning.Distributors) {
				distributorMapping.Add(distributor.Id, distributor);
			}
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (Product product in room.UsedProductsForQuickDimensioning) {
						foreach (KeyValuePair<string, int> distributorEntry in product.QuickDimensioningConnectedDistributors) {
							if (distributorMapping.ContainsKey(distributorEntry.Key) && distributorEntry.Value > 0) {
								result.Add(new QuickDimensioningDistributorsReportWrapper(distributorMapping[distributorEntry.Key], product));
							}
						}
					}
				}
			}
			return result;
		}

		public List<QuickDimensioningReportWrapper> GetQuickDimensioningRoomReports() {
			List<string> productOrder = GetPlannedProducts();
			List<QuickDimensioningReportWrapper> wrapperList = new List<QuickDimensioningReportWrapper>();
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					QuickDimensioningReportWrapper wrapper = new QuickDimensioningReportWrapper(room, floor, productOrder);
					wrapperList.Add(wrapper);
				}
			}
			return wrapperList;
		}

		public List<string> GetPlannedProducts() {
			List<string> productOrder = new List<string>();
			if (eurovalCheckState != ProductCheckState.None) {
				productOrder.Add(EurovalProduct.QuickDimensioningNameStatic);
			}
			if (concreteActivationCheckState != ProductCheckState.None) {
				productOrder.Add(ConcreteActivationProduct.QuickDimensioningNameStatic);
			}
			if (hithermCheckState != ProductCheckState.None) {
				productOrder.Add(HithermProduct.QuickDimensioningNameStatic);
			}
			if (hithermCompactCheckState != ProductCheckState.None) {
				productOrder.Add(HithermCompactProduct.QuickDimensioningNameStatic);
			}
			if (hithermCompactRoofCheckState != ProductCheckState.None) {
				productOrder.Add(HithermCompactRoofProduct.QuickDimensioningNameStatic);
			}
			if (modulBodenCheckState != ProductCheckState.None) {
				productOrder.Add(ModulKlimaBodenProduct.QuickDimensioningNameStatic);
			}
			if (modulDeckeCheckState != ProductCheckState.None) {
				productOrder.Add(ModulKlimaDeckeProduct.QuickDimensioningNameStatic);
			}
			return productOrder;
		}


	}
	
}
