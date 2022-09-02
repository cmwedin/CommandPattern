using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    /// <summary>
    /// Manages the ui elements for modifying the settings of the WaitCommandFactory for the simple async demo
    /// </summary>
    public class AsyncSettingsManager : MonoBehaviour
    {
        /// <summary>
        /// The WaitCommandFactory to modify the setting of
        /// </summary>
        [SerializeField] private WaitCommandFactory waitCommandFactory;
        /// <summary>
        /// the minimum runtime for a WaitForSecondsCommand
        /// </summary>
        [SerializeField,Tooltip("Minimum value for the WaitForSecondsCommands time length, modifying this is discouraged")] float minRuntime;
        /// <summary>
        /// the maximum runtime for a WaitForSecondsCommand
        /// </summary>
        [SerializeField,Tooltip("Maximum value for the WaitForSecondsCommands time length, modifying this is discouraged")] float maxRuntime;
        /// <summary>
        /// The minimum time-step for  the AsyncCommand to await
        /// </summary>
        [SerializeField,Tooltip("Minimum value for the time-step the async commands will wait for, modifying this is discouraged")] int minTimeStep;
        /// <summary>
        /// The minimum time-step for  the AsyncCommand to await
        /// </summary>
        [SerializeField,Tooltip("Maximum value for the time-step the async commands will wait for, modifying this is discouraged")] int maxTimeStep;

        /// <summary>
        /// The slider to modify the runtime of bar one's WaitForSecondsCommand
        /// </summary>
        [SerializeField] Slider bar1Slider;
        /// <summary>
        /// The display for the minimum value of bar1Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar1MinValue;
        /// <summary>
        /// The display for the maximum value of bar1Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar1MaxValue;
        /// <summary>
        /// The display for the current value of bar1Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar1CurrentValue;

        /// <summary>
        /// The slider to modify the runtime of bar two's WaitForSecondsCommand
        /// </summary>
        [SerializeField] Slider bar2Slider;
        /// <summary>
        /// The display for the minimum value of bar2Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar2MinValue;
        /// <summary>
        /// The display for the maximum value of bar2Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar2MaxValue;
        /// <summary>
        /// The display for the current value of bar2Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar2CurrentValue;

        /// <summary>
        /// The slider to modify the runtime of bar three's WaitForSecondsCommand
        /// </summary>
        [SerializeField] Slider bar3Slider;
        /// <summary>
        /// The display for the minimum value of bar3Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar3MinValue;
        /// <summary>
        /// The display for the maximum value of bar3Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar3MaxValue;
        /// <summary>
        /// The display for the current value of bar3Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar3CurrentValue;

        /// <summary>
        /// The slider to modify the runtime of bar four's WaitForSecondsCommand
        /// </summary>
        [SerializeField] Slider bar4Slider;
        /// <summary>
        /// The display for the minimum value of bar4Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar4MinValue;
        /// <summary>
        /// The display for the maximum value of bar4Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar4MaxValue;
        /// <summary>
        /// The display for the current value of bar4Slider
        /// </summary>
        [SerializeField] TextMeshProUGUI bar4CurrentValue;

        /// <summary>
        /// The slider to modify the time-step of the WaitForSecondsCommand's
        /// </summary>
        [SerializeField] Slider timeStepSlider;
        /// <summary>
        /// The display for the minimum value of the time-step
        /// </summary>
        [SerializeField] TextMeshProUGUI timeStepMinValue;
        /// <summary>
        /// The display for the maximum value of the time-step
        /// </summary>
        [SerializeField] TextMeshProUGUI timeStepMaxValue;
        /// <summary>
        /// The display for the current value of the time-step
        /// </summary>
        [SerializeField] TextMeshProUGUI timeStepCurrentValue;



        /// <summary>
        /// Sets the displays of the ui elements for the minimum, maximum, and current values of the various sliders
        /// Subscribes delegates to the sliders onValueChanged events to invoke the appropriate methods of WaitCommandFactory to updates its wait commands to the new settings 
        /// </summary>
        void Start()
        {
            bar1MinValue.text = minRuntime.ToString();
            bar1MaxValue.text = maxRuntime.ToString();
            bar1Slider.minValue = minRuntime;
            bar1Slider.maxValue = maxRuntime;
            bar1CurrentValue.text = Math.Round(bar1Slider.value,2).ToString();
            bar2MinValue.text = minRuntime.ToString();
            bar2MaxValue.text = maxRuntime.ToString();
            bar2Slider.minValue = minRuntime;
            bar2Slider.maxValue = maxRuntime;
            bar2CurrentValue.text = Math.Round(bar2Slider.value,2).ToString();
            bar3MinValue.text = minRuntime.ToString();
            bar3MaxValue.text = maxRuntime.ToString();
            bar3Slider.minValue = minRuntime;
            bar3Slider.maxValue = maxRuntime;
            bar3CurrentValue.text = Math.Round(bar3Slider.value,2).ToString();
            bar4MinValue.text = minRuntime.ToString();
            bar4MaxValue.text = maxRuntime.ToString();
            bar4Slider.minValue = minRuntime;
            bar4Slider.maxValue = maxRuntime;
            bar4CurrentValue.text = Math.Round(bar4Slider.value,2).ToString();
            timeStepMinValue.text = minTimeStep.ToString();
            timeStepMaxValue.text = maxTimeStep.ToString();
            timeStepSlider.minValue = minTimeStep;
            timeStepSlider.maxValue = maxTimeStep;
            timeStepCurrentValue.text = timeStepSlider.value.ToString();

            bar1Slider.onValueChanged.AddListener((value) => {
                bar1CurrentValue.text = Math.Round(value, 2).ToString();
                waitCommandFactory.ChangeBarRuntime(1,value);
            });            
            bar2Slider.onValueChanged.AddListener((value) => {
                bar2CurrentValue.text = Math.Round(value, 2).ToString();
                waitCommandFactory.ChangeBarRuntime(2,value);
            });            
            bar3Slider.onValueChanged.AddListener((value) => {
                bar3CurrentValue.text = Math.Round(value, 2).ToString();
                waitCommandFactory.ChangeBarRuntime(3,value);
            });            
            bar4Slider.onValueChanged.AddListener((value) => {
                bar4CurrentValue.text = Math.Round(value, 2).ToString();
                waitCommandFactory.ChangeBarRuntime(4,value);
            });
            timeStepSlider.onValueChanged.AddListener((value) => {
                timeStepCurrentValue.text = ((int)value).ToString();
                waitCommandFactory.ChangeTimeStep((int)value);
            });
        }
    }
}