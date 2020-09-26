using System;
using System.Collections.Generic;

namespace GFramework
{
    public partial class GlobalServices
    {
        public static GlobalServices Instance
        {
            get
            {
                lock(_syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new GlobalServices();
                    }
                }
                return _instance;
            }
        }
        private static GlobalServices _instance;

        private static object _syncLock = new object();

        private Dictionary<int, IService> _services;

        private GlobalServices() 
        {
            _services = new Dictionary<int, IService>();
        }

        public void RegisterService(int id, IService service)
        {
            lock (_syncLock)
            {
                if (_services.ContainsKey(id)) { _services[id] = service; }
                else { _services.Add(id, service); }
            }
        }
        public void UnRegisterService(int id)
        {
            lock (_syncLock)
            {
                if (_services.ContainsKey(id)) { _services.Remove(id); }
            }
        }

        public R CallService<T1, T2, T3, T4, R>(int id, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            R result = default;
            if (_services.TryGetValue(id, out IService service))
            {
                result = ((Func<T1, T2, T3, T4, R>)service.Call)(arg1, arg2, arg3, arg4);
            }
            return result;
        }
        public R CallService<T1, T2, T3, R>(int id, T1 arg1, T2 arg2, T3 arg3)
        {
            R result = default;
            if (_services.TryGetValue(id, out IService service))
            {
                result = ((Func<T1, T2, T3, R>)service.Call)(arg1, arg2, arg3);
            }
            return result;
        }
        public R CallService<T1, T2, R>(int id, T1 arg1, T2 arg2)
        {
            R result = default;
            if (_services.TryGetValue(id, out IService service))
            {
                result = ((Func<T1, T2, R>)service.Call)(arg1, arg2);
            }
            return result;
        }
        public R CallService<T1, R>(int id, T1 arg1)
        {
            R result = default;
            if (_services.TryGetValue(id, out IService service))
            {
                result = ((Func<T1, R>)service.Call)(arg1);
            }
            return result;
        }
        public R CallService<R>(int id)
        {
            R result = default;
            if (_services.TryGetValue(id, out IService service))
            {
                result = ((Func<R>)service.Call)();
            }
            return result;
        }

        // public T CallService<T>(int id, object sender, object[] args)
        // {
        //     T result = default;
        //     if(_services.TryGetValue(id, out IService service))
        //     {
        //         result = service.Call<T>(sender, args);
        //     }
        //     return result;
        // }
    }
}

