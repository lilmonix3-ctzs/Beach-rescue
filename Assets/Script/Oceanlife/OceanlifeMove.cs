using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanlifeMove : MonoBehaviour
{

    private Transform poolTransform; // 水池的Transform
    private float currentRadius;     // 当前水池半径

    void Start()
    {
        UpdatePoolReference();
    }

    void Update()
    {
        if (transform.parent != null && transform.parent.CompareTag("Player"))
        {

            UpdatePoolReference();

            // 获取水池当前半径
            float newRadius = GetPoolRadius()/2.5f;
            if (newRadius > 0)
            {
                // 计算当前位置相对于水池中心的距离
                Vector2 direction = transform.position - poolTransform.position;
                float distance = direction.magnitude;

                // 如果距离大于当前半径，向中心移动
                if (distance > newRadius*0.95)
                {
                    float targetDistance = newRadius * 0.9f;
                    Vector3 newPos = poolTransform.position + (Vector3)direction.normalized * targetDistance;
                    transform.position = newPos;
                }
            }
        }
    }

    void UpdatePoolReference()
    {
        if (poolTransform == null || poolTransform != transform.parent)
        {
            poolTransform = transform.parent;
        }
    }

    float GetPoolRadius()
    {
        if (poolTransform == null) return 0;
        // 获取水池的 Reduce 组件
        Reduce reduce = poolTransform.GetComponentInChildren<Reduce>();
        if (reduce != null)
        {
            // 通过面积计算半径
            float area = reduce.GetCurrentArea();
            return Mathf.Sqrt(area / Mathf.PI);
        }

        // 备用方案：通过 scale 计算
        return poolTransform.localScale.x / 2f;
    }
}
