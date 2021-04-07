using System;
using System.Collections.Generic;

namespace GMessage
{
    /// <summary>
    /// 全局框架内部服务管理器，某个想提供服务（具体方法）的实例可以向管理器注册，调用者通过服务 ID 并传入参数，来获取结果
    /// </summary>
    public partial class GlobalServices
    {
        /// <summary>
        /// 全局框架内服务管理器的单例
        /// </summary>
        public static GlobalServices Instance => _instance ?? (_instance = new GlobalServices());
        private static GlobalServices _instance;

        private Dictionary<int, IService> _services;

        private GlobalServices() 
        {
            _services = new Dictionary<int, IService>();
        }

        /// <summary>
        /// 注册一个服务
        /// </summary>
        /// <param name="id">服务 ID</param>
        /// <param name="service">服务实例</param>
        public void RegisterService(int id, IService service)
        {
            if (_services.ContainsKey(id)) { _services[id] = service; }
            else { _services.Add(id, service); }
        }

        /// <summary>
        /// 解除一个服务
        /// </summary>
        /// <param name="id">服务 ID</param>
        public void UnRegisterService(int id)
        {
            if (_services.ContainsKey(id)) { _services.Remove(id); }
        }

        /// <summary>
        /// 调用一个内部服务
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="T2">第二个参数类型</typeparam>
        /// <typeparam name="T3">第三个参数类型</typeparam>
        /// <typeparam name="T4">第四个参数类型</typeparam>
        /// <typeparam name="R">结果类型</typeparam>
        /// <param name="id">服务 ID</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="arg4">参数4</param>
        /// <returns>服务返回值</returns>
        public R CallService<T1, T2, T3, T4, R>(int id, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            R result = default;
            if (_services.TryGetValue(id, out IService service))
            {
                result = ((Func<T1, T2, T3, T4, R>)service.Call)(arg1, arg2, arg3, arg4);
            }
            return result;
        }

        /// <summary>
        /// 调用一个内部服务
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="T2">第二个参数类型</typeparam>
        /// <typeparam name="T3">第三个参数类型</typeparam>
        /// <typeparam name="R">结果类型</typeparam>
        /// <param name="id">服务 ID</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <returns>服务返回值</returns>
        public R CallService<T1, T2, T3, R>(int id, T1 arg1, T2 arg2, T3 arg3)
        {
            R result = default;
            if (_services.TryGetValue(id, out IService service))
            {
                result = ((Func<T1, T2, T3, R>)service.Call)(arg1, arg2, arg3);
            }
            return result;
        }

        /// <summary>
        /// 调用一个内部服务
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="T2">第二个参数类型</typeparam>
        /// <typeparam name="R">结果类型</typeparam>
        /// <param name="id">服务 ID</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <returns>服务返回值</returns>
        public R CallService<T1, T2, R>(int id, T1 arg1, T2 arg2)
        {
            R result = default;
            if (_services.TryGetValue(id, out IService service))
            {
                result = ((Func<T1, T2, R>)service.Call)(arg1, arg2);
            }
            return result;
        }

        /// <summary>
        /// 调用一个内部服务
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="R">结果类型</typeparam>
        /// <param name="id">服务 ID</param>
        /// <param name="arg1">参数1</param>
        /// <returns>服务返回值</returns>
        public R CallService<T1, R>(int id, T1 arg1)
        {
            R result = default;
            if (_services.TryGetValue(id, out IService service))
            {
                result = ((Func<T1, R>)service.Call)(arg1);
            }
            return result;
        }

        /// <summary>
        /// 调用一个内部服务
        /// </summary>
        /// <typeparam name="R">结果类型</typeparam>
        /// <param name="id">服务 ID</param>
        /// <returns>服务返回值</returns>
        public R CallService<R>(int id)
        {
            R result = default;
            if (_services.TryGetValue(id, out IService service))
            {
                result = ((Func<R>)service.Call)();
            }
            return result;
        }

        /// <summary>
        /// 直接返回内部服务所包装的方法，调用者可以缓存后重复调用，避免多次查找
        /// </summary>
        /// <param name="id">服务 ID</param>
        /// <returns>服务内包装的方法</returns>
        public Delegate GetServiceCall(int id)
        {
            if (_services.TryGetValue(id, out IService service))
            {
                return service.Call;
            }
            return null;
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

