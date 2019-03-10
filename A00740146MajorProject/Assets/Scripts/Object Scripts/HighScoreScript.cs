using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreScript : MonoBehaviour {

    public GameObject GameManager;

    private bool refreshScore;
    private int scoreMin;
    private int scoreSec;

	// initialization
	void Start () {
        GameManager = GameObject.FindWithTag("GameManager");
        refreshScore = true;
	}

    public void setScore(float score)
    {
        if (score > 100000)
            score = 0;
        scoreMin = Mathf.FloorToInt(score / 60);
        scoreSec = Mathf.FloorToInt(score % 60);
        GetComponent<TextMesh>().text = "Best Time: " + scoreMin.ToString("00") + ":" + scoreSec.ToString("00");
    }
	
	// Update is called once per frame
	void Update () {
		if(refreshScore)
        {
            setScore(GameManager.GetComponent<GameManagerScript>().getHighScore());
            refreshScore = false;
        }
	}
}
