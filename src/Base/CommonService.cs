using System;

namespace GFramework
{
    /// <summary>
    /// 提供普通的框架内服务，调用者通过服务ID来调用并获取结果
    /// </summary>
    public class CommonService : IService
    {
        /// <summary>
        /// 暴露给公众的方法
        /// </summary>
        public Delegate Call { get; }

        /// <summary>
        /// 生成普通的框架内服务
        /// </summary>
        /// <example>示例
        /// <code>
        /// var cs = new CommonService(new Func<int, float>(method_param_int_return_float));
        /// AppFacade.Instance.RegisterService(serviceID, cs);
        /// </code>
        /// </example>
        /// <param name="delegate">执行服务的具体方法，不可以直接传入方法名，而是需要实例化一个相同声明格式的委托</param>
        public CommonService(Delegate @delegate)
        {
            Call = @delegate;
        }
    }
}
