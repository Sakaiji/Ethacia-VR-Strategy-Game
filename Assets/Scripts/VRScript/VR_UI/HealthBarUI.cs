using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Material[] healthState;
    [SerializeField] private GameObject barModel;

    private float healthPercent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Material matNow = healthState[0];
        for (int i=0; i<healthState.Length;i++)
        {
            if (healthPercent >= i*(100/healthState.Length))
            {
                matNow = healthState[i];
                
            }
        }
        barModel.GetComponent<Renderer>().material = matNow;
    }

    public void SetHealthPercent(float healthP)
    {
        healthPercent = healthP;
    }
}
