using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColorSystem : MonoBehaviour {

    [SerializeField] private GameObject[] teamColor;
    public TeamColorLib teamColorLib;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeColor(int num)
    {
        foreach (GameObject teamMat in teamColor)
        {
            teamColorLib.GetColorList();
            teamMat.GetComponent<Renderer>().material = teamColorLib.GetColorList()[num];
        }
    }
}
