using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 40;
    [SerializeField] Animator animator;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayers;

    bool isAttacking = false; // Indica si el personaje está atacando
    float attackTime = 0.5f; // Duración de la animación de ataque
    float attackTimer = 0f; // Temporizador para controlar el tiempo de ataque

    void Update() {
        if (isAttacking) {
            attackTimer -= Time.deltaTime; // Reduce el temporizador
            if (attackTimer <= 0) {
                EndAttack(); // Finaliza el ataque cuando el temporizador llega a 0
            }
        }

        // Solo permite atacar si no está atacando actualmente
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking) {
            Attack();
        }
    }

    void Attack() {
        isAttacking = true;
        attackTimer = attackTime; // Inicia el temporizador
        animator.SetTrigger("Attack"); // Activa la animación de ataque

        // Detectar enemigos en el rango de ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void EndAttack() {
        isAttacking = false;
        animator.ResetTrigger("Attack"); // Reinicia el trigger de ataque
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null) {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}