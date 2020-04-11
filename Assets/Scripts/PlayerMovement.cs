using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 25f;
    public bool hasJumpPotion = false;
    public bool hasSpeedPotion = false;
    public int potionModAmount = 0;

    public AudioClip jumpClip;

    private float potionTimeMax = 10f;
    private float potionTimeCur = 0f;

    float horizontalMove = 0f;
    bool jumpFlag = false;
    bool jump = false;

    public float m_SpeedForceMod = 1f;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed * m_SpeedForceMod;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (jumpFlag)
        {
            animator.SetBool("IsJumping", true);
            jumpFlag = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(animator.GetBool("IsJumping") == false)
            {
                AudioSource.PlayClipAtPoint(jumpClip, transform.position);
                jump = true;
                animator.SetBool("IsJumping", true);
            }
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        jump = false;
    }

    void FixedUpdate()
    {
        if(hasJumpPotion && potionTimeCur < potionTimeMax)
        {
            controller.m_JumpForceMod = potionModAmount;
            potionTimeCur += Time.fixedDeltaTime;
            hasSpeedPotion = false;
            m_SpeedForceMod = 1;
        }
        else if(hasSpeedPotion && potionTimeCur < potionTimeMax)
        {
            m_SpeedForceMod = potionModAmount;
            potionTimeCur += Time.fixedDeltaTime;
            hasJumpPotion = false;
            controller.m_JumpForceMod = 0;
        }
        else
        {
            potionTimeCur = 0f;
            controller.m_JumpForceMod = 0;
            m_SpeedForceMod = 1;
            hasJumpPotion = false;
            hasSpeedPotion = false;
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);

        if (jump)
        {
            jumpFlag = true;
        }

        
    }
}
