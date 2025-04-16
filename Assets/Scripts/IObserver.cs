using UnityEngine;

public interface IObserver
{
    void OnNotify(string eventName, object data);
} 