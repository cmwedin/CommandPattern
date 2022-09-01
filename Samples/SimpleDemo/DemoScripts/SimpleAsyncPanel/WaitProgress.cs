using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo
{
    public class WaitProgress : IProgress<float>
    {
        public event Action<float> ProgressChanged;
        public void Report(float value)
        {
            ProgressChanged.Invoke(Mathf.Clamp(value,0,1));
        }
    }
}