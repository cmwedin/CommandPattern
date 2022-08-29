using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    [CreateAssetMenu]
    public class LinearProj : ProjSO
    {
        private float yMax;
        public override RectTransform BoundingBox { set {
                Vector3[] corners = new Vector3[4];
                value.GetWorldCorners(corners);
                yMax = corners[1].y;
            }}

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