using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public static class RectTransformExtensions
    {
        /// <summary>
        /// This is a QoL extension method to determine if a vector is within the bounds of a RectTransform, typically used with the mouse position
        /// </summary>
        /// <param name="rectTransform"> The RectTransform this method is being invoked as a member of</param>
        /// <param name="vector">The vector we want to determine if the bounds of the RectTransform contains</param>
        /// <returns>Wether the vector is in the RectTransform</returns>
        public static bool Contains(this RectTransform rectTransform, Vector3 vector)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return (
                vector.x <= corners[2].x
                && vector.y <= corners[2].y
                && vector.x >= corners[0].x
                && vector.y >= corners[0].y
            );
        }
        public static Vector3[] GetWorldCorners(this RectTransform rectTransform) {
            Vector3[] output = new Vector3[4];
            rectTransform.GetWorldCorners(output);
            return output;
        }
    }
}