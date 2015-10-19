using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCDTool
{
    class Model : IModel
    {
        ObservableCollection<ResearchFactor> ocr;
        List<IObserver<ResearchFactor>> observers;
        //private List<ResearchFactor> _factors;

        public Model()
        {
            this.observers = new List<IObserver<ResearchFactor>>();
            //this._factors = new List<ResearchFactor>();
            ocr = new ObservableCollection<ResearchFactor>();
            ocr.CollectionChanged += notifyObservers;
            
        }

        public IDisposable Subscribe(IObserver<ResearchFactor> observer)
        {
            observers.Add(observer);
            return null;
        }

        public void addFactor(ResearchFactor newFactor)
        {
            this.ocr.Add(newFactor);
            
            
        }
        public void removeFactor(ResearchFactor newFactor)
        {
            this.ocr.Remove(newFactor);
        }

        public void notifyObservers(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach(var o in observers)
                {
                    foreach (var item in e.NewItems)
                    {
                        o.OnNext((ResearchFactor)item);
                    }
                }
            }
            else if(e.Action == NotifyCollectionChangedAction.Replace)
            {
                //do something??
            }


            foreach (var item in e.NewItems)
            {
                
            }
        }
    }
}
