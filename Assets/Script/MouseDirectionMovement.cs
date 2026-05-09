using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDirectionMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 direction;
    public float maxSpeed = 10f;

    // 新增移动边界限制
    public bool enableBoundary = true;
    public Vector2 minBoundary;
    public Vector2 maxBoundary;

    public Rigidbody2D childRb;
    void Start()
    {
        if (rb == null)
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
        Vector2 rawDirection = (mousePos - transform.position);
        Vector2 desiredVelocity = rawDirection * moveSpeed;
        desiredVelocity = Vector2.ClampMagnitude(desiredVelocity, maxSpeed);

        // 边界处理：限制速度，防止越界震动
        if (enableBoundary)
        {
            Vector2 newPos = rb.position + desiredVelocity * Time.fixedDeltaTime;

            // 左边界
            if (newPos.x < minBoundary.x && desiredVelocity.x < 0)
                desiredVelocity.x = 0;
            // 右边界
            if (newPos.x > maxBoundary.x && desiredVelocity.x > 0)
                desiredVelocity.x = 0;
            // 下边界
            if (newPos.y < minBoundary.y && desiredVelocity.y < 0)
                desiredVelocity.y = 0;
            // 上边界
            if (newPos.y > maxBoundary.y && desiredVelocity.y > 0)
                desiredVelocity.y = 0;
        }

        rb.velocity = desiredVelocity;

    }

    void ParentPositionUpdate()
    {
        childRb.velocity = rb.velocity;
    }
}



