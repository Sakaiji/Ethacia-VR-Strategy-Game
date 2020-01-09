using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtherDeposit : MonoBehaviour {

    [SerializeField] private int etherNow;
    [SerializeField] private GameObject[] etherModel;

    private GameObject modelNow; 

	// Use this for initialization
	void Start () {
        int modID = Random.Range(0, etherModel.Length-1);
        foreach (GameObject md in etherModel)
        {
            md.SetActive(false);
        }
        etherModel[modID].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetEther(int et)
    {
        etherNow = et;
    }

    public void HarvestEther(int et)
    {
        etherNow -= et;
    }
}
