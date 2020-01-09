using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatSystem : MonoBehaviour {

    public enum UnitObjectType
    {
        Units = 0,
        Building = 1
    }

    public UnitObjectType uniType = 0;


    [SerializeField] private int maxHealth;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int owner;
    [SerializeField] private float deadTime;
    [SerializeField] private HealthBarUI healthBar;
    private int healthNow;
    private int energyNow;

    private bool isSelected;

    
    public void Start()
    {
        healthNow = maxHealth;
        energyNow = maxEnergy;
    }

    public void Update()
    {
        if (GetAlive())
        {
            healthBar.SetHealthPercent((healthNow * 100 / maxHealth));
            healthBar.gameObject.SetActive(isSelected);
            VRPlayerIndicator player = null;
            foreach (GameObject plays in GameObject.FindGameObjectsWithTag("Player"))
            {
                VRPlayerIndicator ind = plays.GetComponent<VRPlayerIndicator>();
                if (ind.GetPlayerNumber() == GetOwner())
                {
                    player = ind;
                }
            }
            VRHeadTracking head = player.GetComponent<VRHeadTracking>();
            healthBar.transform.LookAt(head.GetHead().transform);
        }
        else
        {
            healthBar.gameObject.SetActive(false);
            StartCoroutine(Dies());
        }
    }

    private IEnumerator Dies()
    {
        if (uniType == UnitObjectType.Units)
        {
            Animator ani = GetComponent<Animator>();
            ani.SetBool("Alive", false);
        }
        else
        {
            BuildingSystem build = GetComponent<BuildingSystem>();
            build.DestroyNow();
        }
        yield return new WaitForSeconds(deadTime);
        Destroy(this.gameObject);
        yield return null;
    }

    public int GetOwner()
    {
        return owner;
    }

    public void SetOwner(int num)
    {
        owner = num;
        TeamColorSystem tc = GetComponent<TeamColorSystem>();
        tc.ChangeColor(owner);
    }

    public void GetDamage(int dmg)
    {
        healthNow -= dmg;
    }
    

    public UnitObjectType GetUnitType()
    {
        return uniType;
    }

    public bool GetAlive()
    {
        return (healthNow > 0);
    }

    public float GetHealthPercent()
    {
        return healthNow / maxHealth;
    }

    public void SetSelected(bool con)
    {
        isSelected = con;
    }

}