using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("玩家赢了！");
        if (collision.gameObject.CompareTag("Player"))
        {
            // 显示胜利界面
            
            GameOver.Instance.ShowGameOver();
        }
    }
}
