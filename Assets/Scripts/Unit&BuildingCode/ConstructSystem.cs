using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructSystem : MonoBehaviour {

    [SerializeField] private BuildingSystem[] buildUnit;
    [SerializeField] private BuildingUI[] buildUI;
    [SerializeField] private ConstructDummy[] dummy;
    [SerializeField] private int[] buildCost;
    [SerializeField] private BuildingUI constructUI;

    //VR UI
    [SerializeField] private GameObject baseUI;

    private bool constructShow;
    private BuildingSystem build;
    private UnitStatSystem stat;

    // Use this for initialization
    void Start () {
        build = GetComponent<BuildingSystem>();
        stat = GetComponent<UnitStatSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (build.CheckFinish() && build.CheckSelect())
        {
            constructUI.gameObject.SetActive(true);
        }
        else
        {
            constructUI.gameObject.SetActive(false);
        }

        if (constructUI.CheckClick())
        {
            constructShow = !constructShow;
            constructUI.SetClick(false);
        }

        if (build.CheckSelect() && build.CheckFinish() && constructShow)
        {
            baseUI.SetActive(true);
            foreach(BuildingUI showUI in buildUI)
            {
                showUI.gameObject.SetActive(true);
            }
            Construction();
        }
        else
        {
            constructShow = false;
            baseUI.SetActive(false);
        }
	}

    void Construction()
    {
        VRPlayerIndicator player = null;
        foreach (GameObject plays in GameObject.FindGameObjectsWithTag("Player"))
        {
            VRPlayerIndicator ind = plays.GetComponent<VRPlayerIndicator>();
            if (ind.GetPlayerNumber() == stat.GetOwner())
            {
                player = ind;
            }
        }
        for (int i=0;i<buildUI.Length;i++)
        {
            
            if (buildUI[i].CheckClick() && player.GetEther() >= buildCost[i])
            {
                InGameFirstHand hand = buildUI[i].GetHandClicked();
                ConstructDummy dumNow = Instantiate(dummy[i], this.transform.position, this.transform.rotation) as ConstructDummy;
                dumNow.StartDummy(buildUnit[i].GetModel(), buildUnit[i], stat.GetOwner(), buildCost[i]);
                hand.SelectConstruction(dumNow);
                buildUI[i].SetClick(false);
            }
        }
    }

    public BuildingSystem GetBuildingToConstruct(int selec)
    {
        return buildUnit[selec];
    }
}
