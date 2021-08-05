using System.Collections.Generic;

namespace GMessage
{
    /// <summary>
    /// 抽象的消息监听者，需要子类实现消息接收后处理的方法
    /// </summary>
    /// <typeparam name="T">消息派发者类型，指定之后，本监听者只会接收特定的派发者所传入的消息，而忽略掉其他消息，使结构更模块化并使查询更高效</typeparam>
    public abstract class Listener<T> :
        // TODO 修改为具体实体类
        // UnityEngine.MonoBehaviour,
        IListener
        where T : IDispatcher
    {
        /// <summary>
        /// 接收消息并进行处理，内部通常使用switch来处理消息类型
        /// </summary>
        public abstract void Receive(IMessage message);

        /// <summary>
        /// 获取自身所监听的消息派发者，一般是具体 Module 的单例或者 GlobalDispatcher 的单例，也可以从 Facade 处通过 ID 来查询获得
        /// </summary>
        public abstract T GetDispatcher();

        /// <summary>
        /// 注册自身需要监听的消息
        /// </summary>
        /// <param name="listener">消息的监听者，绝大多数情况为实例本身，请勿指定为 GetDispatcher()</param>
        /// <param name="messageID">需要监听的消息列表</param>
        public void RegisterMessage(IListener listener, IList<int> messageID)
        {
            if (messageID == null || messageID.Count < 1) { return; }
            GetDispatcher().RegisterMessage(listener, messageID);
        }

        /// <summary>
        /// 注册自身需要监听的消息
        /// </summary>
        /// <param name="listener">消息的监听者，绝大多数情况为实例本身，请勿指定为 GetDispatcher()</param>
        /// <param name="messageID">需要监听的消息，可以指定多个</param>
        public void RegisterMessage(IListener listener, params int[] messageID)
        {
            if (messageID == null || messageID.Length < 1) { return; }
            GetDispatcher().RegisterMessage(listener, messageID);
        }

        /// <summary>
        /// 解除自身需要监听的消息
        /// </summary>
        /// <param name="listener">消息的监听者，绝大多数情况为实例本身，请勿指定为 GetDispatcher()</param>
        /// <param name="messageID">需要解除监听的消息列表</param>
        public void UnRegisterMessage(IListener listener, IList<int> messageID)
        {
            if (messageID == null || messageID.Count < 1) { return; }
            GetDispatcher().UnRegisterMessage(listener, messageID);
        }

        /// <summary>
        /// 解除自身需要监听的消息
        /// </summary>
        /// <param name="listener">消息的监听者，绝大多数情况为实例本身，请勿指定为 GetDispatcher()</param>
        /// <param name="messageID">需要解除监听的消息，可以指定多个</param>
        public void UnRegisterMessage(IListener listener, params int[] messageID)
        {
            if (messageID == null || messageID.Length < 1) { return; }
            GetDispatcher().UnRegisterMessage(listener, messageID);
        }
    }
}