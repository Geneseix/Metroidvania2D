using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public SpriteRenderer healthBarSprite;
    public Sprite[] healthBarSprites; 
    private SpriteRenderer spriteRenderer;
    private Color originalColor; 
    public Color hitColor = new Color(1f, 1f, 0f, 1f); 
    public float hitFlashDuration = 0.2f; 
    private int enemiesDefeated = 0; 
    public int totalEnemiesToDefeat = 2;
    [Header("Combat Settings")]
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 40;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;

    private Animator animator;
    private bool isAttacking = false; 
    private float attackTime = 0.5f; 
    private float attackTimer = 0f; 

    private AudioSource audioSource; 
    public AudioClip deathSound; 

    void Start()
    {
        currentHealth = maxHealth; 

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                EndAttack();
            }
        }

        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            Attack();
        }
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage; 

        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitColor;
            Invoke("ResetColor", hitFlashDuration); 
        }
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die(); 
        }

        Debug.Log($"Jugador recibió {damage} de daño. Salud actual: {currentHealth}");
    }

    void UpdateHealthBar()
    {
        if (healthBarSprite != null && healthBarSprites.Length > 0)
        {
            float healthPercentage = (float)currentHealth / maxHealth;

            if (healthPercentage >= 1f)
            {
                healthBarSprite.sprite = healthBarSprites[0];
            }
            else if (healthPercentage >= 0.8f)
            {
                healthBarSprite.sprite = healthBarSprites[1]; 
            }
            else if (healthPercentage >= 0.6f)
            {
                healthBarSprite.sprite = healthBarSprites[2]; 
            }
            else if (healthPercentage >= 0.4f)
            {
                healthBarSprite.sprite = healthBarSprites[3]; 
            }
            else if (healthPercentage >= 0.2f)
            {
                healthBarSprite.sprite = healthBarSprites[4]; 
            }
            else
            {
                healthBarSprite.sprite = healthBarSprites[5]; 
            }
        }
    }

    void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
    void Die()
    {
        Debug.Log("Jugador derrotado!");
        if (audioSource != null && deathSound != null)
        {
            Debug.Log("Reproduciendo sonido de muerte");
            audioSource.PlayOneShot(deathSound);
            StartCoroutine(LoadMenuAfterSound(deathSound.length));
        }
        else
        {
            Debug.LogWarning("AudioSource o deathSound no están asignados");
            LoadMenu(); 
        }
    }

    IEnumerator LoadMenuAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadMenu();
    }

    void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator DestroyAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay); 
        Destroy(gameObject); 
    }

    void Attack()
    {
        isAttacking = true;
        attackTimer = attackTime;
        animator.SetTrigger("Attack");

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    public void RegisterEnemyDefeated()
    {
        enemiesDefeated++; // Incrementar el contador de enemigos derrotados

        Debug.Log($"Enemigos derrotados: {enemiesDefeated}");

        // Verificar si todos los enemigos han sido derrotados
        if (enemiesDefeated >= totalEnemiesToDefeat)
        {
            Debug.Log("Todos los enemigos han sido derrotados. Volviendo al menú...");
            LoadMenu(); // Cargar la escena del menú
        }
    }

    void EndAttack()
    {
        isAttacking = false;
        animator.ResetTrigger("Attack");
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}