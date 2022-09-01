using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    public class WaitCommandFactory : MonoBehaviour
    {
        [SerializeField, Tooltip("the increment in which the async commands will run in milliseconds"), Range(1, 1000)] int timeStep;
        [SerializeField] ProgressBar bar1;
        [SerializeField] Button startBar1;
        [SerializeField] Button cancelBar1;
        [SerializeField, Tooltip("runtime in seconds")] float runtimeBar1;
        [SerializeField] ProgressBar bar2;
        [SerializeField] Button startBar2;
        [SerializeField] Button cancelBar2;
        [SerializeField, Tooltip("runtime in seconds")] float runtimeBar2;
        [SerializeField] ProgressBar bar3;
        [SerializeField] Button startBar3;  
        [SerializeField] Button cancelBar3;
        [SerializeField, Tooltip("runtime in seconds")] float runtimeBar3;
        [SerializeField] ProgressBar bar4;
        [SerializeField] Button startBar4;
        [SerializeField] Button cancelBar4;
        [SerializeField, Tooltip("runtime in seconds")] float runtimeBar4;
        [SerializeField] Button startAll;
        [SerializeField] Button cancelAll;

        private WaitForSecondsCommand bar1Command;
        private WaitForSecondsCommand bar2Command;
        private WaitForSecondsCommand bar3Command;
        private WaitForSecondsCommand bar4Command;


        Dictionary<ProgressBar, bool> runningBars;

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
        
        public void ChangeTimeStep(int value)
        {
            timeStep = value;
            for (int i = 1; i <= 4; i++)
            {
                SetBarCommand(i);
            };
        }
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

        // Start is called before the first frame update
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

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
