using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCDTool
{
    public class Controller : IController
    {
        IModel _model;
        MainWindow _mw;
       
        
        
        public Controller(IModel model)
        {
            this._model = model;
            
            this._mw = new MainWindow("Within and Between Subjects", this._model, this);
            model.Subscribe(_mw);
            this._mw.Show();
            //System.Windows.Threading.Dispatcher.Run();
        }

        public void addFactor(ResearchFactor factor)
        {
            this._model.addFactor(factor);
        }

        public void removeFactor(ResearchFactor factor)
        {
            this._model.removeFactor(factor);
        }
    }
}
