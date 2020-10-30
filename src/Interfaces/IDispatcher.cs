using System;

namespace GFramework
{
    public interface IDispatcher : IListener
    {
        bool Listening(int ID);

        void BindingCommand(Type command, int messageID);

        void UnBindingCommand(int messageID);
    }
}