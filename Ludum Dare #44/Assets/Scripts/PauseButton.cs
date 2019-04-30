using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseButton : MonoBehaviour {

    [SerializeField]
    Sprite pause, play;

    public void Pause() {
        GetComponent<Image>().sprite = pause;
    }

    public void Play() {
        GetComponent<Image>().sprite = play;
    }
}
