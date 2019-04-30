using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    // Platforms
    [SerializeField]
    GameObject[] orangePlatformsBG, orangePlatformsFG, bluePlatformsBG, bluePlatformsFG, redPlatformsBG, redPlatformsFG;

    [SerializeField]
    GameObject flag;

    [SerializeField]
    float largeOffset, mediumOffset, smallOffset;

    [SerializeField]
    float minFG, maxFG, minBG, maxBG;
    public float timeBetweenSpawns;

    // for use in GameManager
    [HideInInspector]
    public bool checkpoint, blueZone, redZone;
    public bool spawning;

    // timers
    float timeSinceSpawn;

    void Start() {
        checkpoint = false;
        blueZone = false;
        redZone = false;
        timeSinceSpawn = 0;
    }

    void Update() {
        if (spawning)
            Spawn();        
    }

    public void HardReset() {
        blueZone = false;
        redZone = false;
        timeSinceSpawn = 0;
    }

    public void SoftReset() {
        timeSinceSpawn = 0;
    }

    void Spawn() {
        if (timeSinceSpawn <= 0) {
            GameObject platBG, platFG;

            int indexBG = Random.Range(0, orangePlatformsBG.Length);
            int indexFG = Random.Range(0, orangePlatformsFG.Length);
            float heightBG = Random.Range(minBG, maxBG);
            float heightFG = Random.Range(minFG, maxFG);

            // BG and FG cannot have same height
            while (heightFG <= heightBG + 3 && heightFG >= heightBG + 1)
                heightFG = Random.Range(minFG, maxFG);

            Vector2 positionBG = new Vector2(transform.position.x, heightBG);
            Vector2 positionFG = new Vector2(transform.position.x, heightFG);

            // red zone
            if (redZone) {
                platBG = redPlatformsBG[indexBG];
                platFG = redPlatformsFG[indexFG];

                // blue zone
            } else if (blueZone) {
                platBG = bluePlatformsBG[indexBG];
                platFG = bluePlatformsFG[indexFG];

                // orange zone
            } else {
                platBG = orangePlatformsBG[indexBG];
                platFG = orangePlatformsFG[indexFG];
            }            

            if (checkpoint) {
                float max = Mathf.Max(heightBG, heightFG - 2);
                float heightFlag = max + 4.78f;
                Vector2 position1 = new Vector2(transform.position.x, heightBG + 4.78f);
                Vector2 position2 = new Vector2(transform.position.x, heightFG + 2.78f);

                GameObject pair = Instantiate(flag, position1, Quaternion.identity);
                GameObject withCollider = Instantiate(flag, position2, Quaternion.identity);

                withCollider.GetComponent<Checkpoint>().pair = pair;
                withCollider.AddComponent<BoxCollider2D>().size = new Vector2(0.5f, 20);
                withCollider.GetComponent<BoxCollider2D>().isTrigger = true;
                checkpoint = false;
            }

            // spawn platoforms
            Instantiate(platBG, positionBG, Quaternion.identity);
            Instantiate(platFG, positionFG, Quaternion.identity);

            timeSinceSpawn = timeBetweenSpawns;

            // decrease time between spawns

        } else {
            timeSinceSpawn -= Time.deltaTime;
        }
    }
}
