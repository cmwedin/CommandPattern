using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.EditorTesting
{
    public class AlwaysFailsCommand : Command, IFailable
    {
        public override void Execute()
        {
            throw new System.Exception("This command will fail");
        }

        public bool WouldFail()
        {
            return true;
        }
    }
}