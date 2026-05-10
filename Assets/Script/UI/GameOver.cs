using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    public GameObject gameOverPanel; // 游戏结束界面
    public Image resultImage;                 // 用于显示胜利/失败图片的 Image
    public Sprite winSprite;
    public Sprite loseSprite;
    // 唯一静态实例
    public static GameOver Instance { get; private set; }

    private void Awake()
    {
        // 保证全局唯一
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void ShowGameOver(bool IsWin)
    {
        // 根据胜负切换图片
        resultImage.sprite = IsWin ? winSprite : loseSprite;
        if(IsWin) DataStore.Instance.AddDate();
        Debug.Log("分数已保存到 DataStore。当前分数：" + DataStore.Instance.Score);
        // 显示游戏结束界面
        //gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<PanelAnim>().Show();
        //Time.timeScale = 0;
        Invoke("Sleep", 1f); // 延迟1秒后调用Sleep方法
    }
    private void Sleep()
    {
        Time.timeScale = 0;
    }
}
