using UnityEngine;
using System.Collections;

public class Trump : MonoBehaviour {

    public GameObject explosion;        // Prefab of explosion effect.
    

    void OnCollisionEnter2D(Collision2D objectYouCollidedWith)
    {
        Collider2D collider = objectYouCollidedWith.collider;
        if (collider.tag == "Bullet") {
            OnExplode();
        }
    }

    private void blowUp(Collider2D toBlow)
    {
        Destroy(toBlow);
        
    }

    void OnExplode()
    {
        // Create a quaternion with a random rotation in the z-axis.
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        // Instantiate the explosion where the rocket is with the random rotation.
        Instantiate(explosion, transform.position, randomRotation);
        Destroy(this.gameObject);
    }
}
