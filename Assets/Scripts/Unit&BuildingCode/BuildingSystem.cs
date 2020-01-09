using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour {

    [SerializeField] private GameObject models;
    [SerializeField] private GameObject selectorCircle;
    
    [SerializeField] private int buildTime;
    [SerializeField] private bool buildingFinished;

    private UnitStatSystem stat;
    //VR UI
    [SerializeField] private GameObject baseUI;
    [SerializeField] private BuildingUI cancelUI;
    [SerializeField] private ProgressBarUI barUI;

    [SerializeField] private GameObject constructEff;
    [SerializeField] private GameObject destroyEff;

    private bool isSelected;

    private bool isDestroyed;

	// Use this for initialization
	void Start () {
        stat = GetComponent<UnitStatSystem>();
        stat.SetOwner(stat.GetOwner());
    }
	
	// Update is called once per frame
	void Update () {
        constructEff.SetActive(!buildingFinished);
        foreach (Transform child in constructEff.transform)
        {
            child.gameObject.SetActive(!buildingFinished);
        }
        stat.SetSelected(isSelected);
        if (isSelected)
        {
            selectorCircle.SetActive(true);
            baseUI.SetActive(true);
            VRPlayerIndicator player = null;
            foreach (GameObject plays in GameObject.FindGameObjectsWithTag("Player"))
            {
                VRPlayerIndicator ind = plays.GetComponent<VRPlayerIndicator>();
                if (ind.GetPlayerNumber() == stat.GetOwner())
                {
                    player = ind;
                }
            }
            VRHeadTracking head = player.GetComponent<VRHeadTracking>();
            baseUI.transform.LookAt(head.GetHead().transform);
            if (!buildingFinished)
            {
                cancelUI.gameObject.SetActive(true);
                barUI.gameObject.SetActive(true);
               
            }
            else
            {
                cancelUI.gameObject.SetActive(false);
                barUI.gameObject.SetActive(false);
                
            }
        }
        else
        {
            selectorCircle.SetActive(false);
            baseUI.gameObject.SetActive(false);
        }
        if (cancelUI.CheckClick())
        {
            cancelUI.SetClick(false);
            Destroy(this.gameObject);
        }
	}

    public void StartToBuild(int owner)
    {
        stat = GetComponent<UnitStatSystem>();
        models.transform.position += Vector3.down * 50;
        stat.SetOwner(owner);
        StartCoroutine(BuildingConstrutMove(models.transform, this.transform.position, buildTime));
    }

    public void DestroyNow()
    {
        if (!isDestroyed)
        {
            Instantiate(destroyEff, transform.position, transform.localRotation);
            isDestroyed = true;
        }
        
        StartCoroutine(DestroyBuilding(models.transform, this.transform.position + (Vector3.down * 50), 5f));
    }

    void FinishBuild()
    {
        buildingFinished = true;
    }

    public bool CheckFinish()
    {
        return buildingFinished;
    }

    public void SetSelect(bool sett)
    {
        isSelected = sett;
    }

    public bool CheckSelect()
    {
        return isSelected;
    }

    public int GetOwner()
    {
        return stat.GetOwner();
    }

    public GameObject GetModel()
    {
        return models;
    }

    public IEnumerator BuildingConstrutMove(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        barUI.SetProgress(0);
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            barUI.SetProgress(t);
            yield return null;
        }
        FinishBuild();
        yield return null;
    }

    public IEnumerator DestroyBuilding(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        yield return null;
    }
}
