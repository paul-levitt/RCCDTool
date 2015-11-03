using System;
using System.Collections.Generic;

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
            _factors = new List<ResearchFactor>();
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
            get { return _subjects; }
            set { _subjects = value; }
        }

        public int RepeatedMeasures
        {
            get { return _repeatedMeasures; }
            set { _repeatedMeasures = value; }
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
