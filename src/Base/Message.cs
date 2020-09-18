namespace GFramework
{
    public class Message : IMessage
    {
        public virtual int ID { get; private set; }

        public virtual object Sender { get; private set; }

        public virtual object Content { get; set; }

        public Message(int id, object sender = null, object content = null)
        {
            ID = id;
            Sender = sender;
            Content = content;
        }

        public override string ToString()
        {
            return "ID->" + ID
                + "\nSender->" + Sender == null ? "null" : Sender.ToString()
                + "\nContent->" + Content == null ? "null" : Content.ToString();
        }
    }
}