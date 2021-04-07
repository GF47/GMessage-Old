namespace GMessage
{
    /// <summary>
    /// 自定义的命令，绑定具体消息后，会执行 Execute(IMessage message) 方法
    /// <example>示例
    /// <code>
    /// AppFacade.Instance.BindingCommand(typeof(CommandImpl), messageID)
    /// ModuleImpl.Instance.BindingCommand(typeof(CommandImpl), messageID)
    /// </code>
    /// </example>
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 接收消息后执行
        /// </summary>
        /// <param name="message">消息体，通常会将 message.Content 解包并执行具体操作</param>
        void Execute(IMessage message);
    }
}