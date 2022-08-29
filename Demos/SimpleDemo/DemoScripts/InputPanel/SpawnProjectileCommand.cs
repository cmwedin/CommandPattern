using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class SpawnProjectileCommand : Command
    {
        private Player player;
        private ProjSO data;
        private float Lifespan;
        public SpawnProjectileCommand(Player player, ProjSO data, float Lifespan) {
            this.player = player;
            this.data = data;
            this.Lifespan = Lifespan;
            data.BoundingBox = player.boundingBox;

        }

        public override void Execute() {
            GameObject projGO = GameObject.Instantiate(data.Visuals,player.boundingBox.transform);
            Projectile proj = projGO.AddComponent<Projectile>();
            proj.Origin = projGO.transform.position = player.transform.position;
            proj.Lifespan = Lifespan;
            proj.LoadData(data);
            proj.Activate();
        }
    }
}