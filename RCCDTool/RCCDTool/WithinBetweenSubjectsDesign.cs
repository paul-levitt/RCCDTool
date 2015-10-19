using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCDTool
{
    class WithinBetweenSubjectsDesign : IResearchDesign
    {
        private int _subjects;
        private int _groups;
        private int _repeatedMeasures;
        private List<ResearchFactor> _factors;

        public WithinBetweenSubjectsDesign()
        {
            //int subjects, int groups, int repeatedMeasures, int factors
            //this._subjects = subjects;
            //this._repeatedMeasures = repeatedMeasures;
            //this._groups = groups;
            this._factors = new List<ResearchFactor>();
        }


        public void CounterBalance()
        {
            throw new NotImplementedException();
        }

        public void Randomize()
        {
            throw new NotImplementedException();
        }


        #region Properties
        public int Subjects
        {
            get { return this._subjects; }
            set { this._subjects = value; }
        }

        public int RepeatedMeasures
        {
            get { return this._repeatedMeasures; }
            set { this._repeatedMeasures = value; }
        }

        public List<ResearchFactor> Factors
        {
            get { return _factors; }
            set { _factors = value; }
        }

        public int Groups
        {
            get { return _groups; }
            set { _groups = value; }
        }
        #endregion
    }

    
}
