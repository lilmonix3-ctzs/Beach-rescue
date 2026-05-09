using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public Text txtScore; // Inspector拖入UI文本
    private List<int> scoreList = new List<int>();
    private void Start()
    {
        UpdateScore();
    }
    void UpdateScore()
    {
        if (txtScore == null)
        {
            Debug.LogError("ScoreBoard: txtScore 未在 Inspector 中赋值。请把 UI Text 拖到此字段。");
            return;
        }

        DataStore ds = DataStore.Instance;
        if (ds == null)
        {
            ds = FindObjectOfType<DataStore>();
            if (ds != null)
            {
                Debug.LogWarning("ScoreBoard: DataStore.Instance 为 null，已通过 FindObjectOfType 找到实例。请在场景中确保只有一个 DataStore 并且其 Awake 在 ScoreBoard 之前执行。");
            }
            else
            {
                Debug.LogError("ScoreBoard: 找不到 DataStore 实例。请在场景中添加一个挂有 DataStore 脚本的 GameObject。");
                txtScore.text = "No data";
                return;
            }
        }

        scoreList = ds.GetDate() ?? new List<int>();
        string displayText = "";

        for (int i = 0; i < scoreList.Count; i++)
        {
            displayText += (i + 1).ToString() + "      " + scoreList[i].ToString() + "\n";
        }
        txtScore.text = displayText;
    }
}
