using System;

namespace GFramework
{
    /// <summary>
    /// 消息派发接口，本身监听到具体消息后，会执行消息所绑定的 ICommand 或者通知下一级消息的监听者 IListener
    /// </summary>
    public interface IDispatcher : IListener
    {
        /// <summary>
        /// 是否在监听ID为当前参数的消息
        /// </summary>
        /// <param name="ID">消息的ID</param>
        bool Listening(int ID);

        /// <summary>
        /// 绑定具体的命令和消息
        /// </summary>
        /// <param name="command">被绑定的命令类型</param>
        /// <param name="messageID">被绑定的消息ID</param>
        void BindingCommand(Type command, int messageID);

        /// <summary>
        /// 解绑ID为当前参数的消息
        /// </summary>
        /// <param name="messageID">被解绑的消息ID</param>
        void UnBindingCommand(int messageID);
    }
}