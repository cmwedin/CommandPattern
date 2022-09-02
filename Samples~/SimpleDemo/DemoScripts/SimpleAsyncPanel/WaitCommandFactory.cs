using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    public class WaitCommandFactory : MonoBehaviour
    {
        /// <summary>
        /// the timeStep which the wait for seconds commands will await before checking for cancellation and reporting progress
        /// </summary>
        int timeStep = 10;
        /// <summary>
        /// The progress bar to display the progress of the first WaitForSecondsCommand
        /// </summary>
        [SerializeField] ProgressBar bar1;
        /// <summary>
        /// The button to start the first WaitForSecondsCommand
        /// </summary>
        [SerializeField] Button startBar1;
        /// <summary>
        /// The button to cancel the first WaitForSecondsCommand
        /// </summary>
        [SerializeField] Button cancelBar1;
        /// <summary>
        /// The runtime of the first WaitForSecondsCommand
        /// </summary>
        float runtimeBar1 = 1;
        /// <summary>
        /// The progress bar to display the progress of the second WaitForSecondsCommand
        /// </summary>
        [SerializeField] ProgressBar bar2;
        /// <summary>
        /// The button to start the second WaitForSecondsCommand
        /// </summary>
        [SerializeField] Button startBar2;
        /// <summary>
        /// The button to cancel the second WaitForSecondsCommand
        /// </summary>
        [SerializeField] Button cancelBar2;
        /// <summary>
        /// The runtime of the second WaitForSecondsCommand
        /// </summary>
        float runtimeBar2 = 2;
        /// <summary>
        /// The progress bar to display the progress of the third WaitForSecondsCommand
        /// </summary>
        [SerializeField] ProgressBar bar3;
        /// <summary>
        /// The button to start the third WaitForSecondsCommand
        /// </summary>
        [SerializeField] Button startBar3;  
        /// <summary>
        /// The button to cancel the third WaitForSecondsCommand
        /// </summary>
        [SerializeField] Button cancelBar3;
        /// <summary>
        /// The runtime of the third WaitForSecondsCommand
        /// </summary>
        float runtimeBar3 = 3;
        /// <summary>
        /// The progress bar to display the progress of the fourth WaitForSecondsCommand
        /// </summary>
        [SerializeField] ProgressBar bar4;
        /// <summary>
        /// The button to start the fourth WaitForSecondsCommand
        /// </summary>
        [SerializeField] Button startBar4;
        /// <summary>
        /// The button to cancel the fourth WaitForSecondsCommand
        /// </summary>
        [SerializeField] Button cancelBar4;
        /// <summary>
        /// The runtime of the fourth WaitForSecondsCommand
        /// </summary>
        float runtimeBar4 = 4;
        /// <summary>
        /// The button to start all of the WaitForSecondsCommands
        /// </summary>
        [SerializeField] Button startAll;
        /// <summary>
        /// The button to cancel all of the WaitForSecondsCommands
        /// </summary>
        [SerializeField] Button cancelAll;
        /// <summary>
        /// the first WaitForSecondsCommand
        /// </summary>
        private WaitForSecondsCommand bar1Command;
        /// <summary>
        /// the second WaitForSecondsCommand
        /// </summary>
        private WaitForSecondsCommand bar2Command;
        /// <summary>
        /// the third WaitForSecondsCommand
        /// </summary>
        private WaitForSecondsCommand bar3Command;
        /// <summary>
        /// the fourth WaitForSecondsCommand
        /// </summary>
        private WaitForSecondsCommand bar4Command;

        /// <summary>
        /// A dictionary of which WaitForSecondsCommands are running
        /// </summary>
        Dictionary<ProgressBar, bool> runningBars;

        /// <summary>
        /// Changes the runtime of one of the WaitForSecondsCommands
        /// </summary>
        /// <param name="bar">which of the command to change the runtime of</param>
        /// <param name="newRuntime">what the new runtime should be</param>
        public void ChangeBarRuntime(int bar, float newRuntime) {
            switch (bar)
            {
                case 1:
                    runtimeBar1 = newRuntime;
                    SetBarCommand(1);
                    break;
                case 2:
                    runtimeBar2 = newRuntime;
                    SetBarCommand(2);
                    break;
                case 3:
                    runtimeBar3 = newRuntime;
                    SetBarCommand(3);
                    break;
                case 4:
                    runtimeBar4 = newRuntime;
                    SetBarCommand(4);
                    break;
                default:
                    return;
            }
        }
        /// <summary>
        /// Changes the time-step of the WaitForSecondsCommands
        /// </summary>
        /// <param name="value"></param>
        public void ChangeTimeStep(int value)
        {
            timeStep = value;
            for (int i = 1; i <= 4; i++)
            {
                SetBarCommand(i);
            };
        }
        /// <summary>
        /// Creates or re-creates one of the WaitForSecondsCommands and sets up its ProgressChanged and OnAnyTaskEnd events
        /// </summary>
        /// <param name="bar">which command to recreate</param>
        private void SetBarCommand(int bar) {
            switch (bar)
            {
                case 1:
                    bar1Command = new WaitForSecondsCommand(runtimeBar1,timeStep);
                    bar1Command.ProgressChanged += (value) => { bar1.SetFilledPortion(value); };
                    bar1Command.OnAnyTaskEnd += () => runningBars[bar1] = false;
                    break;
                case 2:
                    bar2Command = new WaitForSecondsCommand(runtimeBar2,timeStep);
                    bar2Command.ProgressChanged += (value) => { bar2.SetFilledPortion(value); };
                    bar2Command.OnAnyTaskEnd += () => runningBars[bar2] = false;
                    break;
                case 3:
                    bar3Command = new WaitForSecondsCommand(runtimeBar3,timeStep);
                    bar3Command.ProgressChanged += (value) => { bar3.SetFilledPortion(value); };
                    bar3Command.OnAnyTaskEnd += () => runningBars[bar3] = false;
                    break;
                case 4:
                    bar4Command = new WaitForSecondsCommand(runtimeBar4,timeStep);
                    bar4Command.ProgressChanged += (value) => { bar4.SetFilledPortion(value); };
                    bar4Command.OnAnyTaskEnd += () => runningBars[bar4] = false;
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Creates each of the WaitForSecondsCommands
        /// Sets all of the entries in the runningBars dictionaries to false
        /// Sets up the events of each buttons to start/cancel WaitForSecondsCommands
        /// </summary>
        void Start()
        {
            for (int i = 1; i <= 4; i++)
            {
                SetBarCommand(i);
            }
            runningBars = new Dictionary<ProgressBar, bool>{
                {bar1,false},
                {bar2,false},
                {bar3,false},
                {bar4,false}
            };

            startBar1.onClick.AddListener(() => { 
                if(!runningBars[bar1]) {
                    WaitCommandStream.Instance.QueueCommand(bar1Command);
                    runningBars[bar1] = true;
                }
            });
            cancelBar1.onClick.AddListener(() => { 
                if(runningBars[bar1]) {
                    WaitCommandStream.Instance.CancelCommand(bar1Command);
                }
            });
            startBar2.onClick.AddListener(() => { 
                if(!runningBars[bar2]) {
                    WaitCommandStream.Instance.QueueCommand(bar2Command);
                    runningBars[bar2] = true;
                }
            });
            cancelBar2.onClick.AddListener(() => { 
                if(runningBars[bar2]) {
                    WaitCommandStream.Instance.CancelCommand(bar2Command);
                }
            });            
            startBar3.onClick.AddListener(() => { 
                if(!runningBars[bar3]) {
                    WaitCommandStream.Instance.QueueCommand(bar3Command);
                    runningBars[bar3] = true;
                }
            });  
            cancelBar3.onClick.AddListener(() => { 
                if(runningBars[bar3]) {
                    WaitCommandStream.Instance.CancelCommand(bar3Command);
                }
            });          
            startBar4.onClick.AddListener(() => { 
                if(!runningBars[bar4]) {
                    WaitCommandStream.Instance.QueueCommand(bar4Command);
                    runningBars[bar4] = true;
                }
            });
            cancelBar4.onClick.AddListener(() => { 
                if(runningBars[bar4]) {
                    WaitCommandStream.Instance.CancelCommand(bar4Command);
                }
            });
            startAll.onClick.AddListener(() =>
            {
                if(!runningBars[bar1]) {
                    WaitCommandStream.Instance.QueueCommand(bar1Command);
                    runningBars[bar1] = true;
                }
                if(!runningBars[bar2]) {
                    WaitCommandStream.Instance.QueueCommand(bar2Command);
                    runningBars[bar2] = true;
                }
                if(!runningBars[bar3]) {
                    WaitCommandStream.Instance.QueueCommand(bar3Command);
                    runningBars[bar3] = true;
                }
                if(!runningBars[bar4]) {
                    WaitCommandStream.Instance.QueueCommand(bar4Command);
                    runningBars[bar4] = true;
                }
            });
            cancelAll.onClick.AddListener(() =>
            {
                if(runningBars[bar1]) {
                    WaitCommandStream.Instance.CancelCommand(bar1Command);
                }
                if(runningBars[bar2]) {
                    WaitCommandStream.Instance.CancelCommand(bar2Command);
                }
                if(runningBars[bar3]) {
                    WaitCommandStream.Instance.CancelCommand(bar3Command);
                }
                if(runningBars[bar4]) {
                    WaitCommandStream.Instance.CancelCommand(bar4Command);
                }
            });
        }
    }
}
