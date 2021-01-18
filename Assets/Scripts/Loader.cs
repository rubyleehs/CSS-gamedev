using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;

    // If the game manager does not exist, instantiate one
    void Awake() {
        if(GameManager.instance == null) {
            Instantiate(gameManager);
        }
    }
}
