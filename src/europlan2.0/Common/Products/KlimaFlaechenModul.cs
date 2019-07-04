using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using WW.Math;
using WW.Math.Geometry;

namespace Europlan.Common
{
    public class KlimaFlaechenModul
    {
        public static readonly double CONNECTION_DISTANCE = 0.035;  // Abstand der Anschlüsse zum Rand 2.45cm + hälte der breite (2.1cm / 2)

        private static List<ModulTypeEnum> modulesGraphicSizeEnabled = new List<ModulTypeEnum>();

        private static double module_100_40_height = 1.0;
        private static double module_100_30_height = 1.0;
        private static double module_120_30_height = 1.2;
        private static double module_160_30_height = 1.6;
        private static double module_80_30_height = 0.8;
        private static double module_60_60_height = 0.6;
        private static double module_100_40_20_height = 1.0;

        private static double module_100_40_20_height_graphical_project = module_100_40_20_height;
        private static double module_100_40_height_graphical_project = module_100_40_height;

        private static double module_100_40_width = 0.4;
        private static double module_100_30_width = 0.3;
        private static double module_120_30_width = 0.3;
        private static double module_160_30_width = 0.3;
        private static double module_80_30_width = 0.3;
        private static double module_60_60_width = 0.6;
        private static double module_100_40_20_width = 0.4;

        private static double module_100_40_20_width_graphical_project = module_100_40_20_width;
        private static double module_100_40_width_graphical_project = module_100_40_width;

        private static double module_additional_width = 0.03 * 2;

        private static double module_100_40_floor_heatarea = module_100_40_height * module_100_40_width;
        private static double module_100_40_roof_heatarea = module_100_40_height * (module_100_40_width + module_additional_width);
        private static double module_100_30_heatarea = module_100_30_height * (module_100_30_width + module_additional_width);
        private static double module_120_30_heatarea = module_120_30_height * (module_120_30_width + module_additional_width);
        private static double module_160_30_heatarea = module_160_30_height * (module_160_30_width + module_additional_width);
        private static double module_80_30_heatarea = module_80_30_height * (module_80_30_width + module_additional_width);
        private static double module_60_60_heatarea = module_60_60_height * module_60_60_width;
        private static double module_100_40_20_heatarea = module_100_40_20_height * module_100_40_20_width; // TO BE DEFINED

        private static double module_100_40_floor_area = module_100_40_floor_heatarea;
        private static double module_100_40_roof_area = module_100_40_roof_heatarea;
        private static double module_100_30_area = module_100_30_heatarea;
        private static double module_120_30_area = module_120_30_heatarea;
        private static double module_160_30_area = module_160_30_heatarea;
        private static double module_80_30_area = module_80_30_heatarea;
        private static double module_60_60_area = module_60_60_heatarea;
        private static double module_100_40_20_area = module_100_40_20_heatarea; // TO BE DEFINED

        internal enum PossibleConnectionPointType
        {
            CONNECTION_PRODUCT,
            CONNECTION_MODULE,
            CONNECTION_SUBAREA
        }

        internal struct PossibleConnectionPoint
        {
            private PossibleConnectionPointType connectionType;
            private GraphicalConnectionAnbindungsPunkt productConnection;
            private Nullable<Point2D> connectionPoint;
            private Polygon2D connectionArea;
            private bool vorlauf;
            private bool ruecklauf;

            private KlimaFlaechenModul modul;
            private KlimaFlaechenSubAreaVerbindung verbindung;

            public PossibleConnectionPoint(GraphicalConnectionAnbindungsPunkt productConnection, bool vorlauf, bool ruecklauf)
            {
                this.connectionType = PossibleConnectionPointType.CONNECTION_PRODUCT;
                this.productConnection = productConnection;
                this.connectionPoint = null;
                this.connectionArea = null;
                this.modul = null;
                this.verbindung = null;
                this.vorlauf = vorlauf && !ruecklauf;
                this.ruecklauf = ruecklauf && !vorlauf;
            }

            public PossibleConnectionPoint(Point2D connectionPoint, Polygon2D connectionArea, KlimaFlaechenModul modul, bool vorlauf, bool ruecklauf)
            {
                this.connectionType = PossibleConnectionPointType.CONNECTION_MODULE;
                this.connectionPoint = connectionPoint;
                this.connectionArea = connectionArea;
                this.modul = modul;
                this.verbindung = null;
                this.productConnection = null;
                this.vorlauf = vorlauf && !ruecklauf;
                this.ruecklauf = ruecklauf && !vorlauf;
            }

            public PossibleConnectionPoint(Point2D connectionPoint, KlimaFlaechenSubAreaVerbindung verbindung, double measure, bool vorlauf, bool ruecklauf)
            {
                this.connectionType = PossibleConnectionPointType.CONNECTION_SUBAREA;
                this.connectionPoint = connectionPoint;
                double connSize = 0.05 * measure;
                this.connectionArea = new Polygon2D(new Point2D[] { connectionPoint + new Vector2D(connSize, connSize), connectionPoint + new Vector2D(connSize, -connSize), connectionPoint + new Vector2D(-connSize, -connSize), connectionPoint + new Vector2D(-connSize, connSize) });
                this.verbindung = verbindung;
                this.modul = null;
                this.productConnection = null;
                this.vorlauf = vorlauf && !ruecklauf;
                this.ruecklauf = ruecklauf && !vorlauf;
            }

            public GraphicalConnectionAnbindungsPunkt ProductConnection
            {
                get { return this.productConnection; }
            }

            public PossibleConnectionPointType ConnectionType
            {
                get { return this.connectionType; }
            }

            public Point2D ConnectionPoint
            {
                get
                {
                    switch (this.connectionType)
                    {
                        case PossibleConnectionPointType.CONNECTION_PRODUCT:
                            return this.productConnection.Point;

                        case PossibleConnectionPointType.CONNECTION_SUBAREA:
                        case PossibleConnectionPointType.CONNECTION_MODULE:
                            return this.connectionPoint.Value;

                        default:
                            throw new Exception();
                    }
                }
            }

            public Polygon2D ConnectionArea
            {
                get
                {
                    switch (this.connectionType)
                    {
                        case PossibleConnectionPointType.CONNECTION_PRODUCT:
                            return this.productConnection.Area;

                        case PossibleConnectionPointType.CONNECTION_SUBAREA:
                        case PossibleConnectionPointType.CONNECTION_MODULE:
                            return this.connectionArea;

                        default:
                            throw new Exception();
                    }
                }
            }

            public bool Vorlauf
            {
                get { return this.vorlauf; }
            }

            public bool Ruecklauf
            {
                get { return this.ruecklauf; }
            }

            public KlimaFlaechenModul Modul
            {
                get { return this.modul; }
            }

            public KlimaFlaechenSubAreaVerbindung Verbindung
            {
                get { return this.verbindung; }
            }
        }

        public class ModulTypeEnumConverter : System.ComponentModel.TypeConverter
        {
            private static readonly string modul_100_40 = EuroplanRes.KlimaFlaechenModul_100_40; //"Modul 100/40"
            private static readonly string modul_100_30 = EuroplanRes.KlimaFlaechenModul_100_30; //"Modul 100/30"
            private static readonly string modul_120_30 = EuroplanRes.KlimaFlaechenModul_120_30; //"Modul 120/30"
            private static readonly string modul_80_30 = EuroplanRes.KlimaFlaechenModul_80_30; //"Modul 80/30"
            private static readonly string modul_60_60 = EuroplanRes.KlimaFlaechenModul_60_60; //"Modul 60/60 Typ A"
            private static readonly string modul_60_60B = EuroplanRes.KlimaFlaechenModul_60_60B; //"Modul 60/60 Typ B"
            private static readonly string modul_60_60C = EuroplanRes.KlimaFlaechenModul_60_60C; //"Modul 60/60 Typ C"
            private static readonly string modul_60_60D = EuroplanRes.KlimaFlaechenModul_60_60D; //"Modul 60/60 Typ D"
            private static readonly string modul_100_40_20 = EuroplanRes.KlimaFlaechenModul_100_40_20; //"Modul 100/40 20"
            private static readonly string modul_160_30U = EuroplanRes.KlimaFlaechenModul_160_30U; // "Module 160/30 Universal"
            private static readonly string modul_120_30U = EuroplanRes.KlimaFlaechenModul_120_30U; //"Module 120/30 Universal"
            private static readonly string modul_100_30U = EuroplanRes.KlimaFlaechenModul_100_30U; //"Module 100/30 Universal"
            private static readonly string modul_80_30U = EuroplanRes.KlimaFlaechenModul_80_30U; //"Module 80/30 Universal"

            private static readonly string modul_100_40_short = EuroplanRes.KlimaFlaechenModul_100_40_Short; //"100/40"
            private static readonly string modul_100_30_short = EuroplanRes.KlimaFlaechenModul_100_30_Short; //"100/30"
            private static readonly string modul_120_30_short = EuroplanRes.KlimaFlaechenModul_120_30_Short; //"120/30"
            private static readonly string modul_80_30_short = EuroplanRes.KlimaFlaechenModul_80_30_Short; //"80/30"
            private static readonly string modul_60_60_short = EuroplanRes.KlimaFlaechenModul_60_60_Short; //"60/60 A"
            private static readonly string modul_60_60B_short = EuroplanRes.KlimaFlaechenModul_60_60B_Short; //"60/60 B"
            private static readonly string modul_60_60C_short = EuroplanRes.KlimaFlaechenModul_60_60C_Short; //"60/60 C"
            private static readonly string modul_60_60D_short = EuroplanRes.KlimaFlaechenModul_60_60D_Short; //"60/60 D"
            private static readonly string modul_100_40_20_short = EuroplanRes.KlimaFlaechenModul_100_40_20_Short; //"100/40 20"
            private static readonly string modul_160_30U_short = EuroplanRes.KlimaFlaechenModul_160_30U_Short; //"160/30 U"
            private static readonly string modul_120_30U_short = EuroplanRes.KlimaFlaechenModul_120_30U_Short; //"120/30 U"
            private static readonly string modul_100_30U_short = EuroplanRes.KlimaFlaechenModul_100_30U_Short; //"100/30 U"
            private static readonly string modul_80_30U_short = EuroplanRes.KlimaFlaechenModul_80_30U_Short; //"80/30 U"

            private Dictionary<string, ModulTypeEnum> mappingFromString = new Dictionary<string, ModulTypeEnum>();
            private Dictionary<ModulTypeEnum, string> mappingToString = new Dictionary<ModulTypeEnum, string>();
            private Dictionary<string, ModulTypeEnum> mappingFromShortString = new Dictionary<string, ModulTypeEnum>();
            private Dictionary<ModulTypeEnum, string> mappingToShortString = new Dictionary<ModulTypeEnum, string>();

            private bool shortNames = false;

            public ModulTypeEnumConverter()
            {
                this.Initialize();
            }

            public ModulTypeEnumConverter(bool shortNames)
            {
                this.shortNames = shortNames;
                this.Initialize();
            }

            private void Initialize()
            {
                mappingFromString.Add(modul_100_40, ModulTypeEnum.MODUL_100_40);
                mappingFromString.Add(modul_100_30, ModulTypeEnum.MODUL_100_30);
                mappingFromString.Add(modul_120_30, ModulTypeEnum.MODUL_120_30);
                mappingFromString.Add(modul_80_30, ModulTypeEnum.MODUL_80_30);
                mappingFromString.Add(modul_60_60, ModulTypeEnum.MODUL_60_60);
                mappingFromString.Add(modul_60_60B, ModulTypeEnum.MODUL_60_60B);
                mappingFromString.Add(modul_60_60C, ModulTypeEnum.MODUL_60_60C);
                mappingFromString.Add(modul_60_60D, ModulTypeEnum.MODUL_60_60D);
                mappingFromString.Add(modul_100_40_20, ModulTypeEnum.MODUL_100_40_20);
                mappingFromString.Add(modul_160_30U, ModulTypeEnum.MODUL_160_30U);
                mappingFromString.Add(modul_120_30U, ModulTypeEnum.MODUL_120_30U);
                mappingFromString.Add(modul_100_30U, ModulTypeEnum.MODUL_100_30U);
                mappingFromString.Add(modul_80_30U, ModulTypeEnum.MODUL_80_30U);
                mappingToString.Add(ModulTypeEnum.MODUL_100_40, modul_100_40);
                mappingToString.Add(ModulTypeEnum.MODUL_100_30, modul_100_30);
                mappingToString.Add(ModulTypeEnum.MODUL_120_30, modul_120_30);
                mappingToString.Add(ModulTypeEnum.MODUL_80_30, modul_80_30);
                mappingToString.Add(ModulTypeEnum.MODUL_60_60, modul_60_60);
                mappingToString.Add(ModulTypeEnum.MODUL_60_60B, modul_60_60B);
                mappingToString.Add(ModulTypeEnum.MODUL_60_60C, modul_60_60C);
                mappingToString.Add(ModulTypeEnum.MODUL_60_60D, modul_60_60D);
                mappingToString.Add(ModulTypeEnum.MODUL_100_40_20, modul_100_40_20);
                mappingToString.Add(ModulTypeEnum.MODUL_160_30U, modul_160_30U);
                mappingToString.Add(ModulTypeEnum.MODUL_120_30U, modul_120_30U);
                mappingToString.Add(ModulTypeEnum.MODUL_100_30U, modul_100_30U);
                mappingToString.Add(ModulTypeEnum.MODUL_80_30U, modul_80_30U);
                mappingFromShortString.Add(modul_100_40_short, ModulTypeEnum.MODUL_100_40);
                mappingFromShortString.Add(modul_100_30_short, ModulTypeEnum.MODUL_100_30);
                mappingFromShortString.Add(modul_120_30_short, ModulTypeEnum.MODUL_120_30);
                mappingFromShortString.Add(modul_80_30_short, ModulTypeEnum.MODUL_80_30);
                mappingFromShortString.Add(modul_60_60_short, ModulTypeEnum.MODUL_60_60);
                mappingFromShortString.Add(modul_60_60B_short, ModulTypeEnum.MODUL_60_60B);
                mappingFromShortString.Add(modul_60_60C_short, ModulTypeEnum.MODUL_60_60C);
                mappingFromShortString.Add(modul_60_60D_short, ModulTypeEnum.MODUL_60_60D);
                mappingFromShortString.Add(modul_100_40_20_short, ModulTypeEnum.MODUL_100_40_20);
                mappingFromShortString.Add(modul_160_30U_short, ModulTypeEnum.MODUL_160_30U);
                mappingFromShortString.Add(modul_120_30U_short, ModulTypeEnum.MODUL_120_30U);
                mappingFromShortString.Add(modul_100_30U_short, ModulTypeEnum.MODUL_100_30U);
                mappingFromShortString.Add(modul_80_30U_short, ModulTypeEnum.MODUL_80_30U);
                mappingToShortString.Add(ModulTypeEnum.MODUL_100_40, modul_100_40_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_100_30, modul_100_30_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_120_30, modul_120_30_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_80_30, modul_80_30_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_60_60, modul_60_60_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_60_60B, modul_60_60B_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_60_60C, modul_60_60C_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_60_60D, modul_60_60D_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_100_40_20, modul_100_40_20_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_160_30U, modul_160_30U_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_120_30U, modul_120_30U_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_100_30U, modul_100_30U_short);
                mappingToShortString.Add(ModulTypeEnum.MODUL_80_30U, modul_80_30U_short);
            }

            public bool ShortNames
            {
                get { return this.shortNames; }
                set { this.shortNames = value; }
            }

            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(string);
            }

            public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value is string)
                {
                    if (mappingFromString.ContainsKey((string)value))
                    {
                        return mappingFromString[(string)value];
                    }
                    if (mappingFromShortString.ContainsKey((string)value))
                    {
                        return mappingFromShortString[(string)value];
                    }
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (value is ModulTypeEnum && destinationType == typeof(string))
                {
                    if (this.shortNames)
                    {
                        if (mappingToShortString.ContainsKey((ModulTypeEnum)value))
                        {
                            return mappingToShortString[(ModulTypeEnum)value];
                        }
                    }
                    else
                    {
                        if (mappingToString.ContainsKey((ModulTypeEnum)value))
                        {
                            return mappingToString[(ModulTypeEnum)value];
                        }
                    }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        [System.ComponentModel.TypeConverter(typeof(ModulTypeEnumConverter))]
        public enum ModulTypeEnum
        {
            MODUL_100_40,
            MODUL_100_30,
            MODUL_120_30,
            MODUL_80_30,
            MODUL_60_60,
            MODUL_60_60B,
            MODUL_60_60C,
            MODUL_60_60D,
            MODUL_100_40_20,
            MODUL_120_30U,
            MODUL_100_30U,
            MODUL_80_30U,
            MODUL_160_30U,
        }

        public class ModulOrientationEnumConverter : System.ComponentModel.TypeConverter
        {
            private static readonly string left = EuroplanRes.KlimaFlaechenModul_AusrichtungLinks; //"Links"
            private static readonly string right = EuroplanRes.KlimaFlaechenModul_AusrichtungRechts; //"Rechts"

            private Dictionary<string, ModulOrientationEnum> mappingFromString = new Dictionary<string, ModulOrientationEnum>();
            private Dictionary<ModulOrientationEnum, string> mappingToString = new Dictionary<ModulOrientationEnum, string>();

            public ModulOrientationEnumConverter()
            {
                mappingFromString.Add(left, ModulOrientationEnum.ORIENTATION_LEFT);
                mappingFromString.Add(right, ModulOrientationEnum.ORIENTATION_RIGHT);
                mappingToString.Add(ModulOrientationEnum.ORIENTATION_LEFT, left);
                mappingToString.Add(ModulOrientationEnum.ORIENTATION_RIGHT, right);
            }

            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(string);
            }

            public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value is string)
                {
                    if (mappingFromString.ContainsKey((string)value))
                    {
                        return mappingFromString[(string)value];
                    }
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (value is ModulOrientationEnum && destinationType == typeof(string))
                {
                    if (mappingToString.ContainsKey((ModulOrientationEnum)value))
                    {
                        return mappingToString[(ModulOrientationEnum)value];
                    }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public class ModulModulationEnumConverter : System.ComponentModel.TypeConverter
        {
            private static readonly string none = EuroplanRes.KlimaFlaechenModulModulation_None; //"Dicht"
            private static readonly string single = EuroplanRes.KlimaFlaechenModulModulation_Single; //"Modulierend"

            private Dictionary<string, ModulModulationEnum> mappingFromString = new Dictionary<string, ModulModulationEnum>();
            private Dictionary<ModulModulationEnum, string> mappingToString = new Dictionary<ModulModulationEnum, string>();

            public ModulModulationEnumConverter()
            {
                mappingFromString.Add(none, ModulModulationEnum.MODULATION_NONE);
                mappingFromString.Add(single, ModulModulationEnum.MODULATION_SINGLE_MODULATED);
                mappingToString.Add(ModulModulationEnum.MODULATION_NONE, none);
                mappingToString.Add(ModulModulationEnum.MODULATION_SINGLE_MODULATED, single);
            }

            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(string);
            }

            public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value is string)
                {
                    if (mappingFromString.ContainsKey((string)value))
                    {
                        return mappingFromString[(string)value];
                    }
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (value is ModulModulationEnum && destinationType == typeof(string))
                {
                    if (mappingToString.ContainsKey((ModulModulationEnum)value))
                    {
                        return mappingToString[(ModulModulationEnum)value];
                    }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        [System.ComponentModel.TypeConverter(typeof(ModulOrientationEnumConverter))]
        public enum ModulOrientationEnum
        {
            ORIENTATION_LEFT,
            ORIENTATION_RIGHT
        }

        [System.ComponentModel.TypeConverter(typeof(ModulModulationEnumConverter))]
        public enum ModulModulationEnum
        {
            MODULATION_NONE = 0,
            MODULATION_SINGLE_MODULATED = 10,
        }

        private ModulTypeEnum modulType;
        private ModulOrientationEnum orientation;

        // position in graphical mode (for klimadecke)
        private int graphLane = -1;
        private double graphPositionInLane = double.NaN;
        private bool graphBottomUp = false;

        // position in graphical mode (for klimaboden)
        private double graphPosX = double.NaN;
        private double graphPosY = double.NaN;
        private double graphRotation = 0;

        public KlimaFlaechenModul()
        {
            this.modulType = ModulTypeEnum.MODUL_100_40;
            this.orientation = ModulOrientationEnum.ORIENTATION_LEFT;
        }

        public KlimaFlaechenModul(ModulTypeEnum modulType, ModulOrientationEnum orientation)
        {
            this.modulType = modulType;
            this.orientation = orientation;
        }

        public KlimaFlaechenModul(KlimaFlaechenModul other)
        {
            this.modulType = other.modulType;
            this.orientation = other.orientation;
            this._modulModulationLength = other._modulModulationLength;
            this._modulModulationWidth = other._modulModulationWidth;
        }

        public void ClearGraphicalRepresentation()
        {
            graphLane = -1;
            graphPositionInLane = double.NaN;
            graphBottomUp = false;

            graphPosX = double.NaN;
            graphPosY = double.NaN;
            graphRotation = 0;
        }

        public ModulTypeEnum ModulType
        {
            get { return this.modulType; }
            set { this.modulType = value; }
        }

        private ModulModulationEnum _modulModulationLength;
        private ModulModulationEnum _modulModulationWidth;

        public ModulModulationEnum ModulationWidth
        {
            get { return _modulModulationWidth; }
            set { _modulModulationWidth = value; }
        }

        public ModulModulationEnum ModulationLength
        {
            get { return _modulModulationLength; }
            set { _modulModulationLength = value; }
        }

        private ModulModulationEnum _modulGraphModulationLength;
        private ModulModulationEnum _modulGraphModulationWidth;

        public ModulModulationEnum GraphModulationWidth
        {
            get { return _modulGraphModulationWidth; }
            set { _modulGraphModulationWidth = value; }
        }

        public ModulModulationEnum GraphModulationLength
        {
            get { return _modulGraphModulationLength; }
            set { _modulGraphModulationLength = value; }
        }

        [XmlIgnore]
        public double ModulationWidthValue
        {
            get { return ((double)ModulationWidth) / 100; }
        }

        [XmlIgnore]
        public double ModulationLengthValue
        {
            get { return ((double)ModulationLength) / 100; }
        }

        public static bool IsDiagonalDurchstroemt(ModulTypeEnum modulType)
        {
            switch (modulType)
            {
                case ModulTypeEnum.MODUL_60_60:
                case ModulTypeEnum.MODUL_60_60B:
                case ModulTypeEnum.MODUL_80_30U:
                case ModulTypeEnum.MODUL_100_30U:
                case ModulTypeEnum.MODUL_120_30U:
                case ModulTypeEnum.MODUL_160_30U:
                    return false;
                default:
                    return true;
            }
        }

        [XmlIgnore]
        public bool DiagonalDurchstroemt
        {
            get { return IsDiagonalDurchstroemt(this.modulType); }
        }

        public ModulOrientationEnum Orientation
        {
            get { return this.orientation; }
            set { this.orientation = value; }
        }

        public string PartNumber
        {
            get
            {
                switch (this.modulType)
                {
                    case ModulTypeEnum.MODUL_100_40:
                        if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT)
                        {
                            return "MK01";
                        }
                        else
                        {
                            return "MK02";
                        }

                    case ModulTypeEnum.MODUL_80_30:
                        if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT)
                        {
                            return "MK34";
                        }
                        else
                        {
                            return "MK35";
                        }

                    case ModulTypeEnum.MODUL_60_60:
                        if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT)
                        {
                            return "MK40";
                        }
                        else
                        {
                            return "MK40";
                        }

                    case ModulTypeEnum.MODUL_60_60B:
                        if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT)
                        {
                            return "MK41";
                        }
                        else
                        {
                            return "MK41";
                        }

                    case ModulTypeEnum.MODUL_60_60C:
                        if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT)
                        {
                            return "MK42";
                        }
                        else
                        {
                            return "MK42";
                        }

                    case ModulTypeEnum.MODUL_60_60D:
                        if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT)
                        {
                            return "MK43";
                        }
                        else
                        {
                            return "MK43";
                        }

                    case ModulTypeEnum.MODUL_100_30:
                        if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT)
                        {
                            return "MK32";
                        }
                        else
                        {
                            return "MK33";
                        }

                    case ModulTypeEnum.MODUL_120_30:
                        if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT)
                        {
                            return "MK30";
                        }
                        else
                        {
                            return "MK31";
                        }

                    case ModulTypeEnum.MODUL_100_40_20:
                        if (orientation == ModulOrientationEnum.ORIENTATION_RIGHT)
                        {
                            return "MK70";
                        }
                        else
                        {
                            return "MK71";
                        }

                    case ModulTypeEnum.MODUL_80_30U:
                        return "MKU03";

                    case ModulTypeEnum.MODUL_100_30U:
                        return "MKU02";

                    case ModulTypeEnum.MODUL_120_30U:
                        return "MKU01";

                    case ModulTypeEnum.MODUL_160_30U:
                        return "MKU00";

                    default:
                        return "";
                }
            }
        }

        [XmlIgnore]
        public double WasserInhalt
        {
            get
            {
                switch (this.modulType)
                {
                    case ModulTypeEnum.MODUL_100_40:
                        return 1.0;

                    case ModulTypeEnum.MODUL_80_30:
                    case ModulTypeEnum.MODUL_80_30U:
                        return 0.6;

                    case ModulTypeEnum.MODUL_120_30:
                    case ModulTypeEnum.MODUL_120_30U:
                    case ModulTypeEnum.MODUL_60_60:
                    case ModulTypeEnum.MODUL_60_60B:
                    case ModulTypeEnum.MODUL_60_60C:
                    case ModulTypeEnum.MODUL_60_60D:
                        return 0.9;

                    case ModulTypeEnum.MODUL_100_30:
                    case ModulTypeEnum.MODUL_100_30U:
                        return 0.75;

                    case ModulTypeEnum.MODUL_160_30U:
                        return 1.2;

                    case ModulTypeEnum.MODUL_100_40_20:
                        return 1.0;

                    default:
                        throw new Exception("Unknown Register Type");
                }
            }
        }

        public double GetHeatArea(bool floor)
        {
            switch (this.modulType)
            {
                case ModulTypeEnum.MODUL_100_40:
                    return floor ? KlimaFlaechenModul.module_100_40_floor_heatarea : KlimaFlaechenModul.module_100_40_roof_heatarea;

                case ModulTypeEnum.MODUL_80_30:
                case ModulTypeEnum.MODUL_80_30U:
                    return KlimaFlaechenModul.module_80_30_heatarea;

                case ModulTypeEnum.MODUL_60_60:
                case ModulTypeEnum.MODUL_60_60B:
                case ModulTypeEnum.MODUL_60_60C:
                case ModulTypeEnum.MODUL_60_60D:
                    return KlimaFlaechenModul.module_60_60_heatarea;

                case ModulTypeEnum.MODUL_100_30:
                case ModulTypeEnum.MODUL_100_30U:
                    return KlimaFlaechenModul.module_100_30_heatarea;

                case ModulTypeEnum.MODUL_120_30:
                case ModulTypeEnum.MODUL_120_30U:
                    return KlimaFlaechenModul.module_120_30_heatarea;

                case ModulTypeEnum.MODUL_160_30U:
                    return KlimaFlaechenModul.module_160_30_heatarea;

                case ModulTypeEnum.MODUL_100_40_20:
                    return KlimaFlaechenModul.module_100_40_20_heatarea;

                default:
                    return 0;
            }
        }

        public double GetCoveredArea(bool floor)
        {
            switch (this.modulType)
            {
                case ModulTypeEnum.MODUL_100_40:
                    return floor ? KlimaFlaechenModul.module_100_40_floor_area : KlimaFlaechenModul.module_100_40_roof_area;

                case ModulTypeEnum.MODUL_80_30:
                case ModulTypeEnum.MODUL_80_30U:
                    return KlimaFlaechenModul.module_80_30_area;

                case ModulTypeEnum.MODUL_60_60:
                case ModulTypeEnum.MODUL_60_60B:
                case ModulTypeEnum.MODUL_60_60C:
                case ModulTypeEnum.MODUL_60_60D:
                    return KlimaFlaechenModul.module_60_60_area;

                case ModulTypeEnum.MODUL_100_30:
                case ModulTypeEnum.MODUL_100_30U:
                    return KlimaFlaechenModul.module_100_30_area;

                case ModulTypeEnum.MODUL_120_30:
                case ModulTypeEnum.MODUL_120_30U:
                    return KlimaFlaechenModul.module_120_30_area;

                case ModulTypeEnum.MODUL_160_30U:
                    return KlimaFlaechenModul.module_160_30_area;

                case ModulTypeEnum.MODUL_100_40_20:
                    return KlimaFlaechenModul.module_100_40_20_area;

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Area which is used as free space between the modules based in both directions.
        /// </summary>
        /// <returns></returns>
        public double GetModulationArea()
        {
            double areaA = ModulationLengthValue * GetModuleWidth(ModulType);
            double areaB = ModulationWidthValue * GetModuleHeight(ModulType);
            double areaC = ModulationWidthValue * ModulationLengthValue;

            return areaA + areaB + areaC;
        }

        public double Druckverlust(double massenstrom)
        {
            switch (this.modulType)
            {
                case ModulTypeEnum.MODUL_100_40:
                    return EN1264.Instance.DruckverlustModul_100_40(1, massenstrom);

                case ModulTypeEnum.MODUL_80_30:
                case ModulTypeEnum.MODUL_80_30U:
                    return EN1264.Instance.DruckverlustModul_80_30(1, massenstrom);

                case ModulTypeEnum.MODUL_100_30:
                case ModulTypeEnum.MODUL_100_30U:
                    return EN1264.Instance.DruckverlustModul_100_30(1, massenstrom);

                case ModulTypeEnum.MODUL_120_30:
                case ModulTypeEnum.MODUL_120_30U:
                    return EN1264.Instance.DruckverlustModul_120_30(1, massenstrom);

                case ModulTypeEnum.MODUL_160_30U:
                    return EN1264.Instance.DruckverlustModul_160_30(1, massenstrom);

                case ModulTypeEnum.MODUL_60_60:
                case ModulTypeEnum.MODUL_60_60B:
                case ModulTypeEnum.MODUL_60_60C:
                case ModulTypeEnum.MODUL_60_60D:
                    return EN1264.Instance.DruckverlustModul_120_30(1, massenstrom);

                case ModulTypeEnum.MODUL_100_40_20:
                    return EN1264.Instance.DruckverlustModul_100_40_20(1, massenstrom);

                default:
                    return 0;
            }
        }


        public static double GetModuleHeight(ModulTypeEnum type)
        {
            switch (type)
            {
                case ModulTypeEnum.MODUL_100_40:
                    return module_100_40_height;

                case ModulTypeEnum.MODUL_100_30:
                case ModulTypeEnum.MODUL_100_30U:
                    return module_100_30_height;

                case ModulTypeEnum.MODUL_120_30:
                case ModulTypeEnum.MODUL_120_30U:
                    return module_120_30_height;

                case ModulTypeEnum.MODUL_160_30U:
                    return module_160_30_height;

                case ModulTypeEnum.MODUL_80_30:
                case ModulTypeEnum.MODUL_80_30U:
                    return module_80_30_height;

                case ModulTypeEnum.MODUL_60_60:
                case ModulTypeEnum.MODUL_60_60B:
                case ModulTypeEnum.MODUL_60_60C:
                case ModulTypeEnum.MODUL_60_60D:
                    return module_60_60_height;

                case ModulTypeEnum.MODUL_100_40_20:
                    return module_100_40_20_height;

                default:
                    return 0;
            }
        }

        public static double GetModuleWidth(ModulTypeEnum type)
        {
            switch (type)
            {
                case ModulTypeEnum.MODUL_100_40:
                    return module_100_40_width;

                case ModulTypeEnum.MODUL_100_30:
                case ModulTypeEnum.MODUL_100_30U:
                    return module_100_30_width;

                case ModulTypeEnum.MODUL_120_30:
                case ModulTypeEnum.MODUL_120_30U:
                    return module_120_30_width;

                case ModulTypeEnum.MODUL_160_30U:
                    return module_160_30_width;

                case ModulTypeEnum.MODUL_80_30:
                case ModulTypeEnum.MODUL_80_30U:
                    return module_80_30_width;

                case ModulTypeEnum.MODUL_60_60:
                case ModulTypeEnum.MODUL_60_60B:
                case ModulTypeEnum.MODUL_60_60C:
                case ModulTypeEnum.MODUL_60_60D:
                    return module_60_60_width;

                case ModulTypeEnum.MODUL_100_40_20:
                    return module_100_40_20_width;

                default:
                    return 0;
            }
        }

        public static void SetModuleHeightGraphical(ModulTypeEnum type, double size)
        {
            switch (type)
            {
                case ModulTypeEnum.MODUL_100_40:
                    module_100_40_height_graphical_project = size;
                    break;

                case ModulTypeEnum.MODUL_100_40_20:
                    module_100_40_20_height_graphical_project = size;
                    break;
            }
        }

        public static void SetModuleWidthGraphical(ModulTypeEnum type, double size)
        {
            switch (type)
            {
                case ModulTypeEnum.MODUL_100_40:
                    module_100_40_width_graphical_project = size;
                    break;

                case ModulTypeEnum.MODUL_100_40_20:
                    module_100_40_20_width_graphical_project = size;
                    break;
            }
        }

        public static double GetModuleHeightGraphical(ModulTypeEnum type)
        {
            if (!modulesGraphicSizeEnabled.Contains(type))
            {
                return GetModuleHeight(type);
            }
            switch (type)
            {
                case ModulTypeEnum.MODUL_100_40:
                    return module_100_40_height_graphical_project;

                case ModulTypeEnum.MODUL_100_40_20:
                    return module_100_40_20_height_graphical_project;

                default:
                    return GetModuleHeight(type);
            }
        }

        public static double GetModuleWidthGraphical(ModulTypeEnum type)
        {
            if (!modulesGraphicSizeEnabled.Contains(type))
            {
                return GetModuleWidth(type);
            }
            switch (type)
            {
                case ModulTypeEnum.MODUL_100_40:
                    return module_100_40_width_graphical_project;

                case ModulTypeEnum.MODUL_100_40_20:
                    return module_100_40_20_width_graphical_project;

                default:
                    return GetModuleWidth(type);
            }
        }

        #region Properties for graphical mode (Klimadecke)
        public int GraphLane
        {
            get { return this.graphLane; }
            set { this.graphLane = value; }
        }

        public double GraphPositionInLan
        {
            get { return this.graphPositionInLane; }
            set { this.graphPositionInLane = value; }
        }

        public bool GraphBottomUp
        {
            get { return this.graphBottomUp; }
            set { this.graphBottomUp = value; }
        }

        public double GraphBottomPositionInLane(double measure)
        {
            // only used for klimadecke
            return this.graphPositionInLane + measure * KlimaFlaechenModul.GetModuleHeight(this.modulType);
        }
        #endregion

        #region Properties for graphical mode (Klimaboden)
        public double GraphPosX
        {
            get { return this.graphPosX; }
            set { this.graphPosX = value; }
        }

        public double GraphPosY
        {
            get { return this.graphPosY; }
            set { this.graphPosY = value; }
        }

        public double GraphRotation
        {
            get { return this.graphRotation; }
            set { this.graphRotation = value; }
        }

        private Polygon2D GetConnectionArea(double measure, bool invertYAxis, Product product, bool topConnection)
        {
            Matrix3D laneRotation;
            Matrix3D moduleRotation;

            double x;
            double y;

            if (product is ModulKlimaDeckeProduct)
            {
                ModulKlimaDeckeProduct mkd = product as ModulKlimaDeckeProduct;

                laneRotation = Transformation3D.Rotate(-mkd.GraphConstruction.Rotation * Math.PI / 180.0);
                moduleRotation = Transformation3D.Rotate(mkd.GraphConstruction.Rotation * Math.PI / 180.0);

                x = laneRotation.Transform(mkd.GraphConstruction.PossibleLanes[this.GraphLane].BorderLeft.Origin).X;
                y = this.GraphPositionInLan;
            }
            else if (product is ModulKlimaBoden20Product)
            {
                ModulKlimaBoden20Product mkb = product as ModulKlimaBoden20Product;

                laneRotation = Transformation3D.Rotate(-this.GraphRotation * Math.PI / 180.0);
                moduleRotation = Transformation3D.Rotate(this.GraphRotation * Math.PI / 180.0);

                Point2D p = new Point2D(GraphPosX, GraphPosY);

                x = laneRotation.Transform(p).X;
                y = laneRotation.Transform(p).Y;
            }
            else
            {
                return null;
            }


            Matrix3D transformation = moduleRotation * Transformation3D.Translation(x, y);
            double height = KlimaFlaechenModul.GetModuleHeight(this.ModulType) * measure;
            double width = KlimaFlaechenModul.GetModuleWidth(this.ModulType) * measure;

            // graphical different representation is used for Klimaboden products only
            if (product is ModulKlimaBoden20Product || product is ModulKlimaBodenProduct)
            {
                height = KlimaFlaechenModul.GetModuleHeightGraphical(this.ModulType) * measure;
                width = KlimaFlaechenModul.GetModuleWidthGraphical(this.ModulType) * measure;
            }

            bool left = ((this.Orientation == ModulOrientationEnum.ORIENTATION_LEFT) != (invertYAxis ^ Product.ConfigViewGrundriss));
            if (!this.DiagonalDurchstroemt)
            {
                if ((this.GraphBottomUp != topConnection) != invertYAxis)
                {
                    left = !left;
                }
            }
            if (topConnection)
            {
                if (left)
                {
                    Point2D input12D = transformation.Transform(new Point2D(width, height));
                    Point2D input22D = transformation.Transform(new Point2D(width, height - 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value));
                    Point2D input32D = transformation.Transform(new Point2D(width - 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value));
                    Point2D input42D = transformation.Transform(new Point2D(width - 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
                    return new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D });
                }
                else
                {
                    Point2D input12D = transformation.Transform(new Point2D(0, height));
                    Point2D input22D = transformation.Transform(new Point2D(0, height - 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value));
                    Point2D input32D = transformation.Transform(new Point2D(0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value));
                    Point2D input42D = transformation.Transform(new Point2D(0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
                    return new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D });
                }
            }
            else
            {
                if (left)
                {
                    Point2D input12D = transformation.Transform(new Point2D(0, 0));
                    Point2D input22D = transformation.Transform(new Point2D(0, 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value));
                    Point2D input32D = transformation.Transform(new Point2D(0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value));
                    Point2D input42D = transformation.Transform(new Point2D(0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
                    return new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D });
                }
                else
                {
                    Point2D input12D = transformation.Transform(new Point2D(width, 0));
                    Point2D input22D = transformation.Transform(new Point2D(width, 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value));
                    Point2D input32D = transformation.Transform(new Point2D(width - 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value));
                    Point2D input42D = transformation.Transform(new Point2D(width - 0.1 * product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
                    return new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D });
                }
            }
        }

        public Polygon2D GetOutputConnectionArea(double measure, bool invertYAxis, Product product)
        {
            return this.GetConnectionArea(measure, invertYAxis, product, invertYAxis != this.GraphBottomUp);
        }

        public Polygon2D GetInputConnectionArea(double measure, bool invertYAxis, Product product)
        {
            return this.GetConnectionArea(measure, invertYAxis, product, invertYAxis == this.GraphBottomUp);
        }

        public Point2D GetOutputConnection(double measure, bool invertYAxis, Product product)
        {
            if (invertYAxis)
            {
                return InternalGetInputConnection(measure, invertYAxis, product);
            }
            else
            {
                return InternalGetOutputConnection(measure, invertYAxis, product);
            }
        }

        private Point2D InternalGetOutputConnection(double measure, bool invertYAxis, Product product)
        {
            ModulOrientationEnum orientationToUse = this.orientation;
            if ((invertYAxis && this.DiagonalDurchstroemt) ^ Product.ConfigViewGrundriss)
            {
                orientationToUse = orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT ? ModulOrientationEnum.ORIENTATION_RIGHT : ModulOrientationEnum.ORIENTATION_LEFT;
            }
            if (product is ModulKlimaBodenProduct)
            {
                Matrix3D transformation = Transformation3D.Translation(this.GraphPosX, this.graphPosY);
                transformation = transformation * Transformation3D.Rotate(this.graphRotation * Math.PI / 180.0);

                double height = KlimaFlaechenModul.GetModuleHeightGraphical(this.ModulType) * measure;
                double width = KlimaFlaechenModul.GetModuleWidthGraphical(this.ModulType) * measure;
                double connectionDist = CONNECTION_DISTANCE * measure;

                if (this.graphBottomUp)
                {
                    if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                    {
                        return transformation.Transform(new Point2D(width - connectionDist, height - connectionDist));
                    }
                    else
                    {
                        return transformation.Transform(new Point2D(connectionDist, height - connectionDist));
                    }
                }
                else
                {
                    if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                    {
                        return transformation.Transform(new Point2D(connectionDist, connectionDist));
                    }
                    else
                    {
                        return transformation.Transform(new Point2D(width - connectionDist, connectionDist));
                    }
                }
            }
            else if (product is ModulKlimaDeckeProduct)
            {
                Matrix3D laneRotation = Transformation3D.Rotate(-(product as ModulKlimaDeckeProduct).GraphConstruction.Rotation * Math.PI / 180.0);
                Matrix3D moduleRotation = Transformation3D.Rotate((product as ModulKlimaDeckeProduct).GraphConstruction.Rotation * Math.PI / 180.0);
                double height = KlimaFlaechenModul.GetModuleHeight(this.ModulType) * measure;
                double width = KlimaFlaechenModul.GetModuleWidth(this.ModulType) * measure;
                double connectionDist = 0.035 * measure; // Abstand der Anschlüsse zum Rand 2.45cm + hälte der breite (2.1cm / 2)
                double x = laneRotation.Transform((product as ModulKlimaDeckeProduct).GraphConstruction.PossibleLanes[this.GraphLane].BorderLeft.Origin).X;
                double y = this.GraphPositionInLan;
                Matrix3D transformation = moduleRotation * Transformation3D.Translation(x, y);

                if (this.graphBottomUp)
                {
                    if (!this.DiagonalDurchstroemt)
                    {
                        if (orientationToUse == ModulOrientationEnum.ORIENTATION_RIGHT)
                        {
                            return transformation.Transform(new Point2D(connectionDist, height - connectionDist));
                        }
                        else
                        {
                            return transformation.Transform(new Point2D(width - connectionDist, height - connectionDist));
                        }
                    }
                    else
                    {
                        if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                        {
                            return transformation.Transform(new Point2D(width - connectionDist, height - connectionDist));
                        }
                        else
                        {
                            return transformation.Transform(new Point2D(connectionDist, height - connectionDist));
                        }
                    }
                }
                else
                {
                    if (!this.DiagonalDurchstroemt)
                    {
                        if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                        {
                            return transformation.Transform(new Point2D(connectionDist, connectionDist));
                        }
                        else
                        {
                            return transformation.Transform(new Point2D(width - connectionDist, connectionDist));
                        }
                    }
                    else
                    {
                        if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                        {
                            return transformation.Transform(new Point2D(connectionDist, connectionDist));
                        }
                        else
                        {
                            return transformation.Transform(new Point2D(width - connectionDist, connectionDist));
                        }
                    }
                }
            }
            else if (product is ModulKlimaBoden20Product)
            {
                Matrix3D transformation = Transformation3D.Translation(this.GraphPosX, this.graphPosY);
                transformation = transformation * Transformation3D.Rotate(this.graphRotation * Math.PI / 180.0);
                double height = KlimaFlaechenModul.GetModuleHeightGraphical(this.ModulType) * measure;
                double width = KlimaFlaechenModul.GetModuleWidthGraphical(this.ModulType) * measure;
                double connectionDist = CONNECTION_DISTANCE * measure;

                if (this.graphBottomUp)
                {
                    if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                    {
                        return transformation.Transform(new Point2D(width - connectionDist, height - connectionDist));
                    }
                    else
                    {
                        return transformation.Transform(new Point2D(connectionDist, height - connectionDist));
                    }
                }
                else
                {
                    if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                    {
                        return transformation.Transform(new Point2D(connectionDist, connectionDist));
                    }
                    else
                    {
                        return transformation.Transform(new Point2D(width - connectionDist, connectionDist));
                    }
                }
            }
            else
            {
                throw new Exception("invalid Product");
            }
        }

        public Point2D GetInputConnection(double measure, bool invertYAxis, Product product)
        {
            if (invertYAxis)
            {
                return InternalGetOutputConnection(measure, invertYAxis, product);
            }
            else
            {
                return InternalGetInputConnection(measure, invertYAxis, product);
            }
        }

        public Point2D InternalGetInputConnection(double measure, bool invertYAxis, Product product)
        {
            ModulOrientationEnum orientationToUse = this.orientation;
            if ((invertYAxis && this.DiagonalDurchstroemt) ^ Product.ConfigViewGrundriss)
            {
                orientationToUse = orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT ? ModulOrientationEnum.ORIENTATION_RIGHT : ModulOrientationEnum.ORIENTATION_LEFT;
            }
            if (product is ModulKlimaBodenProduct)
            {
                Matrix3D transformation = Transformation3D.Translation(this.GraphPosX, this.graphPosY);
                transformation = transformation * Transformation3D.Rotate(this.graphRotation * Math.PI / 180.0);

                double height = KlimaFlaechenModul.GetModuleHeightGraphical(this.ModulType) * measure;
                double width = KlimaFlaechenModul.GetModuleWidthGraphical(this.ModulType) * measure;
                double connectionDist = 0.035 * measure; // Abstand der Anschlüsse zum Rand 2.45cm + hälte der breite (2.1cm / 2)

                if (this.graphBottomUp)
                {
                    if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                    {
                        return transformation.Transform(new Point2D(connectionDist, connectionDist));
                    }
                    else
                    {
                        return transformation.Transform(new Point2D(width - connectionDist, connectionDist));
                    }
                }
                else
                {
                    if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                    {
                        return transformation.Transform(new Point2D(width - connectionDist, height - connectionDist));
                    }
                    else
                    {
                        return transformation.Transform(new Point2D(connectionDist, height - connectionDist));
                    }
                }
            }
            else if (product is ModulKlimaDeckeProduct)
            {
                Matrix3D laneRotation = Transformation3D.Rotate(-(product as ModulKlimaDeckeProduct).GraphConstruction.Rotation * Math.PI / 180.0);
                Matrix3D moduleRotation = Transformation3D.Rotate((product as ModulKlimaDeckeProduct).GraphConstruction.Rotation * Math.PI / 180.0);
                double height = KlimaFlaechenModul.GetModuleHeight(this.ModulType) * measure;
                double width = KlimaFlaechenModul.GetModuleWidth(this.ModulType) * measure;
                double connectionDist = 0.035 * measure; // Abstand der Anschlüsse zum Rand 2.45cm + hälte der breite (2.1cm / 2)
                double x = laneRotation.Transform((product as ModulKlimaDeckeProduct).GraphConstruction.PossibleLanes[this.GraphLane].BorderLeft.Origin).X;
                double y = this.GraphPositionInLan;
                Matrix3D transformation = moduleRotation * Transformation3D.Translation(x, y);

                if (this.graphBottomUp)
                {
                    if (!this.DiagonalDurchstroemt)
                    {
                        if (orientationToUse == ModulOrientationEnum.ORIENTATION_RIGHT)
                        {
                            return transformation.Transform(new Point2D(connectionDist, connectionDist));
                        }
                        else
                        {
                            return transformation.Transform(new Point2D(width - connectionDist, connectionDist));
                        }
                    }
                    else
                    {
                        if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                        {
                            return transformation.Transform(new Point2D(connectionDist, connectionDist));
                        }
                        else
                        {
                            return transformation.Transform(new Point2D(width - connectionDist, connectionDist));
                        }
                    }
                }
                else
                {
                    if (!this.DiagonalDurchstroemt)
                    {
                        if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                        {
                            return transformation.Transform(new Point2D(connectionDist, height - connectionDist));
                        }
                        else
                        {
                            return transformation.Transform(new Point2D(width - connectionDist, height - connectionDist));
                        }
                    }
                    else
                    {
                        if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                        {
                            return transformation.Transform(new Point2D(width - connectionDist, height - connectionDist));
                        }
                        else
                        {
                            return transformation.Transform(new Point2D(connectionDist, height - connectionDist));
                        }
                    }
                }
            }
            else if (product is ModulKlimaBoden20Product)
            {
                Matrix3D transformation = Transformation3D.Translation(this.GraphPosX, this.graphPosY);
                transformation = transformation * Transformation3D.Rotate(this.graphRotation * Math.PI / 180.0);

                double height = KlimaFlaechenModul.GetModuleHeightGraphical(this.ModulType) * measure;
                double width = KlimaFlaechenModul.GetModuleWidthGraphical(this.ModulType) * measure;
                double connectionDist = CONNECTION_DISTANCE * measure; // Abstand der Anschlüsse zum Rand 2.45cm + hälte der breite (2.1cm / 2)

                if (this.graphBottomUp)
                {
                    if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                    {
                        return transformation.Transform(new Point2D(connectionDist, connectionDist));
                    }
                    else
                    {
                        return transformation.Transform(new Point2D(width - connectionDist, connectionDist));
                    }
                }
                else
                {
                    if (orientationToUse == ModulOrientationEnum.ORIENTATION_LEFT)
                    {
                        return transformation.Transform(new Point2D(width - connectionDist, height - connectionDist));
                    }
                    else
                    {
                        return transformation.Transform(new Point2D(connectionDist, height - connectionDist));
                    }
                }
            }
            else
            {
                throw new Exception("invalid Product");
            }
        }

        public KlimaFlaechenModulVerbindung GetInputLink(Circuit circuit, bool invertYAxis)
        {
            KlimaFlaechenModulVerbindung link = null;
            if (circuit is ModulBodenCircuit)
            {
                ModulBodenCircuit mbc = circuit as ModulBodenCircuit;
                if (mbc.Links != null)
                {
                    foreach (KlimaFlaechenModulVerbindung verbindung in mbc.Links)
                    {
                        if (verbindung.End == this)
                        {
                            link = verbindung;
                            break;
                        }
                    }
                }
            }
            else if (circuit is ModulDeckeCircuit)
            {
                ModulDeckeCircuit mdc = circuit as ModulDeckeCircuit;
                foreach (ModulDeckeSubArea sa in mdc.SubAreas)
                {
                    foreach (KlimaFlaechenList row in sa.Rows)
                    {
                        if (row.Links != null)
                        {
                            foreach (KlimaFlaechenModulVerbindung verbindung in row.Links)
                            {
                                if (verbindung.End == this)
                                {
                                    return verbindung;
                                }
                            }
                        }
                    }
                }
            }
            else if (circuit is ModulKlimaBoden20Circuit)
            {
                ModulKlimaBoden20Circuit mdc = circuit as ModulKlimaBoden20Circuit;
                foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                {
                    foreach (KlimaFlaechenList row in sa.Rows)
                    {
                        if (row.Links != null)
                        {
                            foreach (KlimaFlaechenModulVerbindung verbindung in row.Links)
                            {
                                if (verbindung.End == this)
                                {
                                    return verbindung;
                                }
                            }
                        }
                    }
                }
            }
            return link;
        }

        public KlimaFlaechenModulVerbindung GetOutputLink(Circuit circuit, bool invertYAxis)
        {
            KlimaFlaechenModulVerbindung link = null;
            if (circuit is ModulBodenCircuit)
            {
                ModulBodenCircuit mbc = circuit as ModulBodenCircuit;
                if (mbc.Links != null)
                {
                    foreach (KlimaFlaechenModulVerbindung verbindung in mbc.Links)
                    {
                        if (verbindung.Start == this)
                        {
                            link = verbindung;
                            break;
                        }
                    }
                }
            }
            else if (circuit is ModulDeckeCircuit)
            {
                ModulDeckeCircuit mdc = circuit as ModulDeckeCircuit;
                foreach (ModulDeckeSubArea sa in mdc.SubAreas)
                {
                    foreach (KlimaFlaechenList row in sa.Rows)
                    {
                        if (row.Links != null)
                        {
                            foreach (KlimaFlaechenModulVerbindung verbindung in row.Links)
                            {
                                if (verbindung.Start == this)
                                {
                                    return verbindung;
                                }
                            }
                        }
                    }
                }
            }
            else if (circuit is ModulKlimaBoden20Circuit)
            {
                ModulKlimaBoden20Circuit mdc = circuit as ModulKlimaBoden20Circuit;
                foreach (ModulKlimaBoden20SubArea sa in mdc.SubAreas)
                {
                    foreach (KlimaFlaechenList row in sa.Rows)
                    {
                        if (row.Links != null)
                        {
                            foreach (KlimaFlaechenModulVerbindung verbindung in row.Links)
                            {
                                if (verbindung.Start == this)
                                {
                                    return verbindung;
                                }
                            }
                        }
                    }
                }
            }
            return link;
        }

        public KlimaFlaechenSubAreaVerbindung GetSubareaInputLink(Circuit circuit, bool invertYAxis)
        {
            KlimaFlaechenSubAreaVerbindung link = null;
            // no subareas for boden!
            if (circuit is ModulDeckeCircuit)
            {
                ModulDeckeCircuit mdc = circuit as ModulDeckeCircuit;
                if (mdc.Links != null)
                {
                    foreach (KlimaFlaechenSubAreaVerbindung saLink in mdc.Links)
                    {
                        if (saLink.End.Contains(this))
                        {
                            link = saLink;
                            break;
                        }
                    }
                }
            }
            else if (circuit is ModulKlimaBoden20Circuit)
            {
                ModulKlimaBoden20Circuit mdc = circuit as ModulKlimaBoden20Circuit;
                if (mdc.Links != null)
                {
                    foreach (KlimaFlaechenSubAreaVerbindung saLink in mdc.Links)
                    {
                        if (saLink.End.Contains(this))
                        {
                            link = saLink;
                            break;
                        }
                    }
                }
            }
            return link;
        }

        public KlimaFlaechenSubAreaVerbindung GetSubareaOutputLink(Circuit circuit, bool invertYAxis)
        {
            KlimaFlaechenSubAreaVerbindung link = null;
            // no subareas for boden!
            if (circuit is ModulDeckeCircuit)
            {
                ModulDeckeCircuit mdc = circuit as ModulDeckeCircuit;
                if (mdc.Links != null)
                {
                    foreach (KlimaFlaechenSubAreaVerbindung saLink in mdc.Links)
                    {
                        if (saLink.Start.Contains(this))
                        {
                            link = saLink;
                            break;
                        }
                    }
                }
            }
            else if (circuit is ModulKlimaBoden20Circuit)
            {
                ModulKlimaBoden20Circuit mdc = circuit as ModulKlimaBoden20Circuit;
                if (mdc.Links != null)
                {
                    foreach (KlimaFlaechenSubAreaVerbindung saLink in mdc.Links)
                    {
                        if (saLink.Start.Contains(this))
                        {
                            link = saLink;
                            break;
                        }
                    }
                }
            }
            return link;
        }

        public bool IsOutputOpen(Circuit circuit, bool invertYAxis)
        {
            return this.GetOutputLink(circuit, invertYAxis) == null && this.GetSubareaOutputLink(circuit, invertYAxis) == null;
        }

        public bool IsInputOpen(Circuit circuit, bool invertYAxis)
        {
            return this.GetInputLink(circuit, invertYAxis) == null && this.GetSubareaInputLink(circuit, invertYAxis) == null;
        }
        #endregion

        internal static void SetEnabledGraphicSizes(List<ModulTypeEnum> enabledModules)
        {
            modulesGraphicSizeEnabled = enabledModules;
        }
    }
}
