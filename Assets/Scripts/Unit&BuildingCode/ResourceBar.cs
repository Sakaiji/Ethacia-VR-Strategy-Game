using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBar : MonoBehaviour {

    [SerializeField] private TextMesh etherText;

    private BuildingSystem build;
    private UnitStatSystem stat;

	// Use this for initialization
	void Start () {
        build = GetComponent<BuildingSystem>();
        stat = GetComponent<UnitStatSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        stat = GetComponent<UnitStatSystem>();
        VRPlayerIndicator player = null;
        foreach (GameObject plays in GameObject.FindGameObjectsWithTag("Player"))
        {
            VRPlayerIndicator ind = plays.GetComponent<VRPlayerIndicator>();
            if (ind.GetPlayerNumber() == stat.GetOwner())
            {
                player = ind;
            }
        }
        etherText.text = "Ether : " + player.GetEther();
    }
}
