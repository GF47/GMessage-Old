using System;
using System.Collections.Generic;

namespace GMessage
{
    /// <summary>
    /// 抽象的消息派发者，通常是一个具体的模块，所以直接命名为 Module，
    /// 监听到消息后，执行所绑定的 ICommand 或者通知下一级的消息监听者 IListener
    /// </summary>
    public abstract class Module : IDispatcher, IDisposable
    {
        #region 单例

        /// <summary>
        /// 通过ID来获取模块单例
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="id">模块ID</param>
        public static T Instance<T>(int id) where T : Module
        {
            return (_instance.TryGetValue(id, out var module) ? module : Activator.CreateInstance(typeof(T), id)) as T;
        }

        private static readonly Dictionary<int, Module> _instance = new Dictionary<int, Module>();

        #endregion 单例

        /// <summary>
        /// 模块ID，不能重复
        /// </summary>
        public int ID { get; protected set; }

        /// <summary>
        /// 绑定的命令字典，接收消息后查找具体命令并执行
        /// </summary>
        protected IDictionary<int, ICommand> commands;

        /// <summary>
        /// 下一级的消息监听者，接收消息后通知已经注册过此消息的监听者
        /// </summary>
        protected IDictionary<IListener, List<int>> listeners;

        private List<IListener> _expectedListeners;

        protected readonly object syncLocker = new object(); // 线程锁

        /// <summary>
        /// 默认的构造函数，已经实例化了命令字典和监听者字典，子类的构造函数如果继承自默认构造函数，则无需再次实例化。
        /// </summary>
        protected Module(int id)
        {
            ID = id;
            _instance.Add(id, this);

            commands = new Dictionary<int, ICommand>();
            listeners = new Dictionary<IListener, List<int>>();

            _expectedListeners = new List<IListener>();
        }

        ~Module()
        {
            commands = null;
            listeners = null;
            _expectedListeners = null;
            Dispose();
        }

        public void Dispose()
        {
            _instance.Remove(ID);
        }

        /// <summary>
        /// 是否在监听ID为当前参数的消息
        /// </summary>
        /// <param name="ID">消息的ID</param>
        /// <returns></returns>
        public bool Listening(int ID)
        {
            lock (syncLocker)
            {
                return commands.ContainsKey(ID);
            }
        }

        /// <summary>
        /// 接收消息，执行相应命令，并将消息通知给下一级的监听者
        /// </summary>
        /// <param name="message">消息体</param>
        public virtual void Receive(IMessage message)
        {
            ICommand command = null;
            lock (syncLocker)
            {
                if (commands.ContainsKey(message.ID)) { command = commands[message.ID]; }

                foreach (var pair in listeners)
                {
                    if (pair.Value.Contains(message.ID))
                    {
                        _expectedListeners.Add(pair.Key);
                    }
                }
            }

            command?.Execute(message);

            for (int i = 0; i < _expectedListeners.Count; i++)
            {
                _expectedListeners[i].Receive(message);
            }
            _expectedListeners.Clear();
        }

        /// <summary>
        /// 绑定具体的命令和消息
        /// </summary>
        /// <param name="type">被绑定的命令类型</param>
        /// <param name="messageID">被绑定的消息ID</param>
        public void BindingCommand(Type type, int messageID)
        {
            lock (syncLocker)
            {
                var command = Activator.CreateInstance(type) as ICommand;
                if (commands.ContainsKey(messageID)) { commands[messageID] = command; }
                else { commands.Add(messageID, command); }
            }
        }

        /// <summary>
        /// 注册监听者和需要监听的消息
        /// </summary>
        /// <param name="listener">消息的监听者，绝大多数为 IListener，当指定为 IDispatcher 时，切勿产生循环调用</param>
        /// <param name="messageID">需要监听的消息列表</param>
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

        /// <summary>
        /// 解绑ID为当前参数的消息
        /// </summary>
        /// <param name="messageID">被解绑的消息ID</param>
        public void UnBindingCommand(int messageID)
        {
            lock (syncLocker)
            {
                if (commands.ContainsKey(messageID)) { commands.Remove(messageID); }
            }
        }

        /// <summary>
        /// 解除监听者和需要监听的消息
        /// </summary>
        /// <param name="listener">消息的监听者，绝大多数为 IListener，当指定为 IDispatcher 时，切勿产生循环调用</param>
        /// <param name="messageID">需要解除监听的消息列表</param>
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
