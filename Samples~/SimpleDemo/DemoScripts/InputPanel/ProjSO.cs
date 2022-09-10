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
        /// The bounds of a projectile
        /// </summary>
        protected struct ProjBounds {
            /// <summary>
            /// The max x position
            /// </summary>
            public float xMax;
            /// <summary>
            /// The max y position
            /// </summary>
            public float yMax;
            /// <summary>
            /// The min x position
            /// </summary>
            public float xMin;
            /// <summary>
            /// The min y position
            /// </summary>
            public float yMin;

            /// <summary>
            /// Constructs a ProjBounds struct from a rect transform
            /// </summary>
            /// <param name="rectTransform"></param>
            public ProjBounds(RectTransform rectTransform)
            {
                var corners = rectTransform.GetWorldCorners();
                xMax = corners[2].x;
                yMax = corners[2].y;
                xMin = corners[0].x;
                yMin = corners[0].y;
            }
        }
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
        /// Invoked in the projectile FixedUpdate method to determine where it should move to next based on where it currently is, where it started, and what direction it was last moving in 
        /// Some projectile types may not use all of this information
        /// </summary>
        /// <param name="currentPos">the current position of the projectile</param>
        /// <param name="origin">the starting position of the projectile</param>
        /// <param name="prevDirection">the previous direction the projectile moved in</param>
        /// <param name="newDirection">the direction the projectile is moving in now</param>
        /// <returns> The new position of the projectile </returns>
        public abstract Vector3 UpdatePosition(Vector3 currentPos, Vector3 origin, RectTransform rectBounds, Vector3 prevDirection, out Vector3 newDirection);

    }
}