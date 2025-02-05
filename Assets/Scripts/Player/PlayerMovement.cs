using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    [SerializeField] float moveSpeed = 5f;
    bool isFacingRight = false;
    [SerializeField] float jumpPower = 5f;
    bool isGrounded = false;

    Rigidbody2D rb;
    Animator animator;

    [SerializeField] float fallMultiplier = 2.5f; // Multiplicador de la caída
    [SerializeField] float lowJumpMultiplier = 2f; // Multiplicador para saltos cortos

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();

        // Lógica para saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }

        // Llamamos a la función para modificar la gravedad cuando caemos
        AdjustFallSpeed();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocityY);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", rb.linearVelocityY);
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    // Ajustar la velocidad de caída
    void AdjustFallSpeed()
    {
        if (rb.linearVelocityY < 0) // Si el jugador está cayendo
        {
            rb.gravityScale = fallMultiplier; // Aumentamos la gravedad
        }
        else if (rb.linearVelocityY > 0 && !Input.GetButton("Jump")) // Si está subiendo pero no está presionando el salto
        {
            rb.gravityScale = lowJumpMultiplier; // Reducimos la gravedad para un salto corto
        }
        else
        {
            rb.gravityScale = 1f; // Valor normal de gravedad si está en el aire sin estar cayendo
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")) // Detecta si tocamos el suelo
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")) // Cuando dejamos de tocar el suelo
        {
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }
    }
}