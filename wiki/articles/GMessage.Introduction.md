# 一个简单的消息框架
---


主要目标是实现模块间的解耦

## 发送消息方式:

``` csharp
// 定义
public void SendMessage(int messageID, object sender = null, object content = null, IDispatcher dispatcher = null)

// 调用
AppFacade.Instance.SendMessage(DefinedID.TEST_MESSAGE, this, new Vector3(1, 1, 1));
```


## 定义消息方式：

* 使用Unity编辑器中的命令生成

1. 菜单栏 -> Framework -> 生成新的ID
2. 弹出面板上点击 **选择文件**
3. 选择消息定义类文件并打开
4. 点击 **添加**
5. 填写注释和ID
6. 点击 **生成**

![Unity编辑器中的命令](../images/gf_intro_1.jpg)

* 或者使用命令行工具，`GMessage -> GenerateUniqueID`工程可以生成命令行工具

    > ```
    > GenerateUniqueID.exe file_path message_name [message_comment] [message_content_type]
    > GenerateUniqueID.exe DefinedID.cs TEST_MESSAGE 测试消息 float
    > ```

会生成如下格式的文本。

``` csharp
namespace GMessage
{
    public static partial class DefinedID
    {

        // ...

        /// <summary>
        /// 消息注释
        /// </summary>
        public const int MESSAGE_NAME = 0;

        // ...
    }
}
```
