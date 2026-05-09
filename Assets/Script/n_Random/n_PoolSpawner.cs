using UnityEngine;
using System.Collections.Generic;

public class n_PoolSpawner : MonoBehaviour
{
    [Header("水池预制体数组（可拖入多个不同预制体）")]
    public GameObject[] poolPrefabs;  // 改成数组

    [Header("生成数量")]
    public int spawnCount = 5;

    [Header("生成范围（矩形）")]
    public Vector2 spawnArea = new Vector2(20, 10);

    [Header("格子大小")]
    public float cellSize = 2f;

    [Header("水池标签")]
    public string poolTag = "WaterPool";

    void Start()
    {
        //SpawnPools();
    }

    void SpawnPools()
    {
        if (poolPrefabs == null || poolPrefabs.Length == 0)
        {
            Debug.LogError("请拖入水塘预制体数组！");
            return;
        }

        // 1. 随机生成所有水池（从数组中随机选择预制体）
        List<GameObject> spawnedPools = new List<GameObject>();

        for (int i = 0; i < spawnCount; i++)
        {
            // 随机选择一个预制体
            int randomIndex = Random.Range(0, poolPrefabs.Length);
            GameObject selectedPrefab = poolPrefabs[randomIndex];

            GameObject pool = Instantiate(selectedPrefab);
            pool.transform.SetParent(transform);
            pool.transform.position = Vector3.zero; // 临时位置
            spawnedPools.Add(pool);
        }

        // 2. 给所有生成的水池设置标签
        foreach (GameObject pool in spawnedPools)
        {
            pool.tag = poolTag;
        }

        // 3. 使用 ItemRandomPlacer 工具在矩形区域内随机排列
        Vector2 rectMin = (Vector2)transform.position - spawnArea;
        Vector2 rectMax = (Vector2)transform.position + spawnArea;

        ItemRandomPlacer.RandomPlaceInRect(
            transform,           // 父物体
            poolTag,            // 标签
            rectMin,            // 矩形左下角
            rectMax,            // 矩形右上角
            cellSize            // 格子大小
        );

        Debug.Log($"成功生成并排列了 {spawnCount} 个水池（从 {poolPrefabs.Length} 种预制体中随机选择）！");
    }
    public void ClearAllPools()
    {
        // 删除所有标记为 "WaterPool" 的子物体
        foreach (Transform child in transform)
        {
            if (child.CompareTag(poolTag))
                Destroy(child.gameObject);
        }
    }

    public void RegeneratePools()
    {
        ClearAllPools();
        SpawnPools(); // 原有生成方法
    }
}