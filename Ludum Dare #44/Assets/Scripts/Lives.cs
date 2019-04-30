using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class Lives : MonoBehaviour {
    [SerializeField]
    Heart[] hearts;

    public void GainHeart() {
        // start at back of array and move forward
        for (int i = hearts.Length - 1; i >= 0; i--) {
            if (hearts[i].empty) {
                hearts[i].GainHeart();
                break;
            }
        }
    }

    public void LoseHeart() {
        // start at back of array and move forward
        for (int i = hearts.Length - 1; i >= 0; i--) {
            if (!hearts[i].empty) {
                hearts[i].LoseHeart();
                break;
            }
        }
    }

    public void Reset() {
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].GainHeart();
        }
    }

    // display during upgrades
    public void Display() {
        transform.localScale = new Vector2(2, 2);
        transform.position = new Vector2(-2, 3.5f);
        GetComponent<SortingGroup>().sortingLayerName = "UI";
        GetComponent<SortingGroup>().sortingOrder = 1;
    }

    // display during gameplay
    public void Revert() {
        transform.localScale = new Vector2(1, 1);
        transform.position = new Vector2(-8.15f, 3.8f);
        GetComponent<SortingGroup>().sortingLayerName = "Foreground";
    }
}
