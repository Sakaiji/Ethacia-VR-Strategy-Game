using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHands : VRHandExtend {

    [SerializeField] private GameObject pointer;
    private float coolDown = .5f;
    private float coolNow;

    private void Start()
    {
        base.Start();
    }

    private void Update()
    {
        base.Update();
        RaycastHit hit;
        if (Physics.Raycast(pointer.transform.position, pointer.transform.TransformDirection(Vector3.forward), out hit))
        {
            Debug.DrawRay(pointer.transform.position, pointer.transform.TransformDirection(Vector3.forward), Color.blue);
            GameObject hitObject = hit.transform.gameObject;
            MenuUI menObj = hitObject.GetComponent<MenuUI>();
            if (menObj)
            {
                Debug.Log("Point At Button");
                menObj.GetSelected();
            }
        }
        if (coolNow > 0)
        {
            coolNow -= Time.deltaTime;
        }
    }

    public override void OnIndexTriggered()
    {
        base.OnIndexTriggered();
        if (coolNow <= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(pointer.transform.position, pointer.transform.TransformDirection(Vector3.forward), out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                MenuUI menObj = hitObject.GetComponent<MenuUI>();
                if (menObj)
                {
                    menObj.GetClicked(this);
                    Debug.Log("Click Button");
                }
                else
                {
                    Debug.Log("Click Air");
                }
            }
            coolNow = coolDown;
        }
        
    }

    public override void OnHandTriggered()
    {
        base.OnHandTriggered();

    }


}
