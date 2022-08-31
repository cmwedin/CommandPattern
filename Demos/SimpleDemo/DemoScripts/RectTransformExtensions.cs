using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtensions
{
    public static bool Contains(this RectTransform rectTransform, Vector3 vector) {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return (
            vector.x <= corners[2].x
            && vector.y <= corners[2].y
            && vector.x >=  corners[0].x
            && vector.y >=  corners[0].y
        );
    }
}
