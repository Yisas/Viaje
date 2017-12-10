using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class EnemyController : MonoBehaviour
{

    public float moveSpeed = 2f;
    // The speed the enemy moves at.
    public float deathDespawnInterval;
    // Time the character is on the scene after dying.
    public float headbobSpeedMultiplier;
    // To be passed to animator.

    public AudioClip[] idleClips;
    public float idleSoundCountdown;                        // Time until next clip
    public float idleSoundRandom;                           // +- range for a random clip after countdown

    private float idleSoundTimer = 0f;
    private float deathTimer = 0f;
    // When timer reaches despawn time, destroy object.
    protected bool isDead = false;
    // Check for whether enemy is dead, despawn after interval.
    protected Animator anim;
    // Reference to the enemy's animator component.
    protected Transform frontCheck;
    // Reference to the position of the gameobject used for checking if something is in front.
    private GameController gameController;
    private PlayerController playerController;
    private AudioSource audioSource;
    private bool isFacingLeft = true;
    private bool isGrounded = false;                        // Check is now using OnCollisionExit with Ground layer mask
    private bool hasEnemyBelow = false;                     // See EnemyBelow() function
    private float dismountTime = 3f;                        // Wait time when stacked on top of another enemy to unstack
    private float dismountTimer = 0;                        // Countdown initially set to 0
    protected bool lockMovement = false;
    protected int directionSign = 0;

    [HideInInspector]
    public EnemySpawner enemySpawner;

    protected void Awake()
    {
        // Setting up references.
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("headbobSpeedMultiplier", headbobSpeedMultiplier);
        frontCheck = transform.Find("frontCheck").transform;
        gameController = GameController.GetInstance();
        gameController.numberOfEnemies++;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

        // Setup variables
        idleSoundTimer = 0;
    }

    // Update is called once per frame
    protected void Update()
    {
        // Destroy object when it should despawn.
        if (isDead)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer >= deathDespawnInterval)
                Destroy(this.gameObject);
        }
        else
        {
            idleSoundTimer -= Time.deltaTime;

            if (idleSoundTimer <= 0)
            {
                PlayRandomSound(idleClips);

                idleSoundTimer = idleSoundCountdown + Random.Range(0, idleSoundRandom);
            }
        }

        // Checks should happen in update and movements in Fixed
        hasEnemyBelow = EnemyBelow();

        // If just stacked on top of an enemy, start timer.
        if (hasEnemyBelow && dismountTimer == 0)
            dismountTimer = dismountTime;

        // Reset timer if necessary
        if (dismountTimer < 0)
            dismountTimer = 0;

    }

    void FixedUpdate()
    {

        if (!lockMovement)
        {

            directionSign = FindPlayer();

            // If stacked on an enemy cancel horizontal velocity
            if (dismountTimer > 0)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
                dismountTimer -= Time.deltaTime;
            }
            else
            // If not stacked on an enemy, move towards player.
            if (!hasEnemyBelow)
                // Move towards player
                Move();

            // Flip if necessary to look towards the player
            Reorient(directionSign);

            // The Speed animator parameter is set.
            anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "DeadZone")
            Die(3);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            isGrounded = true;
    }


    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.layer == 8
            && isGrounded)
        {
            isGrounded = false;
        }

    }

    void OnParticleCollision(GameObject col)
    {
        ParticleSystem.CollisionModule ps;

        ps = col.GetComponent<ParticleSystem>().collision;
        Die(1);
    }

    public void Flip()
    {
        // No flipping when dead
        if (!isDead)
        {
            // Multiply the x component of localScale by -1.
            Vector3 enemyScale = transform.localScale;
            enemyScale.x *= -1;
            transform.localScale = enemyScale;
            isFacingLeft = !isFacingLeft;
        }
    }

    // killType is 0 when melee, 1 when ranged, 3 when suicide
    public void Die(int killType)
    {
        if (!isDead)
        {

            // Find all of the colliders on the gameobject and set them all to be triggers.
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (Collider2D c in cols)
            {
                //c.isTrigger = true;
                c.enabled = false;
            }


            // Move all sprite parts of the player to the front
            SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer s in spr)
            {
                s.sortingLayerName = "UI";
            }

            // ... Trigger the 'Die' animation state
            anim.SetTrigger("Dead");

            // Start despawning timer in update.
            isDead = true;

            gameController.numberOfEnemies--;
            if (enemySpawner != null)
                enemySpawner.numberOfEnemies--;

            if (killType != 3)
                playerController.EnemyKill(killType);
        }
    }

    // Uses horizontal position diference to return the sign of the offset to the player.
    protected int FindPlayer()
    {
        int direction = 0;

        Vector3 tempFloat = transform.position - playerController.transform.position;

        if (tempFloat.x <= 0)
            direction = 1;
        else
            direction = -1;

        return direction;
    }

    // Flip if not facing the playe. Direction must be the sign of direction towards player.
    private void Reorient(int direction)
    {
        if (direction == -1 && !isFacingLeft)
            Flip();
        else if (direction == 1 && isFacingLeft)
            Flip();
    }

    protected virtual void Move()
    {
        if (!isDead)
            // Set the enemy's velocity to moveSpeed in the x direction.
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * directionSign, GetComponent<Rigidbody2D>().velocity.y);
    }

    // Raycast to see if stacked on top of an enemy.
    private bool EnemyBelow()
    {

        GameObject bottomCheckPosition = transform.Find("bottomCheck").gameObject;
        RaycastHit2D raycastHit;

        raycastHit = Physics2D.Raycast(bottomCheckPosition.transform.position, Vector2.down);
        if (raycastHit)
            if (raycastHit.collider.tag == "Enemy")
                return true;

        return false;
    }

    private void PlayRandomSound(AudioClip[] soundArray)
    {
        if (soundArray.Length == 0)
            return;

        int i = Random.Range(0, soundArray.Length);
        audioSource.clip = soundArray[i];
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
