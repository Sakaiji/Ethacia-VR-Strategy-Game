using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllianceSystem : MonoBehaviour {

    [SerializeField] private int maxPlayer;
    private static Dictionary<int, List<bool>> allyDict;

	// Use this for initialization
	void Start () {
        allyDict = new Dictionary<int, List<bool>>();
        for (int i=0;i<=maxPlayer;i++ )
        {
            List<bool> allyList = new List<bool>();
            for (int j=0;j<=maxPlayer;j++)
            {
                if (i == j)
                {
                    allyList.Add(true);
                }
                else
                {
                    allyList.Add(false);
                }
            }
            allyDict.Add(i, allyList);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static Dictionary<int, List<bool>> GetAlly()
    {
        return allyDict;
    }

    public static bool CheckAlly(int player1, int player2)
    {
        if (allyDict[player1][player2] == allyDict[player2][player1] && allyDict[player1][player2] == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetAlly(int player1, int player2, bool isAlly)
    {
        allyDict[player1][player2] = isAlly;
        allyDict[player2][player1] = isAlly;
    }
}
