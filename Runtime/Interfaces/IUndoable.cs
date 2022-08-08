using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern {
    interface IUndoable {
        public Command GetUndoCommand();
    }
}
