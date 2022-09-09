using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// A projectile type that shots out at a random angle then bounces off of the walls of its bounding box (excluding the top, which destroys it)
    /// </summary>
    public class RandomProj : ProjSO
    {
        /// <summary>
        /// an enum for which directions the projectile should reflect its direction, if any
        /// </summary>
        private enum ReflectAxis {
            vertical,horizontal,both,none
        }
        /// <summary>
        /// the minimum y value the projectile can have before reflecting
        /// </summary>
        private float yMin;
        /// <summary>
        /// the y value at which the projectile is destroyed
        /// </summary>
        private float yMax;
        /// <summary>
        /// the minimum x value the projectile can have before reflecting
        /// </summary>
        private float xMin;
        /// <summary>
        /// the maximum x value the projectile can have before reflecting
        /// </summary>
        private float xMax;

        /// <summary>
        /// a property to set the x and y min and max values through a RectTransform
        /// </summary>
        public override RectTransform BoundingBox { set {
                Vector3[] corners = new Vector3[4];
                value.GetWorldCorners(corners);
                yMax = corners[1].y;
                yMin = corners[0].y;
                xMax = corners[2].x;
                xMin = corners[0].x;
            }}

        /// <summary>
        /// Moves the projectile in a random direction until one of the reflecting bounds is reached, 
        /// at which point the direction is flipped either horizontally, vertically, or both depending on which bound was hit
        /// </summary>
        /// <remark> unused parameters exclude </remark>
        /// <param name="currentPos">The current position of the projectile</param>
        /// <param name="prevDirection">The previous direction the projectile was moving in</param>
        /// <param name="nextDirection">The new direction the projectile is moving ing
        /// The same as the previous direction unless this was its first update or a reflection occurred
        /// </param>
        /// <returns> The new position of the projectile </returns>
        public override Vector3 UpdatePosition(Vector3 currentPos, Vector3 origin, Vector3 prevDirection, out Vector3 nextDirection) {
            if (prevDirection == Vector3.zero) { prevDirection = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right; }
            nextDirection = prevDirection;
            ReflectAxis reflectDirection = ReflectAxis.none;
            Vector3 targetPos = currentPos + prevDirection * speed * Time.fixedDeltaTime;
            if (targetPos.y > yMax) { return Vector3.zero; }
            if(targetPos.x > xMax || targetPos.x < xMin) { //? Bounce of right wall
                reflectDirection = ReflectAxis.horizontal;
            } 
            if(targetPos.y < yMin) {
                if(reflectDirection == ReflectAxis.horizontal) {
                    reflectDirection = ReflectAxis.both;
                } else {
                    reflectDirection = ReflectAxis.vertical;
                }
            }
            switch (reflectDirection)
            {
                case ReflectAxis.horizontal: {
                        nextDirection = new Vector3(-prevDirection.x, prevDirection.y, prevDirection.z);
                        targetPos = currentPos + nextDirection * speed * Time.fixedDeltaTime;
                        return targetPos;
                    }
                case ReflectAxis.vertical: {
                        nextDirection = new Vector3(prevDirection.x, -prevDirection.y, prevDirection.z);
                        targetPos = currentPos + nextDirection * speed * Time.fixedDeltaTime;
                        return targetPos;
                    }
                case ReflectAxis.both: {
                        nextDirection = new Vector3(-prevDirection.x, -prevDirection.y, prevDirection.z);
                        targetPos = currentPos + nextDirection * speed * Time.fixedDeltaTime;
                        return targetPos;;
                    }
                case ReflectAxis.none: {
                        return targetPos;
                    }
                default: {
                        Debug.LogWarning("Something went wrong");
                        return Vector3.zero;
                    }
            }
        }
    }
}