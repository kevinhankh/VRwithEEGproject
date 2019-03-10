using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchScript : MonoBehaviour {

    public GameObject EEGManager;
    public AudioClip SwitchSfx;

    private AudioSource sound;
    private bool switchStatus;
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

    //Flips the switch on and off. Changes colour.
    public void interaction()
    {
        switchStatus = !switchStatus;

        if (switchStatus)
        {
            rend.material.color = Color.green;
            lit.color = Color.green;
        }
        else
        {
            rend.material.color = Color.red;
            lit.color = Color.red;
        }

        sound.clip = SwitchSfx;
        sound.Play();
    }

    public bool getStatus()
    {
        return switchStatus;
    }
    
	// Update is called once per frame
	void Update () {

    }
}
