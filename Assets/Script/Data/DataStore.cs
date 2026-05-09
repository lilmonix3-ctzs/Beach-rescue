using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class DataStore : MonoBehaviour
{
    public static DataStore Instance { get; private set; }
    public int Score =0;
    private List<int> arr = new List<int>();

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
    private void SaveData(List<int> arr)
    {
        PlayerPrefs.SetInt("Score" + 0, Score);
        for (int i = 1; i < arr.Count+1; i++)
        {
            PlayerPrefs.SetInt("Score" + i, arr[i-1] );
        }
        PlayerPrefs.Save();
        arr.Clear();
    }

    public void AddDate()
    {
        //arr.Add(Score);
        SaveData(arr);
    }

    public List<int> LoadData()
    {
        for (int i = 0; i < 8; i++)
        {
            if (PlayerPrefs.HasKey("Score" + i))
            {
                arr.Add(PlayerPrefs.GetInt("Score" + i));
            }
            else
            {
                arr.Add(0); // 如果没有数据，默认添加0
            }
        }
        arr.Sort();          // 先升序
        arr.Reverse();       // 反转 = 降序
        return arr;
    }
    public List<int> GetDate()
    {
        return LoadData();
    }
    public void SetScore(int a)
    {
        Score = a;
    }
}
