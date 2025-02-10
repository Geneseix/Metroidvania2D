using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 5.0f; 
    public int maxHealth = 100;
    public Player player;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f; 
    public float shootInterval = 2f; 

    private Rigidbody2D rb;
    private Transform currentPoint;
    private int currentHealth;
    private float shootTimer; 

    private SpriteRenderer spriteRenderer; 
    private Color originalColor; 
    public Color hitColor = Color.red; 
    public float hitFlashDuration = 0.1f; 
    public int numberOfFlashes = 3;
    public float freezeDuration = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB;
        currentHealth = maxHealth;
        shootTimer = shootInterval; 

        
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
       
        if (!IsFreezeActive())
        {
            MoveEnemy();
            ShootProjectile();
        }
    }

    void MoveEnemy()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
            Flip();
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
            Flip();
        }
    }

    void ShootProjectile()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            Vector2 shootDirection = (currentPoint == pointA.transform) ? Vector2.left : Vector2.right;

            projectile.GetComponent<Rigidbody2D>().linearVelocity = shootDirection * projectileSpeed;

            shootTimer = shootInterval;
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; 
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashEffect());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    IEnumerator FlashEffect()
    {
        // Congela al enemigo
        FreezeEnemy(true);


        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = hitColor; 
            yield return new WaitForSeconds(hitFlashDuration); 

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(hitFlashDuration); 
        }

  
        yield return new WaitForSeconds(freezeDuration);
        FreezeEnemy(false);
    }


    void FreezeEnemy(bool freeze)
    {
        if (freeze)
        {
            rb.linearVelocity = Vector2.zero; 
        }
        else
        {
     
            if (currentPoint == pointB.transform)
            {
                rb.linearVelocity = new Vector2(speed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(-speed, 0);
            }
        }
    }

    bool IsFreezeActive()
    {
        return rb.linearVelocity == Vector2.zero;
    }

   
    void Die()
    {
        
        if (player != null)
        {
            player.RegisterEnemyDefeated();
        }

        Destroy(gameObject); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}