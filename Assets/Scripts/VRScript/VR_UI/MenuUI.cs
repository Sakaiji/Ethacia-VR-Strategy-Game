using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour {

    public enum MenuType
    {
        Start = 0,
        Quit = 1,
        Option = 2,
        Test_Mode = 3,
        Credit = 4,
        Return_Menu = 5,
        Restart = 6,
        MusicVolUp = 7,
        MusicVolDown = 8,
        EffectVolUp = 9,
        EffectVolDown = 10,
        BackFirstScreen = 11,
        StartSkirmish = 12,
        MapChange = 13,
        FactionChange = 14,
        AllyChange = 15,
        OptionInGame = 16,
        BackInGame = 17,
        AdjustBot = 18,
        SkirmishLoadGame = 19
    }
    private bool isClicked;
    private bool isSelected;
    private MenuHands handClicked;
    [SerializeField] private MenuType menT;
    [SerializeField] private GameObject hilightBox;
    private float selecTimes;
    [SerializeField] private GameObject firstScreen;
    [SerializeField] private GameObject optionScreen;
    [SerializeField] private GameObject skirmishScreen;
    [SerializeField] private GameObject soundEffectTest;
    private int map_Now;
    [SerializeField] private int map_max;
    private int faction_Now;
    [SerializeField] private int faction_max;
    [SerializeField] private bool playerAvalible;
    [SerializeField]private int teamNow;
    

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (isClicked)
        {
            isClicked = false;
            switch (menT)
            {
                case MenuType.Start:
                    break;
                case MenuType.Quit:
                    Application.Quit();
                    break;
                case MenuType.Test_Mode:
                    SceneManager.LoadScene("RTS_TestMap");
                    break;
                case MenuType.Return_Menu:
                    SceneManager.LoadScene("MainMenu");
                    break;
                case MenuType.Restart:
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                    break;
                case MenuType.Option:
                    firstScreen.SetActive(false);
                    skirmishScreen.SetActive(false);
                    optionScreen.SetActive(true);
                    break;
                case MenuType.BackFirstScreen:
                    optionScreen.SetActive(false);
                    skirmishScreen.SetActive(false);
                    firstScreen.SetActive(true);
                    break;
                case MenuType.MusicVolUp:
                    float musicVol = PlayerPrefs.GetFloat("MusicVolume");
                    if (musicVol < 1)
                    {
                        musicVol += .2f;
                        PlayerPrefs.SetFloat("MusicVolume", musicVol);
                    }
                    break;
                case MenuType.MusicVolDown:
                    float musicVold = PlayerPrefs.GetFloat("MusicVolume");
                    if (musicVold > 0)
                    {
                        musicVold -= .2f;
                        PlayerPrefs.SetFloat("MusicVolume", musicVold);
                    }
                    break;
                case MenuType.EffectVolUp:
                    float effVol = PlayerPrefs.GetFloat("EffectVolume");
                    if (effVol < 1)
                    {
                        effVol += .2f;
                        PlayerPrefs.SetFloat("EffectVolume", effVol);
                        Instantiate(soundEffectTest, transform);
                    }
                    break;
                case MenuType.EffectVolDown:
                    float effVold = PlayerPrefs.GetFloat("EffectVolume");
                    if (effVold > 0)
                    {
                        effVold -= .2f;
                        PlayerPrefs.SetFloat("EffectVolume", effVold);
                        Instantiate(soundEffectTest, transform);
                    }
                    break;
                case MenuType.StartSkirmish:
                    optionScreen.SetActive(false);
                    firstScreen.SetActive(false);
                    skirmishScreen.SetActive(true);
                    break;
                case MenuType.MapChange:
                    map_Now += 1;
                    if (map_Now >= map_max)
                    {
                        map_Now -= map_max;
                    }
                    break;
                case MenuType.FactionChange:
                    faction_Now += 1;
                    if (faction_Now >= faction_max)
                    {
                        faction_Now -= faction_max;
                    }
                    break;
                case MenuType.AllyChange:
                    teamNow += 1;
                    if (teamNow >= 6)
                    {
                        teamNow -= 6;
                    }
                    break;
                case MenuType.SkirmishLoadGame:
                    SkirmishScreenSelection skirmishSelec = FindObjectOfType<SkirmishScreenSelection>();
                    skirmishSelec.SkirmishStart();
                    break;
                case MenuType.AdjustBot:
                    playerAvalible = !playerAvalible;
                    break;
            }
        }
        if (selecTimes > 0)
        {
            selecTimes -= Time.deltaTime;
        }
        else
        {
            isSelected = false;
        }
        hilightBox.SetActive(isSelected);
        
	}

    public void GetSelected()
    {
        isSelected = true;
        selecTimes = .1f;
    }

    public bool CheckClick()
    {
        return isClicked;
    }

    public void SetClick(bool cli)
    {
        isClicked = cli;
    }

    public MenuHands GetHandClicked()
    {
        return handClicked;
    }

    public void GetClicked(MenuHands hand)
    {
        isClicked = true;
        handClicked = hand;
    }
    public int GetMapNow()
    {
        return map_Now;
    }
    public int GetTeamNow()
    {
        return teamNow;
    }
    public int GetFactionNow()
    {
        return faction_Now;
    }
    public bool GetAvalibleNow()
    {
        return playerAvalible;
    }
    public void SetAvalibleNow(bool avi)
    {
        playerAvalible = avi;
    }

    
}
