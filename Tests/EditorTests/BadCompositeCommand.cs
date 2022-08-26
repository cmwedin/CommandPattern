using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.EditorTesting
{
    /// <summary>
    /// A CompositeCommand who's children will quietly fail (It does not implement IFailable) but who can be reverted
    /// </summary>
    public class BadCompositeCommandReversible : CompositeCommand
    {
        public BadCompositeCommandReversible()
        {
            AddChild(new NullCommand());
            AddChild(new AlwaysFailsCommand());
        }
    }

    public class BadCompositeCommandIrreversible : CompositeCommand
    {
        public BadCompositeCommandIrreversible()
        {
            AddChild(new IrreversibleNullCommand());
            AddChild(new AlwaysFailsCommand());
        }
    }
}