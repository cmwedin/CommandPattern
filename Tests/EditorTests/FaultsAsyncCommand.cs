using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.EditorTesting
{
    public class FaultsAsyncCommand : AsyncCommand
    {
        private bool isComplete;

        public override async Task ExecuteAsync() {
            while(!isComplete) {
                await Task.Delay(1);
                CancellationToken.ThrowIfCancellationRequested();
            }
            throw new System.Exception("This is a test exception");
        }

        public void Complete() {
            isComplete = true;
        }
    }
}