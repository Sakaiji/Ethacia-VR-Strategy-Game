using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRSuperHand : MonoBehaviour {

    [SerializeField] private VRHandExtend Hand; //OBJ In hand.//

    //Hand Positions for setting.
    private enum HandPosition
    {
        LeftHand = 0,
        RightHand = 1
    }
    //Make sure to dependence the hand.
    [SerializeField]private HandPosition handNow = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Call Hand Tracking function.
        VRTracking();
        if (Hand != null)
        {
            //If you have hand ojbect : call Controller's Trigger check.
            VRTriggerCheck();
        }
	}

    private void VRTracking()
    {
        //This function make superhand object track controller position in real-time.//

        Vector3 joyPosition;
        Quaternion joyRotation;
        if (handNow == HandPosition.LeftHand)
        {
            joyPosition = InputTracking.GetLocalPosition(XRNode.LeftHand);
            joyRotation = InputTracking.GetLocalRotation(XRNode.LeftHand);
        }
        else
        {
            joyPosition = InputTracking.GetLocalPosition(XRNode.RightHand);
            joyRotation = InputTracking.GetLocalRotation(XRNode.RightHand);
        }
        transform.localPosition = joyPosition;
        transform.localRotation = joyRotation;
    }

    private void VRTriggerCheck()
    {
        string indexTrigger = "";
        string handTrigger = "";
        if (handNow == HandPosition.LeftHand)
        {
            indexTrigger = "PrimaryIndexTrigger";
            handTrigger = "PrimaryHandTrigger";
        }
        else
        {
            indexTrigger = "SecondaryIndexTrigger";
            handTrigger = "SecondaryHandTrigger";
        }
        //---VRButtonCheck---//

        //IndexTriggerTrue
        if (Input.GetAxisRaw(indexTrigger) > 0.5)
        {
            Hand.setIndexTrigger(true);
        }
        //IndexTriggerFalse
        if (Input.GetAxisRaw(indexTrigger) <= 0.5)
        {
            Hand.setIndexTrigger(false);
        }
        //HandTriggerTrue-False
        if(Input.GetAxisRaw(handTrigger) >= 0.1)
        {
            Hand.setHandTrigger(true);
        }
        else
        {
            Hand.setHandTrigger(false);
        }

    }

    public void SetHandObj(GameObject obj)
    {
        obj.transform.parent = this.gameObject.transform;
        Hand = obj.GetComponent<VRHandExtend>();
    }

    public void RemoveHandObj()
    {
        Hand.gameObject.transform.parent = null;
        Hand = null;
    }
}
