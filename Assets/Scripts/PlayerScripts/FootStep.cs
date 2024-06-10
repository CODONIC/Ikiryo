using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public Animator animator;
    public AudioSource footstepSounds;
    public AudioSource sprintSounds;
    public float walkingSpeed = 0.46f; // 30/65 to adjust to your walking animation sample frame
    public float sprintingSpeed = 1f; // Default speed for sprinting animation sample frame

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!sprintSounds.isPlaying)
                {
                    sprintSounds.Play();
                    footstepSounds.Stop();
                }
                animator.speed = sprintingSpeed;
            }
            else
            {
                if (!footstepSounds.isPlaying)
                {
                    footstepSounds.Play();
                    sprintSounds.Stop();
                }
                animator.speed = walkingSpeed;
            }
        }
        else
        {
            if (footstepSounds.isPlaying || sprintSounds.isPlaying)
            {
                footstepSounds.Stop();
                sprintSounds.Stop();
            }
            animator.speed = 0f; // Stop animation when no movement
        }
    }
}
