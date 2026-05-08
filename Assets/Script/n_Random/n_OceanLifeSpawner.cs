using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class n_OceanLifeSpawner : MonoBehaviour
{
    [Header("多种海洋生物预制体")]
    public List<GameObject> oceanLifePrefabs;

    [Header("格子大小（用于分布）")]
    public float cellSize = 0.8f;

    [Header("生成生物占水池水量比")]
    public float waterOccupancyCoefficient = 0.5f;

    [Header("生物标签")]
    public string oceanLifeTag = "Oceanlife";

    private Reduce poolReduce;
    private bool hasSpawned = false;

    void Start()
    {
        poolReduce = GetComponentInChildren<Reduce>();
    }

    void Update()
    {
        if (!hasSpawned && poolReduce != null)
        {
            SpawnOceanLivesByWaterLimit();
            hasSpawned = true;
        }
    }

    // 按水量一半限制生成生物
    public void SpawnOceanLivesByWaterLimit()
    {
        if (oceanLifePrefabs == null || oceanLifePrefabs.Count == 0)
        {
            Debug.LogError("请给列表拖入生物预制体");
            return;
        }

        // 清空之前生成的生物
        ClearSpawnedLives();

        // 水池当前总面积
        float poolTotalArea = GetPoolCurrentArea();
        // 生物总占用面积不能超过所定系数
        float maxAllowLifeArea = poolTotalArea * waterOccupancyCoefficient;

        float nowTotalLifeArea = 0f;
        List<GameObject> spawnedLives = new List<GameObject>();

        // 循环生成，直到超过水量上限
        int maxAttempts = 100; // 防止无限循环
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            attempts++;

            // 随机选一种生物预制体
            GameObject randomPrefab = oceanLifePrefabs[Random.Range(0, oceanLifePrefabs.Count)];
            OceanLife lifeCfg = randomPrefab.GetComponent<OceanLife>();
            if (lifeCfg == null) continue;

            // 再加这个生物就超一半水量 → 停止生成
            if (nowTotalLifeArea + lifeCfg.GetLifeArea() > maxAllowLifeArea)
            {
                break;
            }

            // 生成生物（先放在原点）
            GameObject life = Instantiate(randomPrefab, transform);
            life.transform.position = Vector3.zero;
            life.tag = oceanLifeTag;
            spawnedLives.Add(life);
            nowTotalLifeArea += lifeCfg.GetLifeArea();
        }

        // 使用 ItemRandomPlacer 在圆形区域内随机排列生物
        if (spawnedLives.Count > 0)
        {
            // 获取水池半径
            float poolRadius = Mathf.Sqrt(poolTotalArea / Mathf.PI)/2.5f;

            // 使用工具在圆形区域内排列
            ItemRandomPlacer.RandomPlaceInCircle(
                transform,           // 父物体
                oceanLifeTag,       // 标签
                transform.position, // 圆心
                poolRadius,         // 半径
                cellSize            // 格子大小
            );
        }

        Debug.Log($"生成完毕，共生成 {spawnedLives.Count} 个生物，生物总占用面积：{nowTotalLifeArea}，允许最大一半面积：{maxAllowLifeArea}");
    }

    // 获取水池当前实际面积
    float GetPoolCurrentArea()
    {
        return poolReduce.GetCurrentArea();
    }

    // 清空所有生成生物
    public void ClearSpawnedLives()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<OceanLife>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}