using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRLeftRightHandTracking : MonoBehaviour {

    public enum HandPosition
    {
        LeftHand = 0,
        RightHand = 1
    }

    public HandPosition handNow = 0;
    public bool isUse = false;
    public float angleCal = 0;

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        if (isUse)
        {
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
            transform.Rotate(angleCal, 0, 0);


        }
	}

}
