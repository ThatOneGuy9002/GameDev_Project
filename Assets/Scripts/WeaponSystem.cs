using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawn;

    public float fireRate = 3f;
    public float projectileSpeed = 3f;
    private float timeStamp = 0f;

    public AudioSource shootAudioSource;

    private void FixedUpdate()
    {
        if ((Time.time >= timeStamp) && (Input.GetKey(KeyCode.Mouse0))) 
        {
            Fire();
            shootAudioSource.Play();
            timeStamp = Time.time + (1/fireRate);
        }
    }

    void Fire()
    {
        var projectile = (GameObject)Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);

        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;

        Destroy(projectile, 2.0f);

    }

}
