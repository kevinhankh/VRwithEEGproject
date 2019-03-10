using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Phase 2:
 * - spawns breakable cube
 * - when cube is destroyed, spawn 2 enemies
 * - when the 2 enemies are destroyed, change level to phase 3
 */
public class Phase2Script : MonoBehaviour {

    public GameObject LevelManager;
    public GameObject EnemyPrefab;
    public GameObject BreakablePrefab;

    public GameObject BreakableSpawner;
    public GameObject Breakable;
    public GameObject Spawner1;
    public GameObject Spawner2;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public AudioClip BreakableSpawnSfx;
    public AudioClip BreakableDestroyedSfx;

    private AudioSource sound;
    private bool spawnP2Enemies;

    private const int phaseId = 2;

    // initialization
    void Start () {
        LevelManager = GameObject.FindWithTag("LevelManager");
        sound = GetComponent<AudioSource>();
        Breakable = (GameObject)Instantiate(BreakablePrefab, BreakableSpawner.transform.position, BreakableSpawner.transform.rotation);
        spawnP2Enemies = false;
    }

    private void spawnEnemy()
    {
        Enemy1 = (GameObject)Instantiate(EnemyPrefab, Spawner1.transform.position, Spawner1.transform.rotation);
        Enemy2 = (GameObject)Instantiate(EnemyPrefab, Spawner2.transform.position, Spawner2.transform.rotation);
        sound.clip = BreakableDestroyedSfx;
        sound.loop = false;
        sound.Play();
    }

    private void exitPhase()
    {
        LevelManager.GetComponent<LevelManagerScript>().changePhase(phaseId);
    }

    // Update is called once per frame
    void Update () {
        if ((Breakable == null) && (!spawnP2Enemies))
        {
            spawnEnemy();
            spawnP2Enemies = true;
        }

        if ((Enemy1 == null) && (Enemy2 == null) && spawnP2Enemies)
            exitPhase();
    }
}
