using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // Objects
    [SerializeField]
    Spawner spawner;
    [SerializeField]
    GameObject distanceDisplay, gameOverDistance;
    [SerializeField]
    Lives heartsGUI;
    [SerializeField]
    GameObject upgrades, gameOver;
    [SerializeField]
    Button button1, button2;
    [SerializeField]
    PauseButton pauseButton;
    [SerializeField]
    CurrentUpgrades[] currentUpgrades;

    // GameManager singleton
    public static GameManager instance;

    // game values
    public float scrollSpeed;
    public int lives;
    float speedIncrease, sloMoTime;
    int distance, initialLives;
    [SerializeField]
    int checkpointGap, blueZone, redZone;
    [SerializeField]
    float maxSloMo;

    // Player power rups
    public bool hasGlide;
    public bool hasDoubleJump;
    public bool hasLongJump;
    public bool hasSloMo;

    // State
    bool paused, sloMo, upgrading;
    public bool allowInputs;

    void Start() {
        // load level additively
        if (SceneManager.sceneCount < 2)
            StartCoroutine(LoadLevel());

        // game variables
        paused = false;
        sloMo = false;
        upgrading = false;
        allowInputs = true;
        distance = 0;
        speedIncrease = 0;
        initialLives = lives;
        distanceDisplay.GetComponent<TextMesh>().text = "Distance: " + distance;

        // player power ups
        hasGlide = false;
        hasDoubleJump = false;
        hasLongJump = false;
        hasSloMo = false;
    }

    IEnumerator LoadLevel() {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);

        // wait until scene loaded
        yield return null;
        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Play"));
        spawner.spawning = true;

        // maintain starting platform color consistency
        StartingPlatform platformScript = GameObject.FindGameObjectWithTag("Platform").GetComponent<StartingPlatform>();

        // zone check
        if (distance >= redZone)
            platformScript.Red();
        else if (distance >= blueZone)
            platformScript.Blue();
        else
            platformScript.Orange();
    }

    void Awake() {
        instance = this;
    }

    void Update() {
        if (upgrading) {
            if (Input.GetKeyDown(KeyCode.A))
                button1.GetComponent<UpgradeButton>().Upgrade();

            if (Input.GetKeyDown(KeyCode.D))
                button2.GetComponent<UpgradeButton>().Upgrade();
        }

        if (!paused) {
            distance++;
            distanceDisplay.GetComponent<TextMesh>().text = "Distance: " + distance;

            // zone check
            if (distance >= redZone)
                spawner.redZone = true;
            else if (distance >= blueZone)
                spawner.blueZone = true;

            // spawn checkpoint
            if (distance % checkpointGap == 0)
                spawner.checkpoint = true;
        }

        // slo-mo
        if (sloMo) {
            if (sloMoTime <= 0)
                SloMo();
            else
                sloMoTime -= Time.deltaTime;
        }  
    }

    public void Pause() {
        if (paused) {
            if (sloMo)
                Time.timeScale = 0.35f;
            else
                Time.timeScale = 1;

            spawner.spawning = true;
            paused = false;
            pauseButton.Pause();

        } else {
            Time.timeScale = 0;
            spawner.spawning = false;
            paused = true;
            pauseButton.Play();
        }
    }

    public void SloMo() {
        if (sloMo) {
            Time.timeScale = 1;
            sloMo = false;

        } else {
            Time.timeScale = 0.35f;
            sloMo = true;
            sloMoTime = maxSloMo;
        }

        AudioManager.instance.SloMo();
    }

    public void Restart() {
        // reset game variables
        distance = 0;
        scrollSpeed = scrollSpeed - speedIncrease;
        speedIncrease = 0;
        heartsGUI.Reset();
        spawner.HardReset();
        spawner.spawning = false;
        lives = initialLives;
        AudioManager.instance.ResetPitch("Leap_Theme");

        for (int i = 0; i < currentUpgrades.Length; i++) {
            currentUpgrades[i].Wipe();
        }

        if (paused)
            Pause();

        // reset player power ups
        hasGlide = false;
        hasDoubleJump = false;
        hasLongJump = false;
        hasSloMo = false;

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Play"));
        StartCoroutine(LoadLevel());
        upgrades.SetActive(false);
        gameOver.SetActive(false);
    }

    public void SoftReset() {
        spawner.spawning = false;
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Play"));
        StartCoroutine(LoadLevel());
        spawner.SoftReset();
        upgrades.SetActive(false);
    }

    public void GameOver() {
        Pause();
        spawner.spawning = false;

        gameOver.SetActive(true);
        gameOverDistance.GetComponent<TextMesh>().text = "Total Distance: " + distance;
    }

    public void Upgrades() {
        // only if player has lives to spare
        if (lives > 1) {
            // temporarily block keyboard inputs and pause
            allowInputs = false;
            upgrading = true;
            Pause();

            upgrades.SetActive(true);
            heartsGUI.Display();

            // randomly select available upgrades
            button1.GetComponent<UpgradeButton>().LoadPowerUps();
            button2.GetComponent<UpgradeButton>().LoadPowerUps();
        }       
    }

    public void CloseUpgrades() {
        upgrades.SetActive(false);
        heartsGUI.Revert();

        // allow inputs and unpause
        allowInputs = true;
        upgrading = false;
        Pause();
    }

    public void DoubleJumpUpgrade() {
        hasDoubleJump = true;
    }

    public void GlideUpgrade() {
        hasGlide = true;
    }

    public void LongJumpUpgrade() {
        hasLongJump = true;
        spawner.timeBetweenSpawns -= 0.15f;
    }

    public void SloMoUpgrade() {
        hasSloMo = true;
    }

    public void SpeedUp() {
        scrollSpeed += 0.01f;
        speedIncrease += 0.01f;
    }

    public void LoseHeart() {
        heartsGUI.LoseHeart();
    }
}
