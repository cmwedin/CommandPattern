using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    /// <summary>
    /// A Command to move the player
    /// </summary>
    public class MovePlayerCommand : Command, IFailable, IUndoable
    {
        /// <summary>
        /// The player to move
        /// </summary>
        private Player player;
        /// <summary>
        /// The vector to move the player by
        /// </summary>
        private Vector2 moveBy;
        /// <summary>
        /// The commands undo Command
        /// </summary>
        private Command undoCommand;
        /// <summary>
        /// Constructs a MovePlayerCommand based on a direction and speed
        /// </summary>
        /// <param name="direction">The direction to move the player in</param>
        /// <param name="speed">The speed with which to move the player</param>
        /// <exception cref="System.ArgumentNullException">thrown if the player or direction are null</exception>
        public MovePlayerCommand(Player player, Vector2 direction, float speed) {
            if (player == null || direction == null) { throw new System.ArgumentNullException(); }
            this.player = player;
            this.moveBy = direction.normalized * speed * Time.fixedDeltaTime;
        }
        /// <summary>
        /// Constructs a MovePlayerCommand with an exact vector to move the player by
        /// </summary>
        /// <exception cref="System.ArgumentNullException">thrown if the player or moveBy vector are null</exception>
        public MovePlayerCommand(Player player, Vector2 moveBy) {
            if (player == null || moveBy == null) { throw new System.ArgumentNullException(); }
            this.player = player;
            this.moveBy = moveBy;
        }
        /// <summary>
        /// Execute the command
        /// </summary>
        public override void Execute() {
            player.transform.position += (Vector3)moveBy;
        }
        /// <summary>
        /// Creates the undo command if it has yet to be created and returns it 
        /// </summary>
        /// <returns>the undo command</returns>
        public ICommand GetUndoCommand() {
            if(undoCommand == null) {
                undoCommand = new MovePlayerCommand(player, -1*moveBy);
            }
            return undoCommand;
        }
        /// <summary>
        /// Determines if executing the command would move the player outside its bounding box
        /// </summary>
        /// <returns>True if the above is the case</returns>
        public bool WouldFail() {
            Vector2 target = player.transform.position + (Vector3)moveBy;
            float playerWidth = player.GetComponent<RectTransform>().rect.width / 2;
            float playerHeight = player.GetComponent<RectTransform>().rect.height / 2;
            Vector3[] boundaryCorners = new Vector3[4];
            player.boundingBox.GetWorldCorners(boundaryCorners);
            return
                !(target.x - playerWidth >= boundaryCorners[0].x &&
                target.x + playerWidth <= boundaryCorners[2].x &&
                target.y - playerHeight >= boundaryCorners[0].y &&
                target.y + playerHeight <= boundaryCorners[2].y);
        }
    }
}