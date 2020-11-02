using System;
using System.Collections.Generic;

namespace GFramework
{
    /// <summary>
    /// 框架内的全局消息派发者
    /// </summary>
    public sealed class GlobalDispatcher : Module, IDispatcher
    {
        private static readonly object staticSyncLocker = new object(); // 线程锁

        /// <summary>
        /// 全局消息派发者的单例
        /// </summary>
        public static GlobalDispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (staticSyncLocker)
                    {
                        if (_instance == null)
                        {
                            _instance = new GlobalDispatcher();
                        }
                    }
                }
                return _instance;
            }
        }
        private static volatile GlobalDispatcher _instance;

        private GlobalDispatcher()
        {
            commands = new Dictionary<int, Type>();
            listeners = new Dictionary<IListener, List<int>>();
        }
    }
}