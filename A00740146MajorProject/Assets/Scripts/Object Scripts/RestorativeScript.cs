using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorativeScript : MonoBehaviour {

    public GameObject EEGManager;
    public GameObject FillBar;
    public AudioClip FillSfx;

    private AudioSource sound;
    private Behaviour halo;

    private float fill;
    private float fillPosition;
    private Vector3 tempPos;
    private bool isFilled;
    private bool soundFlag;
    private float soundTimer;

    private const int maxFill = 30;

    // Use this for initialization
    void Start()
    {
        EEGManager = GameObject.FindWithTag("EEGManager");
        FillBar = gameObject.transform.GetChild(0).gameObject;
        sound = GetComponent<AudioSource>();
        sound.clip = FillSfx;
        sound.Play();
        halo = (Behaviour)gameObject.GetComponent("Halo");
        fill = 0;
        isFilled = false;
        soundFlag = false;
        soundTimer = 0;
    }

    //Gazed behaviour
    public void activate()
    {
        EEGManager.GetComponent<EEGManagerScript>().setTarget(this.gameObject);
        halo.enabled = true;
    }

    //No longer gazed
    public void ignore()
    {
        EEGManager.GetComponent<EEGManagerScript>().resetTarget();
        halo.enabled = false;
    }

    public void addFill(float fillIn)
    {
        fill += fillIn;
        fillCylinder();
    }

    public float getFill()
    {
        return fill;
    }

    //Fills the cylinder every second using the relaxed state. Updates visual of green filling.
    public void interaction()
    {
        if(fill < maxFill)
        {
            fill += Time.deltaTime;
            fillCylinder();
        }

        soundFlag = true;
        soundTimer = fill;
        soundTimer -= 1;
    }

    public void fillCylinder()
    {
        fillPosition = ((fill / maxFill) * 5.5f) - 2f;
        tempPos = FillBar.gameObject.transform.position;
        tempPos.y = fillPosition;
        FillBar.gameObject.transform.position = tempPos;
        if (fill > maxFill)
            completeFill();
    }

    public bool checkFilled()
    {
        return isFilled;
    }

    private void completeFill()
    {
        fill = maxFill;
        isFilled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(soundFlag)
        {
            soundTimer += Time.deltaTime;
            if (!sound.isPlaying)
            {
                sound.UnPause();
            }
            if (soundTimer > fill)
            {
                soundFlag = false;
            }
        }
        else
        {
            sound.Pause();
        }
    }
}
