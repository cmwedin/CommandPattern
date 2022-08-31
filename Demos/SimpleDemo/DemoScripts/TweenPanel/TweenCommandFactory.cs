using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class TweenCommandFactory : MonoBehaviour
    {
        private RectTransform tweenArea { set {
            Vector3[] corners = new Vector3[4];
            value.GetWorldCorners(corners);
            xMin = corners[0].x;
            yMin = corners[0].y;
            xMax = corners[2].x;
            yMax = corners[2].y;
        }}
        private float xMin;
        private float xMax;
        private float yMin;
        private float yMax;
        [SerializeField] private GameObject demoObject;
        [SerializeField] private float tweenLength;
        private float scaleFactor;
        [SerializeField] private Slider scaleFactorSlider;
        [SerializeField] private TextMeshProUGUI minScaleLabel;
        [SerializeField] private TextMeshProUGUI maxScaleLabel;
        [SerializeField] private TextMeshProUGUI currentScaleLabel;
        [SerializeField] private Button scaleButton;


        private float rotateAngle;
        [SerializeField] private Slider rotateAngleSlider;
        [SerializeField] private TextMeshProUGUI minAngleLabel;
        [SerializeField] private TextMeshProUGUI maxAngleLabel;
        [SerializeField] private TextMeshProUGUI currentAngleLabel;
        [SerializeField] private Button rotateButton;





        // Start is called before the first frame update
        void Start()
        {
            tweenArea = GetComponent<RectTransform>();

            minScaleLabel.text = scaleFactorSlider.minValue.ToString();
            maxScaleLabel.text = scaleFactorSlider.maxValue.ToString();
            currentScaleLabel.text = scaleFactorSlider.value.ToString();
            scaleFactor = scaleFactorSlider.value;

            minAngleLabel.text = rotateAngleSlider.minValue.ToString();
            maxAngleLabel.text = rotateAngleSlider.maxValue.ToString();
            currentAngleLabel.text = rotateAngleSlider.value.ToString();
            rotateAngle = rotateAngleSlider.value;

            scaleFactorSlider.onValueChanged.AddListener((value) => {
                scaleFactor = value;
                currentScaleLabel.text = Math.Round(value,1).ToString();
            });
            rotateAngleSlider.onValueChanged.AddListener((value) => {
                rotateAngle = value;
                currentAngleLabel.text = Mathf.RoundToInt(value).ToString();
            });

            scaleButton.onClick.AddListener(() => { 
                    TweenCommandStream.Instance.QueueCommand(new ScaleTweenCommand(demoObject, scaleFactor, tweenLength));
            });
            rotateButton.onClick.AddListener(() => { 
                    TweenCommandStream.Instance.QueueCommand(new RotateTweenCommand(demoObject, rotateAngle, tweenLength));
            });
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
            } if (Input.GetMouseButtonDown(1)) {
                    TweenCommandStream.Instance.QueueCommand(new ScaleTweenCommand(demoObject, scaleFactor, tweenLength));

            } if (Input.GetMouseButtonDown(2)) {
                    TweenCommandStream.Instance.QueueCommand(new RotateTweenCommand(demoObject, rotateAngle, tweenLength));
            }
        }
    }
}