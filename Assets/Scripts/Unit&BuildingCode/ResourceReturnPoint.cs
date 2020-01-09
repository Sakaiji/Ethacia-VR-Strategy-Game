using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceReturnPoint : MonoBehaviour {


    [SerializeField] private float ResourceReturnRange;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetReturnRange()
    {
        return ResourceReturnRange;
    }
}
