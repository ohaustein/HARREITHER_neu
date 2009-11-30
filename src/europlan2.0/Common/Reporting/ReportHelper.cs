using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

namespace Europlan.Common {
	public class ReportHelper {

		public static DataTable ListToDataTable<T>(List<T> list) {
			DataTable dt = new DataTable();

			foreach (PropertyInfo info in typeof(T).GetProperties()) {
				Type colType = info.PropertyType;
				if ((info.PropertyType.IsGenericType)/* && ((info.PropertyType) == typeof(Nullable<>))*/) {
					colType = colType.GetGenericArguments()[0];
				}
				dt.Columns.Add(new DataColumn(info.Name, colType));

				//dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
			}
			foreach (T t in list) {
				DataRow row = dt.NewRow();
				foreach (PropertyInfo info in typeof(T).GetProperties()) {
					row[info.Name] = info.GetValue(t, null);
				}
				dt.Rows.Add(row);
			}
			return dt;
		}

	}
}
