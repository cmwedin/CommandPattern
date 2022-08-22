using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class SpawnProjectileCommand : Command
    {
        private Player player;
        private GameObject visuals;
        private int speed;
        public SpawnProjectileCommand(Player player, GameObject visuals, int speed) {
            this.player = player;
            this.visuals = visuals;
            this.speed = speed;

        }

        public override void Execute() {
            GameObject projGO = GameObject.Instantiate(visuals,player.boundingBox.transform);
            projGO.transform.position = player.transform.position;
            Projectile proj = projGO.AddComponent<Projectile>();
            proj.Speed = speed;
            proj.BoundingBox = player.boundingBox;
            proj.Activate();
        }
    }
}