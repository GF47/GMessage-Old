using System.Collections.Generic;

namespace GFramework
{
    public abstract class MacroCommand : ICommand
    {
        protected IList<ICommand> subCommands;

        public MacroCommand()
        {
            AppendCommands();
        }

        /// <summary>
        /// 在方法中实例化子命令，并放入subCommands
        /// </summary>
        protected abstract void AppendCommands();

        public void Execute(IMessage message)
        {
            foreach (var command in subCommands)
            {
                command.Execute(message);
            }
        }
    }
}