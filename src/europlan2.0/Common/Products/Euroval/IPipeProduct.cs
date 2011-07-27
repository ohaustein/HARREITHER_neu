using System;
using System.Collections.Generic;
using System.Text;
using WW.Math.Geometry;
using WW.Math;

namespace Europlan.Common {
	public interface IPipeProduct<LayDistanceType, RimType> : IRequiredMaterial where LayDistanceType : struct where RimType : struct {

		#region Product Definitions
		SerializableDictionary<int, Circuit.CircuitConnection> ConnectedCircuits {
			get;
			set;
		}

		SerializableDictionary<int, Circuit.CircuitConnection> InverseConnectedCircuits {
			get;
			set;
		}

		Circuit.CircuitConnection GetCircuitConnected(int thisCircuit);

		Circuit.CircuitConnection GetCircuitInverseConnected(int thisCircuit);

		string ImageKey {
			get;
		}

		string SelectedImageKey {
			get;
		}

		bool QuickDimensioningCanHeat {
			get;
		}

		bool QuickDimensioningCanCool {
			get;
		}

		int QuickDimensioningHeatPowerPerSquareMeter {
			get;
		}

		int QuickDimensioningCoolPowerPerSquareMeter {
			get;
		}

		int QuickDimensioningHeatPower {
			get;
		}
		
		int QuickDimensioningCoolPower {
			get;
		}

		float QuickDimensioningPlannedArea {
			get;
			set;
		}

		float QuickDimensioningMaximumArea {
			get;
		}

		int QuickDimensioningCircuits {
			get;
			set;
		}

		string QuickDimensioningCircuitsAsString {
			get;
			set;
		}

		bool UsedForQuickDimensioning {
			get;
			set;
		}

		Room AssociatedRoom {
			get;
			set;
		}

		SerializableDictionary<string, int> QuickDimensioningConnectedDistributors {
			get;
			set;
		}

		//protected void CheckPlannedCircuits();

		string Name {
			get;
		}

		string QuickDimensioningName {
			get;
		}

		string FullName {
			get;
		}

		Product.ProductType Type {
			get;
		}

		string Comment {
			get;
			set;
		}

		Product.CalculateModeEnum CalculateMode {
			get;
			set;
		}

		Product.CalculateModeEnum DefaultCalculateMode {
			get;
		}

		float AvailableFloorArea {
			get;
		}

		float AvailableCeilingArea {
			get;
		}

		double Dichte {
			get;
		}

		double Waermekapazitaet {
			get;
		}

		double Viskositaet {
			get;
		}

		float PlannedNetArea {
			get;
		}

		float PlannedFloorArea {
			get;
			set;
		}

		float PlannedCeilingArea {
			get;
			set;
		}

		float PlannedWallArea {
			get;
			set;
		}

		float PlannedRoofArea {
			get;
			set;
		}

		float TotalPlannedArea {
			get;
		}

		double PlannedHeatLoad {
			get;
		}

		double PlannedHeatLoadAnbindung {
			get;
		}

		double PlannedRemoveArea {
			get;
		}

		double PlannedHeatLoadIncludingConnectionsThrough {
			get;
		}

		double PlannedCoolLoad {
			get;
		}

		double PlannedCoolLoadAnbindung {
			get;
		}

		double PlannedCoolLoadIncludingConnectionsThrough {
			get;
		}

		List<ConnectionPipe> PlannedConnectionPipes {
			get;
			set;
		}

		List<ConnectionPipe> PlannedConnectionPipesThroughThisProduct {
			get;
		}

		int PlannedCircuitCount {
			get;
		}

		ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get;
		}

		double PlannedVorlaufTempHeat {
			get;
		}

		double PlannedRuecklaufTempHeat {
			get;
		}

		double PlannedVorlaufTempCool {
			get;
		}

		double PlannedruecklaufTempCool {
			get;
		}

		void CalculateHeatAndCoolFlow();

		void GetHeatFlow(out double vorlauf, out double ruecklauf);

		void GetCoolFlow(out double vorlauf, out double ruecklauf);

		ProductConnection PlannedConnection {
			get;
			set;
		}

		List<Circuit> PlannedCircuits {
			get;
			set;
		}

		List<Product> PlannedConnectedProducts {
			get;
		}

		bool PlannedCalculationComplete {
			get;
		}

		double PlannedSpreizungHeat {
			get;
		}

		double PlannedSpreizungCool {
			get;
		}

		double PlannedDeltaRhoHeat {
			get;
		}

		double PlannedDeltaRhoDistributorHeat {
			get;
		}

		double PlannedDeltaRhoCool {
			get;
		}

		double PlannedDeltaRhoDistributorCool {
			get;
		}

		double PlannedMhHeat {
			get;
		}

		double PlannedDurchflussHeat {
			get;
		}

		double PlannedMhCool {
			get;
		}
		double PlannedDurchflussCool {
			get;
		}

		double PlannedMaxMhHeat {
			get;
		}
		double PlannedMaxDurchflussHeat {
			get;
		}

		double PlannedMaxMhCool {
			get;
		}
		double PlannedMaxDurchflussCool {
			get;
		}

		bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, bool variableSpreizung);

		//internal void FinalizeLoading(PlannedProduct pp);

		//protected void CalculateVorlaufRuecklauf(out double[] vorlaufTotal, out double[] vorlaufNotIsolated, out double[] ruecklaufTotal, out double[] ruecklaufNotIsolated, out double[] vorlaufWithoutOtherProductTotal, out double[] vorlaufWithoutOtherProductNotIsolated, out double[] ruecklaufWithoutOtherProductTotal, out double[] ruecklaufWithoutOtherProductNotIsolated, out double longestVorlaufTotal, out double longestRuecklaufTotal);

		Circuit GetCircuit(int index);

		bool StellMotore {
			get;
			set;
		}

		float PlannedInsideConstructionRValue {
			get;
		}

		bool HasInsideConstruction {
			get;
		}

		Construction PlannedInsideConstruction {
			get;
		}

		float PlannedOutsideConstructionRValue {
			get;
		}

		bool HasOutsideConstruction {
			get;
		}

		double WasserInhalt {
			get;
		}

		Construction PlannedOutsideConstruction {
			get;
		}

		double TransmissionFloorHeat {
			get;
		}

		double TransmissionWallHeat {
			get;
		}

		double TransmissionCeilingHeat {
			get;
		}

		double TransmissionRoofHeat {
			get;
		}

		double TransmissionFloorCool {
			get;
		}

		double TransmissionWallCool {
			get;
		}

		double TransmissionCeilingCool {
			get;
		}

		double TransmissionRoofCool {
			get;
		}

		float PlannedRoomTemperatureBelowHeat {
			get;
			set;
		}

		float PlannedRoomTemperatureBelowCool {
			get;
			set;
		}

		void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial);

		string LastErrorMessage {
			get;
		}

		bool IsOtherProductConnected {
			get;
		}

		bool PlannedProductIsConnection {
			get;
			set;
		}

		double PlannedHeizlastBereinigung {
			get;
		}

		double PlannedKuehllastBereinigung {
			get;
		}

		string NotificationMessage {
			get;
		}

		bool ManualMode {
			get;
		}

		string[] ErrorMessageArray {
			get;
		}

		string[] NotificationMessageArray {
			get;
		}

		//protected void AddRequiredMaterialForConnections(SerializableDictionary<string, double> requiredMaterial, bool usePlus, double additional21mm);

		Nullable<bool> GraphicalMode {
			get;
			set;
		}

		bool AllowToSwitchMode {
			get;
		}

		PossibleProductConnection GetPossibleProductConnection(bool input, bool output, bool firstCircuit, bool otherCircuits, double measure, bool invertYAxis, Point2D currentMousePoint);

		List<GraphicalProductConnection> Connections {
			get;
			set;
		}

		Polygon2D GraphicalArea {
			get;
		}
		#endregion

		#region PipeProduct Definitions
		void ResetProduct();

		bool UseClipSchieneKlebeband {
			get;
			set;
		}

		bool UseAnhydritEstrich {
			get;
			set;
		}

		Nullable<LayDistanceType> RequestedLayDistance {
			get;
			set;
		}

		Nullable<RimType> RequestedRimType {
			get;
			set;
		}

		Nullable<int> RequestedCircuits {
			get;
			set;
		}

		float PlannedRimLength {
			get;
			set;
		}

		List<Segment2D> PlannedRimSegments {
			get;
			set;
		}

		List<Point2D> PlannedAreaGraphical {
			get;
			set;
		}

		List<List<Point2D>> PlannedReducedAreas {
			get;
		}

		int PlannedRimCorners {
			get;
			set;
		}

		string PlannedFloorConstructionId {
			get;
			set;
		}

		string PlannedInsulationConstructionId {
			get;
			set;
		}

		Construction PlannedFloorConstruction {
			get;
			set;
		}

		Construction PlannedInsulationConstruction {
			get;
			set;
		}

		float PlannedAreaReduced {
			get;
			set;
		}

		float PlannedAreaUnheated {
			get;
			set;
		}

		float PlannedAreaRim {
			get;
		}

		float PlannedAreaResidence {
			get;
		}

		float PlannedFloorAreaPercentage {
			get;
			set;
		}

		Nullable<LayDistanceType> PlannedLayDistance {
			get;
			set;
		}

		Nullable<RimType> PlannedRimType {
			get;
			set;
		}

		Nullable<LayDistanceType> PlannedRimLayDistance {
			get;
		}

		int PlannedRimWidth {
			get;
		}

		double PlannedHeatLoadPerSqM {
			get;
		}

		double PlannedHeatLoadPerSqMRim {
			get;
		}

		double PlannedHeatLoadPerSqMResidence {
			get;
		}

		double PlannedHeatLoadRim {
			get;
		}

		double PlannedHeatLoadResidence {
			get;
		}

		double PlannedFloorTemperatureHeatRim {
			get;
		}

		double PlannedFloorTemperatureHeatResidence {
			get;
		}

		double PlannedCoolLoadPerSqM {
			get;
		}

		double PlannedCoolLoadPerSqMRim {
			get;
		}

		double PlannedCoolLoadPerSqMResidence {
			get;
		}

		double PlannedCoolLoadRim {
			get;
		}

		double PlannedCoolLoadResidence {
			get;
		}

		double PlannedFloorTemperatureCoolRim {
			get;
		}

		double PlannedFloorTemperatureCoolResidence {
			get;
		}

		double PlannedPipeLengthPerCircuit {
			get;
		}

		double LongestPipeLengthPerCircuitWithAllConnections {
			get;
		}

		double PipeLengthWithoutConnectionsOfLongestPipeWithConnections {
			get;
		}

		double ConnectionLengthOfLongestPipeWithConnections {
			get;
		}

		bool PlannedCorrections {
			get;
			set;
		}

		List<ExtendedCorrections> PlannedCorrectionList {
			get;
			set;
		}

		Point2D TextBoxPosition {
			get;
			set;
		}


		float TextBoxFontSize {
			get;
			set;
		}
	#endregion
	}
}
