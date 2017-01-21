using UnityEngine;
using System.Collections;
using System;

public class LaunchControl : MonoBehaviour{

    private const float MAX_DEG = 90f;
    private const float MAC_LAUNCH_FORCE = 30f;
    private const float FIRE_INTERVAL_IN_SECONDS = 0.2f;

    public float launchForce;
    public float chargeForce = 0;
    public float verticalPercent;
    private float FORCE_ACC;
    private bool charging;
    private bool aimingUp;
    private bool skipOne;
    private float numberToFire;
    private float timeSinceLastShot;
    public AudioClip launchClip;
    public AudioClip chargingClip;
    public Transform gun;
    public Transform chargeBar;
    public Rigidbody2D launchable;
    AudioSource audioSource;
    

    // Use this for initialization
    void Start () {
        chargeForce = 0;
        launchForce = 0;
        verticalPercent = 0F;
        FORCE_ACC = 50;
        charging = false;
        aimingUp = true;
        numberToFire = 0;
        timeSinceLastShot = 0;
        audioSource = GetComponent<AudioSource>();
    }

    

    // Update is called once per frame
    void Update ()
    {
        // If the jump button is pressed and the player is grounded then the player should jump.
        handleChargingGun();

        if (shouldStartShooting())
        {
            numberToFire = 5;
            launchForce = chargeForce;
            chargeForce = 0;
            audioSource.clip = launchClip;
        }
        if (currentlyShooting())
        {
            shootProjectilesAsNeeded();
        }
        rotateGunIfNeeded();


    }

    private void handleChargingGun()
    {
        
        if (startedChargingNextShot())
        {
            charging = true;
            audioSource.clip = chargingClip;
            audioSource.Play();
        }

        if (charging)
        {
            chargeForce += FORCE_ACC * Time.deltaTime;
        }
        chargeBar.localScale = new Vector3(chargeForce / 20, 0.1f, 0);
    }

    private void rotateGunIfNeeded()
    {
        if (!charging && notFiring())
        {
            rotateGun();
        }
    }

    private bool shouldStartShooting()
    {

        bool playerStopedPressing = Input.GetButtonUp("Jump");
        bool cannotChargeAnyMore = chargeForce > MAC_LAUNCH_FORCE;
        bool mustStopCharging = playerStopedPressing || cannotChargeAnyMore;
        if (!mustStopCharging)
        {
            return false;
        }
        charging = false;
        stopPlayinSound();
        if (skipOne)
        {
            skipOne = false;
            return false;
        }
        if (cannotChargeAnyMore)
        {
            skipOne = true;
        }
        return notFiring() && chargeForce > 0;
    }

    private void stopPlayinSound()
    {
       if (audioSource.isPlaying) {
          audioSource.Stop();
       }
    }

    private bool currentlyShooting()
    {
        return !notFiring();
    }

    private bool startedChargingNextShot()
    {
        return Input.GetButtonDown("Jump") && notFiring();
    }

    private bool notFiring()
    {
        return numberToFire == 0;
    }

    private void shootProjectilesAsNeeded()
    {
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot > FIRE_INTERVAL_IN_SECONDS) {
            numberToFire--;
            timeSinceLastShot = 0;
            shootNewProjectile();
        } if (numberToFire == 0) {
            launchForce = 0;
        }
    }

    private void shootNewProjectile()
    {

        audioSource.Play();
        
        float xForce = launchForce * (MAX_DEG - verticalPercent);
        float yForce = launchForce * verticalPercent;


        Rigidbody2D toShoot = (Rigidbody2D)Instantiate(launchable, gun.position, launchable.transform.rotation);
        toShoot.tag = "Bullet";
        toShoot.AddForce(new Vector2(xForce, yForce));
    }

    private void rotateGun()
    {
        float diff = Time.deltaTime * FORCE_ACC;
        if (aimingUp)
        {
            verticalPercent = verticalPercent + (diff);
            if (verticalPercent > MAX_DEG)
            {
                verticalPercent = MAX_DEG;
                aimingUp = false;
            }
            else
            {
                gun.Rotate(new Vector3(0, 0, diff));
            }

        }
        else
        {
            verticalPercent = verticalPercent - (diff);
            if (verticalPercent < 0)
            {
                verticalPercent = 0;
                aimingUp = true;
            }
            else
            {
                gun.Rotate(new Vector3(0, 0, -diff));
            }
        }
    }

}
