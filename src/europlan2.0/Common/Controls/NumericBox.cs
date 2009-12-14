using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Common {
	public class NumericBox : TextBox {

		public enum NumericEditType {
			DEFAULT = 0,
			REGULATORY_CIRCUIT_TEMP = 1,
			ROOM_AREA = 2,
			ROOM_TEMPERATURE = 3,
			ROOM_HEAT_POWER = 4,
			ROOM_COOL_POWER = 5,
			FLOOR_CONSTRUCTION_THICKNESS = 6,
			LAMBDA_VALUE = 7,
			R_VALUE = 8,
			DENOMINATION = 9,
			PRICE = 10,
			FLOW_TEMPERATURE = 11,
			PERCENTAGE = 12,
			POWER_WITH_SIGN = 13,
			PIPE_LENGTH = 14,
			MODULE_COUNT = 15,
			HK_COUNT = 16,
			PIPE_LENGTH_HITHERM = 17,
			FACTOR = 18,
		}
		                                                                              //   DEF   RCTMP       AREA   TEMP             HPW             CPW  CONSTR_THICK        LAMBDA             R,   DENOMINATION        PRICE    FLOW_TEMP   PERC     PWR_SIGN       PIPE_L    MODULE_COUNT HK_CNT     PIPE_L_HT         FACTOR
		private static readonly Nullable<decimal>[] minValue = new Nullable<decimal>[] {  null,     0,          0,  -273,              0,              0,            0,            0,            0,              0,           0,           0,      0,       null,           0,              0,     1,            0, (decimal)0.01};
		private static readonly Nullable<decimal>[] maxValue = new Nullable<decimal>[] {  null,    99,       null,   999, Int32.MaxValue, Int32.MaxValue,         null,         null,         null, Int32.MaxValue,        null,        null,    100,       null,        null, Int32.MaxValue,    12,         null,            99};
		private static readonly int[] decimalPlaces = new int[]                        {     0,     0,          1,     0,              0,              0,            2,            3,            3,              0,           2,           1,      1,          0,           1,              0,     0,            2,             2};
		private static readonly string[] masks = new string[]                          {   "0",  "90", "999990.9", "990",      "9999990",      "9999990",  "999990.99", "999990.999", "999990.999",       "999990", "999990.99", "999990.99", "990.9", "9999990", "9999990.9",      "9999990",  "90", "9999990.99",       "90.99"};
		private static readonly bool[] sign = new bool[]                               { false, false,      false, false,          false,          false,        false,        false,        false,          false,       false,       false,   false,      true,       false,          false, false,        false,         false};

		private Nullable<decimal> realMaxValue = null;
		private Nullable<decimal> realMinValue = null;

		private NumericBox.NumericEditType editType = NumericBox.NumericEditType.DEFAULT;
		public event EventHandler ValueChanged;

		private bool internalValueChange = false;

		//private decimal lastValue = 0;

		public NumericBox() {
			this.InternalValue = 0;
		}

		public NumericBox.NumericEditType EditType {
			get { return this.editType; }
			set { this.editType = value; }
		}

		protected virtual void OnValueChanged(EventArgs args) {
			if (this.ValueChanged != null) {
				this.ValueChanged(this, args);
			}
		}

		public decimal InternalValue {
			get { return this.Value; }
			set {
				this.internalValueChange = true;
				this.Value = value;
				this.internalValueChange = false;
			}
		}

		public decimal Value {
			get {
				decimal val;
				if (this.Text.Length == 0) {
					val = 0;
				} else {
					string text = this.Text;
					if (text.StartsWith(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)) {
						text = "0" + text;
					} else if (text.StartsWith("-" + Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)) {
						text = "-0" + text.Substring(1);
					}
					//try {
					if (!decimal.TryParse(text, out val)) {
						//val = Convert.ToDecimal(text);
					//} catch (Exception) {
						if (text.StartsWith("-")) {
							val = (this.MinValue.HasValue ? this.MinValue.Value : decimal.MinValue);
						} else {
							val = (this.MaxValue.HasValue ? this.MaxValue.Value : decimal.MaxValue);
						}
					}
				}
				if (this.MaxValue.HasValue && val > this.MaxValue.Value) {
					val = this.MaxValue.Value;
				}
				if (this.MinValue.HasValue && val < this.MinValue.Value) {
					val = this.MinValue.Value;
				}
				return val;
				//return this.numValueBox.Value;
			}
			set {
				/*NumberFormatInfo info = new NumberFormatInfo();
				String.Format(*/
				/*string formatString = masks[(int)this.editType];
				formatString = formatString.Replace('9', '0');
				formatString = "{0:" + formatString + "}";
				string formattedString = String.Format(formatString, value);
				int i = 0;
				while (i < formattedString.Length && formattedString[i] == '0') {
					formattedString = formattedString.Substring(0, i) + ' ' + formattedString.Substring(i + 1);
					i++;
				}
				this.maskedTextBox1.Text = formattedString;*/
				//this.lastValue = value;
				decimal correctedVal = Math.Round(value, decimalPlaces[(int)this.editType]);
				if (this.MaxValue.HasValue && correctedVal > this.MaxValue.Value) {
					correctedVal = this.MaxValue.Value;
				}
				if (this.MinValue.HasValue && correctedVal < this.MinValue.Value) {
					correctedVal = this.MinValue.Value;
				}
				string text = "";
				if (correctedVal >= 0 && sign[(int)this.editType]) {
					text = "+";
				}
				text += correctedVal.ToString();
				this.Text = text;
				//this.numValueBox.Value = value;
			}
		}

		public override string Text {
			get { return base.Text; }
			set {
				// TODO implement
				base.Text = value;
			}
		}

		public static int DecimalPlaces(NumericBox.NumericEditType type) {
			return decimalPlaces[(int)type];
		}

		protected override void OnTextChanged(EventArgs e) {
			if (!this.internalValueChange) {
				base.OnTextChanged(e);
				this.OnValueChanged(e);
			}
		}

		private string decimalCharacters = "0123456789";

		/*protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e) {
			if (e.KeyCode == Keys.Escape) {
				e.IsInputKey = false;
			} else {
				base.OnPreviewKeyDown(e);
			}
		}*/

		protected override void OnKeyPress(KeyPressEventArgs e) {
			base.OnKeyPress(e);
			string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			if (decimalCharacters.IndexOf(e.KeyChar) >= 0) {
				if (this.Text.StartsWith("-") && this.SelectionStart == 0 && this.SelectionLength == 0) {
					this.SelectionStart = 1;
				}
			} else if (e.KeyChar == '-') {
				if (this.MinValue.HasValue && this.MinValue.Value >= 0) {
					e.Handled = true;
				} else {
					if (this.Text.StartsWith("-")) {
						e.Handled = true;
					} else {
						int selStart = this.SelectionStart;
						int selLength = this.SelectionLength;
						this.Text = "-" + this.Text;
						this.SelectionStart = selStart + 1;
						this.SelectionLength = selLength;
						e.Handled = true;
					}
				}
			} else if (e.KeyChar == '+') {
				if (this.Text.StartsWith("-")) {
					int sel = this.SelectionStart;
					this.Text = this.Text.Substring(1);
					this.SelectionStart = (sel == 0 ? 0 : sel - 1);
					e.Handled = true;
				} else {
					e.Handled = true;
				}
			} else if (e.KeyChar == '.' || e.KeyChar == ',' || decimalSeparator.IndexOf(e.KeyChar) >= 0) {
				if ((this.Text.Contains(decimalSeparator) && !this.SelectedText.Contains(decimalSeparator)) || decimalPlaces[(int)this.editType] == 0) {
					e.Handled = true;
				} else {
					int selStart = this.SelectionStart;
					int selEnd = this.SelectionStart + this.SelectionLength;
					this.Text = this.Text.Substring(0, selStart) + decimalSeparator + this.Text.Substring(selEnd);
					this.SelectionStart = selStart + decimalSeparator.Length;
					this.SelectionLength = 0;
					e.Handled = true;
				}
			} else if (e.KeyChar == '\b') {
			/*} else if (((int)e.KeyChar)  == 27) {
				this.Value = this.lastValue;*/
			} else {
				e.Handled = true;
			}
		}

		protected override void OnValidating(System.ComponentModel.CancelEventArgs e) {
			this.InternalValue = this.Value; // reset value to correct displayed string
			base.OnValidating(e);
		}

		public Nullable<decimal> MinValue {
			get { return (this.realMinValue.HasValue ? this.realMinValue.Value : minValue[(int)this.editType]); }
			set {
				if (value.HasValue) {
					if (minValue[(int)this.editType].HasValue && value.Value <= minValue[(int)this.editType]) {
						this.realMinValue = null;
					} else {
						this.realMinValue = value;
					}
				} else {
					this.realMinValue = null;
				}
			}
		}

		public Nullable<decimal> MaxValue {
			get { return (this.realMaxValue.HasValue ? this.realMaxValue.Value : maxValue[(int)this.editType]); }
			set {
				if (value.HasValue) {
					if (maxValue[(int)this.editType].HasValue && value.Value >= maxValue[(int)this.editType]) {
						this.realMaxValue = null;
					} else {
						this.realMaxValue = value;
					}
				} else {
					this.realMaxValue = null;
				}
			}
		}
	}
}
