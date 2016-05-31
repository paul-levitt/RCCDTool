using System.Collections.Generic;
using System.Windows;

namespace RCCDTool
{
    public class ResearchFactor : DependencyObject
    {
        private string _name;
        private int _levels;
        private bool _isWithinSubjects;
        private bool _isRandomized;

        static ResearchFactor()
        {
            //FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata(new string(),
            //    FrameworkPropertyMetadataOptions.None);

        }

        public override string ToString()
        {
            return _name + "; " + _levels + "; isWS: " + _isWithinSubjects + "; Rand: " + _isRandomized;
        }

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(ResearchFactor));
        public static readonly DependencyProperty LevelsProperty = DependencyProperty.Register("Levels", typeof(int), typeof(ResearchFactor));
        public static readonly DependencyProperty IsWithinSubjectsProperty = DependencyProperty.Register("IsWithinSubjects", typeof(bool), typeof(ResearchFactor));
        public static readonly DependencyProperty IsRandomizedProperty = DependencyProperty.Register("IsRandomized", typeof(bool), typeof(ResearchFactor));

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

        public bool IsWithinSubjects
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
