using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// A Scriptable object for the data of a projectile that moves in a straight line
    /// </summary>
    public class LinearProj : ProjSO
    {
        /// <summary>
        /// the maximum y value the projectile can reach before being destroyed
        /// </summary>
        private float yMax;
        /// <summary>
        /// a property to set yMax based on the bounds of a RectTransform
        /// </summary>
        public override RectTransform BoundingBox { set {
                Vector3[] corners = new Vector3[4];
                value.GetWorldCorners(corners);
                yMax = corners[1].y;
            }}
        /// <summary>
        /// Updates the y value of the position to be a distance above the current position's y value based on this scriptable objects set speed
        /// </summary>
        /// <remark> unused parameters exclude </remark>
        /// <param name="currentPos">the current position of the projectile</param>
        /// <returns>The new position of the projectile</returns>
        public override Vector3 UpdatePosition(Vector3 currentPos, Vector3 origin,  Vector3 prevDirection, out Vector3 nextDirection) {
            float targetY = currentPos.y + speed * Time.fixedDeltaTime;
            nextDirection = Vector3.up;
            if (targetY <= yMax) {
                return new Vector3(currentPos.x, targetY, currentPos.z);
            } else {
                return Vector3.zero;
            }
        }
    }
}