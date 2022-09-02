using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// A component for the behavior of the various types of projectiles
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// wether or not to start updating the projectiles position
        /// </summary>
        private bool active = false;
        /// <summary>
        /// The projectile type to update the position 
        /// </summary>
        private ProjSO data;
        /// <summary>
        /// The starting position of the projectile, relevant for some projectile types
        /// </summary>
        public Vector3 Origin { get; set; }
        /// <summary>
        /// How long the projectile should exist before destroying itself regardless of position
        /// </summary>
        public float Lifespan { get; set; }
        /// <summary>
        /// The previous direction the projectile moved in, relevant for some projectile types
        /// </summary>
        private Vector3 prevDirection;

        /// <summary>
        /// Sets the projectile to start updating its position
        /// </summary>
        public void Activate() {
            active = true;
        }
        /// <summary>
        /// Sets the projectile SO to be used to update the position of the projectile
        /// </summary>
        /// <param name="_data"></param>
        public void LoadData(ProjSO _data) {
            this.data = _data;
        }

        /// <summary>
        /// Updates the position of the projectile based on its scriptable object data. 
        /// Destroys itself if its data is null or it its data returns Vector3.Zero for the new position
        /// Otherwise destroys itself after its lifespan has elapsed
        /// </summary>
        void FixedUpdate()
        {
            if (active)
            {
                if (data == null) { Destroy(gameObject); }
                Destroy(gameObject, Lifespan);
                Vector3 target = data.UpdatePosition(transform.position, Origin, prevDirection, out prevDirection);
                if (target == Vector3.zero)
                {
                    Destroy(gameObject);
                }
                else
                {
                    this.transform.position = target;
                }
            }
        }
    }
}