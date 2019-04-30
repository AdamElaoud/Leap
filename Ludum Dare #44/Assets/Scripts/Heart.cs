using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {
    [SerializeField]
    Sprite full, blank;

    public bool empty;

    void Start() {
        empty = false;
    }

    public void GainHeart() {
        GetComponent<SpriteRenderer>().sprite = full;
        empty = false;
    } 

    public void LoseHeart() {
        GetComponent<SpriteRenderer>().sprite = blank;
        empty = true;
    }
}
