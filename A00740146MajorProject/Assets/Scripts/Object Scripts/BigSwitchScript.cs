using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSwitchScript : MonoBehaviour {

    public GameObject EEGManager;
    public AudioClip LogicPass;
    public AudioClip LogicFail;

    private AudioSource sound;
    private bool switchStatus;
    private bool confirm;
    private Behaviour halo;
    private Renderer rend;
    private Light lit;

    // Use this for initialization
    void Start()
    {
        EEGManager = GameObject.FindWithTag("EEGManager");
        sound = GetComponent<AudioSource>();
        halo = (Behaviour)gameObject.GetComponent("Halo");
        rend = GetComponent<Renderer>();
        lit = GetComponent<Light>();
        switchStatus = false;
        confirm = false;
    }

    //Gazed behaviour
    public void activate()
    {
        halo.enabled = true;
        EEGManager.GetComponent<EEGManagerScript>().setTarget(this.gameObject);
    }

    //No longer gazed
    public void ignore()
    {
        halo.enabled = false;
        EEGManager.GetComponent<EEGManagerScript>().resetTarget();
    }

    //When the player is targeting this object and blinks,
    public void interaction()
    {
        //check confirmation and add cooldown timer
        if(!confirm)
        {
            confirm = true;
        }
    }

    public void resetStatus()
    {
        confirm = false;
        rend.material.color = Color.yellow;
        lit.color = Color.yellow;
    }

    public bool getStatus()
    {
        return switchStatus;
    }

    public bool getConfirm()
    {
        return confirm;
    }

    public void confirmCheck(bool checkedData)
    {
        //green correct, red incorrect, set confirm
        switchStatus = checkedData;

        if (switchStatus)
        {
            rend.material.color = Color.green;
            lit.color = Color.green;
            sound.clip = LogicPass;
            sound.Play();
        }
        else
        {
            rend.material.color = Color.red;
            lit.color = Color.red;
            sound.clip = LogicFail;
            sound.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
