using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace RCCDTool
{
    class Model : IModel
    {
        ObservableCollection<ResearchFactor> ocr;
        List<IObserver<ResearchFactor>> observers;
        //private List<ResearchFactor> _factors;

        public Model()
        {
            observers = new List<IObserver<ResearchFactor>>();
            //this._factors = new List<ResearchFactor>();
            ocr = new ObservableCollection<ResearchFactor>();
            ocr.CollectionChanged += notifyObservers;
            
        }

        public IDisposable Subscribe(IObserver<ResearchFactor> observer)
        {
            observers.Add(observer);
            return null;
        }

        public IDisposable Unsubscribe(IObserver<ResearchFactor> observer)
        {
            observers.Remove(observer);
            return null;
        }

        public void addFactor(ResearchFactor newFactor)
        {
            ocr.Add(newFactor);
            
            
        }
        public void removeFactor(ResearchFactor newFactor)
        {
            ocr.Remove(newFactor);
        }

        public void notifyObservers(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var o in observers)
                {
                    foreach (var item in e.OldItems)
                    {
                        o.OnNext((ResearchFactor)item);
                    }
                }
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
                foreach (var o in observers)
                {
                    foreach (var item in e.NewItems)
                    {
                        o.OnNext((ResearchFactor)item);
                    }
                }
            }


            //foreach (var item in e.NewItems)
            //{
                
            //}
        }

        public bool HasData => ocr != null && ocr.Count > 0;

        public int NumFactors => ocr == null ? 0 : ocr.Count; //wow, how compact!!

        public ObservableCollection<ResearchFactor> ResearchFactors => ocr;
        public void ClearFactors()
        {
            while(ocr.Count >0)
                ocr.Remove(ocr[0]);

            //ocr.Clear();
        }
    }
}
