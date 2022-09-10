using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// A projectile type that moves upwards at an angle before changing direction an moving upwards at the same angle in the opposite direction
    /// </summary>
    public class ZigZagProj : ProjSO
    {
        /// <summary>
        /// The angle at which the projectile moves
        /// </summary>
        [SerializeField, Range(10,80), Tooltip("angle in degrees")] private float angle;
        /// <summary>
        /// The width the projectile will bounce between
        /// </summary>
        [SerializeField] private float width;

        /// <summary>
        ///  Moves the projectile in a line the specified angle above Vector3.right
        ///  Until its x value reaches either xMax, or Width/2 greater than its starting value
        ///  at which point it continues at the specifies angle above Vector3.left instead (i.e. the x value of prevDirection is inverted)
        ///  Projectile destroyed once its y value reaches yMax
        /// </summary>
        /// <param name="currentPos">The current position of the projectile</param>
        /// <param name="origin">The starting position of the projectile</param>
        /// <param name="prevDirection">The previous direction the projectile was moving in</param>
        /// <param name="nextDirection">The new direction the projectile is moving ing
        /// The same as the previous direction unless this was its first update or a reflection occurred
        /// </param>
        /// <returns> The new position of the projectile </returns>
        public override Vector3 UpdatePosition(Vector3 currentPos, Vector3 origin, RectTransform rectBounds,Vector3 prevDirection, out Vector3 nextDirection) {
            if (prevDirection == Vector3.zero) { prevDirection = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right; }
            ProjBounds bounds = new ProjBounds(rectBounds);
            bool changeDirection = false;
            nextDirection = prevDirection;
            Vector3 targetPos = currentPos + prevDirection * speed * Time.fixedDeltaTime;
            if (targetPos.y > bounds.yMax) { return Vector3.zero; }
            if (prevDirection.x > 0 ) { //? moving right
                if(targetPos.x <= bounds.xMax  && targetPos.x <= origin.x + width / 2) {
                    return targetPos;
                } else {
                    changeDirection = true;
                }
            } else { //? moving left
                if(targetPos.x >= bounds.xMin && targetPos.x >= origin.x - width / 2 ) {
                    return targetPos;
                } else {
                    changeDirection = true;
                }
            } if(changeDirection) {
                nextDirection = new Vector3(-prevDirection.x,prevDirection.y,prevDirection.z);
                targetPos = currentPos + nextDirection * speed * Time.fixedDeltaTime;
                return targetPos;
            } else {
                Debug.LogWarning("something went wrong");
                return Vector3.zero;
            }
        }
    }
}