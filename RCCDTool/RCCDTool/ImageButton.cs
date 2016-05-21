using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RCCDTool
{
    class ImageButton : Button
    {
        public ImageSource Source
        {
            get { return base.GetValue(SourceProperty) as ImageSource; }
            set { base.SetValue(SourceProperty, value);}
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source",
            typeof (ImageSource), typeof (ImageButton));
    }
}
