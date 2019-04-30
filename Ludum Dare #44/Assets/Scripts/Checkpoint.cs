using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [SerializeField]
    Sprite complete;

    [HideInInspector]
    public GameObject pair;

    // Component
    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate() {
        // move left
        transform.Translate(Vector2.left * GameManager.instance.scrollSpeed);
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Player")) {
            // animation
            anim.SetBool("Checkpoint", true);
            pair.GetComponent<Animator>().SetBool("Checkpoint", true);

            // change sprites
            GetComponent<SpriteRenderer>().sprite = complete;
            pair.GetComponent<SpriteRenderer>().sprite = complete;

            GameManager.instance.Upgrades();
            GameManager.instance.SpeedUp();

            AudioManager.instance.SpeedUp("Leap_Theme");
        }
    }
    
}
