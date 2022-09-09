using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// A Command to create a projectile using the specified projectile data
    /// </summary>
    public class SpawnProjectileCommand : Command
    {
        /// <summary>
        /// The player that is spawning the projectile, used to set the bound of the projectile and parent it to the player BoundingBox
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
                //? This is a bad but easy way to set the bound the projectile can move in, 
                //? bad because the projSO is shared by all projectile of that type and the data persists when leaving play mode (meaning most of the time these values have already been set)
                //? however this isn't a demo for using SO's to store the data for the behaviour of components so this dirty solution works
                    //? That said I wanted to mention this so people don't copy it
                //? A better implementation would be to use this as an argument in the ProjSO UpdatePosition method so different projectiles could use different bounding boxes
            data.BoundingBox = player.boundingBox;

        }
        /// <summary>
        /// Executes the command
        /// </summary>
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