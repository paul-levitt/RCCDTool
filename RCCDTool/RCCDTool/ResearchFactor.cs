using System.Collections.Generic;

namespace RCCDTool
{
    public class ResearchFactor
    {
        private string _name;
        private int _levels;
        private bool _isWithinSubjects;
        private bool _isRandomized;

        public override string ToString()
        {
            return _name + "; " + _levels + "; isWS: " + _isWithinSubjects + "; Rand: " + _isRandomized;
        }
        #region Getters/Setters
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
        
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

        public List<string> Labels { get; set; }
        #endregion
    }
}
