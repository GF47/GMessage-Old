namespace GMessage
{
    /// <summary>
    /// 框架门户实现，提供向具体消息派发者注册、解除、调用消息和命令的方法，以及框架内部服务的注册和调用的方法等
    /// </summary>
    public sealed class AppFacade : Facade
    {
        /// <summary>
        /// 门户单例
        /// </summary>
        public static AppFacade Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppFacade();
                }
                return _instance;
            }
        }
        private static AppFacade _instance;

        private AppFacade()
        {
            __InitModule();
            __BindingCommands();
        }

        /// <summary>
        /// 通过ID获取模块(消息派发者)
        /// </summary>
        /// <typeparam name="T">模块(消息派发者)类型</typeparam>
        /// <param name="moduleID">模块(消息派发者)ID</param>
        public override T GetModule<T>(int moduleID)
        {
            T module = null;
            switch (moduleID)
            {
                case ModuleID.GLOBAL_DISPATCHER:
                    module = GlobalDispatcher.Instance as T;
                    break;
                // HACK 模块如果需要被查找，则在这里指定键值对
                // 或者直接新建一个字典
            }
            return module;
        }

        private void __InitModule()
        {
            // HACK 初始化模块，如果模块类继承自MonoBehaviour，则自行在Awake中初始化
        }

        private void __BindingCommands()
        {
            // HACK 绑定必要的消息
        }
    }
}
