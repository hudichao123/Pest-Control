﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOver : MonoBehaviour {

    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            print("Level Over Boi");
            SceneManager.LoadScene("GameOverScreen");
        }
    }
}