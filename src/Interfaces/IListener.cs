using System.Collections.Generic;

namespace GFramework
{
    public interface IListener
    {
        void Receive(IMessage message);

        void RegisterMessage(IListener listener, IList<int> messageID);

        void UnRegisterMessage(IListener listener, IList<int> messageID);
    }
}