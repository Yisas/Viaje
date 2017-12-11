using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_KillAllEnemies : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            EnemySpawner[] spawners = GameObject.FindObjectsOfType<EnemySpawner>();
            foreach (EnemySpawner spawner in spawners)
            {
                Destroy(spawner.gameObject);
            }

            EnemyController[] enemies = GameObject.FindObjectsOfType<EnemyController>();
            foreach(EnemyController enemy in enemies)
            {
                enemy.Die(3);
            }
        }
    }
}
