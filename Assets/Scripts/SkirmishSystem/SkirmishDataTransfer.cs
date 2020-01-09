using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkirmishDataTransfer : MonoBehaviour {

    private bool have_transfered;
    private int maxPlayer;
    private bool[] assignPlayer;
    private int[] teamPlayer;
    private SkirmishScreenSelection.FactionType[] factions;

	// Use this for initialization
	void Start () {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "MainMenu" && have_transfered)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Creating(int maxP, bool[] assignP, int[] teamP, SkirmishScreenSelection.FactionType[] fac)
    {
        maxPlayer = maxP;
        assignPlayer = assignP;
        teamPlayer = teamP;
        factions = fac;
    }

    public int GetMaxPlayer()
    {
        return maxPlayer;
    }
    public bool[] GetAssignPlayer()
    {
        return assignPlayer;
    }
    public int[] GetTeam()
    {
        return teamPlayer;
    }
    public SkirmishScreenSelection.FactionType[] GetFaction()
    {
        return factions;
    }
    public void SetTransfered(bool trans)
    {
        have_transfered = trans;
    }
}
