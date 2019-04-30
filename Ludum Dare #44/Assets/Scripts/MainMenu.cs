using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    GameObject instructions;

    public void Instructions() {
        instructions.SetActive(true);
    }

    public void Back() {
        instructions.SetActive(false);
    }

    public void Play() {
        SceneManager.LoadScene(1);
    }

    public void Quit() {
        Application.Quit();
    }
}
