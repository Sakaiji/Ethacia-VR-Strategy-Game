using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour {

    [SerializeField] private Transform target;
    [SerializeField] private bool isNeedStat;
    [SerializeField] private bool needAni;
    [SerializeField] private bool canFight;

    private CharacterController _charController;
    private Animator _animator;

    public float moveSpeed = 6.0f;
    public float rotSpeed = 3.0f;

    public float deceleration = 2f;
    public float targetBuffer = 10f;
    private float _curSpeed = 0f;
    private Vector3 _targetPos = Vector3.one;

    private UnitStatSystem stat;
    private UnitCombat combStat;

    private bool isColEvd=false;
    [SerializeField] private GameObject checkColPoint;
    [SerializeField] private List<GameObject> evdPoints = new List<GameObject>();
    [SerializeField] private float colCheckDistance;
    [SerializeField] private GameObject evaderGAMEOBJ;

    // Use this for initialization
    void Start () {
        checkColPoint = this.gameObject;
        _charController = GetComponent<CharacterController>();
        if (isNeedStat)
        {
            stat = GetComponent<UnitStatSystem>();

        }
        if (needAni)
        {
            _animator = GetComponent<Animator>();
        }
        if (canFight)
        {
            combStat = GetComponent<UnitCombat>();
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        CheckEvader();
        if (isNeedStat)
        {
            if (stat.GetAlive())
            {
                if (canFight)
                {
                    if (!combStat.IsAttacking())
                    {
                        Moving();
                    }
                    else
                    {
                        _animator.SetFloat("Speed",0);
                    }
                }
                else
                {
                    Moving();
                }
            }
        }
        else
        {
            Moving();
        }
        
    }

    private void Moving()
    {
        
        Vector3 movement = Vector3.zero;
        Vector3 adjustedPos = new Vector3();
        if (isColEvd)
        {
            adjustedPos = new Vector3(evdPoints[0].transform.position.x, transform.position.y, evdPoints[0].transform.position.z);
        }
        else
        {
            adjustedPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
        }
        

        if (_targetPos != Vector3.one)
        {
            Quaternion targetRot = Quaternion.LookRotation(adjustedPos - transform.position);
            //
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

            movement = _curSpeed * Vector3.forward;
            movement = transform.TransformDirection(movement);
        }

        if (isColEvd)
        {

        }
        else
        {
            //Debug.Log(Vector3.Distance(adjustedPos, transform.position));
            if (Vector3.Distance(adjustedPos, transform.position) < targetBuffer)
            {
                _curSpeed -= deceleration * Time.deltaTime;
                if (_curSpeed <= 0)
                {
                    _targetPos = Vector3.one;
                }
            }
        }
        

        if (needAni)
        {
            _animator.SetFloat("Speed", movement.sqrMagnitude);
        }


        movement *= Time.deltaTime;
        _charController.Move(movement);

        if (isColEvd)
        {
            CollisionEvading();
        }
        else
        {
            RaycastHit hit;
            GameObject hitObject = null;
            
            Debug.DrawRay(checkColPoint.transform.position + (transform.up * 2), checkColPoint.transform.TransformDirection(Vector3.forward) * colCheckDistance, Color.blue);
            if (Physics.Raycast(checkColPoint.transform.position + transform.up, checkColPoint.transform.TransformDirection(Vector3.forward), out hit, colCheckDistance))
            {
                Debug.Log("Raycast Hit");
                hitObject = hit.transform.gameObject;
                UnitMove colUnit = hitObject.gameObject.GetComponent<UnitMove>();
                
                if (colUnit)
                {
                    UnitStatSystem colUnitStat = colUnit.GetComponent<UnitStatSystem>();
                    if (colUnitStat && colUnitStat.GetOwner() == stat.GetOwner())
                    {
                        if (colUnit.GetCurrentSpeed() < _curSpeed)
                        {
                            if (evdPoints.Count == 0)
                            {
                                //StartColEvade();
                            }
                        }
                    }
                    else
                    {
                        UnitGroupSystem colGroup = hitObject.GetComponent<UnitGroupSystem>();
                        if (colGroup && colGroup.GetOwner() == stat.GetOwner())
                        {
                            if (evdPoints.Count == 0)
                            {
                                //StartColEvade();
                            }
                        }
                    }
                    
                }
                else
                {
                    if (evdPoints.Count == 0)
                    {
                        //StartColEvade();
                    }
                    
                }
            }
            else
            {
                isColEvd = false;
            }
        }
    }

    public void OrderMove(Vector3 point)
    {
        _targetPos = point;
        _curSpeed = moveSpeed;
        isColEvd = false;
        foreach (GameObject po in evdPoints)
        {
            Destroy(po.gameObject);
        }
        evdPoints.Clear();
        //Vector3 adjustedPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
        //transform.LookAt(adjustedPos);
    }

    public void StopMove()
    {
        _targetPos = Vector3.one;
        _curSpeed = 0;
    }

    public bool GetIsColEvd()
    {
        return isColEvd;
    }

    public float GetCurrentSpeed()
    {
        return _curSpeed;
    }

    void StartColEvade()
    {
        GameObject unit1 = Instantiate(evaderGAMEOBJ, transform.position + (transform.up * 2) - (transform.right * 2), transform.rotation) as GameObject;
        EvaderPoint leftEvd = unit1.GetComponent<EvaderPoint>();
        leftEvd.StartCheck(colCheckDistance, EvaderPoint.EvaderDirection.Left);
        evdPoints.Add(unit1);
        GameObject unit2 = Instantiate(evaderGAMEOBJ, transform.position + (transform.up * 2) + (transform.right * 2), transform.rotation) as GameObject;
        EvaderPoint rightEvd = unit1.GetComponent<EvaderPoint>();
        leftEvd.StartCheck(colCheckDistance, EvaderPoint.EvaderDirection.Right);
        evdPoints.Add(unit2);
        isColEvd = true;
    }

    void CheckEvader()
    {
        if (evdPoints.Count == 2)
        {
            EvaderPoint removePoint = null;
            float minDis = 0;
            foreach (GameObject po in evdPoints)
            {
                EvaderPoint evdPoi = po.GetComponent<EvaderPoint>();
                if (evdPoi.getCheck() == false)
                {
                    if (minDis >= evdPoi.GetDistance())
                    {
                        Debug.Log("Min Distance : " + evdPoi.GetDistance());
                        minDis = evdPoi.GetDistance();
                        removePoint = evdPoi;
                    }
                    if (evdPoi.isDeadEnd())
                    {
                        Debug.Log("Dead End!!!");
                        evdPoints.Remove(po);
                        Destroy(evdPoi.gameObject);
                    }
                }
            }
            Destroy(removePoint.gameObject);
        }
        if (evdPoints.Count == 1)
        {
            isColEvd = true;
        }
    }

    void CollisionEvading()
    {
        if (evdPoints.Count == 1)
        {
            Vector3 adjustedPos = new Vector3(evdPoints[0].transform.position.x, transform.position.y, evdPoints[0].transform.position.z);
            if (Vector3.Distance(adjustedPos, transform.position) < targetBuffer)
            {
                isColEvd = false;
            }
        }
        
    }

}
