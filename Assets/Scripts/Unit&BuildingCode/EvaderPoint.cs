using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaderPoint : MonoBehaviour
{
    public enum EvaderDirection
    {
        Left = 0,
        Right = 1,
    }

    private float passedDistance;
    private float checkDistance;
    private bool isChecking;
    private bool deadEnd = false;
    private EvaderDirection evdDir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (getCheck())
        {
            Vector3 vecDir = new Vector3();
            if (evdDir == EvaderDirection.Left)
            {
                vecDir = Vector3.left;
            }
            else
            {
                vecDir = Vector3.right;
            }
            RaycastHit hit;
            GameObject hitObject = null;
            if (Physics.Raycast(transform.position, transform.TransformDirection(vecDir), out hit, checkDistance / 2))
            {
                hitObject = hitObject = hit.transform.gameObject;
                Vector3 adjustedPos = new Vector3(hitObject.transform.position.x, transform.position.y, hitObject.transform.position.z);
                if (Vector3.Distance(adjustedPos, transform.position) <= checkDistance / 2)
                {
                    setCheck(false);
                    deadEnd = true;
                }
            } 
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, checkDistance))
            {
                hitObject = hitObject = hit.transform.gameObject;
                Vector3 adjustedPos = new Vector3(hitObject.transform.position.x, transform.position.y, hitObject.transform.position.z);
                if (Vector3.Distance(adjustedPos, transform.position) > checkDistance + 1)
                {
                    setCheck(false);
                }
            }
            else
            {
                setCheck(false);
            }
            if (getCheck())
            {
                passedDistance += 1;
                if (evdDir == EvaderDirection.Right)
                {
                    transform.position = transform.position + transform.right;
                }
                else
                {
                    transform.position = transform.position - transform.right;
                }
                
            }
        }
    }

    public void StartCheck(float checkDis, EvaderDirection dir)
    {
        checkDistance = checkDis;
        passedDistance = 0;
        evdDir = dir;
        deadEnd = false;
        setCheck(true);
    }

    public void setCheck(bool che)
    {
        isChecking = che;
    }

    public bool getCheck()
    {
        return isChecking;
    }

    public float GetDistance()
    {
        return passedDistance;
    }
    public bool isDeadEnd()
    {
        return deadEnd;
    }
}
