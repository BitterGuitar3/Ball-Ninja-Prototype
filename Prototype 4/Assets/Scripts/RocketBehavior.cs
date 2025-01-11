using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    private Transform target;
    private float speed = 15.0f;
    private bool homing;

    private float rocketStrength = 150.0f;
    private float aliveTimer = 3.0f;

    // Update is called once per frame
    void Update()
    {
        //Move rocket towards enemy. Acts a homing missile
        if(homing && target != null)
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.LookAt(target);
        }
        
    }

    public void Fire(Transform newTarget)
    {
        target = newTarget;
        homing = true;
        //Rocekts have a limited time to reach their targets
        Destroy(gameObject, aliveTimer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(target != null)
        {
            //Send the enemy flying when the rocket hits them
            if (collision.gameObject.CompareTag(target.tag))
            {
                Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 away = -collision.contacts[0].normal;
                targetRigidbody.AddForce(away * rocketStrength, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }
}
