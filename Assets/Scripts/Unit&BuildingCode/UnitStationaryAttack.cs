using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStationaryAttack : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private UnitCombat.CombatType comType;
    [SerializeField] private float atkRange;
    [SerializeField] private float atkNotice;
    [SerializeField] private float atkCool;
    [SerializeField] private ProjectieDummy missile;
    [SerializeField] private GameObject projPoint;
    [SerializeField] private GameObject soundEffect;
    [SerializeField] private bool isBuilding;
    private float coolNow;
    private UnitStatSystem stat;
    private UnitStatSystem target;
    
    // Start is called before the first frame update
    void Start()
    {
        stat = GetComponent<UnitStatSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        coolNow -= Time.deltaTime;
        if (stat.GetAlive())
        {
            if (isBuilding)
            {
                BuildingSystem bui = GetComponent<BuildingSystem>();
                if (bui.CheckFinish())
                {
                    CheckTarget();
                }
                else
                {
                    target = null;
                }
            }
            else
            {
                CheckTarget();
            }
            
        }
        else
        {
            target = null;
        }
    }
    private void StartAttack()
    {
        if (coolNow <= 0)
        {
            StartCoroutine(aquireATK());
            coolNow = atkCool;
        }
    }

    private IEnumerator aquireATK()
    {
        //projPoint.transform.LookAt(target.transform);
        if (soundEffect)
        {
            Instantiate(soundEffect, transform.position, transform.localRotation);
        }
        ProjectieDummy proj = Instantiate(missile, projPoint.transform.position, projPoint.transform.rotation) as ProjectieDummy;
        if (target != null)
        {
            //proj.transform.LookAt(target.transform.position);
            Debug.Log("Throw to Target");
            proj.StartProj(target.transform.position, stat.GetOwner(), damage);
        }
        else
        {
            //proj.transform.LookAt(transform.position + (transform.forward * atkRange));
            Debug.Log("Throw to Point");
            proj.StartProj(transform.position + (transform.forward * atkRange), stat.GetOwner(), damage);
        }

        yield return null;
    }

    private void CheckTarget()
    {
        if (target != null)
        {
            Vector3 unitPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 targetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
            if (Vector3.Distance(unitPos, targetPos) <= atkRange)
            {
                projPoint.transform.LookAt(target.transform);
                //projPoint.transform.rotation = Quaternion.Euler(0, projPoint.transform.eulerAngles.y, 0);
                StartAttack();
            }
            else
            {
                target = null;

            }
            if (target == null || !target.GetAlive())
            {
                target = null;
            }
        }
        else
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, atkNotice);
            int i = 0;
            while (i < hitColliders.Length)
            {
                UnitStatSystem enemyStat = hitColliders[i].GetComponent<UnitStatSystem>();
                if (enemyStat != null && stat.GetOwner() != enemyStat.GetOwner() && !AllianceSystem.CheckAlly(stat.GetOwner(), enemyStat.GetOwner()) && enemyStat.GetAlive())
                {
                    target = enemyStat;
                    Debug.Log("Found Enemy");
                }
                i++;
            }
        }
    }

}
