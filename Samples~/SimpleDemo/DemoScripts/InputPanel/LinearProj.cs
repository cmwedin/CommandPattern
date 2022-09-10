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
        /// Updates the y value of the position to be a distance above the current position's y value based on this scriptable objects set speed
        /// </summary>
        /// <remark> unused parameters exclude </remark>
        /// <param name="currentPos">the current position of the projectile</param>
        /// <returns>The new position of the projectile</returns>
        public override Vector3 UpdatePosition(Vector3 currentPos, Vector3 origin,  RectTransform boundsRect, Vector3 prevDirection, out Vector3 nextDirection) {
            ProjBounds bounds = new ProjBounds(boundsRect);
            float targetY = currentPos.y + speed * Time.fixedDeltaTime;
            nextDirection = Vector3.up;
            if (targetY <= bounds.yMax) {
                return new Vector3(currentPos.x, targetY, currentPos.z);
            } else {
                return Vector3.zero;
            }
        }
    }
}