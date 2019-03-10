using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Organize the phases in the level.
public class LevelManagerScript : MonoBehaviour {

    public GameObject GameManager;
    public GameObject Player;

    public GameObject Phase1Manager;
    public GameObject Phase2Manager;
    public GameObject Phase3Manager;
    public GameObject Phase4Manager;
    public GameObject VictoryObject;
    public GameObject ScoreText;
    public GameObject FailObject;

    public GameObject[] Enemies;

    private int currentPhase;
    private bool fail;
    private bool victory;
    private float scoreTimer;
    private int scoreMin;
    private int scoreSec;

    // initialization
    void Start () {
        GameManager = GameObject.FindWithTag("GameManager");
        Player = GameObject.FindWithTag("Player");
        fail = false;
        victory = false;
        scoreTimer = 0;
        changePhase(0);
    }

    public void changePhase(int previousPhase)
    {
        currentPhase = previousPhase + 1;
        cleanPhase(previousPhase);

        switch (previousPhase)
        {
            case 0:
                Phase1Manager.SetActive(true);
                break;
            case 1:
                Phase2Manager.SetActive(true);
                break;
            case 2:
                Phase3Manager.SetActive(true);
                break;
            case 3:
                Phase4Manager.SetActive(true);
                break;
            case 4:
                playerWon();
                break;
        }
    }

    public void cleanPhase(int phaseId)
    {
        switch(phaseId)
        {
            case 1:
                Destroy(Phase1Manager);
                break;
            case 2:
                Destroy(Phase2Manager);
                break;
            case 3:
                Destroy(Phase3Manager);
                break;
            case 4:
                Destroy(Phase4Manager);
                break;
        }
    }

    public void playerWon()
    {
        victory = true;
        scoreMin = Mathf.FloorToInt(scoreTimer / 60);
        scoreSec = Mathf.FloorToInt(scoreTimer % 60);
        VictoryObject.SetActive(true);
        ScoreText.GetComponent<TextMesh>().text = "Time: " + scoreMin.ToString("00") + ":" + scoreSec.ToString("00");
        GameManager.GetComponent<GameManagerScript>().setHighScore(scoreTimer);
    }

    public void playerLost()
    {
        fail = true;
        cleanPhase(currentPhase);

        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject Enemy in Enemies)
        {
            Destroy(Enemy);
        }

        FailObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
        if(!fail && !victory)
            scoreTimer += Time.deltaTime;
        if (!Player.GetComponent<PlayerScript>().isAlive())
            playerLost();
	}
}
