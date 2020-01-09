using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTraining : MonoBehaviour {

    //Make sure to match varriable
    [SerializeField] private GameObject[] trainUnit;
    [SerializeField] private BuildingUI[] trainUI;
    [SerializeField] private float[] trainTime;
    [SerializeField] private int[] trainCost;
    [SerializeField] private GameObject baseUI;
    [SerializeField] private GameObject spawnPoint;

    [SerializeField] private ProgressBarUI barUI;
    [SerializeField] private BuildingUI cancelUI;

    private bool isTraining;
    private GameObject unitTrainNow;
    private float trainTimeMax;
    private float trainTimeNow;

    private BuildingSystem build;
    private UnitStatSystem stat;

	// Use this for initialization
	void Start () {
        build = GetComponent<BuildingSystem>();
        stat = GetComponent<UnitStatSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (build.CheckSelect())
        {
            if (build.CheckFinish())
            {
                baseUI.SetActive(true);
                foreach (BuildingUI showUI in trainUI)
                {
                    showUI.gameObject.SetActive(true);
                }
                if (isTraining)
                {
                    cancelUI.gameObject.SetActive(true);
                }
                else
                {
                    cancelUI.gameObject.SetActive(false);
                }
            }
            else
            {
                baseUI.SetActive(false);
                foreach (BuildingUI showUI in trainUI)
                {
                    showUI.gameObject.SetActive(false);
                }
            }

        }
        else
        {
            baseUI.SetActive(false);
        }
        TrainingUnit();
        
    }

    void TrainingUnit()
    {
        if (isTraining)
        {
            cancelUI.gameObject.SetActive(true);
            barUI.gameObject.SetActive(true);
            if (cancelUI.CheckClick())
            {
                cancelUI.SetClick(false);
                isTraining = false;
            }
            if (trainTimeNow >= trainTimeMax)
            {
                isTraining = false;
                GameObject unit = Instantiate(unitTrainNow, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
                UnitGroupSystem group = unit.GetComponent<UnitGroupSystem>();
                if (group != null)
                {
                    group.SetOwner(build.GetOwner());
                }
                WorkerUnit worker = unit.GetComponent<WorkerUnit>();
                if (worker != null)
                {
                    worker.SetOwner(this.build.GetOwner());
                }
                
            }
            trainTimeNow += Time.deltaTime;
            barUI.SetProgress(trainTimeNow/trainTimeMax);
        }
        else
        {
            cancelUI.gameObject.SetActive(false);
            barUI.gameObject.SetActive(false);
            VRPlayerIndicator player = null;
            foreach (GameObject plays in GameObject.FindGameObjectsWithTag("Player"))
            {
                VRPlayerIndicator ind = plays.GetComponent<VRPlayerIndicator>();
                if (ind.GetPlayerNumber() == stat.GetOwner())
                {
                    player = ind;
                }
            }
            for (int i=0; i < trainUI.Length; i++)
            {  
                if (trainUI[i].CheckClick() && player.GetEther() >= trainCost[i])
                {
                    isTraining = true;
                    unitTrainNow = trainUnit[i];
                    trainTimeMax = trainTime[i];
                    trainTimeNow = 0;
                    player.ChangeEther(-trainCost[i]);
                    barUI.SetProgress(0);
                    trainUI[i].SetClick(false);
                    Debug.Log("Start Training!!!");
                }
                
            }
        }
    }

    public bool IsTraining()
    {
        return isTraining;
    }

    public void TrainOrder(int selec)
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
        if (!isTraining && player.GetEther() >= trainCost[selec])
        {
            isTraining = true;
            unitTrainNow = trainUnit[selec];
            trainTimeMax = trainTime[selec];
            trainTimeNow = 0;
            player.ChangeEther(-trainCost[selec]);
            barUI.SetProgress(0);
            trainUI[selec].SetClick(false);
            Debug.Log("Start Training!!!");
        }

    }
}
