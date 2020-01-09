
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMovement : MonoBehaviour {

    public enum HandPosition
    {
        LeftHand = 0,
        RightHand = 1,
        AI = 2
    }

    public HandPosition handNow = 0;
    public float speed = 5.0f;
    [SerializeField] private GameObject head;
    private float playerScale;
    [SerializeField] private float zoomSpeed;

    // Use this for initialization
    void Start () {
        playerScale = this.transform.localScale.x;
    }

    // Update is called once per frame
    void Update () {
        if (PlayerPrefs.GetInt("HandNow") == 0)
        {
            handNow = HandPosition.LeftHand;
        }else if (PlayerPrefs.GetInt("HandNow") == 1)
        {
            handNow = HandPosition.RightHand;
        }

        float movementX;
        float movementZ;

        float zoomingZ;

        if (handNow == HandPosition.LeftHand)
        {
            movementX = Input.GetAxisRaw("PrimaryThumbstickHorizontal");
            movementZ = -Input.GetAxisRaw("PrimaryThumbstickVertical");
            zoomingZ = Input.GetAxisRaw("SecondaryThumbstickVertical");
        }

        else if(handNow == HandPosition.RightHand)
        {
            movementX = Input.GetAxisRaw("SecondaryThumbstickHorizontal");
            movementZ = -Input.GetAxisRaw("SecondaryThumbstickVertical");
            zoomingZ = Input.GetAxisRaw("PrimaryThumbstickVertical");
        }
        else
        {
            movementX = 0;
            movementZ = 0;
            zoomingZ = 0;
        }

        if (handNow != HandPosition.AI)
        {
            Vector3 movement = new Vector3(movementX, 0, movementZ);
            movement = Vector3.ClampMagnitude(movement, speed);

            Quaternion tmp = head.transform.rotation;
            head.transform.eulerAngles = new Vector3(0, head.transform.eulerAngles.y, 0);
            movement = head.transform.TransformDirection(movement);
            head.transform.rotation = tmp;

            //movement *= Time.deltaTime;
            //movement = transform.TransformDirection(movement);

            transform.Translate(movement);

            if (movementX > 0 || movementZ > 0)
            {
                //Debug.Log("X : " + movementX);
                //Debug.Log("Z : " + movementZ);
            }

            if (zoomingZ != 0)
            {

                if (zoomingZ > 0 && playerScale < 20f)
                {
                    playerScale += zoomSpeed * Time.deltaTime;
                    ZoomMove(-zoomingZ);
                }
                else if (zoomingZ < 0 && playerScale > 1f)
                {
                    playerScale -= zoomSpeed * Time.deltaTime;
                    ZoomMove(-zoomingZ);
                }


                this.transform.localScale = new Vector3(playerScale, playerScale, playerScale);
            }
        
        }
    }

    void ZoomMove(float zoomingZ)
    {
        Vector3 movementZoom = new Vector3(0, 0, zoomingZ);
        movementZoom = Vector3.ClampMagnitude(movementZoom, zoomSpeed / 2);
        Quaternion tmp = head.transform.rotation;
        head.transform.eulerAngles = new Vector3(0, head.transform.eulerAngles.y, 0);
        movementZoom = head.transform.TransformDirection(movementZoom);
        head.transform.rotation = tmp;
        transform.Translate(movementZoom);
    }
}
