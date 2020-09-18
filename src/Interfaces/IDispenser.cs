using System;

namespace GFramework
{
    public interface IDispenser : IListener
    {
        bool Listening(int ID);

        void BindingCommand(Type command, int messageID);

        void UnBindingCommand(int messageID);
    }
}