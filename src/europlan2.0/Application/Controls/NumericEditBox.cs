using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace Europlan.Application {
	public partial class NumericEditBox : UserControl {

		public enum NumericEditType {
			DEFAULT = 0,
			REGULATORY_CIRCUIT_TEMP = 1,
			ROOM_AREA = 2,
			ROOM_TEMPERATURE = 3,
			ROOM_HEAT_POWER = 4,
			ROOM_COOL_POWER = 5
		}

		private NumericEditType editType = NumericEditType.DEFAULT;
		                                                          //  DEF  RCTMP       AREA  TEMP        HPW        CPW
		private static readonly decimal[] minValue = new decimal[] {    0,    0,          0,    0,         0,         0};
		private static readonly decimal[] maxValue = new decimal[] {   99,   99,     999999,   99,   9999999,   9999999};
		private static readonly int[] decimalPlaces = new int[]    {    0,    0,          1,    0,         0,         0};
		private static readonly string[] masks = new string[]      { "90", "90", "999990.9", "90", "9999990", "9999990"};

		public event EventHandler ValueChanged;

		public NumericEditBox() {
			InitializeComponent();
		}

		public NumericEditBox(NumericEditType editType) {
			InitializeComponent();
		}

		protected override void OnResize(EventArgs e) {
			this.txtValue.Left = (this.ClientBorder == BorderStyle.None ? 3 : 0);
			this.txtValue.Width = (this.ClientBorder == BorderStyle.None ? this.Width - 6 : this.Width);
			if (this.ClientBorder != BorderStyle.None) {
				this.Height = this.txtValue.Height;
			} else {
				this.txtValue.Top = (this.Height - this.txtValue.Height) / 2;
			}



			base.OnResize(e);
			Console.WriteLine("textbox (" + this.txtValue.Location.X + "," + this.txtValue.Location.Y + " / " + this.txtValue.Size.Width + "," + this.txtValue.Size.Height);
		}

		public NumericEditType EditType {
			get { return this.editType; }
			set {
				this.editType = value;
				//this.numValueBox.Maximum = maxValue[(int)this.editType];
				//this.numValueBox.Minimum = minValue[(int)this.editType];
				//this.numValueBox.DecimalPlaces = decimalPlaces[(int)this.editType];
			}
		}

		protected virtual void OnValueChanged(EventArgs args) {
			if (this.ValueChanged != null) {
				this.ValueChanged(this, args);
			}
		}

		public decimal Value {
			get {
				decimal val;
				if (this.txtValue.Text.Length == 0) {
					val = 0;
				} else {
					string text = this.txtValue.Text;
					if (text.StartsWith(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)) {
						text = "0" + text;
					}
					try {
						val = Convert.ToDecimal(text);
					} catch {
						val = 0;
					}
				}
				if (val > maxValue[(int)this.editType]) {
					val = maxValue[(int)this.editType];
				}
				if (val < minValue[(int)this.editType]) {
					val = minValue[(int)this.editType];
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
				this.txtValue.Text = value.ToString();
				//this.numValueBox.Value = value;
			}
		}

		private void numValueBox_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == ',' || e.KeyChar == '.' && System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator.Length == 1) {
				e.KeyChar = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
			}
			if (new string(e.KeyChar, 1).Equals(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator) && DecimalPlaces(this.editType) == 0) {
				e.Handled = true;
			}
			if (e.KeyChar == '-' && minValue[(int)this.editType] >= 0) {
				e.Handled = true;
			}
		}

		private void numValueBox_ValueChanged(object sender, EventArgs e) {
			this.OnValueChanged(EventArgs.Empty);
		}

		public BorderStyle ClientBorder {
			get { return this.txtValue.BorderStyle; }
			set {
				this.txtValue.BorderStyle = value;
				this.txtValue.Left = (this.ClientBorder == BorderStyle.None ? 3 : 0);
				this.txtValue.Width = (this.ClientBorder == BorderStyle.None ? this.Width - 6 : this.Width);
				if (this.ClientBorder != BorderStyle.None) {
					this.Height = this.txtValue.Height;
				} else {
					this.txtValue.Top = (this.Height - this.txtValue.Height) / 2;
				}
			}
		}

		public static int DecimalPlaces(NumericEditType type) {
			return decimalPlaces[(int)type];
		}

		private void maskedTextBox1_TextChanged(object sender, EventArgs e) {
			this.OnValueChanged(EventArgs.Empty);
		}

		public void SelectAll() {
			this.txtValue.SelectionStart = 0;
			this.txtValue.SelectionLength = this.txtValue.Text.Length;
			/*this.maskedTextBox1.SelectionStart = 0;
			this.maskedTextBox1.SelectionLength = this.maskedTextBox1.Text.Length;*/
		}

		private string decimalCharacters = "0123456789";

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
			string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			if (decimalCharacters.IndexOf(e.KeyChar) >= 0) {
				if (this.txtValue.Text.StartsWith("-") && this.txtValue.SelectionStart == 0 && this.txtValue.SelectionLength == 0) {
					this.txtValue.SelectionStart = 1;
				}
			} else if (e.KeyChar == '-') {
				if (minValue[(int)this.editType] >= 0) {
					e.Handled = true;
				} else {
					if (this.txtValue.Text.StartsWith("-")) {
						e.Handled = true;
					} else {
						int selStart = this.txtValue.SelectionStart;
						int selLength = this.txtValue.SelectionLength;
						this.txtValue.Text = "-" + this.txtValue.Text;
						this.txtValue.SelectionStart = selStart + 1;
						this.txtValue.SelectionLength = selLength;
						e.Handled = true;
					}
				}
			} else if (e.KeyChar == '+') {
				if (this.txtValue.Text.StartsWith("-")) {
					int sel = this.txtValue.SelectionStart;
					this.txtValue.Text = this.txtValue.Text.Substring(1);
					this.txtValue.SelectionStart = (sel == 0 ? 0 : sel - 1);
					e.Handled = true;
				} else {
					e.Handled = true;
				}
			} else if (e.KeyChar == '.' || e.KeyChar == ',' || decimalSeparator.IndexOf(e.KeyChar) >= 0) {
				if (this.txtValue.Text.Contains(decimalSeparator) && !this.txtValue.SelectedText.Contains(decimalSeparator)) {
					e.Handled = true;
				} else {
					int selStart = this.txtValue.SelectionStart;
					int selEnd = this.txtValue.SelectionStart + this.txtValue.SelectionLength;
					this.txtValue.Text = this.txtValue.Text.Substring(0, selStart) + decimalSeparator + this.txtValue.Text.Substring(selEnd);
					this.txtValue.SelectionStart = selStart + decimalSeparator.Length;
					this.txtValue.SelectionLength = 0;
					e.Handled = true;
				}
			} else if (e.KeyChar == '\b') {
			} else {
				e.Handled = true;
			}
		}

		private void txtValue_Validating(object sender, CancelEventArgs e) {
			this.Value = this.Value; // reset the Value to correct text
		}

		public HorizontalAlignment TextAlign {
			get { return this.txtValue.TextAlign; }
			set { this.txtValue.TextAlign = value; }
		}
	}
}
