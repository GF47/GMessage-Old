using System;
using System.Collections.Generic;

namespace GFramework
{
    public sealed class GlobalDispatcher : Module, IDispatcher
    {
        private static readonly object staticSyncLocker = new object();

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