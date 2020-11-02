using System;

namespace GFramework
{
    /// <summary>
    /// 框架内服务接口，注册给 Facade，调用者向 Facade 传入服务的ID和具体参数来获取处理后的结果，没事少用
    /// </summary>
    public interface IService
    {
        // T Call<T>(object caller, params object[] args);

        /// <summary>
        /// 暴露给公众的方法，最多支持4个输入参数和1个返回值，多了自行想办法
        /// </summary>
        Delegate Call { get; }
    }
}
