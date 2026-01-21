using Doozy.Engine;
using UnityEngine;

namespace TwoCore
{
    public class GameEventTrigger : MonoBehaviour
    {
        public string gameEvent;

        public void Trigger() => DoTrigger(gameEvent);

        public void Trigger(string gameEvent) => DoTrigger(gameEvent);

        private void DoTrigger(string gameEvent)
        {
            if(string.IsNullOrEmpty(gameEvent)) return;
            GameEventMessage.SendEvent(gameEvent, null);
        }
    }
}