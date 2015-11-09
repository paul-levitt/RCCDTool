using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using Xceed.Wpf.DataGrid;
using DataRow = System.Data.DataRow;

namespace RCCDTool
{
    class Model : IModel
    {
        private static DataTable factorSet;
        private DataGridControl dgControl;
        
        public DataTable FactorSet => factorSet;

        public Model()
        {
            
            Type entityType = typeof(ResearchFactor);
            factorSet = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                factorSet.Columns.Add(prop.Name, prop.PropertyType);
            }
            //set primary key. Not sure how to use this, but hopefully I can figure this out.
            DataColumn[] column = new DataColumn[1];
            column[0] = factorSet.Columns["Label"];
            factorSet.PrimaryKey = column;

            DataRow rowTest = factorSet.NewRow();
            rowTest["Label"] = "testRow";
            rowTest["isRandomized"] = true;
            rowTest["isWithinSubjects"] = false;
            rowTest["Levels"] = 5;

            factorSet.Rows.Add(rowTest);

            dgControl = new DataGridControl
            {
                ItemsSource = new DataGridCollectionView(FactorSet.DefaultView)
            };

        }


        public void addFactor(ResearchFactor newFactor)
        {
            Type entityType = typeof(ResearchFactor);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            //now we need to add the data to the table.
            
            DataRow row = factorSet.NewRow();

            foreach (PropertyDescriptor prop in properties)
            {
                row[prop.Name] = prop.GetValue(newFactor);
            }

            factorSet.Rows.Add(row);
            
            //dgControl.ItemsSource = new DataGridCollectionView(FactorSet.DefaultView);
        }

        public void removeFactor(ResearchFactor newFactor)
        {
            
        }

        public bool HasData => factorSet != null && factorSet.Rows.Count > 0;

        public int NumFactors => factorSet?.Rows.Count ?? 0; //wow, how compact!!

        public void ClearFactors()
        {
            factorSet.Clear();
        }
    }
}
