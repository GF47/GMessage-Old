using System;

namespace GFramework
{
    /// <summary>
    /// 没事少用
    /// </summary>
    public interface IService
    {
        // T Call<T>(object caller, params object[] args);
        Delegate Call { get; }
    }
}
