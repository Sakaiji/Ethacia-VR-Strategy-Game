using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private GameObject barGauge;
    [SerializeField] private GameObject barBG;
    private float barSize;

    private float localY;

    private float progress;

	// Use this for initialization
	void Start () {
       barSize = barBG.transform.localScale.y;
        barGauge.transform.localScale = barBG.transform.localScale;
        localY = barBG.transform.localScale.y;
        SetProgress(1);
    }
	


	// Update is called once per frame
	void Update () {
        
    }

    public void SetProgress(float pro)
    {
        progress = pro;
        //Debug.Log("Progress : " + 100 * (progress) + "%");
        this.barGauge.transform.localScale = new Vector3(this.barBG.transform.localScale.x, this.barBG.transform.localScale.y * this.progress, this.barBG.transform.localScale.z);
        //barGauge.transform.localScale = barBG.transform.localScale - new Vector3(0,barBG.transform.localScale.y * progress,0);
    }
    public void AddProgress(float pro)
    {
        progress += pro;
        barGauge.transform.localScale += new Vector3(0, pro, 0);
    }
    public void RemoveProgress(float pro)
    {
        progress -= pro;
        barGauge.transform.localScale -=  new Vector3(0, pro, 0);
    }

    public float GetProgress()
    {
        return progress;
    }
}
