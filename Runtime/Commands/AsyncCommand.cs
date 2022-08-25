using System.Threading.Tasks;

namespace SadSapphicGames.CommandPattern
{
    public abstract class AsyncCommand : Command, IAsyncCommand
    {
        private Task commandTask;
        public Task CommandTask { get => commandTask; private set => commandTask = value; }

        public override void Execute() {
            CommandTask = ExecuteAsync();
        }

        public abstract Task ExecuteAsync();
    }
}