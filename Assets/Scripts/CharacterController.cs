using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float velocidadMov;
    public float multiVel;

    private Vector2 direccion;

    private float movX;
    private float movY;

    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        animator.SetBool("Idle", true);
        animator.SetBool("Walk", true);
    }

    private void Update()
    {
        movX = Input.GetAxisRaw("Horizontal");
        movY = Input.GetAxisRaw("Vertical");

        animator.SetFloat("X", movX);
        animator.SetFloat("Y", movY);

        direccion = new Vector2(movX, movY).normalized;

        if (movX != 0 || movY != 0)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Idle", false);
            animator.SetFloat("UltimoX", movX);
            animator.SetFloat("UltimoY", movY);
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
        }
    }

    private void FixedUpdate()
    {
        float velocidadFinal = velocidadMov * multiVel;
        rb.MovePosition(rb.position + direccion * velocidadFinal * Time.deltaTime);
    }
}