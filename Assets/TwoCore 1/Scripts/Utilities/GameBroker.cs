using System.Collections.Generic;
using System;

namespace TwoCore
{
    public class GameBroker
    {
        private static GameBroker _instance;
        public static GameBroker Ins => _instance ??= new GameBroker();

        private readonly Dictionary<string, Delegate> eventTable = new();

        public void Subscribe(string eventName, Action callback)
        {
            if (!eventTable.ContainsKey(eventName))
                eventTable[eventName] = (Action)(() => { });

            eventTable[eventName] = (Action)eventTable[eventName] + callback;
        }
        public void Subscribe<T>(string eventName, Action<T> callback)
        {
            if (!eventTable.ContainsKey(eventName))
                eventTable[eventName] = (Action<T>)(_ => { });

            eventTable[eventName] = (Action<T>)eventTable[eventName] + callback;
        }

        public void Unsubscribe(string eventName, Action callback)
        {
            if (eventTable.ContainsKey(eventName))
            {
                eventTable[eventName] = (Action)eventTable[eventName] - callback;
                if (eventTable[eventName] == null) eventTable.Remove(eventName);
            }
        }
        public void Unsubscribe<T>(string eventName, Action<T> callback)
        {
            if (eventTable.ContainsKey(eventName))
            {
                eventTable[eventName] = (Action<T>)eventTable[eventName] - callback;
                if (eventTable[eventName] == null) eventTable.Remove(eventName);
            }
        }

        public void Emit(string eventName)
        {
            if (eventTable.ContainsKey(eventName))
                (eventTable[eventName] as Action)?.Invoke();
        }
        public void Emit<T>(string eventName, T arg)
        {
            if (eventTable.ContainsKey(eventName))
                (eventTable[eventName] as Action<T>)?.Invoke(arg);
        }
    }
}