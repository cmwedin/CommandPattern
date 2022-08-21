using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    public class MovePlayerCommand : Command, IFailable
    {
        private Player player;
        private Vector2 moveBy;

        public MovePlayerCommand(Player player, Vector2 moveBy)
        {
            this.player = player;
            this.moveBy = moveBy;
        }

        public override void Execute()
        {
            player.transform.position += (Vector3)moveBy;
        }

        public bool WouldFail() {
            Vector2 target = player.transform.position + (Vector3)moveBy;
            Vector3[] boundaryCorners = new Vector3[4];
            player.boundingBox.GetWorldCorners(boundaryCorners);
            // Vector2 lowerLeftBound = player.GetComponentInParent<RectTransform>().anchorMin;
            return
                !(target.x >= boundaryCorners[0].x &&
                target.x <= boundaryCorners[2].x &&
                target.y >= boundaryCorners[0].y &&
                target.y <= boundaryCorners[2].y);
        }
    }
}