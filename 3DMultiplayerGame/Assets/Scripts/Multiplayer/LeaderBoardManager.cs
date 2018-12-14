using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour
{


    private static LeaderBoardManager _instance;

    public static LeaderBoardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LeaderBoardManager>();
            }
            return _instance;
        }
    }

    public GameObject PlayerScorePrefab;
    public Transform scoresHolder;

    private List<GameObject> scoreList = new List<GameObject>();


	private void Start ()
    {
        FindScoresPrefabs();
        

    }

    private void FindScoresPrefabs()
    {
        for(int i =0; i < scoresHolder.childCount; i++)
        {
            scoreList.Add(scoresHolder.GetChild(i).gameObject);
        }
    }

    private void CleanAll()
    {
        foreach(var score in scoreList)
        {
            score.GetComponent<PlayerScore>().Clean();
        }
    }

    public void  UpdateScoreBoard(List<ScoreDTO> scores)
    {
        CleanAll();
        int playersCount = 0;

        foreach(var score in scores)
        {
            scoreList[playersCount].GetComponent<PlayerScore>().Initialize(score);
            playersCount++;
        }

        //for(int i = playersCount; i < 8; i++)
        //{
        //    scoreList[i].GetComponent<PlayerScore>().Clean();
        //}
    }
}
