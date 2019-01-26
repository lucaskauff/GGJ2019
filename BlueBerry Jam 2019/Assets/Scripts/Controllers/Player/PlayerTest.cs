using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public int ropeAmount = 0;

    [SerializeField]
    private float speed;

    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        //Vector2 direction = inputs.normalized;

        //Vector2 velocity = direction * speed;

        //transform.position = (Vector2)transform.position + (velocity * Time.deltaTime);
    }
}
