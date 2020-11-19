using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GFramework
{
    public class MessageTrigger : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerClickHandler,
        ISelectHandler,
        IDeselectHandler,
        IUpdateSelectedHandler,
        ISubmitHandler,
        ICancelHandler
    {

        public int moduleID = ModuleID.GLOBAL_DISPATCHER;

        [Serializable]
        public struct MessageData : IEquatable<MessageData>
        {
            public EventTriggerType type;
            public int id;
            public string content;
            public object commonContent;

            public bool Equals(MessageData b)
            {
                return type == b.type && id == b.id && content == b.content && commonContent == b.commonContent;
            }
        }
        public List<MessageData> Triggers => _triggers;
        [SerializeField]
        private List<MessageData> _triggers = new List<MessageData>();

        private Module GetModule()
        {
            return AppFacade.Instance.GetModule<Module>(moduleID);
        }

        #region 响应方法

        private void Execute(EventTriggerType type)
        {
            for (int i = 0; i < _triggers.Count; i++)
            {
                if (_triggers[i].type == type)
                {
                    AppFacade.Instance.SendMessage(_triggers[i].id, this, _triggers[i].commonContent ?? _triggers[i].content, GetModule());
                }
            }
        }

        public void OnCancel(BaseEventData eventData)
        {
            Execute(EventTriggerType.Cancel);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Execute(EventTriggerType.Deselect);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Execute(EventTriggerType.PointerClick);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Execute(EventTriggerType.PointerDown);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Execute(EventTriggerType.PointerEnter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Execute(EventTriggerType.PointerExit);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Execute(EventTriggerType.PointerUp);
        }

        public void OnSelect(BaseEventData eventData)
        {
            Execute(EventTriggerType.Select);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Execute(EventTriggerType.Submit);
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            Execute(EventTriggerType.UpdateSelected);
        }

        #endregion
    }
}
