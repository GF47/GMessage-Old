using System;
using System.Collections.Generic;

namespace GFramework
{
    /// <summary>
    /// 项目中只存在一个Facade派生类，单例访问
    /// </summary>
    public abstract class Facade
    {
        protected IDispenser dispenser = GlobalDispenser.Instance;

        public virtual void BindingCommand(Type command, int messageID)
        {
            dispenser.BindingCommand(command, messageID);
        }

        public void BindingVariableCommands(Type command, IList<int> messageID)
        {
            for (int i = 0; i < messageID.Count; i++)
            {
                dispenser.BindingCommand(command, messageID[i]);
            }
        }

        public void BindingVariableCommands(Type command, params int[] messageID)
        {
            for (int i = 0; i < messageID.Length; i++)
            {
                dispenser.BindingCommand(command, messageID[i]);
            }
        }

        public void UnBindingCommand(int messageID)
        {
            dispenser.UnBindingCommand(messageID);
        }
        public void UnBindingVariableCommands(IList<int> messageID)
        {
            for (int i = 0; i < messageID.Count; i++)
            {
                dispenser.UnBindingCommand(messageID[i]);
            }
        }
        public void UnBindingVariableCommands(params int[] messageID)
        {
            for (int i = 0; i < messageID.Length; i++)
            {
                dispenser.UnBindingCommand(messageID[i]);
            }
        }

        public void SendMessage(int messageID, object sender = null, object content = null, IDispenser dispenser = null)
        {
            if (dispenser == null)
            {
                this.dispenser.Receive(new Message(messageID, sender, content));
            }
            else
            {
                dispenser.Receive(new Message(messageID, sender, content));
            }
        }

        public abstract T GetModule<T>(int moduleID) where T : Module;
    }
}
