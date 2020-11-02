namespace GFramework
{
    /// <summary>
    /// 标准的消息体
    /// </summary>
    public class Message : IMessage
    {
        /// <summary>
        /// 消息ID，通常是 DefinedID 中的某一个常量
        /// </summary>
        public virtual int ID { get; private set; }

        /// <summary>
        /// 消息的发送者
        /// </summary>
        public virtual object Sender { get; private set; }

        /// <summary>
        /// 消息内容，在监听者接收消息后，会将内容解包并执行具体操作
        /// </summary>
        public virtual object Content { get; set; }

        /// <summary>
        /// 实例化一个标准消息体
        /// </summary>
        /// <param name="id">消息ID，通常是 DefinedID 中的某一个常量</param>
        /// <param name="sender">消息的发送者，默认值为空</param>
        /// <param name="content">消息内容，默认值为空，在监听者接收消息后，会将内容解包并执行具体操作</param>
        public Message(int id, object sender = null, object content = null)
        {
            ID = id;
            Sender = sender;
            Content = content;
        }

        /// <summary>
        /// 将消息体转化为字符串
        /// </summary>
        public override string ToString()
        {
            return "ID->" + ID
                + "\nSender->" + Sender == null ? "null" : Sender.ToString()
                + "\nContent->" + Content == null ? "null" : Content.ToString();
        }
    }
}