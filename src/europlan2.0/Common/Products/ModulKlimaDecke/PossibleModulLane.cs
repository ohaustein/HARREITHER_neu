using System;
using System.Collections.Generic;
using System.Text;
using WW.Math.Geometry;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class PossibleModulLane {
		private List<PossibleModulLaneArea> areas = new List<PossibleModulLaneArea>();
		private Line2D borderLeft;
		private Line2D borderRight;
		private int nr = -1;

		public PossibleModulLane(Line2D borderLeft, Line2D borderRight, int nr) {
			this.borderLeft = borderLeft;
			this.borderRight = borderRight;
			this.nr = nr;
		}

		public PossibleModulLane(List<PossibleModulLaneArea> areas, int nr) {
			this.areas = areas;
			if (this.areas.Count > 0) {
				this.borderLeft = new Line2D(this.areas[0].TopLeft, this.areas[0].TopLeft - this.areas[0].BottomLeft);
				this.borderRight = new Line2D(this.areas[0].TopRight, this.areas[0].TopRight - this.areas[0].BottomRight);
			}
			this.nr = nr;
		}

		public List<PossibleModulLaneArea> Areas {
			get { return this.areas; }
			set { this.areas = value; }
		}

		public List<FreeModulLaneArea> GetFreeAreas(ModulKlimaDeckeProduct product, List<KlimaFlaechenModul> excludeModules) {
			List<FreeModulLaneArea> freeAreas = new List<FreeModulLaneArea>();
			if (product.AssociatedRoom == null || product.AssociatedRoom.AssociatedPlan == null ||
				!product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
				return freeAreas;
			}

			double measure = product.AssociatedRoom.AssociatedPlan.Measure.Value;
			List<KlimaFlaechenModul> modules = this.GetModulesInThisLane(product);
			foreach (PossibleModulLaneArea area in this.areas) {
				freeAreas.Add(new FreeModulLaneArea(area.Top, area.Bottom));
			}
			foreach (KlimaFlaechenModul modul in modules) {
				if (excludeModules == null || !excludeModules.Contains(modul)) {
					foreach (FreeModulLaneArea area in freeAreas) {
						if (modul.GraphPositionInLan >= area.Top && modul.GraphPositionInLan <= area.Bottom) {
							double modulBottom = modul.GraphBottomPositionInLane(measure);
							if (modulBottom >= area.Top && modulBottom <= area.Bottom) {
								int index = freeAreas.IndexOf(area);
								freeAreas.Remove(area);
								double modulTop = modul.GraphPositionInLan;
								//if (area.Bottom != modulBottom) {
								if (area.Bottom > modulBottom) {
									freeAreas.Insert(index, new FreeModulLaneArea(modulBottom, area.Bottom));
								}
								//if (area.Top != modulTop) {
								if (area.Top < modulTop) {
									freeAreas.Insert(index, new FreeModulLaneArea(area.Top, modulTop));
								}
								break;
							}
						}
					}
				}
			}
			return freeAreas;
		}

		[XmlIgnore]
		public Line2D BorderLeft {
			get { return this.borderLeft; }
		}

		[XmlIgnore]
		public Line2D BorderRight {
			get { return this.borderRight; }
		}

		public List<KlimaFlaechenModul> GetModulesInThisLane(ModulKlimaDeckeProduct product) {
			return product.GetModulesInLane(this.nr);
		}

		[XmlIgnore]
		public int Nr {
			get { return this.nr; }
		}

		public bool ModuleChangesPossible(KlimaFlaechenModul modul, KlimaFlaechenModul.ModulTypeEnum modulType, double position, double measure, ModulKlimaDeckeProduct product) {
			List<KlimaFlaechenModul> excludeModules = new List<KlimaFlaechenModul>();
			excludeModules.Add(modul);
			double top = modul.GraphPositionInLan;
			double bottom = modul.GraphPositionInLan + KlimaFlaechenModul.GetModuleHeight(modulType) * measure;
			foreach (FreeModulLaneArea area in this.GetFreeAreas(product, excludeModules)) {
				if (area.Fits(top, bottom)) {
					return true;
				}
			}
			return false;
		}

		public Nullable<double> BestMovePossible(KlimaFlaechenModul modul, double desiredPosition, double measure, ModulKlimaDeckeProduct product, bool bottomUp) {
			List<KlimaFlaechenModul> excludeModules = new List<KlimaFlaechenModul>();
			excludeModules.Add(modul);
			double top = desiredPosition;
			double bottom = desiredPosition + KlimaFlaechenModul.GetModuleHeight(modul.ModulType) * measure;
			foreach (FreeModulLaneArea area in this.GetFreeAreas(product, excludeModules)) {
				Nullable<double> bestMove = area.BestStart(top, bottom, bottomUp);
				if (bestMove.HasValue) {
					return bestMove;
				}
			}
			return null;
		}
	}
}
