using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStart : MonoBehaviour {

    //public BuildingSystem[] BuildingTest;
    public VRPlayerIndicator player1;
    public BuildingSystem stronghold;
    public GameObject startPoint;

    public BuildingSystem[] startingStrong;

	// Use this for initialization
	void Start () {
        //foreach (BuildingSystem build in BuildingTest)
        //{
        //    build.StartToBuild(1);
        //}
        player1.SetEther(5000);
        AllianceSystem.SetAlly(1, 3, true);
        //BuildingSystem strong = Instantiate(stronghold, startPoint.transform.position, startPoint.transform.rotation) as BuildingSystem;
        //strong.StartToBuild(1);
        foreach (BuildingSystem bui in startingStrong)
        {
            bui.StartToBuild(1);
        }
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
