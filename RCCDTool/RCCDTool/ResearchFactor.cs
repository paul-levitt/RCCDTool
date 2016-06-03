using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace RCCDTool
{
    public class ResearchFactor : DependencyObject
    {

        static ResearchFactor()
        {
            

        }

        public override string ToString()
        {
            return Name + "; " + Levels + "; isWS: " + IsWithinSubjects + "; Rand: " + IsRandomized;
        }

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(ResearchFactor));
        public static readonly DependencyProperty LevelsProperty = DependencyProperty.Register("Levels", typeof(int), typeof(ResearchFactor));
        public static readonly DependencyProperty IsWithinSubjectsProperty = DependencyProperty.Register("IsWithinSubjects", typeof(bool), typeof(ResearchFactor));
        public static readonly DependencyProperty IsRandomizedProperty = DependencyProperty.Register("IsRandomized", typeof(bool), typeof(ResearchFactor));
        public static readonly DependencyProperty LabelsProperty = DependencyProperty.Register("Labels", typeof(ObservableCollection<string>), typeof(ResearchFactor));

        #region Getters/Setters
        public string Name
        {
            get{ return (string)GetValue(NameProperty); }
            set{ SetValue(NameProperty, value); }
        }
        
        public int Levels
        {
            get { return (int) GetValue(LevelsProperty); }
            set { SetValue(LevelsProperty, value); }
        }

        public bool IsWithinSubjects
        {
            get { return (bool)GetValue(IsWithinSubjectsProperty); }
            set{ SetValue(IsWithinSubjectsProperty, value); }
        }

       
        public bool IsRandomized
        {
            get { return (bool) GetValue(IsRandomizedProperty); }
            set { SetValue(IsRandomizedProperty, value); }
        }

        public ObservableCollection<string> Labels
        {
            get { return (ObservableCollection<string>) GetValue(LabelsProperty); }
            set { SetValue(LabelsProperty, value); }
        }
        #endregion
    }
}
