using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// A Command to create a projectile using the specified projectile data
    /// </summary>
    public class SpawnProjectileCommand : Command
    {
        /// <summary>
        /// The player that is spawning the projectile, used to set the bounds of the projectile and parent it to the player's BoundingBox
        /// </summary>
        private Player player;
        /// <summary>
        /// The data to use for the projectile behavior
        /// </summary>
        private ProjSO data;
        /// <summary>
        /// The length of time to set for the projectiles lifespan
        /// </summary>
        private float Lifespan;

        /// <summary>
        /// Constructs a SpawnProjectileCommand
        /// </summary>
        public SpawnProjectileCommand(Player player, ProjSO data, float Lifespan) {
            this.player = player;
            this.data = data;
            this.Lifespan = Lifespan;
        }
        /// <summary>
        /// Executes the command
        /// </summary>
        public override void Execute() {
            GameObject projGO = GameObject.Instantiate(data.Visuals,player.boundingBox.transform);
            Projectile proj = projGO.AddComponent<Projectile>();
            proj.Origin = projGO.transform.position = player.transform.position;
            proj.Lifespan = Lifespan;
            proj.rectBounds = player.boundingBox;
            proj.LoadData(data);
            proj.Activate();
        }
    }
}