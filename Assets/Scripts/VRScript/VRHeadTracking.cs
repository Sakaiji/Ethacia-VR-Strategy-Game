using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRHeadTracking : MonoBehaviour {

    public GameObject head;
    public GameObject camera;

    [SerializeField] private bool isAI;

	// Use this for initialization
	void Start () {
		
	}

    public GameObject GetHead()
    {
        return head;
    }

    // Update is called once per frame
    void Update () {
        if (!isAI)
        {
            Quaternion HeadRotation = InputTracking.GetLocalRotation(XRNode.Head);

            Vector3 headPos = InputTracking.GetLocalPosition(XRNode.Head);
            head.transform.localPosition = headPos;

            //camera.transform.localRotation = Quaternion.Euler(HeadRotation.eulerAngles.x, HeadRotation.eulerAngles.y, HeadRotation.eulerAngles.z);

            transform.localRotation = Quaternion.Euler(0, camera.transform.localEulerAngles.y, 0);
        }
        
    }

    
}
