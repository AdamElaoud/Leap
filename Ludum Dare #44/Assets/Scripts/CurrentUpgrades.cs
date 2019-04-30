using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUpgrades : MonoBehaviour {
    [SerializeField]
    Sprite[] icons;
    // 0: Double Jump
    // 1: Glide
    // 2: Long Jump
    // 3: Slo-Mo

    public bool hasPower;

    void Start() {
        hasPower = false;
    }
    
    public void Display(int powerup) {
        // display powerup
        switch (powerup) {
            case 0:
                GetComponent<SpriteRenderer>().sprite = icons[0];
                break;

            case 1:
                GetComponent<SpriteRenderer>().sprite = icons[1];
                break;

            case 2:
                GetComponent<SpriteRenderer>().sprite = icons[2];
                break;

            case 3:
                GetComponent<SpriteRenderer>().sprite = icons[3];
                break;
        }

        hasPower = true;
    }

    public void Wipe() {
        GetComponent<SpriteRenderer>().sprite = null;
        hasPower = false;
    }
}
