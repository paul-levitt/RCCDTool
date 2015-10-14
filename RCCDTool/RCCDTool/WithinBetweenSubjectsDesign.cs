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
        private List<Factor> _factors;

        public WithinBetweenSubjectsDesign()
        {
            //int subjects, int groups, int repeatedMeasures, int factors
            //this._subjects = subjects;
            //this._repeatedMeasures = repeatedMeasures;
            //this._groups = groups;
            this._factors = new List<Factor>();
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

        public List<Factor> Factors
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

    struct Factor
    {
        private string _label;
        private int _levels;
        private bool _isWithinSubjects;
        private bool _isRandomized;

        public override string ToString()
        {
            return  _label + "; " + _levels.ToString() + "; isWS: " + _isWithinSubjects.ToString() + "; Rand: " + _isRandomized.ToString();
        }
        #region Getters/Setters
        public int Levels
        {
            get
            {
                return _levels;
            }

            set
            {
                _levels = value;
            }
        }

        public bool isWithinSubjects
        {
            get
            {
                return _isWithinSubjects;
            }

            set
            {
                _isWithinSubjects = value;
            }
        }

        public string Label
        {
            get
            {
                return _label;
            }

            set
            {
                _label = value;
            }
        }

        public bool IsRandomized
        {
            get
            {
                return _isRandomized;
            }

            set
            {
                _isRandomized = value;
            }
        } 
        #endregion
    }
}
