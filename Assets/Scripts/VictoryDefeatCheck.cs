using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryDefeatCheck : MonoBehaviour
{
    [SerializeField] ConditionMenu ConditionPrefab;

    [SerializeField] private GameObject victorySound;
    [SerializeField] private GameObject defeatSound;
    private bool haveChecked = false;
    private ConditionMenu conditionDummy;

    private float timePassed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!conditionDummy && !haveChecked)
        {
            SkirmishStartSystem skir = FindObjectOfType<SkirmishStartSystem>();
            if (skir.GetDefeated() || skir.GetVictory())
            {
                conditionDummy = Instantiate(ConditionPrefab) as ConditionMenu;
                conditionDummy.GetConText().text = skir.GetVictory() ? "Victory!" : "Defeat!";
                VRPlayerIndicator player = null;
                foreach (GameObject plays in GameObject.FindGameObjectsWithTag("Player"))
                {
                    VRPlayerIndicator ind = plays.GetComponent<VRPlayerIndicator>();
                    if (ind.GetPlayerNumber() == 1)
                    {
                        player = ind;
                    }
                }
                if (skir.GetVictory())
                {
                    Instantiate(victorySound, player.transform.position, player.transform.rotation);
                }
                else
                {
                    Instantiate(defeatSound, player.transform.position, player.transform.rotation);
                }
                haveChecked = true;
                
            }
        }
        else
        {
            if (timePassed >= 10)
            {
                SceneManager.LoadScene("MainMenu");
            }
            timePassed += Time.deltaTime;
            VRPlayerIndicator player = null;
            foreach (GameObject plays in GameObject.FindGameObjectsWithTag("Player"))
            {
                VRPlayerIndicator ind = plays.GetComponent<VRPlayerIndicator>();
                if (ind.GetPlayerNumber() == 1)
                {
                    player = ind;
                }
            }
            conditionDummy.transform.position = player.gameObject.transform.position + (player.transform.forward * 50) + (player.transform.up * 20);
        }
        
        
        
    }
}
