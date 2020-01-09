using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructDummy : MonoBehaviour {

    [SerializeField] private Material buildableMat;
    [SerializeField] private Material unbuildableMat;

    [SerializeField] private bool needRange;

    [SerializeField] private GameObject constructSound;

    private BuildingSystem buildingCon;
    private GameObject models;
    private int thisOwner;
    private int cost;

    private bool buildable;
    private int collideNow;
    private bool onGround;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int inRange = 0;
        if (needRange)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 40);
            int i = 0;
            while (i < hitColliders.Length)
            {
                UnitStatSystem buildingStat = hitColliders[i].GetComponent<UnitStatSystem>();
                if (buildingStat && buildingStat.GetAlive() && buildingStat.GetUnitType()==UnitStatSystem.UnitObjectType.Building && buildingStat.GetOwner()==thisOwner)
                {
                    inRange++;
                }
                if (buildingStat && buildingStat.GetAlive() && buildingStat.GetUnitType() == UnitStatSystem.UnitObjectType.Building && buildingStat.GetOwner() != thisOwner)
                {
                    inRange--;
                }
                i++;
            }
        }
        else
        {
            inRange = 1;
        }
        buildable = (onGround) && (collideNow <= 0) && (inRange>=1);
        Debug.Log("I have collicion at : " + collideNow);
        if (buildable)
        {
            foreach (Transform child in models.transform)
            {
                GameObject childOBJ = child.gameObject;
                childOBJ.GetComponent<Renderer>().material = buildableMat;
            }
        }
        else
        {
            foreach (Transform child in models.transform)
            {
                GameObject childOBJ = child.gameObject;
                childOBJ.GetComponent<Renderer>().material = unbuildableMat;
            }
        }
	}

    public bool IsBuildable()
    {
        return buildable;
    }

    public void ConfirmBuild()
    {
        BuildingSystem buildNow = Instantiate(buildingCon, transform.position, transform.rotation) as BuildingSystem;
        buildNow.StartToBuild(thisOwner);
        Instantiate(constructSound, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void StartDummy(GameObject mod, BuildingSystem bui, int owner, int cos)
    {
        buildingCon = bui;
        thisOwner = owner;
        cost = cos;
        VRPlayerIndicator player = null;
        foreach (GameObject plays in GameObject.FindGameObjectsWithTag("Player"))
        {
            VRPlayerIndicator ind = plays.GetComponent<VRPlayerIndicator>();
            if (ind.GetPlayerNumber() == thisOwner)
            {
                player = ind;
            }
        }
        player.ChangeEther(-cost);
        models = Instantiate(mod, this.transform) as GameObject;
        
    }

    public void CancelBuild()
    {
        VRPlayerIndicator player = null;
        foreach (GameObject plays in GameObject.FindGameObjectsWithTag("Player"))
        {
            VRPlayerIndicator ind = plays.GetComponent<VRPlayerIndicator>();
            if (ind.GetPlayerNumber() == thisOwner)
            {
                player = ind;
            }
        }
        player.ChangeEther(cost);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something Enter");
        GameObject colOBJ = other.gameObject;
        if (colOBJ.tag == "Terrain")
        {

        }
        else
        {
            collideNow += 1;
        }
        
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Something Enter");
        GameObject colOBJ = col.gameObject;
        if (colOBJ.tag == "Terrain")
        {

        }
        else
        {
            collideNow += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Something Exit");
        GameObject colOBJ = other.gameObject;
        if (colOBJ.tag == "Terrain")
        {

        }
        else
        {
            collideNow -= 1;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        Debug.Log("Something Exit");
        GameObject colOBJ = col.gameObject;
        if (colOBJ.tag == "Terrain")
        {

        }
        else
        {
            collideNow -= 1;
        }
    }

    public void SetGround(bool ong)
    {
        onGround = ong;
    }

}
