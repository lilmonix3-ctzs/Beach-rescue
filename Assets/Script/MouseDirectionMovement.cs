using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDirectionMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 direction;
    public float maxSpeed = 10f;

 
    public Rigidbody2D childRb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //childRb = transform.GetComponentInChildren<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        direction = (mousePos - transform.position).normalized;
        //ParentPositionUpdate();
    }

    void FixedUpdate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.velocity = direction * moveSpeed;

        Vector2 rawDirection = (mousePos - transform.position);

        // 根据距离产生速度
        Vector2 velocity = rawDirection * moveSpeed;

        // ⭐ 限制最大速度
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        rb.velocity = velocity;
        
    }

    void ParentPositionUpdate()
    {
        childRb.velocity = rb.velocity;
    }
}



