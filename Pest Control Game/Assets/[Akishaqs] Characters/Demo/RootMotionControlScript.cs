﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//require some things the bot control needs
[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputControllerTest))]
public class RootMotionControlScript : MonoBehaviour
{
    public float animationSpeed = 1.1f;
    public float rootMovementSpeed = 1.1f;
    public float rootTurnSpeed = 1.1f;

    private Animator anim;
    private Rigidbody rbody;
    private CharacterInputControllerTest cinput;

    private Transform leftFoot;
    private Transform rightFoot;

    //Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;

    public bool isGrounded;

    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<CharacterInputControllerTest>();
        if (cinput == null)
            Debug.Log("CharacterInput could not be found");
    }


    // Use this for initialization
    void Start()
    {
        //example of how to get access to certain limbs
        leftFoot = this.transform.Find("mixamorig:hips/mixamorig:thigh.L/mixamorig:shin.L/mixamorig:foot.L");
        rightFoot = this.transform.Find("mixamorig:hips/mixamorig:thigh.R/mixamorig:shin.R/mixamorig:foot.R");
        
        if (leftFoot == null || rightFoot == null)
            Debug.Log("One of the feet could not be found");

        isGrounded = false;

        //never sleep so that OnCollisionStay() always reports for ground check
        rbody.sleepThreshold = 0f;
    }

    void Update()
    {
        //TODO 
        animationSpeed = 1.1f;
        rootMovementSpeed = 1.1f;
        rootTurnSpeed = 1.2f;
        anim.speed = animationSpeed;
    }

    void FixedUpdate()
    {

        float inputForward = 0f;
        float inputTurn = 0f;

        if (cinput.enabled)
        {
            inputForward = cinput.v;
            inputTurn = cinput.h;
        }

        //onCollisionStay() doesn't always work for checking if the character is grounded from a playability perspective
        //Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
        //Therefore, an additional raycast approach is used to check for close ground
        //if (CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround))
        //    isGrounded = true;


        anim.SetFloat("v", inputTurn);
        anim.SetFloat("h", inputForward);
        anim.SetBool("isFalling", !isGrounded);


    }



    //This is a physics callback
    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;

    }

    //This is a physics callback
    /*void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.gameObject.tag == "wall")
        {

            EventManager.TriggerEvent<PlayerLandsEvent, Vector3, float>(collision.contacts[0].point, collision.impulse.magnitude);

        }

    }*/



    void OnAnimatorMove()
    {

        Vector3 newRootPosition;
        Quaternion newRootRotation;

        if (isGrounded)
        {
            //use root motion as is if on the ground		
            newRootPosition = anim.rootPosition;
        }
        else
        {
            //Simple trick to keep model from climbing other rigidbodies that aren't the ground
            newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        }

        //use rotational root motion as is
        newRootRotation = anim.rootRotation;

        //TODO Here, you could scale the difference in position and rotation to make the character go faster or slower

        this.transform.position = Vector3.LerpUnclamped(this.transform.position, newRootPosition, rootMovementSpeed);
        this.transform.rotation = Quaternion.LerpUnclamped(this.transform.rotation, newRootRotation, rootTurnSpeed);

        //clear IsGrounded
        isGrounded = false;
    }

}
