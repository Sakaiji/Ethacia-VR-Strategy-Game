using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkirmishStartSystem : MonoBehaviour {

    private int maxPlayer;
    [SerializeField] private bool[] playerActive;
    [SerializeField] private SkirmishScreenSelection.FactionType[] factions;
    [SerializeField] private int[] playerTeam;

    [SerializeField] private BuildingSystem chaiyaStrong;
    [SerializeField] private BuildingSystem ulepiaStrong;
    [SerializeField] private BuildingSystem shinStrong;

    [SerializeField] private GameObject playerDummy;
    [SerializeField] private GameObject AIDummy;

    [SerializeField] private GameObject[] startPosition;

    private float coolTimes=0;

    private bool playerDefeated = false;
    private bool playerVictory = false;

    // Use this for initialization
    void Start () {
        SkirmishDataTransfer data = FindObjectOfType<SkirmishDataTransfer>();
        if (data != null)
        {
            maxPlayer = data.GetMaxPlayer();
            playerActive = data.GetAssignPlayer();
            factions = data.GetFaction();
            playerTeam = data.GetTeam();

            //Spawn Player
            for (int i = 0; i < maxPlayer; i++)
            {
                if (playerActive[i])
                {
                    if (i >= 1)
                    {
                        GameObject aiOBJ = Instantiate(AIDummy, transform.position, transform.rotation) as GameObject;
                        VRPlayerIndicator ai =  aiOBJ.gameObject.GetComponent<VRPlayerIndicator>();
                        ai.SetPlayerNum(i+1);
                    }
                    VRPlayerIndicator player = null;
                    foreach (GameObject plays in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        VRPlayerIndicator ind = plays.GetComponent<VRPlayerIndicator>();
                        if (ind.GetPlayerNumber() == i + 1)
                        {
                            player = ind;
                        }
                    }
                    player.SetEther(1500);
                    player.transform.position = startPosition[i].transform.position - (Vector3.forward * 20);

                    if (factions[i] == SkirmishScreenSelection.FactionType.Chaiya)
                    {
                        BuildingSystem strong = Instantiate(chaiyaStrong, startPosition[i].transform.position, startPosition[i].transform.rotation) as BuildingSystem;
                        strong.StartToBuild(i + 1);
                    }
                    if (factions[i] == SkirmishScreenSelection.FactionType.Ulepia)
                    {
                        BuildingSystem strong = Instantiate(ulepiaStrong, startPosition[i].transform.position, startPosition[i].transform.rotation) as BuildingSystem;
                        strong.StartToBuild(i + 1);
                    }
                    if (factions[i] == SkirmishScreenSelection.FactionType.Shin)
                    {
                        BuildingSystem strong = Instantiate(shinStrong, startPosition[i].transform.position, startPosition[i].transform.rotation) as BuildingSystem;
                        strong.StartToBuild(i + 1);
                    }
                }
                
            }
                //Ally Set
            for (int i=0;i<maxPlayer;i++)
            {
                for (int j=0;j<maxPlayer;j++)
                {
                    if (i!=j && playerTeam[i] == playerTeam[j])
                    {
                        AllianceSystem.SetAlly(i+1, j+1, true);
                    }
                }
            }
            data.SetTransfered(true);
        }
        else
        {

        }
	}
	
	// Update is called once per frame
	void Update () {
        coolTimes += Time.deltaTime;
        if (coolTimes >= 5f)
        {
            int[] playerHave = new int[maxPlayer];

            for (int i = 0; i < maxPlayer; i++)
            {
                playerHave[i] = 0;
                UnitStatSystem[] uniG = FindObjectsOfType<UnitStatSystem>();
                foreach (UnitStatSystem uni in uniG)
                {
                    if (uni.GetAlive() && uni.GetOwner() == i + 1)
                    {
                        playerHave[i] += 1;
                    }
                }
            }

            if (playerHave[0] > 0)
            {
                int haveEnemy = 0;
                for (int i = 1; i < maxPlayer; i++)
                {
                    if (playerTeam[0] != playerTeam[i] && playerHave[i] > 0)
                    {
                        haveEnemy += 1;
                    }
                }
                if (haveEnemy == 0)
                {
                    playerVictory = true;
                }
            }
            else
            {
                playerDefeated = true;
            }
        }
        
        
	}

    public int GetMaxPlayer()
    {
        return maxPlayer;
    }

    public bool GetDefeated()
    {
        return playerDefeated;
    }

    public bool GetVictory()
    {
        return playerVictory;
    }

}
