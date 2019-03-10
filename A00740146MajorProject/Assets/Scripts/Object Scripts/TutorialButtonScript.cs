using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonScript : MonoBehaviour {

    public GameObject GameManager;
    public GameObject Player;

    private Behaviour halo;
    private bool gazeStart;
    private float gazeTimer;

    private const float gazeDuration = 2;
    private const int buttonId = 1;

    // initialization
    void Start()
    {
        GameManager = GameObject.FindWithTag("GameManager");
        Player = GameObject.FindWithTag("Player");
        halo = (Behaviour)gameObject.GetComponent("Halo");
        gazeStart = false;
    }

    //This will begin the gaze timer for the player to change the scene to the tutorial.
    public void activate()
    {
        halo.enabled = true;
        gazeStart = true;
        Player.GetComponent<PlayerScript>().setGazeBegin(true);
    }

    //Reset gaze timer when no longer targeted.
    public void ignore()
    {
        halo.enabled = false;
        gazeStart = false;
        gazeTimer = 0;
        Player.GetComponent<PlayerScript>().setGazeBegin(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gazeStart)
        {
            gazeTimer += Time.deltaTime;
            if (gazeTimer > gazeDuration)
            {
                gazeStart = false;
                gazeTimer = 0;
                Player.GetComponent<PlayerScript>().setGazeBegin(false);
                GameManager.GetComponent<GameManagerScript>().changeScene(buttonId);
            }
        }
    }
}
