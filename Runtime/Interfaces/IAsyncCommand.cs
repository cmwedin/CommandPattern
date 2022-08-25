using System.Threading.Tasks;

namespace SadSapphicGames.CommandPattern
{
    public interface IAsyncCommand : ICommand
    {
        public Task CommandTask { get; }
        public abstract Task ExecuteAsync();
    }
}