using System.Collections.Generic;

namespace GFramework
{
    /// <summary>
    /// 消息监听接口，当监听到注册过的消息后，执行 Receive(IMessage message) 方法
    /// </summary>
    public interface IListener
    {
        /// <summary>
        /// 接收消息并执行
        /// </summary>
        /// <param name="message">消息体</param>
        void Receive(IMessage message);

        /// <summary>
        /// 注册监听者需要监听的消息
        /// </summary>
        /// <param name="listener">消息的监听者，绝大多数情况为实例本身，注意勿产生死循环</param>
        /// <param name="messageID">需要监听的消息列表</param>
        void RegisterMessage(IListener listener, IList<int> messageID);

        /// <summary>
        /// 解除监听者需要监听的消息
        /// </summary>
        /// <param name="listener">消息的监听者，绝大多数情况为实例本身，注意勿产生死循环</param>
        /// <param name="messageID">需要解除监听的消息列表</param>
        void UnRegisterMessage(IListener listener, IList<int> messageID);
    }
}