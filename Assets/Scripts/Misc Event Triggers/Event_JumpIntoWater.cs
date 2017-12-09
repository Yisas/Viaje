using UnityEngine;
using System.Collections;

public class Event_JumpIntoWater : MonoBehaviour {

    // Misc script for jumping into water event

    public GameObject[] spawners;               // Zombie spawners from pzo to be turned off

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "Player")
        {
            col.GetComponent<PlayerController>().hasControl = false;
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0, rb.velocity.y);

            foreach(GameObject go in spawners)
            {
                go.SetActive(false);
            }

            // Kill zombies. At this point only EnemyControllers will be zombies
            EnemyController[] zombies = GameObject.FindObjectsOfType<EnemyController>();
            foreach(EnemyController zombie in zombies)
            {
                zombie.Die(3);
            }
        }
    }

	}
