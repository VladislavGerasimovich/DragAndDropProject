using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class EntryFactory : MonoBehaviour
    {
        public static EventTrigger.Entry Create(
            EventTriggerType triggerType,
            UnityAction<BaseEventData> listener)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = triggerType
            };

            entry.callback.AddListener(listener);

            return entry;
        }
    }
}