using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandExtend : MonoBehaviour {

    
    //Set Bool for trigger on VR Controller
    private bool isIndexTriggered = false; //IndexTrigger is index-finger's Trigger//
    private bool isHandTriggered = false; //HandTrigger is Grip's Trigger//

    

    // Use this for initialization
    public void Start () {
        
	}
	
	// Update is called once per frame
	public void Update () {
        if (isIndexTriggered)
        {
            OnIndexTriggered();
        }
        if (isHandTriggered)
        {
            OnHandTriggered();
        }
	}

    public virtual void OnIndexTriggered()
    {

    }

    public virtual void OnHandTriggered()
    {

    }

    public void setIndexTrigger(bool trig)
    {
        isIndexTriggered = trig;
    }

    public void setHandTrigger(bool trig)
    {
        isHandTriggered = trig;
    }

    public bool GetIndexTrigger()
    {
        return isIndexTriggered;
    }
    public bool GetHandTrigger()
    {
        return isHandTriggered;
    }
}
