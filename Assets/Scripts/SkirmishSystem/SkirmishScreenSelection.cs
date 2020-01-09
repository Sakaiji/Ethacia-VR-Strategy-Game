using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkirmishScreenSelection : MonoBehaviour
{

    public enum FactionType
    {
        Chaiya = 0,
        Ulepia = 1,
        Shin = 2
    }

    [SerializeField] private string[] map_id;
    [SerializeField] private string[] map_name;
    [SerializeField] private int[] map_max_player;

    [SerializeField] private MenuUI mapButton;
    [SerializeField] private TextMesh mapNameDisplay;
    [SerializeField] private SkirmishButtonGroup[] playerButton;

    [SerializeField] SkirmishDataTransfer dataTransferDummy;


    private int whatNow;
    [SerializeField] private bool[] playerAvalible;
    [SerializeField] private int[] playerTeam;
    [SerializeField] private FactionType[] playerFaction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        whatNow = mapButton.GetMapNow();
        for (int i=0; i<map_id.Length;i++)
        {
            if (whatNow == i)
            {
                mapNameDisplay.text = map_name[i];
            }
        }

        for(int i = 0; i < playerButton.Length; i++)
        {
            if (i == 0 || i == 1)
            {
                playerButton[i].GetAddButton().SetActive(false);
            }
            else
            {
                if (i < map_max_player[whatNow])
                {
                    if (!playerButton[i].GetAddButton().GetComponent<MenuUI>().GetAvalibleNow() && playerButton[i - 1].GetAddButton().GetComponent<MenuUI>().GetAvalibleNow())
                    {
                        playerButton[i].GetAddButton().SetActive(true);
                    }
                    else
                    {
                        if (playerButton[i].GetAddButton().GetComponent<MenuUI>().GetAvalibleNow() && playerButton[i - 1].GetAddButton().GetComponent<MenuUI>().GetAvalibleNow())
                        {
                            playerButton[i].GetAddButton().SetActive(true);
                        }
                        else
                        {
                            playerButton[i].GetAddButton().SetActive(false);
                        }
                        
                    }
                    if (i!= 5)
                    {
                        if (playerButton[i].GetAddButton().GetComponent<MenuUI>().GetAvalibleNow() && playerButton[i + 1].GetAddButton().GetComponent<MenuUI>().GetAvalibleNow())
                        {
                            playerButton[i].GetAddButton().SetActive(false);
                        }
                    }

                    
                }
                else
                {
                    playerButton[i].GetAddButton().GetComponent<MenuUI>().SetAvalibleNow(false);
                    playerButton[i].GetAddButton().SetActive(false);
                }
                // &&
                
            }
            
            playerAvalible[i] = playerButton[i].GetAddButton().GetComponent<MenuUI>().GetAvalibleNow();
            playerTeam[i] = playerButton[i].GetTeam();
            playerFaction[i] = playerButton[i].GetFaction();
        }
    }

    public void SkirmishStart()
    {
        SkirmishDataTransfer dataTran = Instantiate(dataTransferDummy) as SkirmishDataTransfer;
        dataTran.Creating(map_max_player[whatNow], playerAvalible, playerTeam, playerFaction);
        if (map_id[whatNow] == "")
        {
            Destroy(dataTran);
        }
        else
        {
            SceneManager.LoadScene(map_id[whatNow]);
        }
    }
}
