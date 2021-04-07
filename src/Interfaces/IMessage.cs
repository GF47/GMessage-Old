namespace GMessage
{
    /// <summary>
    /// 消息体接口
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 消息ID，通常是 DefinedID 中的某一个常量
        /// </summary>
        int ID { get; }
        /// <summary>
        /// 消息发送者
        /// </summary>
        object Sender { get; }
        /// <summary>
        /// 消息内容，在监听者接收消息后，会将内容解包并执行具体操作，TODO 需要改进的地方是，如果内容为值类型，会产生拆装箱，但是如果是泛型则会产生一系列麻烦事
        /// </summary>
        object Content { get; set; }
    }
}