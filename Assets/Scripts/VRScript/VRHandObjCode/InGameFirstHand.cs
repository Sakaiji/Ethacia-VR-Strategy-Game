using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameFirstHand : VRHandExtend {

    [SerializeField] private GameObject pointer;
    [SerializeField] private UnitGroupSystem selectedUnit;
    [SerializeField] private GameObject selectorCircle;
    [SerializeField] private BuildingSystem selectedBuilding;
    [SerializeField] private WorkerUnit selectedWorker;
    [SerializeField] private ConstructDummy selectedConstruct;

    [SerializeField] private VRPlayerIndicator player;

    [SerializeField] private GameObject MovePointOBJ;

    private bool startSelected = false;

    public bool showHit;

    private void Start()
    {
        base.Start();
        player.GetComponent<VRPlayerIndicator>();
    }

    private void Update()
    {
        base.Update();
        //pointer.transform.parent = null;
        if (this.GetIndexTrigger() == false && startSelected == true)
        {
            startSelected = false;
        }
        if (selectedConstruct!= null)
        {
            CheckConstruct();
        }
    }

    public override void OnIndexTriggered()
    {
        base.OnIndexTriggered();
        if (!startSelected)
        {
            startSelected = true;
            Debug.Log("Raycast Initated");

            RaycastHit hit;
            GameObject hitObject = null;
            if (Physics.Raycast(pointer.transform.position, pointer.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                hitObject = InitRayCast(hit);
            }

            if (selectedConstruct != null)
            {
                if (selectedConstruct.IsBuildable())
                {
                    selectedConstruct.ConfirmBuild();
                    selectedConstruct = null;
                }
            }
            else
            {
                CheckUnitOBJ(hitObject);
                CheckUIOBJ(hitObject);
            }

            EtherDeposit eth = hitObject.GetComponent<EtherDeposit>();
            if (selectedWorker != null && eth != null)
            {
                selectedWorker.GoHarvest(eth);
            }

            if (hitObject.tag == "Terrain" && (selectedUnit != null || selectedWorker != null))
            {
                UnitMove mover = null;
                if (selectedUnit != null)
                {
                    mover = selectedUnit.GetComponent<UnitMove>();
                    selectedUnit.SetAutoAttaack(false);
                    selectedUnit.SetTarget(null);
                }
                else if (selectedWorker != null)
                {
                    mover = selectedWorker.GetComponent<UnitMove>();
                    selectedWorker.GoHarvest(null);
                }

                mover.OrderMove(hit.point);
                StartCoroutine(MovePointIndicator(hit.point));

            }

        }
        
    }

    public override void OnHandTriggered()
    {
        base.OnHandTriggered();
        UnselectBuilding();
        UnselectUnit();
        UnselectWorker();
        UnselectConstruction();

    }

    GameObject InitRayCast(RaycastHit hit)
    {
        GameObject hitObject = null;
        if (Physics.Raycast(pointer.transform.position, pointer.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(pointer.transform.position, pointer.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Raycast Hit");
            if (showHit)
            {
                StartCoroutine(SphereIndicator(hit.point));
            }

            hitObject = hit.transform.gameObject;
        }
        return hitObject;
    }

    void SelectUnit(UnitGroupSystem uni)
    {
        selectedUnit = uni;
        selectedUnit.SetSelect(true);
    }
    void UnselectUnit()
    {
        if (selectedUnit != null)
        {
            selectedUnit.SetSelect(false);
        }
        selectedUnit = null;
    }

    void SelectBuilding(BuildingSystem bui)
    {
        selectedBuilding = bui;
        selectedBuilding.SetSelect(true);
    }
    void UnselectBuilding()
    {
        if (selectedBuilding != null)
        {
            selectedBuilding.SetSelect(false);
        }
        selectedBuilding = null;
    }

    void SelectWorker(WorkerUnit wor)
    {
        selectedWorker = wor;
        selectedWorker.SetSelect(true);
    }
    void UnselectWorker()
    {
        if (selectedWorker != null)
        {
            selectedWorker.SetSelect(false);
        }
        selectedWorker = null;
    }

    public void SelectConstruction(ConstructDummy dum)
    {
        UnselectBuilding();
        UnselectUnit();
        UnselectWorker();
        selectedConstruct = dum;
    }

    public void UnselectConstruction()
    {
        if (selectedConstruct != null)
        {
            selectedConstruct.CancelBuild();
            Destroy(selectedConstruct.gameObject);
        }       
        selectedConstruct = null;
    }

    void CheckUnitOBJ(GameObject hitObject)
    {
        UnitStatSystem unit = hitObject.GetComponent<UnitStatSystem>();
        if (unit != null)
        {
            Debug.Log("SELECT UNIT");
            if (unit.GetUnitType() == UnitStatSystem.UnitObjectType.Building)
            {
                Debug.Log("SELECT Building");
                BuildingSystem buil = hitObject.GetComponent<BuildingSystem>();
                if (player.GetPlayerNumber() == buil.GetOwner())
                {
                    Debug.Log("Select Own Building");
                    UnselectUnit();
                    UnselectWorker();
                    if (buil != selectedBuilding)
                    {
                        UnselectBuilding();
                        SelectBuilding(buil);
                    }
                }
            }
            if (unit.GetUnitType() == UnitStatSystem.UnitObjectType.Units)
            {
                WorkerUnit wor = unit.GetComponent<WorkerUnit>();
                if (wor != null)
                {
                    if (player.GetPlayerNumber() == wor.GetOwner())
                    {
                        UnselectUnit();
                        UnselectBuilding();
                        if (wor != selectedWorker)
                        {
                            UnselectWorker();
                            SelectWorker(wor);
                        }
                    }
                }
            }
            if (unit.GetOwner() != player.GetPlayerNumber() && selectedUnit != null)
            {
                if (!AllianceSystem.CheckAlly(player.GetPlayerNumber(), unit.GetOwner()))
                {
                    Debug.Log("ATTACK");
                    selectedUnit.SetAutoAttaack(true);
                    selectedUnit.SetTarget(unit.gameObject);
                }
                
            }
        }
        if (hitObject.tag == "Soldier Group")
        {
            Debug.Log("SELECT SOLDIER");
            UnitGroupSystem unitGro = hitObject.GetComponent<UnitGroupSystem>();
            if (player.GetPlayerNumber() == unitGro.GetOwner())
            {
                Debug.Log("Select own Soldier");
                UnselectBuilding();
                UnselectWorker();
                if (unitGro != selectedUnit)
                {
                    UnselectUnit();
                    SelectUnit(unitGro);
                }
                
            }
            if (unitGro.GetOwner() != player.GetPlayerNumber() && selectedUnit != null)
            {
                if (!AllianceSystem.CheckAlly(player.GetPlayerNumber(), unitGro.GetOwner()))
                {
                    Debug.Log("ATTACK");
                    selectedUnit.SetAutoAttaack(true);
                    selectedUnit.SetTarget(unitGro.gameObject);
                }
                

            }
        }
    }

    void CheckUIOBJ(GameObject hitObject)
    {
        BuildingUI buildUI = hitObject.GetComponent<BuildingUI>();
        if (buildUI != null)
        {
            Debug.Log("Click Building UI!!!");
            buildUI.StartCoroutine(buildUI.GetClicked(this));
        }
    }

    void CheckConstruct()
    {
        selectedConstruct.transform.rotation = this.transform.parent.rotation;
        RaycastHit hit;
        GameObject hitObject = null;
        if (Physics.Raycast(pointer.transform.position, pointer.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            hitObject = InitRayCast(hit);
        }
        bool isGround = false;
        if (hitObject != null)
        {
            if (hitObject.tag == "Terrain")
            {
                selectedConstruct.transform.position = hit.point;
                selectedConstruct.transform.rotation = Quaternion.Euler(0, this.transform.parent.eulerAngles.y, 0);
                isGround = true;
            }
        }
        else
        {
            Vector3 pos = pointer.transform.position + (pointer.transform.forward * 10);
            selectedConstruct.transform.position = pos;
        }
        selectedConstruct.SetGround(isGround);
        
    }
    

    private IEnumerator SphereIndicator(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;

        yield return new WaitForSeconds(1);

        Destroy(sphere);
    }

    private IEnumerator MovePointIndicator(Vector3 pos)
    {
        GameObject movepoint = Instantiate(MovePointOBJ) as GameObject;
        movepoint.transform.parent = null;
        movepoint.transform.position = pos;

        yield return new WaitForSeconds(1);

        Destroy(movepoint);
    }

}
