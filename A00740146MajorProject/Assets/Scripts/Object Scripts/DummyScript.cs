using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour {
    public GameObject EEGManager;
    public GameObject Head;
    public GameObject BulletSpawn;
    public GameObject BulletPrefab;
    public Material DefaultMaterial;
    public Material DamagedMaterial;
    public Transform Target;
    
    public AudioClip HitSfx;
    public AudioClip DestroyedSfx;

    private AudioSource sound;
    private Behaviour halo;
    private Renderer rend;
    private Renderer rendHead;

    private int health;
    private float damageTimer;
    private bool damageCooldown;
    private int spawnSfxId;
    private bool dead;
    private bool deathSfx;
    private float deathTimer;

    private const float bulletSpeed = 10;
    private const float bulletDuration = 5;
    private const int healthPoints = 5;

    // initialization
    void Start()
    {
        EEGManager = GameObject.FindWithTag("EEGManager");
        Head = gameObject.transform.GetChild(0).gameObject;
        BulletSpawn = this.gameObject.transform.GetChild(1).gameObject;
        Target = GameObject.FindWithTag("Player").transform;
        sound = GetComponent<AudioSource>();
        halo = (Behaviour)gameObject.GetComponent("Halo");
        rend = GetComponent<Renderer>();
        rendHead = Head.GetComponent<Renderer>();
        health = healthPoints;
        damageTimer = 0;
        damageCooldown = false;
        dead = false;
        deathSfx = false;
        deathTimer = 0;
        
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
        resetStatus();
    }

    //Takes 1 damage every second
    public void interaction()
    {
        rend.material = DamagedMaterial;
        rendHead.material = DamagedMaterial;
        if (!damageCooldown)
        {
            health -= 1;
            damageCooldown = true;
            gameObject.transform.Translate(0, 0.25f, 0);

            sound.clip = HitSfx;
            sound.Play();
        }
    }

    public void resetStatus()
    {
        rend.material = DefaultMaterial;
        rendHead.material = DefaultMaterial;
    }

    // Update is called once per frame
    void Update()
    {

        if (!dead)
        {
            transform.LookAt(Target);

            if (health <= 0)
            {
                dead = true;
                deathSfx = true;
            }

            if (damageCooldown)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer > 1)
                {
                    damageCooldown = false;
                    damageTimer = 0;
                }
            }
            
        }
        else
        {
            deathTimer += Time.deltaTime;
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 800);
            if (deathTimer > 1.5f)
                Destroy(gameObject);
        }

        if (deathSfx)
        {
            sound.clip = DestroyedSfx;
            sound.Play();
            deathSfx = false;
        }
    }
}
