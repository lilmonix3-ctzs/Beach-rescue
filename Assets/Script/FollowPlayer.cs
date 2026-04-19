using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    
        public Transform player;
        public float smoothSpeed = 5f;
        public Vector3 offset = new Vector3(0, 0, -10);

        void LateUpdate()
        {
            if (player != null)
            {
                Vector3 targetPosition = player.position + offset;

                transform.position = Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    smoothSpeed * Time.deltaTime
                );
            }
        }
    
}
