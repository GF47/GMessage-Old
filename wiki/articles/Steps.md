# 使用步骤
---


## 1.导入
将 *GMessage/src* 内的文件全部拷贝至Unity工程中的 *Assets* 文件夹内。


## 2.定义消息(建议使用工具生成)

`Defined` 类示例
``` csharp
// file Assets/~temp/Test/DefinedID.cs

namespace GMessage
{
    public static partial class DefindID
    {

        /// <summary>
        /// 测试消息
        /// </summary>
        [System.ComponentModel.Description("测试消息")]
        public const int TEST_MESSAGE = -1628097645;

        public const int TEST_MESSAGE_VIEW = 0001;


        public const int TEST_SERVICE = 1001;
    }

    public static partial class ModuleID
    {
        public const int TEST_MODULE = 1000;
    }
}
```


## 3.创建 `Module` 类，继承自 `GMessage.Module` 基类

* 如果构造方法继承自`base`，则无需初始化 `commanders` 和 `listeners`，否则需要初始化。
* 绑定希望监听的命令。
* 向 `AppFacade` 注册自身提供的服务。尽量少用。
* 如果有必要，可以将自身设置为单例。

一个 `Module` 类示例
``` csharp
// file Assets/~temp/Test/TestModule.cs

using GMessage;
using System;

namespace Assets._temp.Test
{
    public class TestModule : Module
    {
        #region Singleton
        public static TestModule Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TestModule();
                }
                return _instance;
            }
        }
        private static TestModule _instance;
        #endregion Singleton

        private TestModule() : base()
        {
            BindingCommand(typeof(TestCommand), DefindID.TEST_MESSAGE);

            AppFacade.Instance.RegisterService(DefindID.TEST_SERVICE, new CommonService(new Func<int>(GetRandomNumber)));
        }

        ~TestModule()
        {
            UnBindingCommand(DefindID.TEST_MESSAGE);

            AppFacade.Instance.UnRegisterService(DefindID.TEST_SERVICE);
        }


        private int GetRandomNumber()
        {
            return UnityEngine.Random.Range(0, 100);
        }
    }
}
```


## 4.创建 `View` 类，继承自 `GMessage.Listener<T>` 基类，其中的 `T` 为希望监听的 `Module` 类

> 如果是模块所专有的，则 `T` 最好是其对应的 `Module`。
> 如果是公共的，则 `T` 一般为 `GlobalDispatcher`。

* 实现 `T GetDispatcher()` 方法和 `void Receive(IMessage message)` 方法

    > 第一个方法为获取到对应的模块类实例。
    > 第二个方法为处理接收到的消息。

一个 `View` 类示例
``` csharp
// file Assets/~temp/Test/TestView.cs

using GMessage;
using UnityEngine;

namespace Assets._temp.Test
{
    public class TestView : Listener<TestModule>
    {
        public BindableProperty<int> Result { get; private set; }

        const int ADD = 5;

        public TestView()
        {
            RegisterMessage(this, DefindID.TEST_MESSAGE, DefindID.TEST_MESSAGE_VIEW);
            Result = new BindableProperty<int>();
        }

        ~TestView()
        {
            UnRegisterMessage(this, DefindID.TEST_MESSAGE, DefindID.TEST_MESSAGE_VIEW);
        }

        public override void Receive(IMessage message)
        {
            switch (message.ID)
            {
                case DefindID.TEST_MESSAGE:
                    Result.Value = (int)message.Content + ADD;
                    Debug.Log($"TestView + 5    的结果为{Result.Value}");
                    break;
                case DefindID.TEST_MESSAGE_VIEW:
                default:
                    var add = AppFacade.Instance.CallService<int>(DefindID.TEST_SERVICE);
                    Result.Value = (int)message.Content + add;
                    Debug.Log($"TestView + random   的结果为{Result.Value}");
                    break;
            }
        }

        protected override TestModule GetDispatcher()
        {
            return TestModule.Instance;
        }
    }
}

```


## 5.创建 `Command` 类，继承自 `GMessage.ICommand` 接口

* 实现 `public void Execute(IMessage message)` 方法

一个 Command 类示例
``` csharp
// file Assets/~temp/Test/TestCommand.cs

using GMessage;
using System;
using UnityEngine;

namespace Assets._temp.Test
{
    public class TestCommand : ICommand
    {
        const int ADD = 5;
        public void Execute(IMessage message)
        {
            Debug.Log($"TestCommand + 5 的结果为 {(int)message.Content + ADD}");
        }
    }
}
```
