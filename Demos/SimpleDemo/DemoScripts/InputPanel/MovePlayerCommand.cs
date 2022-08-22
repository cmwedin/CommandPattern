using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    public class MovePlayerCommand : Command, IFailable, IUndoable
    {
        private Player player;
        private Vector2 moveBy;
        private Command undoCommand;
        public MovePlayerCommand(Player player, Vector2 direction, float speed) {
            if (player == null || direction == null) { throw new System.ArgumentNullException(); }
            this.player = player;
            this.moveBy = direction * speed * Time.deltaTime;
        }
        public MovePlayerCommand(Player player, Vector2 moveBy) {
            if (player == null || moveBy == null) { throw new System.ArgumentNullException(); }
            this.player = player;
            this.moveBy = moveBy;
        }

        public override void Execute()
        {
            player.transform.position += (Vector3)moveBy;
        }

        public Command GetUndoCommand() {
            if(undoCommand == null) {
                undoCommand = new MovePlayerCommand(player, -1*moveBy);
            }
            return undoCommand;
        }

        public bool WouldFail() {
            Vector2 target = player.transform.position + (Vector3)moveBy;
            float playerWidth = player.GetComponent<RectTransform>().rect.width / 2;
            float playerHeight = player.GetComponent<RectTransform>().rect.height / 2;
            Vector3[] boundaryCorners = new Vector3[4];
            player.boundingBox.GetWorldCorners(boundaryCorners);
            // Vector2 lowerLeftBound = player.GetComponentInParent<RectTransform>().anchorMin;
            return
                !(target.x - playerWidth >= boundaryCorners[0].x &&
                target.x + playerWidth <= boundaryCorners[2].x &&
                target.y - playerHeight >= boundaryCorners[0].y &&
                target.y + playerHeight <= boundaryCorners[2].y);
        }
    }
}