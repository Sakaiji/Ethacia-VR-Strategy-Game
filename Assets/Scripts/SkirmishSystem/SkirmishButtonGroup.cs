using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkirmishButtonGroup : MonoBehaviour
{

    [SerializeField] private GameObject addButton;
    [SerializeField] private GameObject factionButton;
    [SerializeField] private GameObject teamButton;
    [SerializeField] private Material[] factionMat;

    private SkirmishScreenSelection.FactionType factNow;
    [SerializeField] private bool playerAvalible;
    //[SerializeField] private GameObject colorButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < factionMat.Length; i++)
        {
            MenuUI facButt = factionButton.GetComponent<MenuUI>();
            if (facButt.GetFactionNow()==i)
            {
                factionButton.GetComponent<Renderer>().material = factionMat[i];
            }
        }

        TextMesh teamText = teamButton.GetComponent<TextMesh>();
        MenuUI teamUI = teamButton.GetComponent<MenuUI>();
        int teamPresent = teamUI.GetTeamNow() + 1;
        teamText.text = "" + teamPresent;

        MenuUI addUI = addButton.GetComponent<MenuUI>();
        TextMesh addText = addButton.GetComponent<TextMesh>();
        string addNow = addUI.GetAvalibleNow() ? "-" : "+";
        addText.text = addNow;
        playerAvalible = addUI.GetAvalibleNow();

        foreach (Transform child in this.transform)
        {
            if (child != addButton.transform)
            {
                child.gameObject.SetActive(addUI.GetAvalibleNow());
            }
        }
    }

    public GameObject GetAddButton()
    {
        return addButton;
    }

    public bool GetAvalible()
    {
        return playerAvalible;
    }

    public int GetTeam()
    {
        MenuUI teamUI = teamButton.GetComponent<MenuUI>();
        return teamUI.GetTeamNow();
    }

    public SkirmishScreenSelection.FactionType GetFaction()
    {
        MenuUI facButt = factionButton.GetComponent<MenuUI>();
        if (facButt.GetFactionNow()==0)
        {
            return SkirmishScreenSelection.FactionType.Chaiya;
        }
        if (facButt.GetFactionNow() == 1)
        {
            return SkirmishScreenSelection.FactionType.Ulepia;
        }
        if (facButt.GetFactionNow() == 2)
        {
            return SkirmishScreenSelection.FactionType.Shin;
        }
        else return SkirmishScreenSelection.FactionType.Chaiya;
    }
}
