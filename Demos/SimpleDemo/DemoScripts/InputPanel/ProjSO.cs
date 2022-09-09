using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// The base scriptable object for the data of projectile behavior
    /// </summary>
    public abstract class ProjSO : ScriptableObject {
        /// <summary>
        /// The prefab object to instantiate as the visual base for the projectile
        /// </summary>
        [SerializeField] private GameObject visuals;
        public GameObject Visuals { get => visuals; }
        /// <summary>
        /// The speed at which the projectile should move
        /// </summary>
        [SerializeField] protected float speed;

        /// <summary>
        /// The box that should bound the projectiles movement
        /// </summary>
        public abstract RectTransform BoundingBox { set; }

        /// <summary>
        /// Invoked in the projectile FixedUpdate method to determine where it should move to next based on where it currently is, where it started, and what direction it was last moving in 
        /// Some projectile types may not use all of this information
        /// </summary>
        /// <param name="currentPos">the current position of the projectile</param>
        /// <param name="origin">the starting position of the projectile</param>
        /// <param name="prevDirection">the previous direction the projectile moved in</param>
        /// <param name="newDirection">the direction the projectile is moving in now</param>
        /// <returns> The new position of the projectile </returns>
        public abstract Vector3 UpdatePosition(Vector3 currentPos, Vector3 origin, Vector3 prevDirection, out Vector3 newDirection);

    }
}