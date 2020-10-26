using System.Collections.Generic;

namespace GFramework
{
    public abstract class Listener<T> :
        // TODO 修改为具体实体类
        // UnityEngine.MonoBehaviour,
        IListener
        where T : IDispenser
    {
        /// <summary>
        /// switch来处理消息类型
        /// </summary>
        public abstract void Receive(IMessage message);

        /// <summary>
        /// Module的单例 或者 GlobalDispenser的单例
        /// </summary>
        protected abstract T GetDispenser();

        public void RegisterMessage(IListener listener, IList<int> messageID)
        {
            if (messageID == null || messageID.Count < 1) { return; }
            GetDispenser().RegisterMessage(listener, messageID);
        }

        public void RegisterMessage(IListener listener, params int[] messageID)
        {
            if (messageID == null || messageID.Length < 1) { return; }
            GetDispenser().RegisterMessage(listener, messageID);
        }

        public void UnRegisterMessage(IListener listener, IList<int> messageID)
        {
            if (messageID == null || messageID.Count < 1) { return; }
            GetDispenser().UnRegisterMessage(listener, messageID);
        }

        public void UnRegisterMessage(IListener listener, params int[] messageID)
        {
            if (messageID == null || messageID.Length < 1) { return; }
            GetDispenser().UnRegisterMessage(listener, messageID);
        }
    }
}