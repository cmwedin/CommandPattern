using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class SpawnProjectileCommand : Command
    {
        private Player player;
        private ProjSO data;
        public SpawnProjectileCommand(Player player, ProjSO data) {
            this.player = player;
            this.data = data;
            data.BoundingBox = player.boundingBox;

        }

        public override void Execute() {
            GameObject projGO = GameObject.Instantiate(data.Visuals,player.boundingBox.transform);
            projGO.transform.position = player.transform.position;
            Projectile proj = projGO.AddComponent<Projectile>();
            proj.LoadData(data);
            proj.Activate();
        }
    }
}