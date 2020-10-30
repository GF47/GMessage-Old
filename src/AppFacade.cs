namespace GFramework
{
    public sealed class AppFacade : Facade
    {
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

        public override T GetModule<T>(int moduleID)
        {
            T module = null;
            switch (moduleID)
            {
                case ModuleID.GLOBAL_DISPATCHER:
                    module = GlobalDispatcher.Instance as T;
                    break;
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
