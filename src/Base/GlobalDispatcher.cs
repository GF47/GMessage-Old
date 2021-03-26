namespace GFramework
{
    /// <summary>
    /// 框架内的全局消息派发者
    /// </summary>
    public sealed class GlobalDispatcher : Module, IDispatcher
    {
        /// <summary>
        /// 全局消息派发者的单例
        /// </summary>
        public static GlobalDispatcher Instance => Instance<GlobalDispatcher>(ModuleID.GLOBAL_DISPATCHER);

        private GlobalDispatcher(int id) : base(id) { }
    }
}