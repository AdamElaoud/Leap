using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour {
    
    public void PlayAgain() {
        GameManager.instance.Restart();
    }

    public void ReturnToMenu() {
        GameManager.instance.Pause();
        SceneManager.UnloadSceneAsync(2);
        SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene(0);
    }
}
