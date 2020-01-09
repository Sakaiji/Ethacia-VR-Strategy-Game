using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerIndicator : MonoBehaviour {

    [SerializeField] private int playerNumber;

    [SerializeField] private int resourceEther;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetPlayerNumber()
    {
        return playerNumber;
    }
    public void SetEther(int amount)
    {
        resourceEther = amount;
    }
    public void ChangeEther(int amount)
    {
        resourceEther += amount;
    }
    public int GetEther()
    {
        return resourceEther;
    }

    public void SetPlayerNum(int nu)
    {
        playerNumber = nu;
    }
}
