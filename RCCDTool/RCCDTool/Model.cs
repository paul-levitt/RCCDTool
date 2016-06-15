using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Converters;
using System.Xml;
using Xceed.Wpf.DataGrid;
using DataRow = System.Data.DataRow;

namespace RCCDTool
{
    public class Model : IModel
    {
        private static DataTable factorSet;
        private static DataSet _researchDesignOutput;
        private static ObservableCollection<string> _designTypes = new ObservableCollection<string> {"Within Subjects Factor", "Between Subjects Factor"};
        
        private IEnumerable<IEnumerable<object>> conditionsCombined;
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public ObservableCollection<string> DesignTypes
        {
            get { return _designTypes; }
            set { _designTypes = value; }
        } 

        public Model()
        {
            Tables = new List<string>();
            Type entityType = typeof(ResearchFactor);
            factorSet = new DataTable(entityType.Name);
            ResearchFactors = new List<ResearchFactor>();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                if(prop.ComponentType.Name == "ResearchFactor")
                    factorSet.Columns.Add(prop.Name, prop.PropertyType);
            }

            //set primary key. Not sure how to use this, but hopefully I can figure this out.
            DataColumn[] column = new DataColumn[1];
            column[0] = factorSet.Columns["Name"];
            factorSet.PrimaryKey = column;

            //DataRow rowTest = factorSet.NewRow();
            //rowTest["Name"] = "test row";
            //rowTest["Labels"] = new ObservableCollection<string>() {"one", "two", "three"};
            //rowTest["isRandomized"] = true;
            //rowTest["isWithinSubjects"] = false;
            //rowTest["Levels"] = 3;

            //factorSet.Rows.Add(rowTest);

        }

        #region Properties

        public List<string> Tables { get; set; }
        public int NumSubjects { get; set; }
        public List<ResearchFactor> ResearchFactors { get; set; }

        public DataTable FactorSet => factorSet;

        public bool HasData => factorSet != null && factorSet.Rows.Count > 0;

        public int NumFactors => factorSet?.Rows.Count ?? 0;

        public DataSet ResearchDesignOutput
        {
            get { return _researchDesignOutput; }
            set { _researchDesignOutput = value; }
            
        }

        #endregion

        #region factorset methods

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
           
        }

        public void removeFactor(ResearchFactor newFactor)
        {
            
        }

        public void ClearFactors()
        {
            factorSet.Clear();
        }

        public void SaveFactorSet(string FilePath)
        {
            XmlTextWriter writer = new XmlTextWriter(FilePath, Encoding.Default);

            FactorSet.WriteXml(writer);

            writer.Close();
        }

        public void LoadFactorSet(string FilePath)
        {
            //TODO: Implement error handling in the event a garbage file is selected.
            if (HasData)
            {
                FactorSet.Clear();
                ResearchFactors.Clear();
                
            }

            XmlTextReader reader = new XmlTextReader(FilePath);

            FactorSet.ReadXml(reader);

            reader.Close();

            foreach (var row in FactorSet.AsEnumerable())
            {
                ResearchFactor factor = new ResearchFactor
                {
                    Name = row.Field<string>("Name"),
                    Labels = row.Field<ObservableCollection<string>>("Labels"),
                    Levels = row.Field<int>("Levels"),
                    IsWithinSubjects = row.Field<bool>("IsWithinSubjects"),
                    IsRandomized = row.Field<bool>("IsRandomized")
                };

                ResearchFactors.Add(factor);
            }
        
        }

        #endregion

        #region Research Design generation methods

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

        private DataTable FillTable(DataTable inputTable, int numSubjects)
        {
            //add blank rows for processing for the total number of participants
            for (int h = 0; h < numSubjects; h++)
            {
                DataRow newRow = inputTable.NewRow();
                inputTable.Rows.Add(newRow);
            }

            // Create a queue of participant numbers to randomly select from
            List<int> participants = Enumerable.Range(0, numSubjects).ToList();
            
            //generate all possible conditions for each within subject factor
            GenerateAllPossibleConditions();

            //Now assign the conditions for the number of participants to the specified table
            AssignConditions(participants, inputTable);

            return inputTable;

        }

        private void AssignConditions(List<int> participants, DataTable inputTable)
        {
            //this represents all conditions that have been assigned across participants. It is cleared once all have been assigned.
            List<int> usedIndexes = new List<int>();
 
            try
            {
                //Randomly select a pariticpant, then randomly assign a condition to them
                while (participants.Count > 0)
                {
                    int nextParticipantIndex = GetRand(participants.Count);
                    int nextParticipant = participants[nextParticipantIndex];
                    var tempComboList = conditionsCombined.ToList();
                    int j = 0; //this ties our used indexes to our combolist

                    participants.RemoveAt(nextParticipantIndex);
                    
                    //get a random number to represent the next condition to assign
                    int nextConditionIndex = GetRand(conditionsCombined.Count());

                    //if this condition has already been used, generate a new condition
                    while (usedIndexes.Any(nextNum => nextConditionIndex == nextNum))
                    {
                        //refresh the conditionset if we run out
                        if (usedIndexes.Count >= conditionsCombined.Count())
                            usedIndexes.Clear();

                        nextConditionIndex = GetRand(conditionsCombined.Count());
                    }
                    
                    //add the condition to the list of used conditions; we don't want to use this one again unless all condtions are used
                    usedIndexes.Add(nextConditionIndex);

                    //this represents the specific ordering of conditions for one participant
                    var conditionSet = tempComboList[nextConditionIndex].ToList();
                    
                    //assign the condition to the participant
                    foreach (List<string> t in conditionSet)
                    {
                        for (int i = 0; i < t.Count; i++)
                        {
                            string condition = t[i];
                            inputTable.Rows[nextParticipant][condition] = i;
                        }    
                    }
                    
                }
            }
            catch (Exception e) //TODO: handle actual possible exceptions.
            {
                MessageBox.Show(e.Message);
            }

        }

        private void GenerateAllPossibleConditions()
        {
            //generate all possible conditions for each within subject factor
            List<List<List<string>>> conditions = new List<List<List<string>>>();

            try
            {
                for (int i = 0; i < FactorSet.Rows.Count; i++)
                {
                    if ((bool)FactorSet.Rows[i]["isWithinSubjects"] || (bool)FactorSet.Rows[i]["IsRandomized"])
                    {
                        List<string> labels = (List<string>)FactorSet.Rows[i]["Labels"];

                        var comboLists = GetPermutations(labels, labels.Count).Select(c => c.ToList()).ToList();
                        conditions.Add(comboLists);
                    }
                }

                conditionsCombined = CartesianProduct(conditions);

            }
            catch (Exception e) //TODO: handle actual possible exceptions.
            {
                MessageBox.Show(e.Message);
            }

        }

        private List<string> CreateTableSchema()
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

        private List<string> recursiveGenerateTables(Queue<List<string>> q)
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


        #endregion

        #region General helper methods

        private int GetRand(int max)
        {
            byte[] randomNum = new byte[1];
            rngCsp.GetBytes(randomNum);
            int retVal = randomNum[0] % max;
            return retVal;

        }

        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        //Thanks to Ian Griffiths for this fantastic solution for cartesian products: http://www.interact-sw.co.uk/iangblog/2010/08/01/linq-cartesian-3 
        public static IEnumerable<IEnumerable<object>> CartesianProduct(IEnumerable<IEnumerable<object>> inputs)
        {
            return inputs.Aggregate(
                (IEnumerable<IEnumerable<object>>)new object[][] { new object[0] },
                (soFar, input) =>
                    from prevProductItem in soFar
                    from item in input
                    select prevProductItem.Concat(new object[] { item }));
        }

        #endregion


    }
}
