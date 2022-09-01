using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform filledBar;
    private float filledPortion;
    float barSize;
    IEnumerator GetSizeAtNextFrame(){
        yield return new WaitForEndOfFrame();
        barSize = GetComponent<RectTransform>().rect.width;
        SetFilledPortion(0);
        UpdateBar();
    }
    private bool needsUpdate;
    private void UpdateBar() {
        var offset = barSize * (1 - filledPortion);
        filledBar.offsetMax = new Vector2(-offset, filledBar.offsetMax.y);
        needsUpdate = false;

    }
    public void SetFilledPortion(float value) {
        filledPortion = Mathf.Clamp(value, 0, 1);
        needsUpdate = true;
    }
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(GetSizeAtNextFrame());
    }

    // Update is called once per frame
    void Update()
    {
        if(needsUpdate) {
            UpdateBar();
        }
    }
}
