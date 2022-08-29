using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public abstract class ProjSO : ScriptableObject
    {

        [SerializeField] private GameObject visuals;
        public GameObject Visuals { get => visuals; }
        [SerializeField] protected float speed;

        public abstract RectTransform BoundingBox { set; }

        /// <summary>
        /// Called in the projectile monobehavior's update method
        /// </summary>
        public abstract Vector3 UpdatePosition(Vector3 currentPos);

    }
}