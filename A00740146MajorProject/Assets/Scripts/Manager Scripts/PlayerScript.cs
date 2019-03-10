using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {

    public static PlayerScript instance = null;

    public GameObject EEGManager;
    public RectTransform HealthBar;
    public Slider Brainwave;
    public GameObject GazeBar;
    public RectTransform GazeFill;
    public AudioClip PlayerHitSfx;

    private AudioSource sound;
    private int health;
    private bool alive;
    private ColorBlock cb;
    private bool gazeBegin;
    private float gazeMeter;
    private float gazeSize;
    private bool shield;

    private const int healthPoints = 100;
    private const float FocusedThreshold = 0.72f;
    private const float RelaxedThreshold = 0.28f;
    private const float gazeDuration = 2f;

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

        EEGManager = GameObject.FindWithTag("EEGManager");
        HealthBar = GameObject.FindWithTag("HealthBar").GetComponent<RectTransform>();
        Brainwave = GameObject.FindWithTag("Brainwave").GetComponent<Slider>();
        GazeBar = GameObject.FindWithTag("GazeBar");
        GazeFill = GameObject.FindWithTag("GazeFill").GetComponent<RectTransform>();
        GazeBar.SetActive(false);
        sound = GetComponent<AudioSource>();
        sound.clip = PlayerHitSfx;

        Brainwave.value = 0.5f;
        health = healthPoints;
        alive = true;
        gazeBegin = false;
        gazeMeter = 0;
        shield = false;
	}

    public void takeDamage(int damage)
    {
        if(!shield && alive)
        {
            health -= damage;
            HealthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health);
            sound.Play();
            if (health <= 0)
                alive = false;
        }
    }

    public void heal(int healValue)
    {
        health += healValue;
        if (health > 100)
            health = 100;
        HealthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health);
    }

    public void resetHealth()
    {
        alive = true;
        health = 100;
        HealthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health);
    }

    public int getHealth()
    {
        return health;
    }

    public bool isAlive()
    {
        return alive;
    }

    public void setShield(bool shieldState)
    {
        shield = shieldState;
    }

    public void updateBrainwave()
    {
        Brainwave.value = EEGManager.GetComponent<EEGManagerScript>().getBrainwave();
        cb = Brainwave.colors;
        if (Brainwave.value < RelaxedThreshold)
        {
            cb.normalColor = Color.cyan;
        }
        else if (Brainwave.value > FocusedThreshold)
        {
            cb.normalColor = Color.magenta;
        }
        else
        {
            cb.normalColor = Color.white;
        }
        Brainwave.colors = cb;
    }

    public void setGazeBegin(bool gaze)
    {
        gazeBegin = gaze;
        if (gazeBegin)
        {
            GazeBar.SetActive(true);
        }
        else
        {
            GazeBar.SetActive(false);
            gazeMeter = 0;
            GazeFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, gazeMeter);
        }
            
    }

    public void gazeActive()
    {
        gazeMeter += Time.deltaTime;
        gazeSize = (gazeMeter / gazeDuration) * 100;
        GazeFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, gazeSize);
    }
    
	// Update is called once per frame
	void Update () {
        updateBrainwave();

        if (gazeBegin)
            gazeActive();
	}
}
