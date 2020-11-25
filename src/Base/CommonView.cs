using System;

namespace GFramework
{
    public class CommonView<TContainer> : Listener<CommonModule>
    {
        public int ModuleID { get; protected set; }

        public TContainer ViewContainer { get; set; }

        public event Action<CommonView<TContainer>, IMessage> OnReceive;

        public override void Receive(IMessage message)
        {
            OnReceive?.Invoke(this, message);
        }

        protected override CommonModule GetDispatcher()
        {
            return CommonModule.Instance(ModuleID);
        }
    }
}
