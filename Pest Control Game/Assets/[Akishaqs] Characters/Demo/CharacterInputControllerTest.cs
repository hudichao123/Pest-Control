﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputControllerTest : MonoBehaviour
{

    static Animator anim;
    public float v;
    public float h;
    public bool hasPU;
    public float runTimeCount;
    public float speed = 1.0f;
    public bool reseted = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning 0", false);
        anim.SetBool("isWalking", true);

    }

    // Update is called once per frame
    void Update()
    {
        v = Input.GetAxis("Vertical") * speed;
        h = Input.GetAxis("Horizontal") * speed;
        hasPU = GetComponent<PowerUpCollector>().hasPU;
        transform.Rotate(0, h, 0);
        if (hasPU)
        {
            runTimeCount = 5;
        }
        if (v != 0)
        {
            anim.SetBool("isIdle", false);
            if (v > 0)
            {
                if (runTimeCount > 0 && v > 0)
                {
                    //RUNNING
                    anim.SetBool("isRunning 0", true);
                    anim.SetBool("isWalking", false);
                    if (h < 0)
                    {
                        anim.SetBool("runningleft", true);
                        anim.SetBool("runningright", false);
                        anim.SetBool("right", false);
                        anim.SetBool("left", false);
                    }
                    else if (h > 0)
                    {
                        anim.SetBool("runningright", true);
                        anim.SetBool("runningleft", false);
                        anim.SetBool("right", false);
                        anim.SetBool("left", false);
                    } else
                    {
                        anim.SetBool("runningleft", false);
                        anim.SetBool("right", false);
                        anim.SetBool("left", false);
                        anim.SetBool("runningright", false);
                    }
                }
                else if (runTimeCount <= 0 && v > 0)
                {
                    reseted = true;
                    //WALKING
                    anim.SetBool("isRunning 0", false);
                    anim.SetBool("isWalking", true);
                    if (h < 0)
                    {
                        anim.SetBool("left", true);
                        anim.SetBool("right", false);
                        anim.SetBool("runningright", false);
                        anim.SetBool("runningleft", false);
                    }
                    else if (h > 0)
                    {
                        anim.SetBool("right", true);
                        anim.SetBool("left", false);
                        anim.SetBool("runningright", false);
                        anim.SetBool("runningleft", false);
                    } else
                    {
                        anim.SetBool("runningleft", false);
                        anim.SetBool("right", false);
                        anim.SetBool("left", false);
                        anim.SetBool("runningright", false);
                    }
                } else if (v == 0)
                {
                    anim.SetBool("isIdle", true);
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isRunning 0", false);
                }

            }
        } else
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isRunning 0", false);
            anim.SetBool("isWalking", false);
        }
        runTimeCount -= Time.time;
    }

    void FixedUpdate()
    {
        anim.SetFloat("walk", v);
        anim.SetFloat("turn", h);
    }
}
