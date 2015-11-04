using System;
using System.Collections.ObjectModel;

namespace RCCDTool
{
    public class Controller : IController
    {
        IModel _model;
        MainWindow _mw;
       
        
        
        public Controller(IModel model)
        {
            _model = model;
            
            _mw = new MainWindow("Within and Between Subjects", _model, this);
            model.Subscribe(_mw);
            _mw.Show();
            //System.Windows.Threading.Dispatcher.Run();
        }

        public void addFactor(ResearchFactor factor)
        {
            bool factorAlreadyExists = false;
            foreach (var researchFactor in _model.ResearchFactors)
            {
                if (factor.Label == researchFactor.Label)
                {
                    factorAlreadyExists = true;
                    break;
                }
                    
            }

            if(!factorAlreadyExists)
                _model.addFactor(factor);
        }

        public void removeFactor(ResearchFactor factor)
        {
            _model.removeFactor(factor);
        }

        public void AddSubscriber(IObserver<ResearchFactor> subscriber)
        {
            _model.Subscribe(subscriber);
        }

        public void RemoveSubscriber(IObserver<ResearchFactor> subscriber)
        {
            _model.Unsubscribe(subscriber);
        }

        public bool ModelHasData => _model.HasData;

        public int NumFactors => _model.NumFactors;
        public ObservableCollection<ResearchFactor> ResearchFactors => _model.ResearchFactors;

        public void ClearFactors()
        {
            _model.ClearFactors();
        }
    }
}
