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

            List<string> tablesToAdd = CreateTableSchema();

            foreach (string table in tablesToAdd)
            {
                DataTable dt = new DataTable(table);
                DataColumn participantNum = new DataColumn()
                {
                    ColumnName = "Participant #",
                    DataType = typeof(int),
                    AutoIncrement = true,
                    AutoIncrementSeed = 1,
                    AutoIncrementStep = 1
                };

                dt.Columns.Add(participantNum);
                for (int i = 0; i < FactorSet.Rows.Count; i++)
                {
                    if ((bool)FactorSet.Rows[i]["isWithinSubjects"])
                    {
                        List<string> labels = (List<string>)FactorSet.Rows[i]["Labels"];

                        foreach (string label in labels)
                        {
                            dt.Columns.Add(label, typeof (string));
                            
                        }
                    }
                    
                }
                ResearchDesignOutput.Tables.Add(dt);

            }

            //designOutput = new DataTable("Research Design");
            //designOutput.Columns.Add("Index", typeof(int));
            
            //designOutput.Columns.Add("", typeof(int));
        }

        public List<string> CreateTableSchema()
        {
            //Dictionary<string, List<string>> betweenSubjectsFactors = new Dictionary<string, List<string>>();
            Queue<List<string>> q = new Queue<List<string>>();

            for (int i = 0; i < FactorSet.Rows.Count; i++)
            {
                if (!(bool) FactorSet.Rows[i]["isWithinSubjects"])
                {
                    //betweenSubjectsFactors.Add(FactorSet.Rows[i]["Name"].ToString(), (List<string>)FactorSet.Rows[i]["Labels"]);
                    q.Enqueue((List<string>)FactorSet.Rows[i]["Labels"]);
                }
            }

            return recursiveGenerateTables(q);
   
        }

        public List<string> recursiveGenerateTables(Queue<List<string>> q)
        {
            
            List<string> n = new List<string>();
            List<string> p = new List<string>();
            bool nonBase = false;

            while (q.Count > 0)
            {
                List<string> researchFactor = q.Dequeue();
                
                foreach (string s in researchFactor)
                {
                    //while the queue is >0, recursively call this method. All stacks that call this method are not our base case.
                    if (q.Count > 0)  
                    {
                        n = recursiveGenerateTables(q);
                        nonBase = true;
                    }

                    //For the base case, we simply add all the factors to the list.
                    if(!nonBase)
                    {
                        n.Add(s);
                    }
                    //if this is not the base case, then we must have gotten a list from a recursive call; thus, we add its contents to the each research factor in this list.
                    else
                    {
                        for (int i = 0; i < n.Count; i++)
                        {
                            p.Add(s + " | " + n[i]);

                        }

                        //can use the LINQ version here, but its not as easy to understand, I think.
                        //p.AddRange(n.Select(t => s + " | " + t));

                    }
                }
            }

            if (p.Count == 0) return n;

            return p;
        }

    }
}
