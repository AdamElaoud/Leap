using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPlatform : MonoBehaviour {

    [SerializeField]
    Sprite orange, blue, red;
    
    public void Orange() {
        GetComponent<SpriteRenderer>().sprite = orange;
    }

    public void Blue() {
        GetComponent<SpriteRenderer>().sprite = blue;
    }

    public void Red() {
        GetComponent<SpriteRenderer>().sprite = red;
    }
}
