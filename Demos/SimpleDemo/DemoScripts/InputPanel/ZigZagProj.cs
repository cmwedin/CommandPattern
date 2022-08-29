using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    [CreateAssetMenu]
    public class ZigZagProj : ProjSO
    {
        [SerializeField, Range(10,80), Tooltip("angle in degrees")] private float angle;
        [SerializeField] private float width;
        private float yMax;
        private float xMin;
        private float innerXMin;
        private float xMax;
        private float innerXMax;

        public override RectTransform BoundingBox { set {
                Vector3[] corners = new Vector3[4];
                value.GetWorldCorners(corners);
                yMax = corners[1].y;
                xMax = corners[2].x;
                xMin = corners[0].x;
            }}

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