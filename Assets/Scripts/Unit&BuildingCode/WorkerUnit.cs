using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerUnit : MonoBehaviour {

    [SerializeField] private GameObject selectorCircle;
    [SerializeField] private int harvestRate;
    [SerializeField] private float timePerHarvest;
    [SerializeField] private GameObject etherModel;

    private EtherDeposit etherTarget;
    [SerializeField] private int etherHarvested;
    private ResourceReturnPoint returnBuild;
    private UnitStatSystem stat;
    private bool isSelected;

    private float harvestCool;

	// Use this for initialization
	void Start () {
        stat = this.GetComponent<UnitStatSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        stat.SetSelected(isSelected);
        if (isSelected)
        {
            selectorCircle.SetActive(true);
            
        }
        else
        {
            selectorCircle.SetActive(false);
        }

        etherModel.SetActive(etherHarvested > 0);

        harvestCool -= Time.deltaTime;

        if (stat.GetAlive() && etherTarget != null)
        {
            if (etherHarvested < 100)
            {
                UnitMove mover = GetComponent<UnitMove>();
                if (Vector3.Distance(this.transform.position, etherTarget.transform.position) <= 5)
                {
                    mover.StopMove();
                    this.transform.LookAt(etherTarget.transform);
                    this.transform.rotation = Quaternion.Euler(0, this.transform.eulerAngles.y, 0);
                    if (harvestCool <= 0)
                    {
                        harvestCool = timePerHarvest;
                        StartCoroutine(Harvest());
                    }
                }
                else
                {
                    mover.OrderMove(etherTarget.transform.position);
                }
            }
            else
            {
                if (returnBuild == null)
                {
                    float returnRange = 999999999999f;
                    foreach (ResourceReturnPoint ret in GameObject.FindObjectsOfType<ResourceReturnPoint>())
                    {
                        UnitStatSystem retSta = ret.GetComponent<UnitStatSystem>();
                        if (stat.GetOwner() == retSta.GetOwner() && Vector3.Distance(this.transform.position, ret.transform.position) <= returnRange)
                        {
                            returnRange = Vector3.Distance(this.transform.position, ret.transform.position);
                            returnBuild = ret;
                        }
                    }
                }
                else
                {
                    UnitMove mover = GetComponent<UnitMove>();
                    mover.OrderMove(returnBuild.transform.position);
                }
            }
        }
        if (stat.GetAlive() && returnBuild != null && Vector3.Distance(this.transform.position, returnBuild.transform.position) <= returnBuild.GetReturnRange())
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
            player.ChangeEther(etherHarvested);
            etherHarvested = 0;
        }
	}

    public void SetSelect(bool sel)
    {
        isSelected = sel;
    }

    public void SetOwner(int own)
    {
        stat = this.GetComponent<UnitStatSystem>();
        stat.SetOwner(own);
    }

    public int GetOwner()
    {
        return stat.GetOwner();
    }

    private IEnumerator Harvest()
    {
        Animator ani = GetComponent<Animator>();
        ani.SetBool("Work", true);
        yield return new WaitForSeconds(.6f);
        etherTarget.HarvestEther(harvestRate);
        etherHarvested += harvestRate;
        ani.SetBool("Work", false);
        yield return null;
    }

    public void GoHarvest(EtherDeposit ethe)
    {
        etherTarget = ethe;
    }
}
