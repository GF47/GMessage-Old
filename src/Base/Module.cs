using System;
using System.Collections.Generic;

namespace GFramework
{
    public abstract class Module : IDispenser
    {
        protected IDictionary<int, Type> commands;
        protected IDictionary<IListener, List<int>> listeners;

        protected readonly object syncLocker = new object();

        public bool Listening(int ID)
        {
            lock (syncLocker)
            {
                return commands.ContainsKey(ID);
            }
        }

        public void Receive(IMessage message)
        {
            Type expectedCommand = null;
            List<IListener> expectedListeners = null;

            lock (syncLocker)
            {
                if (commands.ContainsKey(message.ID)) { expectedCommand = commands[message.ID]; }
                else
                {
                    expectedListeners = new List<IListener>();
                    foreach (var pair in listeners)
                    {
                        if (pair.Value.Contains(message.ID))
                        {
                            expectedListeners.Add(pair.Key);
                        }
                    }
                }
            }

            if (expectedCommand != null)
            {
                if (Activator.CreateInstance(expectedCommand) is ICommand cmd)
                {
                    cmd.Execute(message);
                }
            }

            if (expectedListeners != null && expectedListeners.Count > 0)
            {
                for (int i = 0; i < expectedListeners.Count; i++)
                {
                    expectedListeners[i].Receive(message);
                }
            }
        }

        public void BindingCommand(Type command, int messageID)
        {
            lock (syncLocker)
            {
                if (commands.ContainsKey(messageID)) { commands[messageID] = command; }
                else { commands.Add(messageID, command); }
            }
        }

        public void RegisterMessage(IListener listener, IList<int> messageID)
        {
            if (listeners.ContainsKey(listener))
            {
                if (listeners.TryGetValue(listener, out List<int> list))
                {
                    for (int i = 0; i < messageID.Count; i++)
                    {
                        if (list.Contains(messageID[i])) continue;
                        list.Add(messageID[i]);
                    }
                }
            }
            else
            {
                listeners.Add(listener, new List<int>(messageID));
            }
        }

        public void UnBindingCommand(int messageID)
        {
            lock (syncLocker)
            {
                if (commands.ContainsKey(messageID)) { commands.Remove(messageID); }
            }
        }

        public void UnRegisterMessage(IListener listener, IList<int> messageID)
        {
            lock (syncLocker)
            {
                if (listeners.ContainsKey(listener))
                {
                    if (listeners.TryGetValue(listener, out List<int> list))
                    {
                        for (int i = 0; i < messageID.Count; i++)
                        {
                            if (!list.Contains(messageID[i])) continue;
                            list.Remove(messageID[i]);
                        }
                    }
                }
            }
        }
    }
}
