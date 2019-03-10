using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    public GameObject Player;
    public AudioClip PropheticForesight;
    public AudioClip TranquilForce;
    
    public static GameManagerScript instance = null;

    private AudioSource Music;

    private float highScore;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // initialization
    void Start () {
        Player = GameObject.FindWithTag("Player");
        Music = GetComponent<AudioSource>();
        highScore = 1000000;
	}
    
    public void changeScene(int sceneId)
    {
        Player.GetComponent<PlayerScript>().resetHealth();
        switch(sceneId)
        {
            case 0:
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                if (Music.clip != PropheticForesight)
                {
                    Music.clip = PropheticForesight;
                    Music.Play();
                }
                    
                break;
            case 1:
                SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
                if (Music.clip != PropheticForesight)
                {
                    Music.clip = PropheticForesight;
                    Music.Play();
                }
                break;
            case 2:
                SceneManager.LoadScene("Play", LoadSceneMode.Single);
                if (Music.clip != TranquilForce)
                {
                    Music.clip = TranquilForce;
                    Music.Play();
                }
                break;
        }
    }

    public void setHighScore(float score)
    {
        if(score < highScore)
            highScore = score;
    }

    public float getHighScore()
    {
        return highScore;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
