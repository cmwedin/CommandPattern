using System.Collections;
using System.Collections.Generic;
using SadSapphicGames.CommandPattern;
using UnityEngine;

/// <summary>
/// A CompositeCommand who's children will quietly fail (It does not implement IFailable). This should never be used outside of tests. It will always cause an exception.
/// </summary>
public class BadCompositeCommand : CompositeCommand
{
    public BadCompositeCommand() {
        AddChild(new NullCommand());
        AddChild(new AlwaysFailsCommand());
    }
}
