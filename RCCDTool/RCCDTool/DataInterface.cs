using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCDTool
{
    static class DataInterface
    {
        static DataInterface()
        {
            //CreateDataTable();
        }
        public static DataTable FactorSet => factorSet;
        private static DataTable factorSet;
        public static DataTable CreateDataTable(List<ResearchFactor> factors)
        {
            
            ///this part sets up the structure of our datatable
            //http://stackoverflow.com/questions/701223/net-convert-generic-collection-to-datatable
            DataSet ds = new DataSet();
            Type entityType = typeof(ResearchFactor);
            DataTable dt = new DataTable(entityType.Name); // (typeof (ResearchFactor).ToString());
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }

            //now we need to add the data to the table.
            foreach (ResearchFactor factor in factors)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(factor);
                }
            }
            ds.Tables.Add(dt);

            factorSet = dt;
            return dt;
        }
    }
}
