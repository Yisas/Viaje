using UnityEngine;
using System.Collections;

public class EnemyFlying : EnemyController {

    public float verticalMoveSpeed;

    override protected void Move()
    {
        if (!isDead)
            // Set the enemy's velocity to moveSpeed in the x and y direction.
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * directionSign, -verticalMoveSpeed);
    }
}
