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
        private static DataTable designOutput;
        private static DataSet ResearchDesignOutput;

        public DataTable FactorSet => factorSet;

        public DataTable DesignOutput => designOutput;

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
            column[0] = factorSet.Columns["Name"];
            factorSet.PrimaryKey = column;
            
            //DataRow rowTest = factorSet.NewRow();
            //rowTest["Label"] = "testRow";
            //rowTest["isRandomized"] = true;
            //rowTest["isWithinSubjects"] = false;
            //rowTest["Levels"] = 5;

            //factorSet.Rows.Add(rowTest);

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

        public void generateDesign(int numSubjects)
        {

            ResearchDesignOutput = new DataSet();
            for (int i = 0; i < FactorSet.Rows.Count; i++)
            {
                if (!(bool) FactorSet.Rows[i]["isWithinSubjects"] && (bool)FactorSet.Rows[i]["isRandomized"])
                {
                    DataTable dt;
                    for (int j = 0; j < (int) FactorSet.Rows[i]["Levels"]; j++)
                    {
                        dt = new DataTable(FactorSet.Rows[i]["Name"].ToString()+i);

                        for (int k = 0; k < FactorSet.Rows.Count; k++)
                        {
                            if ((bool) FactorSet.Rows[k]["isWithinSubjects"])
                            {
                                dt.Columns.Add(FactorSet.Rows[k]["Name"].ToString(), typeof (string));
                                
                            }
                
                        }

                        ResearchDesignOutput.Tables.Add(dt);
                    }

                }
                    

            }
            designOutput = new DataTable("Research Design");
            designOutput.Columns.Add("Index", typeof(int));
            
            //designOutput.Columns.Add("", typeof(int));
        }

        public void CreateTableSchema()
        {
            Dictionary<string, int> betweenSubjectsFactors = new Dictionary<string, int>();

            for (int i = 0; i < FactorSet.Rows.Count; i++)
            {
                if (!(bool) FactorSet.Rows[i]["isWithinSubjects"])
                    betweenSubjectsFactors.Add(FactorSet.Rows[i]["Name"].ToString(), (int)FactorSet.Rows[i]["Levels"]);
            }

            List<string> tablesToAdd = new List<string>();

            for (int i = 0; i < betweenSubjectsFactors.Count; i++)
            {
                
            }
        }

    }
}
