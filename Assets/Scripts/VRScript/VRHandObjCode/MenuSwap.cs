using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwap : MonoBehaviour
{

    VRMovement movementDummy;
    private string selectButton;
    private bool isCalledMenu;
    private bool isButtonDown;
    private float buttonCool;
    [SerializeField] private GameObject headNow;
    [SerializeField] private VRSuperHand leftSuper;
    [SerializeField] private VRSuperHand rightSuper;

    [SerializeField] private VRHandExtend leftInGame;
    [SerializeField] private VRHandExtend rightInGame;

    [SerializeField] private VRHandExtend leftMenu;
    [SerializeField] private VRHandExtend rightMenu;

    [SerializeField] private GameObject menuUI;




    // Start is called before the first frame update
    void Start()
    {
        movementDummy = GetComponent<VRMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        SkirmishStartSystem skir = FindObjectOfType<SkirmishStartSystem>();
        if (!skir)
        {
            if (((Input.GetButton("PrimaryStart") || Input.GetButton("SecondaryStart")) && !isButtonDown))
            {
                Debug.Log("Push Menu Button");
                isCalledMenu = !isCalledMenu;
                isButtonDown = true;
                buttonCool = 1f;
            }
        }
        else
        {
            if (((Input.GetButton("PrimaryStart") || Input.GetButton("SecondaryStart")) && !isButtonDown) && (!skir.GetDefeated() && !skir.GetVictory()))
            {
                Debug.Log("Push Menu Button");
                isCalledMenu = !isCalledMenu;
                isButtonDown = true;
                buttonCool = 1f;
            }
        }
        

        
        

        if (skir && (skir.GetVictory() || skir.GetDefeated()))
        {
            leftMenu.gameObject.SetActive(true);
            rightMenu.gameObject.SetActive(true);
            leftInGame.gameObject.SetActive(false);
            rightInGame.gameObject.SetActive(false);
            leftSuper.SetHandObj(leftMenu.gameObject);
            rightSuper.SetHandObj(rightMenu.gameObject);
            menuUI.SetActive(false);
        }
        else
        {
            leftMenu.gameObject.SetActive(isCalledMenu);
            rightMenu.gameObject.SetActive(isCalledMenu);

            leftInGame.gameObject.SetActive(!isCalledMenu);
            rightInGame.gameObject.SetActive(!isCalledMenu);

            menuUI.SetActive(isCalledMenu);

            if (isCalledMenu)
            {
                leftSuper.SetHandObj(leftMenu.gameObject);
                rightSuper.SetHandObj(rightMenu.gameObject);

                //menuUI.transform.LookAt(headNow.transform);
            }
            else
            {
                leftSuper.SetHandObj(leftInGame.gameObject);
                rightSuper.SetHandObj(rightInGame.gameObject);
            }
        }
        



        



        if (buttonCool > 0)
        {
            buttonCool -= Time.deltaTime;
        }
        else
        {
            isButtonDown = false;
        }
    }
}
