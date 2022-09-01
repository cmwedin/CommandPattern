using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class AsyncSettingsManager : MonoBehaviour
    {
        [SerializeField] private WaitCommandFactory waitCommandFactory;
        [SerializeField,Tooltip("Minimum value for the WaitForSecondsCommands time length, modifying this is discouraged")] float minRuntime;
        
        [SerializeField,Tooltip("Maximum value for the WaitForSecondsCommands time length, modifying this is discouraged")] float maxRuntime;
        [SerializeField,Tooltip("Minimum value for the time-step the async commands will wait for, modifying this is discouraged")] int minTimeStep;
        [SerializeField,Tooltip("Maximum value for the time-step the async commands will wait for, modifying this is discouraged")] int maxTimeStep;

        [SerializeField] Slider bar1Slider;
        [SerializeField] TextMeshProUGUI bar1MinValue;
        [SerializeField] TextMeshProUGUI bar1MaxValue;
        [SerializeField] TextMeshProUGUI bar1CurrentValue;
        [SerializeField] Slider bar2Slider;
        [SerializeField] TextMeshProUGUI bar2MinValue;
        [SerializeField] TextMeshProUGUI bar2MaxValue;
        [SerializeField] TextMeshProUGUI bar2CurrentValue;
        [SerializeField] Slider bar3Slider;
        [SerializeField] TextMeshProUGUI bar3MinValue;
        [SerializeField] TextMeshProUGUI bar3MaxValue;
        [SerializeField] TextMeshProUGUI bar3CurrentValue;
        [SerializeField] Slider bar4Slider;
        [SerializeField] TextMeshProUGUI bar4MinValue;
        [SerializeField] TextMeshProUGUI bar4MaxValue;
        [SerializeField] TextMeshProUGUI bar4CurrentValue;
        [SerializeField] Slider timeStepSlider;
        [SerializeField] TextMeshProUGUI timeStepMinValue;
        [SerializeField] TextMeshProUGUI timeStepMaxValue;
        [SerializeField] TextMeshProUGUI timeStepCurrentValue;



        // Start is called before the first frame update
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}