using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);

    // 新增范围限制（相机移动边界）
    public bool enableBoundary = true;
    public Vector2 minBoundary;
    public Vector2 maxBoundary;

    void LateUpdate()
        {
            if (player != null)
            {
                Vector3 targetPosition = player.position + offset;

                // 应用边界限制（如果启用）
                if (enableBoundary)
                {
                    targetPosition.x = Mathf.Clamp(targetPosition.x, minBoundary.x, maxBoundary.x);
                    targetPosition.y = Mathf.Clamp(targetPosition.y, minBoundary.y, maxBoundary.y);
                }

            transform.position = Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    smoothSpeed * Time.deltaTime
                );
            }
        }
    
}
