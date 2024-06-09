using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public AudioSource footstepSounds;
    public AudioSource sprintSounds;

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
            }
            else
            {
                if (!footstepSounds.isPlaying)
                {
                    footstepSounds.Play();
                    sprintSounds.Stop();
                }
            }
        }
        else
        {
            if (footstepSounds.isPlaying || sprintSounds.isPlaying)
            {
                footstepSounds.Stop();
                sprintSounds.Stop();
            }
        }
    }
}
