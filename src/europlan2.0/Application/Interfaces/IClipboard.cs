using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Application {

	interface IClipboard {

		bool SupportsCut { get; }
		bool SupportsCopy { get; }
		string DataFormat { get;}
		string SupportedPasteFormat { get; }
		bool SupportsPaste(string data);
		//object Cut();
		object Copy();
		void Paste(object o);
	}

}
