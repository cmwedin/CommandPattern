using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class TweenCommandFactory : MonoBehaviour
    {
        private RectTransform tweenArea;
        private float xMin;
        private float xMax;
        private float yMin;
        private float yMax;
        [SerializeField] private GameObject demoObject;
        [SerializeField] private float tweenLength;
        [SerializeField] private float scaleFactor;


        // Start is called before the first frame update
        void Start()
        {
            tweenArea = GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            tweenArea.GetWorldCorners(corners);
            xMin = corners[0].x;
            yMin = corners[0].y;
            xMax = corners[2].x;
            yMax = corners[2].y;

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 target = Input.mousePosition;
                if (
                    target.x <= xMax
                    && target.x >= xMin
                    && target.y <= yMax
                    && target.y >= yMin
                ) {
                    Debug.Log("queueing move tween");
                    TweenCommandStream.Instance.QueueCommand(new MoveTweenCommand(demoObject, target, tweenLength));
                }
            } if(Input.GetMouseButtonDown(1)) {
                    TweenCommandStream.Instance.QueueCommand(new ScaleTweenCommand(demoObject, scaleFactor * demoObject.transform.localScale, tweenLength));

            }
        }
    }
}