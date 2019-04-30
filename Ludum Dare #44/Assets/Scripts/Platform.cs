using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    void FixedUpdate() {
        // move left
        transform.Translate(Vector2.left * GameManager.instance.scrollSpeed);
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
