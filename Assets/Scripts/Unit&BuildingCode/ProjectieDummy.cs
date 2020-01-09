using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectieDummy : MonoBehaviour {

    [SerializeField] private float spd;
    [SerializeField] private bool isPenetrate;
    [SerializeField] private float startAngle;
    [SerializeField] private int ownerNum;
    [SerializeField] private GameObject soundEff;

    private int damage;
    private float downAngle;
    private bool canMove;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (canMove)
        {
            transform.position += (transform.forward * spd) * Time.deltaTime;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + (downAngle * Time.deltaTime), transform.rotation.eulerAngles.y, 0);
        }
	}

    public void StartProj(Vector3 targetpos, int own, int dmg)
    {
        ownerNum = own;
        damage = dmg;
        float time = Vector3.Distance(transform.position, targetpos) / spd;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - startAngle, transform.rotation.eulerAngles.y, 0);
        downAngle = (startAngle * 2) / time;
        downAngle -= downAngle / 20;
        canMove = true;
    }

    void OnTriggerEnter(Collider other)
    {
        UnitStatSystem uni = other.gameObject.GetComponent<UnitStatSystem>();
        if ((uni != null && uni.GetOwner() != ownerNum && !AllianceSystem.CheckAlly(ownerNum, uni.GetOwner())) || other.gameObject.tag == "Terrain")
        {
            if (uni != null && uni.GetAlive())
            {
                uni.GetDamage(damage);
            }
            if (soundEff)
            {
                Instantiate(soundEff, transform.position, transform.localRotation);
            }
            Destroy(this.gameObject);
        }
        
    }

}
