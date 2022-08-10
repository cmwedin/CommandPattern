using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SadSapphicGames.CommandPattern;

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
