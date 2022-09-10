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
        public override Vector3 UpdatePosition(Vector3 currentPos, Vector3 origin, RectTransform rectBounds,Vector3 prevDirection, out Vector3 nextDirection) {
            if (prevDirection == Vector3.zero) { prevDirection = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right; }
            ProjBounds bounds = new ProjBounds(rectBounds);
            nextDirection = prevDirection;
            ReflectAxis reflectDirection = ReflectAxis.none;
            Vector3 targetPos = currentPos + prevDirection * speed * Time.fixedDeltaTime;
            if (targetPos.y > bounds.yMax) { return Vector3.zero; }
            if(targetPos.x > bounds.xMax || targetPos.x < bounds.xMin) { //? Bounce of right wall
                reflectDirection = ReflectAxis.horizontal;
            } 
            if(targetPos.y < bounds.yMin) {
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