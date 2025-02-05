using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;
    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;
    
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isRunning", true);
    }

    void Update(){
        Vector2 point = currentPoint.position - transform.position;
        if(currentPoint == pointB.transform){
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else{
            rb.linearVelocity = new Vector2(-speed, 0);
        }
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        if(currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log("Enemy died!");
    }
}
