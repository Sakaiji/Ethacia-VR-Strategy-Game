using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAISystem : MonoBehaviour
{
    private BuildingSystem aiStronghold;
    private BuildingSystem aiBarrack;
    private BuildingSystem aiStable;

    private List<WorkerUnit> workerList;
    private List<UnitGroupSystem> armyList;
    private List<EtherDeposit> etherClose;

    private VRPlayerIndicator playerStat;

    private float armyAICool = 10f;
    private float armyCoolNow;

    private float actionCool = 5f;

    [SerializeField] private GameObject aiPointDummy;
    // Start is called before the first frame update
    void Start()
    {
        playerStat = GetComponent<VRPlayerIndicator>();
        workerList = new List<WorkerUnit>();
        armyList = new List<UnitGroupSystem>();
        etherClose = new List<EtherDeposit>();
    }

    // Update is called once per frame
    void Update()
    {
        armyCoolNow -= Time.deltaTime;
        actionCool -= Time.deltaTime;
        if (actionCool <= 0)
        {
            actionCool = 5;
            Debug.Log("AI " + playerStat.GetPlayerNumber() + " have worker : " + workerList.Count);
            int playerNum = playerStat.GetPlayerNumber();
            if (!aiStronghold)
            {
                Debug.Log("AI " + playerStat.GetPlayerNumber() + " find stronghold.");
                BuildingSystem[] bui = FindObjectsOfType<BuildingSystem>();
                foreach (BuildingSystem buiDum in bui)
                {
                    ConstructSystem con = buiDum.GetComponent<ConstructSystem>();
                    if (con && buiDum.GetOwner() == playerNum)
                    {
                        aiStronghold = buiDum;
                    }
                }
            }
            else
            {
                CheckStronghold();
            }
            if (aiBarrack)
            {
                CheckBarrack();
            }
            if (aiStable)
            {
                CheckStable();
            }

            
        }
        if (armyList.Count > 0 && armyCoolNow <= 0)
        {
            CheckArmy();
            armyCoolNow = armyAICool;
        }

        WorkerUnit[] wor = FindObjectsOfType<WorkerUnit>();
        foreach (WorkerUnit workerDum in wor)
        {
            if (workerDum.GetOwner() == playerStat.GetPlayerNumber() && !workerList.Contains(workerDum))
            {
                etherClose.Clear();
                Collider[] hitColliders = Physics.OverlapSphere(aiStronghold.transform.position, 30);
                int i = 0;
                while (i < hitColliders.Length)
                {
                    EtherDeposit etherOBJ = hitColliders[i].GetComponent<EtherDeposit>();
                    if (etherOBJ)
                    {
                        etherClose.Add(etherOBJ);
                    }
                    i++;
                }
                if (etherClose.Count > 0)
                {
                    EtherDeposit etherTarget = etherClose[Random.Range(0, etherClose.Count)].GetComponent<EtherDeposit>();
                    workerDum.GoHarvest(etherTarget);
                    workerList.Add(workerDum);
                }

            }
        }

    }

    private void CheckStronghold()
    {
        if (aiStronghold.CheckFinish() && workerList.Count < 3 && !aiStronghold.GetComponent<UnitTraining>().IsTraining() && playerStat.GetEther() >= 100)
        {
            aiStronghold.GetComponent<UnitTraining>().TrainOrder(0);
        }
        if (aiStronghold.CheckFinish() && !aiBarrack && playerStat.GetEther() >= 850)
        {
            BuildingSystem barrackDummy = aiStronghold.GetComponent<ConstructSystem>().GetBuildingToConstruct(0);
            GameObject posDummy = Instantiate(aiPointDummy, aiStronghold.transform.position, aiStronghold.transform.rotation) as GameObject;
            posDummy.transform.Rotate(new Vector3(0, Random.Range(110f,150f), 0));
            posDummy.transform.position += posDummy.transform.forward * Random.Range(25f,30f);
            aiBarrack = Instantiate(barrackDummy, posDummy.transform.position, aiStronghold.transform.rotation) as BuildingSystem;
            aiBarrack.transform.Rotate(new Vector3(0, 180, 0));
            aiBarrack.StartToBuild(playerStat.GetPlayerNumber());
            playerStat.SetEther(playerStat.GetEther() - 850);
        }
        if (aiStronghold.CheckFinish() && !aiStable && playerStat.GetEther() >= 1000)
        {
            BuildingSystem stableDummy = aiStronghold.GetComponent<ConstructSystem>().GetBuildingToConstruct(2);
            GameObject posDummy = Instantiate(aiPointDummy, aiStronghold.transform.position, aiStronghold.transform.rotation) as GameObject;
            posDummy.transform.Rotate(new Vector3(0, Random.Range(200f, 240f), 0));
            posDummy.transform.position += posDummy.transform.forward * Random.Range(25f, 30f);
            aiStable = Instantiate(stableDummy, posDummy.transform.position, aiStronghold.transform.rotation) as BuildingSystem;
            aiStable.transform.Rotate(new Vector3(0, 180, 0));
            aiStable.StartToBuild(playerStat.GetPlayerNumber());
            playerStat.SetEther(playerStat.GetEther() - 800);
        }
    }

    private void CheckBarrack()
    {
        Debug.Log("AI " + playerStat.GetPlayerNumber() + " build barrack.");
        if (aiBarrack.CheckFinish() && armyList.Count < 6)
        {
            UnitTraining barrackTrain = aiBarrack.GetComponent<UnitTraining>();
            barrackTrain.TrainOrder(Random.Range(0, 3));
        }

        Debug.Log("AI " + playerStat.GetPlayerNumber() + " find army.");
        UnitGroupSystem[] groupF = FindObjectsOfType<UnitGroupSystem>();
        foreach (UnitGroupSystem groupDum in groupF)
        {
            if (groupDum.GetOwner() == playerStat.GetPlayerNumber() && !armyList.Contains(groupDum))
            {
                armyList.Add(groupDum);
                Debug.Log("AI " + playerStat.GetPlayerNumber() + " add 1 army to group.");
            }
        }
        Debug.Log("AI " + playerStat.GetPlayerNumber() + " have army : " + armyList.Count);
    }

    private void CheckStable()
    {
        if (aiStable.CheckFinish() && armyList.Count < 6)
        {
            UnitTraining stableTrain = aiStable.GetComponent<UnitTraining>();
            stableTrain.TrainOrder(0);
        }
    }

    private void CheckArmy()
    {
        foreach(UnitGroupSystem army in armyList)
        {
            if (!army)
            {
                armyList.Remove(army);
                Destroy(army.gameObject);
            }
        }

        foreach (UnitGroupSystem army in armyList)
        {
            GameObject target = null;
            Collider[] hitColliders = Physics.OverlapSphere(army.transform.position, 50);
            int i = 0;
            while (i < hitColliders.Length)
            {
                UnitStatSystem tarStat = hitColliders[i].GetComponent<UnitStatSystem>();
                if (tarStat && tarStat.GetAlive() && tarStat.GetOwner() != playerStat.GetPlayerNumber() && !AllianceSystem.CheckAlly(playerStat.GetPlayerNumber(), tarStat.GetOwner()))
                {
                    target = tarStat.gameObject;
                }
                else 
                {
                    UnitGroupSystem tarGroup = hitColliders[i].GetComponent<UnitGroupSystem>();
                    if (tarGroup && tarGroup.GetOwner() != playerStat.GetPlayerNumber() && !AllianceSystem.CheckAlly(playerStat.GetPlayerNumber(), tarGroup.GetOwner()))
                    {
                        target = tarGroup.gameObject;
                    }
                }
                i++;
            }
            if (target && !army.HaveTarget())
            {
                army.SetAutoAttaack(true);
                army.SetTarget(target);
            }
            else
            {
                if (armyList.Count >= 3)
                {
                    UnitStatSystem[] tarG = FindObjectsOfType<UnitStatSystem>();
                    foreach (UnitStatSystem tarDum in tarG)
                    {
                        if (tarDum && tarDum.GetAlive() && tarDum.GetOwner() != playerStat.GetPlayerNumber() && !AllianceSystem.CheckAlly(playerStat.GetPlayerNumber(), tarDum.GetOwner()))
                        {
                            target = tarDum.gameObject;
                        }
                    }
                    if (target)
                    {
                        army.GetComponent<UnitMove>().OrderMove(target.transform.position);
                    }
                }
                else
                {
                    GameObject posDummy = Instantiate(aiPointDummy, aiStronghold.transform.position, aiStronghold.transform.rotation) as GameObject;
                    posDummy.transform.Rotate(new Vector3(0, Random.Range(135f, 225f), 0));
                    posDummy.transform.position += posDummy.transform.forward * Random.Range(40, 50);
                    army.GetComponent<UnitMove>().OrderMove(posDummy.transform.position);
                    //army.GetComponent<UnitMove>().OrderMove(Vector3.one);
                }
            }
            

        }
    }
}
