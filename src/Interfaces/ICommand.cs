namespace GFramework
{
    public interface ICommand
    {
        void Execute(IMessage message);
    }
}