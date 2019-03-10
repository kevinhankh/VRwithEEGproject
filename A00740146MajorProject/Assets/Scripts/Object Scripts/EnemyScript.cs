using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public GameObject EEGManager;
    public GameObject Head;
    public GameObject BulletSpawn;
    public GameObject BulletPrefab;
    public Material DefaultMaterial;
    public Material DamagedMaterial;
    public Transform Target;

    public AudioClip[] SpawnSfx;
    public AudioClip ShootSfx;
    public AudioClip HitSfx;
    public AudioClip DestroyedSfx;

    private AudioSource sound;
    private Behaviour halo;
    private Renderer rend;
    private Renderer rendHead;

    private int health;
    private float damageTimer;
    private bool damageCooldown;
    private float attackTimer;
    private bool attackCooldown;
    private float moveTimer;
    private bool moveCooldown;
    private float xMove;
    private float zMove;
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
        attackTimer = 0;
        attackCooldown = true;
        moveTimer = 0;
        moveCooldown = false;
        xMove = 0;
        zMove = 0;
        dead = false;
        deathSfx = false;
        deathTimer = 0;

        spawnSfxId = Random.Range(0, 3);
        if (spawnSfxId > 2)
            spawnSfxId = 2;
        sound.clip = SpawnSfx[spawnSfxId];
        sound.Play();
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

    //Takes damage every second. Changes colour and jumps when being hit
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

    //Simple randomized movement behaviour
    public void movement()
    {
        if (gameObject.transform.position.x > 10)
            xMove = -1;
        else if (gameObject.transform.position.x < -10)
            xMove = 1;
        if (gameObject.transform.position.z > 10)
            zMove = -1;
        else if (gameObject.transform.position.z < -10)
            zMove = 1;

        gameObject.transform.Translate(xMove * Time.deltaTime, 0, zMove * Time.deltaTime);
    }

    //Shoots a bullet at the player object
    public void attack()
    {
        var bullet = (GameObject)Instantiate(BulletPrefab, BulletSpawn.transform.position, BulletSpawn.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

        sound.clip = ShootSfx;
        sound.Play();

        Destroy(bullet, bulletDuration);
    }
    
	// Update is called once per frame
	void Update () {
        
        if(!dead)
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

            //Attack setup
            if (attackCooldown)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer > 3)
                {
                    attackCooldown = false;
                    attackTimer = 0;
                }
            }
            else
            {
                attack();
                attackCooldown = true;
            }

            //Movement setup
            if (moveCooldown)
            {
                moveTimer += Time.deltaTime;
                if (moveTimer > 3)
                {
                    moveCooldown = false;
                    moveTimer = 0;
                }
            }
            else
            {
                xMove = Random.Range(-1, 1);
                zMove = Random.Range(-1, 1);
                moveCooldown = true;
            }
            movement();
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
