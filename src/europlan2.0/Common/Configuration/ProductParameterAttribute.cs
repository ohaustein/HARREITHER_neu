using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Europlan.Common {
	/// <summary>
	/// Base class for ProductParameterAttributes. Properties of Products that are marked with this attribute will be treated as product parameters that are stored in the config.
	/// There are currently implementations for product parameters of the following types:
	/// - bool   (BoolProductParameterAttribute)
	/// - int    (IntProductParameterAttribute)
	/// - float  (FloatProductParameterAttribute)
	/// - double (DoubleProductParameterAttribute)
	/// - string (StringProductParameterAttribute)
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public abstract class ProductParameterAttribute: Attribute {
		public bool overrideableInPlanning = false;
		public bool overrideableInQuickDimensioning = false;
		public bool saveForUser = false;
		public bool saveInProject = true;

		public ProductParameterAttribute() {
		}

		public abstract object DefaultValue {
			get;
		}

		public abstract string DefaultValueAsString {
			get;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class DoubleProductParameterAttribute : ProductParameterAttribute {
		public double defaultValue = 0.0;

		public DoubleProductParameterAttribute(double defaultValue) {
			this.defaultValue = defaultValue;
		}

		public override object DefaultValue {
			get { return this.defaultValue; }
		}

		public override string DefaultValueAsString {
			get { return this.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat); }
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class BoolProductParameterAttribute : ProductParameterAttribute {
		public bool defaultValue = false;

		public BoolProductParameterAttribute(bool defaultValue) {
			this.defaultValue = defaultValue;
		}

		public override object DefaultValue {
			get { return this.defaultValue; }
		}

		public override string DefaultValueAsString {
			get { return this.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat); }
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class IntProductParameterAttribute : ProductParameterAttribute {
		public int defaultValue = 0;

		public IntProductParameterAttribute(int defaultValue) {
			this.defaultValue = defaultValue;
		}

		public override object DefaultValue {
			get { return this.defaultValue; }
		}

		public override string DefaultValueAsString {
			get { return this.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat); }
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class FloatProductParameterAttribute : ProductParameterAttribute {
		public float defaultValue = 0;

		public FloatProductParameterAttribute(float defaultValue) {
			this.defaultValue = defaultValue;
		}

		public override object DefaultValue {
			get { return this.defaultValue; }
		}

		public override string DefaultValueAsString {
			get { return this.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat); }
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class StringProductParameterAttribute : ProductParameterAttribute {
		public string defaultValue = "";

		public StringProductParameterAttribute(string defaultValue) {
			this.defaultValue = defaultValue;
		}

		public override object DefaultValue {
			get { return this.defaultValue; }
		}

		public override string DefaultValueAsString {
			get { return this.defaultValue; }
		}
	}
}
