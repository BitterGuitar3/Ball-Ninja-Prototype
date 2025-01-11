using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.0f;
    private float powerUpStrength = 15.0f;
    public bool hasPowerup;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerUpIndicator;
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;
    public PowerUpType currentPowerUp = PowerUpType.None;

    bool smashing;
    float floorY;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerUpIndicator.transform.position = transform.position + new Vector3(0,-0.5f, 0); //A little ring around the player that will appear when the player has a powerup and disapear soon after

        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))//When player has rocket powerup, they have to press F to fire the rockets
        {
            LaunchRockets();
            StopCoroutine(powerupCountdown);
            hasPowerup = false;
            powerUpIndicator.SetActive(false);
            currentPowerUp = PowerUpType.None;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            //Get the name of the powerup
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUptype;
            Destroy(other.gameObject);
            //Indicate player has powerup by setting ring active
            powerUpIndicator.gameObject.SetActive(true);

            //When player currently has a powerup, stop the coroutine so a new one can start for the new powerup
            if(powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            powerupCountdown = StartCoroutine(PowerupCountdonwRoutine());
        }
    }

    //Timer for how long the player has the powerup
    IEnumerator PowerupCountdonwRoutine()
    {
        yield return new WaitForSeconds(5);
        hasPowerup= false;
        powerUpIndicator.SetActive(false);
        currentPowerUp= PowerUpType.None; 
    }

    //Push the enemy back away from the player by the strength of the pushback powerup
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Pushback)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + currentPowerUp.ToString());
        }
    }

    //Spawn a rocket for each enemy and have it fire at the enemy
    void LaunchRockets()
    {
        foreach(var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<RocketBehavior>().Fire(enemy.transform);
        }
    }
}
