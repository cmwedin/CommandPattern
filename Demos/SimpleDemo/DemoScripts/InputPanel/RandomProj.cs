using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    [CreateAssetMenu]
    public class RandomProj : ProjSO
    {
        private enum ReflectAxis {
            vertical,horizontal,both,none
        }
        private float yMin;
        private float yMax;
        private float xMin;
        private float xMax;

        public override RectTransform BoundingBox { set {
                Vector3[] corners = new Vector3[4];
                value.GetWorldCorners(corners);
                yMax = corners[1].y;
                yMin = corners[0].y;
                xMax = corners[2].x;
                xMin = corners[0].x;
            }}

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