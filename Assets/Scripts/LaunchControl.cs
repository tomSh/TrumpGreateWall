using UnityEngine;
using System.Collections;
using System;

public class LaunchControl : MonoBehaviour, FloatProvider{

    private const float MAX_DEG = 90f;
    private const float MAC_LAUNCH_FORCE = 30f;

    public float launchForce;
    public float verticalPercent;
    private float FORCE_ACC;
    private bool charging;
    private bool aimingUp;
    private bool skipOne;
    public AudioClip launchClip;
    public Transform gun;
    public Transform chargeBar;
    public Rigidbody2D launchable;

    // Use this for initialization
    void Start () {
        launchForce = 0;
        verticalPercent = 0F;
        FORCE_ACC = 50;
        charging = false;
        aimingUp = true;

}

    

    // Update is called once per frame
    void Update () {

        // If the jump button is pressed and the player is grounded then the player should jump.

        chargeBar.localScale = new Vector3(launchForce / 20, 0.1f, 0);

        if (Input.GetButtonDown("Jump"))
        {
            charging = true;
            
            launchForce += FORCE_ACC * Time.deltaTime;
        }
        if (Input.GetButtonUp("Jump"))
        {
            if (!skipOne)
            {
                shootNewProjectile();
            }
            else
            {
                skipOne = false; //we skipped one launce
            }
        }
        if (launchForce > MAC_LAUNCH_FORCE)
        {
            skipOne = true;
            shootNewProjectile();

        }
        if (charging)
        {
            launchForce += FORCE_ACC * Time.deltaTime;

        } else
        {
            rotateGun();
        }

    }

    private void shootNewProjectile()
    {
        AudioSource.PlayClipAtPoint(launchClip, transform.position);
        charging = false;
        float xForce = launchForce * (MAX_DEG - verticalPercent);
        float yForce = launchForce * verticalPercent;


        Rigidbody2D toShoot = (Rigidbody2D)Instantiate(launchable, gun.position, launchable.transform.rotation);
        toShoot.tag = "Bullet";
        toShoot.AddForce(new Vector2(xForce, yForce));
        launchForce = 0;
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

    public void FixedUpdate()
    {
        if (charging)
        {
            
        }
    }

    public float getValue()
    {
        return launchForce / MAC_LAUNCH_FORCE;
    }
}
