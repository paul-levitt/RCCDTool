using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Documents;
using Xceed.Wpf.DataGrid;
using DataRow = System.Data.DataRow;

namespace RCCDTool
{
    class Model : IModel
    {
        private static DataTable factorSet;
        private DataGridControl dgControl;
        private static DataTable designOutput;
        private static DataSet _researchDesignOutput;
        public List<string> Tables { get; set; }
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();


        public int NumSubjects { get; set; }
        public DataSet ResearchDesignOutput
        {
            get { return _researchDesignOutput; }
            set { _researchDesignOutput = value; }
            
        }
        public DataTable FactorSet => factorSet;

        public DataTable DesignOutput => designOutput;

        public Model()
        {
            Tables = new List<string>();
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

            _researchDesignOutput = new DataSet();
            Tables = CreateTableSchema();

            int subjectsPerTable = numSubjects/Tables.Count;

            foreach (string table in Tables)
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

                try
                {
                    //DataRow rowTest = dt.NewRow();
                    //rowTest[0] = "testRow";
                    //rowTest[1] = table;
                    //rowTest[2] = table;

                    //dt.Rows.Add(rowTest);
                    dt = FillTable(dt, subjectsPerTable);
                    _researchDesignOutput.Tables.Add(dt);

                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        public DataTable FillTable(DataTable inputTable, int numSubjects)
        {
            
            List<List<string>> combos = new List<List<string>>();

            //add blank rows for processing for the total number of participants
            for (int h = 0; h < numSubjects; h++)
            {
                DataRow newRow = inputTable.NewRow();
                inputTable.Rows.Add(newRow);
            }

            // Create a queue of participant numbers to randomly select from
            List<int> participants = Enumerable.Range(0, numSubjects-1).ToList();

            
            //generate all possible conditions for each within subject factor
            Dictionary<string, List<List<string>>> conditions = new Dictionary<string, List<List<string>>>();

            try
            {
                for (int i = 0; i < FactorSet.Rows.Count; i++)
                {
                    if ((bool) FactorSet.Rows[i]["isWithinSubjects"] || (bool) FactorSet.Rows[i]["IsRandomized"])
                    {
                        List<string> labels = (List<string>) FactorSet.Rows[i]["Labels"];

                        var comboLists = GetPermutations(labels, labels.Count).Select(c => c.ToList()).ToList();
                        conditions.Add(FactorSet.Rows[i]["Name"].ToString(), comboLists.ToList());
                        
                    }
                }
            }
            catch (Exception e) //TODO: handle actual possible exceptions.
            {
                MessageBox.Show(e.Message);
            }

            //this represents all conditions that have been assigned across participants. It is cleared once all have been assigned.
            List<List<int>> usedIndexes = new List<List<int>>();

            //One list for each condition set.
            for (int j = 0; j< conditions.Count; j++)
                usedIndexes.Add(new List<int>());

            try
            {
                //Randomly select a pariticpant, then randomly assign a condition to them
                while (participants.Count > 0)
                {
                    int nextParticipantIndex = GetRand(participants.Count);
                    int nextParticipant = participants[nextParticipantIndex];
                    var tempComboList = conditions;
                    int j = 0; //this ties our used indexes to our combolist

                    participants.RemoveAt(nextParticipantIndex);

                    foreach (var conditionSet in tempComboList)
                    {
                        //get a random number to represent the next condition to assign
                        int nextConditionIndex = GetRand(conditionSet.Value.Count);

                        //if this condition has already been used, generate a new condition
                        while (usedIndexes[j].Any(nextNum => nextConditionIndex == nextNum))
                        {
                            //refresh the conditionset if we run out
                            if (usedIndexes[j].Count >= conditionSet.Value.Count)
                                usedIndexes[j].Clear();

                            nextConditionIndex = GetRand(conditionSet.Value.Count);
                        }

                        //add the condition to the list; we don't want to use this one again unless all condtions are used
                        usedIndexes[j].Add(nextConditionIndex);

                        //assign the condition to the participant
                        for (int i = 0; i < conditionSet.Value[nextConditionIndex].Count; i++)
                        {
                            string condition = conditionSet.Value[nextConditionIndex][i];
                            inputTable.Rows[nextParticipant][condition] = i;
                        }

                        j++;
                    }
                }
            }
            catch (Exception e) //TODO: handle actual possible exceptions.
            {
                MessageBox.Show(e.Message);
            }

            return inputTable;

        }

        private int GetRand(int max)
        {
            byte[] randomNum = new byte[1];
            rngCsp.GetBytes(randomNum);
            int retVal = 0;
            retVal = randomNum[0] % max;
            return retVal;

        }

        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public List<string> CreateTableSchema()
        {
            Queue<List<string>> q = new Queue<List<string>>();

            for (int i = 0; i < FactorSet.Rows.Count; i++)
            {
                if (!(bool) FactorSet.Rows[i]["isWithinSubjects"])
                {
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
