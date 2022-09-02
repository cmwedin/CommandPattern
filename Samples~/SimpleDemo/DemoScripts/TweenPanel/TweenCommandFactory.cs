using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// Simulates the client sending tween commands to the TweenCommandStream
    /// </summary>
    public class TweenCommandFactory : MonoBehaviour
    {
        /// <summary>
        /// The area within which to move the demoObject
        /// </summary>
        private RectTransform tweenArea;
        /// <summary>
        /// The demo object to modify with the tween commands
        /// </summary>
        [SerializeField] private GameObject demoObject;
        /// <summary>
        /// The length of time the tween commands take
        /// </summary>
        [SerializeField] private float tweenLength;
        /// <summary>
        /// The factor to create the ScaleTweenCommand with 
        /// </summary>
        private float scaleFactor;
        /// <summary>
        /// The slider for adjusting the scale factor
        /// </summary>
        [SerializeField] private Slider scaleFactorSlider;
        /// <summary>
        /// The label for the min value of the scale factor
        /// </summary>
        [SerializeField] private TextMeshProUGUI minScaleLabel;
        /// <summary>
        /// The label for the max value of the scale factor
        /// </summary>
        [SerializeField] private TextMeshProUGUI maxScaleLabel;
        /// <summary>
        /// the label for the current value of the scale factor
        /// </summary>
        [SerializeField] private TextMeshProUGUI currentScaleLabel;
        /// <summary>
        /// the button to queue a ScaleTweenCommand
        /// </summary>
        [SerializeField] private Button scaleButton;

        /// <summary>
        /// The angle to construct the RotateTweenCommand with
        /// </summary>
        private float rotateAngle;
        /// <summary>
        /// The slider for adjusting the rotate angle 
        /// </summary>
        [SerializeField] private Slider rotateAngleSlider;
        /// <summary>
        /// The label for the min value of the rotate angle
        /// </summary>
        [SerializeField] private TextMeshProUGUI minAngleLabel;
        /// <summary>
        /// The label for the max value of the rotate angle
        /// </summary>
        [SerializeField] private TextMeshProUGUI maxAngleLabel;
        /// <summary>
        /// the label for the current value of the rotate angle
        /// </summary>
        [SerializeField] private TextMeshProUGUI currentAngleLabel;
        /// <summary>
        /// the button to queue a RotateTweenCommand
        /// </summary>
        [SerializeField] private Button rotateButton;

        /// <summary>
        /// Sets the reference of the tweenArea and the text of the slider ui elements
        /// Subscribes delegates that set the appropriate fields to the slider's onValueChanged events
        /// Subscribes delegates that create and queue the appropriate TweenCommands to the buttons
        /// </summary>
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

        /// <summary>
        /// Creates and queues TweenCommands based on mouse inputs if the mouse is in the TweenArea as an alternative to the buttons
        /// </summary>
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 target = Input.mousePosition;
                if (tweenArea.Contains(target)) {
                    Debug.Log("queueing move tween");
                    TweenCommandStream.Instance.QueueCommand(new MoveTweenCommand(demoObject, target, tweenLength));
                }
            } if (Input.GetMouseButtonDown(1) && tweenArea.Contains(Input.mousePosition)) {
                    TweenCommandStream.Instance.QueueCommand(new ScaleTweenCommand(demoObject, scaleFactor, tweenLength));

            } if (Input.GetMouseButtonDown(2) && tweenArea.Contains(Input.mousePosition)) {
                    TweenCommandStream.Instance.QueueCommand(new RotateTweenCommand(demoObject, rotateAngle, tweenLength));
            }
        }
    }
}