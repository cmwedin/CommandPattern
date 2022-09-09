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
        /// the y value at which the projectile is destroyed
        /// </summary>
        private float yMax;
        /// <summary>
        /// the minimum x value before the projectile reflects regardless of the width
        /// </summary>
        private float xMin;
        /// <summary>
        /// the maximum x value before the projectile reflects regardless of the width
        /// </summary>
        private float xMax;
        /// <summary>
        /// a Property to set the values of xMin, xMax, and yMax through a RectTransform 
        /// </summary>
        public override RectTransform BoundingBox { set {
                Vector3[] corners = new Vector3[4];
                value.GetWorldCorners(corners);
                yMax = corners[1].y;
                xMax = corners[2].x;
                xMin = corners[0].x;
            }}
        
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
        public override Vector3 UpdatePosition(Vector3 currentPos, Vector3 origin, Vector3 prevDirection, out Vector3 nextDirection) {
            if (prevDirection == Vector3.zero) { prevDirection = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right; }
            bool changeDirection = false;
            nextDirection = prevDirection;
            Vector3 targetPos = currentPos + prevDirection * speed * Time.fixedDeltaTime;
            if (targetPos.y > yMax) { return Vector3.zero; }
            if (prevDirection.x > 0 ) { //? moving right
                if(targetPos.x <= xMax  && targetPos.x <= origin.x + width / 2) {
                    return targetPos;
                } else {
                    changeDirection = true;
                }
            } else { //? moving left
                if(targetPos.x >= xMin && targetPos.x >= origin.x - width / 2 ) {
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