using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int speed;
    private float yMax;
    private bool active = false;

    public int Speed { get => speed; set => speed = value; }
    public RectTransform BoundingBox { set {
            Vector3[] corners = new Vector3[4];
            value.GetWorldCorners(corners);
            yMax = corners[1].y;
        } }

    public void Activate() {
        active = true;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (active)
        {
            Vector3 target = this.transform.position + new Vector3(0, Time.deltaTime * Speed, 0);
            if (target.y > yMax) {
                Destroy(gameObject);
            } else {
                this.transform.position = target;
            }
        }
    }
}
