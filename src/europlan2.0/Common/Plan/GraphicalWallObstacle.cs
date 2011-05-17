using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	[XmlInclude(typeof(GraphicalDoor))]
	[XmlInclude(typeof(GraphicalWindow))]
	[XmlInclude(typeof(GraphicalOtherObstacle))]
	public abstract class GraphicalWallObstacle : IGraphicalWallObject {

		public class ObstacleTypeConverter : System.ComponentModel.TypeConverter {
			private static readonly string door = EuroplanRes.GraphicalWallObstacle_Door; //"Tür";
			private static readonly string window = EuroplanRes.GraphicalWallObstacle_Window; //"Fenster";
			private static readonly string windowTriangleLeft = EuroplanRes.GraphicalWallObstacle_WindowTriangleLeft; //"Dreiecksfenster Links";
			private static readonly string windowTriangleRight = EuroplanRes.GraphicalWallObstacle_WindowTriangleRight; //"Dreiecksfenster Rechts";
			private static readonly string other = EuroplanRes.GraphicalWallObstacle_Other; //"Anderes";

			private Dictionary<string, ObstacleTypeEnum> mappingFromString = new Dictionary<string, ObstacleTypeEnum>();
			private Dictionary<ObstacleTypeEnum, string> mappingToString = new Dictionary<ObstacleTypeEnum, string>();

			public ObstacleTypeConverter() {
				mappingFromString.Add(door, ObstacleTypeEnum.Door);
				mappingFromString.Add(window, ObstacleTypeEnum.Window);
				mappingFromString.Add(windowTriangleLeft, ObstacleTypeEnum.WindowTriangleLeft);
				mappingFromString.Add(windowTriangleRight, ObstacleTypeEnum.WindowTriangleRight);
				mappingFromString.Add(other, ObstacleTypeEnum.Other);

				mappingToString.Add(ObstacleTypeEnum.Door, door);
				mappingToString.Add(ObstacleTypeEnum.Window, window);
				mappingToString.Add(ObstacleTypeEnum.WindowTriangleLeft, windowTriangleLeft);
				mappingToString.Add(ObstacleTypeEnum.WindowTriangleRight, windowTriangleRight);
				mappingToString.Add(ObstacleTypeEnum.Other, other);
			}

			public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType) {
				return sourceType == typeof(string);
			}

			public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType) {
				return destinationType == typeof(string);
			}

			public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
				if (value is string) {
					if (mappingFromString.ContainsKey((string)value)) {
						return mappingFromString[(string)value];
					}
				}
				return base.ConvertFrom(context, culture, value);
			}

			public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
				if (value is ObstacleTypeEnum && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((ObstacleTypeEnum)value)) {
						return mappingToString[(ObstacleTypeEnum)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(ObstacleTypeConverter))]
		public enum ObstacleTypeEnum {
			Door,
			Window,
			WindowTriangleLeft,
			WindowTriangleRight,
			Other
		}

		public abstract bool HitTest(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract void PaintObject(System.Drawing.Graphics g, double xOffset, double yOffset, IGraphicalWallObject selectedObject, double scale);
		public abstract IGraphicalWallObject GetPickedObject(WW.Math.Point2D planPoint, double xOffset, double yOffset);
		public abstract WW.Math.Geometry.Polygon2D GetObjectBorders(double xOffset, double yOffset);
		public abstract bool CollisionTest(WW.Math.Geometry.Polygon2D polygon, double xOffset, double yOffset, bool ignoreBorders);
		public abstract bool StartDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall);
		public abstract bool MoveDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall);
		public abstract bool EndDrag(Anchor anchor, WW.Math.Point2D planPoint, GraphicalWall owningWall);
		public abstract List<Anchor> GetAnchors(double scale);
		public abstract bool IsMoveable {
			get;
		}
		public abstract double BorderDistance {
			get;
			set;
		}
		public abstract ObstacleTypeEnum ObstacleType {
			get;
			set;
		}
		public abstract double GraphPosX {
			get;
			set;
		}
		public abstract double GraphPosY {
			get;
			set;
		}
		public abstract double Width {
			get;
			set;
		}
		public abstract double Height {
			get;
			set;
		}

	}

}
