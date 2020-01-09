using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroupSystem : MonoBehaviour {

    [SerializeField] private GameObject movePointDummy;
    [SerializeField] private GameObject unitOBJ;
    [SerializeField] private int maxAmount;
    [SerializeField] private int widthPerUnit;
    [SerializeField] private int heightPerUnit;
    [SerializeField] private int row;
    [SerializeField] private int ownerNum;
    [SerializeField] private GameObject selectorCircle;
    [SerializeField] private bool isWorker;
    [SerializeField] private float maxFollow;
    [SerializeField] private float atkRange;
    [SerializeField] private GameObject target;
    private int amount;
    private List<GameObject> unitList;
    private List<GameObject> pointList;
    private bool isSelected;

	// Use this for initialization
	void Start () { 

        pointList = new List<GameObject>();
        int widthNum = maxAmount / row;
        int heightNum = row / 2;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < widthNum; j++)
            {
                GameObject poi = Instantiate(movePointDummy, transform.position, Quaternion.Euler(0,180,0)) as GameObject;
                poi.transform.parent = transform;
                if (j >= widthNum / 2)
                {
                    Vector3 movepos = poi.transform.position + (transform.right * (widthPerUnit * (j - ( (widthNum / 2) - 1) ) ) );
                    if (widthNum % 2 == 0)
                    {
                        movepos -= (transform.right * widthPerUnit) / 2;
                    }
                    else
                    {
                        movepos -= transform.right * widthPerUnit;
                    }
                    poi.transform.position = movepos;
                }
                else
                {
                    Vector3 movepos = poi.transform.position - (transform.right * (widthPerUnit * ( (widthNum / 2) - j ) ) );
                    if (widthNum % 2 == 0)
                    {
                        movepos += (transform.right * widthPerUnit) / 2;
                    }
                    
                    poi.transform.position = movepos;
                }
                if (i >= heightNum)
                {
                    Vector3 movepos = poi.transform.position - (transform.forward * (heightPerUnit * (i - (heightNum - 1) ) ) );
                    if (row % 2 == 0)
                    {
                        movepos += (transform.forward * heightPerUnit) / 2;
                    }
                    else
                    {
                        movepos += transform.forward * heightPerUnit;
                    }
                    poi.transform.position = movepos;
                }
                else
                {
                    Vector3 movepos = poi.transform.position + (transform.forward * (heightPerUnit * (heightNum - i) ) );
                    if (row % 2 == 0)
                    {
                        movepos -= (transform.forward * heightPerUnit) / 2;
                    }
                    poi.transform.position = movepos;
                }
                pointList.Add(poi);
            }
        }

        unitList = new List<GameObject>();
        for (int i = 0; i < maxAmount; i++)
        {
            GameObject unit = Instantiate(unitOBJ, pointList[i].transform.position, transform.rotation) as GameObject;
            unitList.Add(unit);
            unit.transform.parent = null;
            unit = null;
            ////


        }
        SetOwner(ownerNum);
        
        

    }

    // Update is called once per frame
    void Update() {
        if (isSelected)
        {
            selectorCircle.SetActive(true);
        }
        else
        {
            selectorCircle.SetActive(false);
        }

        for (int i = 0; i < unitList.Count; i++)
        {
            GameObject uniChe = null;
            uniChe = unitList[i];
            if (uniChe)
            {
                UnitStatSystem uniSta = uniChe.GetComponent<UnitStatSystem>();
                uniSta.SetSelected(isSelected);
            }
        }

        if (target != null)
        {
            Vector3 targetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
            Vector3 groPos = new Vector3(transform.position.x, 0, transform.position.z);
            UnitMove mover = GetComponent<UnitMove>();
            if (Vector3.Distance(targetPos, groPos) >= atkRange)
            {
                if (!mover.GetIsColEvd())
                {
                    mover.OrderMove(targetPos);
                }
                
            }
            else
            {
                if (!mover.GetIsColEvd())
                {
                    mover.OrderMove(Vector3.one);
                }
                
            }
        }
        else
        {
            for (int i = 0; i < unitList.Count; i++)
            {
                GameObject uniObj = null;
                uniObj = unitList[i];
                GameObject posObj = null;
                posObj = pointList[i];
                if (uniObj != null && posObj != null)
                {
                    Vector3 uniPos = new Vector3(uniObj.transform.position.x, 0, uniObj.transform.position.z);
                    Vector3 posPos = new Vector3(posObj.transform.position.x, 0, posObj.transform.position.z);
                    UnitMove uni = uniObj.GetComponent<UnitMove>();
                    UnitCombat com = uniObj.GetComponent<UnitCombat>();
                    UnitStatSystem star = uniObj.GetComponent<UnitStatSystem>();
                    if (star.GetAlive())
                    {
                        star.SetSelected(isSelected);
                        if (!com.HaveTarget())
                        {
                            if (Vector3.Distance(uniPos, posPos) > 1)
                            {
                                if (!uni.GetIsColEvd())
                                {
                                    uni.OrderMove(posObj.transform.position);
                                    com.SetAutoATK(false);
                                }
                                    
                            }
                            else
                            {
                                if (!uni.GetIsColEvd())
                                {
                                    uni.OrderMove(Vector3.one);
                                    com.SetAutoATK(true);
                                }
                                    
                            }
                        }
                        else
                        {
                            Vector3 groPos = new Vector3(transform.position.x, 0, transform.position.z);
                            if (Vector3.Distance(groPos, uniPos) >= maxFollow)
                            {
                                com.SetTarget(null);
                                com.SetAutoATK(false);
                            }
                        }
                    }
                
                }
                else
                {
                    unitList.Remove(uniObj);
                    pointList.Remove(posObj);
                    Destroy(posObj.gameObject);
                }
                
            }
        }

        if(unitList.Count <= 0)
        {
            Destroy(this.gameObject);
        }



    }

    public void SetSelect(bool sele)
    {
        isSelected = sele;
    }

    public bool GetSelected()
    {
        return isSelected;
    }

    public void SetOwner(int own)
    {
        ownerNum = own;
        TeamColorSystem teamCol = GetComponent<TeamColorSystem>();
        teamCol.ChangeColor(ownerNum);
        foreach (GameObject uni in unitList)
        {
            UnitStatSystem sta = uni.GetComponent<UnitStatSystem>();
            sta.SetOwner(ownerNum);
        }
    }

    public int GetOwner()
    {
        return ownerNum;
    }

    public void SetAutoAttaack(bool set)
    {
        foreach (GameObject uni in unitList)
        {
            if (uni != null)
            {
                UnitCombat com = uni.GetComponent<UnitCombat>();
                if (com != null)
                {
                    com.SetAutoATK(set);
                }
                
            }
            
        }
    }

    public void SetTarget(GameObject tar)
    {
        target = tar;
        if (target != null)
        {
            UnitStatSystem tarStat = target.GetComponent<UnitStatSystem>();
            if (tarStat != null)
            {
                foreach (GameObject uni in unitList)
                {
                    UnitCombat com = uni.GetComponent<UnitCombat>();
                    com.SetTarget(tarStat);
                }
            }
            UnitGroupSystem tarGro = target.GetComponent<UnitGroupSystem>();
            if (tarGro != null)
            {
                foreach (GameObject uni in unitList)
                {
                    UnitCombat com = uni.GetComponent<UnitCombat>();
                    UnitStatSystem tarLitStat = tarGro.GetUnitList()[Random.Range(0, tarGro.GetUnitList().Count)].GetComponent<UnitStatSystem>();
                    com.SetTarget(tarLitStat);
                }
            }
        }
        
        

    }

    public bool HaveTarget()
    {
        if (target)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<GameObject> GetUnitList()
    {
        return unitList;
    }
    
}
