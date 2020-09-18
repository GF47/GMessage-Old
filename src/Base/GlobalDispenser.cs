using System;
using System.Collections.Generic;

namespace GFramework
{
    public sealed class GlobalDispenser : Module, IDispenser
    {
        private static readonly object staticSyncLocker = new object();

        public static GlobalDispenser Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (staticSyncLocker)
                    {
                        if (_instance == null)
                        {
                            _instance = new GlobalDispenser();
                        }
                    }
                }
                return _instance;
            }
        }
        private static volatile GlobalDispenser _instance;

        private GlobalDispenser()
        {
            commands = new Dictionary<int, Type>();
            listeners = new Dictionary<IListener, List<int>>();
        }
    }
}