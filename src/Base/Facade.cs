using System;
using System.Collections.Generic;

namespace GFramework
{
    /// <summary>
    /// 项目中只存在一个Facade派生类，单例访问
    /// 框架门户，提供向具体消息派发者注册、解除、调用消息和命令的方法，以及框架内部服务的注册和调用的方法
    /// </summary>
    public abstract class Facade
    {
        /// <summary>
        /// 全局消息派发者
        /// </summary>
        protected IDispatcher dispatcher = GlobalDispatcher.Instance;
        /// <summary>
        /// 全局框架内部服务管理器
        /// </summary>
        protected GlobalServices services = GlobalServices.Instance;

        /// <summary>
        /// 绑定具体的命令和消息，并由全局消息派发者来管理
        /// </summary>
        /// <param name="command">被绑定的命令类型</param>
        /// <param name="messageID">被绑定的消息ID</param>
        public virtual void BindingCommand(Type command, int messageID)
        {
            dispatcher.BindingCommand(command, messageID);
        }

        /// <summary>
        /// 使全局消息派发者解绑ID为当前参数的消息
        /// </summary>
        /// <param name="messageID">被解绑的消息ID</param>
        public void UnBindingCommand(int messageID)
        {
            dispatcher.UnBindingCommand(messageID);
        }

        /// <summary>
        /// 向消息派发者发送消息，默认为全局消息派发者 GlobalDispatcher.Instance
        /// </summary>
        /// <param name="messageID">消息ID，通常是 DefinedID 中的某一个常量</param>
        /// <param name="sender">消息发送者</param>
        /// <param name="content">消息内容，在监听者收到消息后，会将其解包并执行具体操作</param>
        /// <param name="dispatcher">接收消息的消息派发者，默认为 GlobalDispatcher.Instance</param>
        public void SendMessage(int messageID, object sender = null, object content = null, IDispatcher dispatcher = null)
        {
            if (dispatcher == null)
            {
                this.dispatcher.Receive(new Message(messageID, sender, content));
            }
            else
            {
                dispatcher.Receive(new Message(messageID, sender, content));
            }
        }

        /// <summary>
        /// 注册一个服务
        /// </summary>
        /// <param name="id">服务ID</param>
        /// <param name="service">服务实例</param>
        public void RegisterService(int id, IService service) { services.RegisterService(id, service); }
        /// <summary>
        /// 解除一个服务
        /// </summary>
        /// <param name="id">服务ID</param>
        public void UnRegisterService(int id) { services.UnRegisterService(id); }
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
        public R CallService<T1, T2, T3, T4, R>(int id, T1 arg1, T2 arg2, T3 arg3, T4 arg4) { return services.CallService<T1, T2, T3, T4, R>(id, arg1, arg2, arg3, arg4); }
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
        public R CallService<T1, T2, T3,     R>(int id, T1 arg1, T2 arg2, T3 arg3         ) { return services.CallService<T1, T2, T3,     R>(id, arg1, arg2, arg3      ); }
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
        public R CallService<T1, T2,         R>(int id, T1 arg1, T2 arg2                  ) { return services.CallService<T1, T2,         R>(id, arg1, arg2            ); }
        /// <summary>
        /// 调用一个内部服务
        /// </summary>
        /// <typeparam name="T1">第一个参数类型</typeparam>
        /// <typeparam name="R">结果类型</typeparam>
        /// <param name="id">服务 ID</param>
        /// <param name="arg1">参数1</param>
        /// <returns>服务返回值</returns>
        public R CallService<T1,             R>(int id, T1 arg1                           ) { return services.CallService<T1,             R>(id, arg1                  ); }
        /// <summary>
        /// 调用一个内部服务
        /// </summary>
        /// <typeparam name="R">结果类型</typeparam>
        /// <param name="id">服务 ID</param>
        /// <returns>服务返回值</returns>
        public R CallService<                R>(int id                                    ) { return services.CallService<                R>(id                        ); }
        /// <summary>
        /// 直接返回内部服务所包装的方法，调用者可以缓存后重复调用，避免多次查找
        /// </summary>
        /// <param name="id">服务 ID</param>
        /// <returns>服务内包装的方法</returns>
        public Delegate GetServiceCall         (int id                                    ) { return services.GetServiceCall                (id                        ); }
        // public T CallService<T>(int id, object sender, params object[] args) { return services.CallService<T>(id, sender, args); }

        /// <summary>
        /// 通过ID获取模块(消息派发者)
        /// </summary>
        /// <typeparam name="T">模块(消息派发者)类型</typeparam>
        /// <param name="moduleID">模块(消息派发者)ID</param>
        public abstract T GetModule<T>(int moduleID) where T : Module;
    }
}
