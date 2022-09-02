using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadSapphicGames.CommandPattern.SimpleDemo {
    /// <summary>
    /// The ui element to show the progress of the async command, functions similarly to HealthBar
    /// </summary>
    public class ProgressBar : MonoBehaviour {
        /// <summary>
        /// The filled part of the bar
        /// </summary>
        [SerializeField] private RectTransform filledBar;
        /// <summary>
        /// the portion of the bar that the filled part should fill
        /// </summary>
        private float filledPortion;
        /// <summary>
        /// the bar's size
        /// </summary>
        float barSize;
        /// <summary>
        /// Waits a frame then gets the size of the bar.
        /// This is needed because these elements are children of a layout group which takes a frame to update the size of the elements it is controlling.
        /// </summary>
        IEnumerator GetSizeAtNextFrame()
        {
            yield return new WaitForEndOfFrame();
            barSize = GetComponent<RectTransform>().rect.width;
            SetFilledPortion(0);
            UpdateBar();
        }
        /// <summary>
        /// Does the bar need to be updated
        /// </summary>
        private bool needsUpdate;
        /// <summary>
        /// updates the amount of the bar the filled part takes up then sets needs update to false
        /// </summary>
        private void UpdateBar()
        {
            var offset = barSize * (1 - filledPortion);
            filledBar.offsetMax = new Vector2(-offset, filledBar.offsetMax.y);
            needsUpdate = false;

        }
        /// <summary>
        /// Sets the value of filledPortion clamped between 0 and 1 then sets needsUpdate to true
        /// </summary>
        /// <param name="value">the value to set filledPortion to</param>
        public void SetFilledPortion(float value)
        {
            filledPortion = Mathf.Clamp(value, 0, 1);
            needsUpdate = true;
        }
        /// <summary>
        /// Runs the the GetSizeAtNextFrame coroutine
        /// </summary>
        void Start() {
            StartCoroutine(GetSizeAtNextFrame());
        }

        /// <summary>
        /// runs UpdateBar() if needsUpdate is true
        /// </summary>
        void Update()
        {
            if (needsUpdate)
            {
                UpdateBar();
            }
        }
    }
}