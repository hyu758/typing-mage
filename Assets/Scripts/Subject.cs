using UnityEngine;
using System.Collections.Generic;

public abstract class Subject : MonoBehaviour
{
    protected List<IObserver> observers = new List<IObserver>();

    public virtual void AddObserver(IObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public virtual void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    protected virtual void NotifyObservers(string eventName, object data)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(eventName, data);
        }
    }
}