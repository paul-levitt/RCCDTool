using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RCCDTool
{
    /// <summary>
    /// Interaction logic for DesignDisplay.xaml
    /// </summary>
    public partial class DesignDisplay : Window
    {
        private int NumSubjects;
        private IController _controller;

        public DesignDisplay()
        {
            InitializeComponent();
        }
        public DesignDisplay(IController controller)
        {
            InitializeComponent();
            this._controller = controller;
            //NumSubjects = numSubjects;
            updateDesign();
        }

        public void updateDesign()
        {
            //this.testLabel.Content = NumSubjects.ToString();
            //this._controller.GetDesign(numSubjects); //or something like that
            
        }
    }
}
