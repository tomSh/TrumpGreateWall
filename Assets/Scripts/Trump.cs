﻿using UnityEngine;
using System.Collections;

public class Trump : MonoBehaviour {

    public GameObject explosion;        // Prefab of explosion effect.
    public AudioClip[] taunts;
    public AudioClip deathSound;
    private int lastTauntIndex = 0;
    private static float TAUNT_INTERVAL = 1;

    void OnCollisionEnter2D(Collision2D objectYouCollidedWith)
    {
        Collider2D collider = objectYouCollidedWith.collider;
        if (collider.tag == "Bullet") {
            OnExplode();
        }
    }


    void OnExplode()
    {
        // Create a quaternion with a random rotation in the z-axis.
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        // Instantiate the explosion where the rocket is with the random rotation.
        Instantiate(explosion, transform.position, randomRotation);
        AudioSource audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = deathSound;
        audioSrc.Play(1);
        Destroy(this.gameObject, 1.5f);
    }

    void FixedUpdate()
    {
        taunt();
    }

    private void taunt()
    {
        AudioSource audioSrc = GetComponent<AudioSource>();
        if (!audioSrc.isPlaying)
        {
            audioSrc.clip = chooseRandomTaunt();
            audioSrc.PlayDelayed(TAUNT_INTERVAL);
        }
    }

    private AudioClip chooseRandomTaunt()
    {
        int i = lastTauntIndex;
        while (lastTauntIndex == i)
        {
            i = Random.Range(0, taunts.Length);
        }
        lastTauntIndex = i;
        return taunts[lastTauntIndex];


    }
}
