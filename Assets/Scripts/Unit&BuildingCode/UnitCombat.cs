using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour {

    public enum CombatType
    {
        Melee = 0,
        Ranged = 1,
        Anti_Calvary = 2,
        Calvary = 3,
        Building = 4
    }
    [SerializeField] private int damage;
    [SerializeField] private CombatType comType;
    [SerializeField] private float atkRange;
    [SerializeField] private float atkNotice;
    [SerializeField] private bool rangedATK;
    [SerializeField] private ProjectieDummy missile;
    [SerializeField] private GameObject projPoint;
    [SerializeField] private float atkCool;
    [SerializeField] private float atkStartSwing;
    [SerializeField] private float atkStopSwing;
    [SerializeField] private GameObject meleeEffect;
    [SerializeField] private GameObject soundEffect;

    private UnitStatSystem target;
    [SerializeField] private bool autoATKEnabled = true;
    private float coolNow;

    private bool isATKing;


    private UnitStatSystem stat;

    // Use this for initialization
    void Start () {
        stat = GetComponent<UnitStatSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        coolNow -= Time.deltaTime;
        if (stat.GetAlive() && autoATKEnabled)
        {
            if (target != null)
            {
                Vector3 unitPos = new Vector3(transform.position.x, 0, transform.position.z);
                Vector3 targetPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
                RaycastHit hit;
                bool isCol = false;
                if (Physics.Raycast(transform.position + transform.up, transform.TransformDirection(Vector3.forward), out hit, atkRange))
                {
                    GameObject hitObject = hit.transform.gameObject;
                    if (hitObject == target.gameObject)
                    {
                        isCol = true;
                    }
                }
                if (Vector3.Distance(unitPos, targetPos) <= atkRange || isCol)
                {
                    UnitMove uni = GetComponent<UnitMove>();
                    uni.OrderMove(Vector3.one);
                    this.transform.LookAt(target.transform);
                    this.transform.rotation = Quaternion.Euler(0, this.transform.eulerAngles.y, 0);
                    CommandATK();
                }
                else
                {
                    UnitMove mover = GetComponent<UnitMove>();
                    mover.OrderMove(target.transform.position);
                    
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
        else
        {
            target = null;
        }

        
	}

    private void CommandATK()
    {
        if (coolNow <= 0)
        {
            StartCoroutine(aquireATK());
            coolNow = atkCool;
        }
    }

    private IEnumerator aquireATK()
    {
        isATKing = true;
        Animator ani = GetComponent<Animator>();
        ani.SetBool("Attack", true);

        yield return new WaitForSeconds(atkStartSwing);

        if (soundEffect)
        {
            Instantiate(soundEffect, transform.position, transform.localRotation);
        }

        if (!rangedATK)
        {
            if (target != null)
            {
                target.GetDamage(damage);
                Instantiate(meleeEffect, target.transform.position + (transform.up * 2), transform.localRotation);
            }
            else
            {

            }
            
        }
        else
        {
            ProjectieDummy proj = Instantiate(missile, projPoint.transform.position, transform.localRotation) as ProjectieDummy;
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
        }
        ani.SetBool("Attack", false);
        yield return new WaitForSeconds(atkStopSwing);
        UnitMove uni = GetComponent<UnitMove>();
        uni.OrderMove(Vector3.one);
        isATKing = false;
        yield return null;
    }

    public bool IsAttacking()
    {
        return isATKing;
    }

    public bool HaveTarget()
    {
        return target != null;
    }

    public void SetAutoATK(bool set)
    {
        autoATKEnabled = set;
    }
    
    public void SetTarget(UnitStatSystem tar)
    {
        target = tar;
    }

    public Dictionary<string, int> CombatDMGDict()
    {
        Dictionary<string, int> dic = new Dictionary<string, int>();

        dic.Add("Melee||Melee", 50);

        return dic;
    }

    public float GetATKRange()
    {
        return atkRange;
    }
}
