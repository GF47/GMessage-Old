using System.Collections.Generic;

namespace GMessage
{
    /// <summary>
    /// 抽象的多步命令，会按顺序依次执行所附加的子命令
    /// </summary>
    public abstract class MacroCommand : ICommand
    {
        /// <summary>
        /// 子命令列表，会依次执行
        /// </summary>
        protected IList<ICommand> subCommands;

        /// <summary>
        /// 实例化一个多步命令
        /// </summary>
        public MacroCommand()
        {
            AppendCommands();
        }

        /// <summary>
        /// 在方法中实例化子命令，并放入subCommands
        /// <example>
        /// 示例
        /// <code>
        /// subCommands = new List&lt;ICommand&gt;();
        ///
        /// subCommands.Add(new CommandImpl_1());
        /// subCommands.Add(new CommandImpl_2());
        /// subCommands.Add(new CommandImpl_3());
        /// subCommands.Add(new CommandImpl_4());
        /// </code>
        /// </example>
        /// </summary>
        protected abstract void AppendCommands();

        /// <summary>
        /// 依次执行子命令
        /// </summary>
        /// <param name="message">消息体，通常会将 message.Content 解包并执行具体操作</param>
        public void Execute(IMessage message)
        {
            foreach (var command in subCommands)
            {
                command.Execute(message);
            }
        }
    }
}
