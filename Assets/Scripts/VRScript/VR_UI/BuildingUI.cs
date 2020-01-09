using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUI : MonoBehaviour {

    private bool isClicked;
    private InGameFirstHand handClicked;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool CheckClick()
    {
        return isClicked;
    }

    public void SetClick(bool cli)
    {
        isClicked = cli;
    }

    public InGameFirstHand GetHandClicked()
    {
        return handClicked;
    }

    public IEnumerator GetClicked(InGameFirstHand hand)
    {
        isClicked = true;
        handClicked = hand;
        yield return new WaitForSeconds(1);
        handClicked = null;
        isClicked = false;
    }
}
