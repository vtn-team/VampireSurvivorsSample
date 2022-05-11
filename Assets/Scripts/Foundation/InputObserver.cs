using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InputDataObservable = IObservable<InputObserver.InputData>;
using InputDataObserver = IObserver<InputObserver.InputData>;

public class InputObserver : InputDataObservable
{ 
    public struct InputData
    {
        public string Label;
    }

    List<InputDataObserver> Subscriber = new List<InputDataObserver>();

    public void AddObserver(InputDataObserver target)
    {
        Subscriber.Add(target);
    }

    public void DeleteObserver(InputDataObserver target)
    {
        Subscriber.Remove(target);
    }

    public void NotifyObserver(InputData data)
    {
        Subscriber.ForEach(s => s.NotifyUpdate(data));
    }
}

