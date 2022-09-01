using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class WaitForSecondsCommand : AsyncCommand
    {
        float secondsToWait;
        public override async Task ExecuteAsync() {
            Debug.Log($"starting {secondsToWait} second wait");
            await Task.Delay((int)(secondsToWait * 1000));
            Debug.Log("Wait done");
        }
    }
}