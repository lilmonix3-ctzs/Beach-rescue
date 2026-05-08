using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanLifeSpawner : MonoBehaviour
{
    [Header("多种海洋生物预制体")]
    public List<GameObject> oceanLifePrefabs;

    [Header("单次最多尝试生成个数")]
    public int maxTrySpawnCount = 15;

    [Header("生物之间最小间距")]
    public float minDistanceBetweenLives = 0.5f;

    [Header("生成生物占水与水池水量比")]
    public float waterOccupancyCoefficient = 0.5f;

    private Reduce poolReduce;

    //记录生成位置
    private List<Vector2> spawnedPositions = new List<Vector2>();

    private bool hasSpawned = false; // 标记是否已经生成过生物

    void Start()
    {
        poolReduce = GetComponentInChildren<Reduce>();
    }

    void Update()
    {
        if (!hasSpawned && poolReduce != null)
        {
            SpawnOceanLivesByWaterLimit();
            hasSpawned = true; // 标记为已生成，防止重复执行
        }
    }


    //按水量一半限制生成生物
    public void SpawnOceanLivesByWaterLimit()
    {
        if (oceanLifePrefabs == null || oceanLifePrefabs.Count == 0)
        {
            Debug.LogError("请给列表拖入生物预制体");
            return;
        }

        spawnedPositions.Clear();

        // 水池当前总面积
        float poolTotalArea = GetPoolCurrentArea();
        // 生物总占用面积不能超过所定系数
        float maxAllowLifeArea = poolTotalArea * waterOccupancyCoefficient;

        float nowTotalLifeArea = 0f;

        // 循环尝试生成，直到超上限或生成够数量
        for (int i = 0; i < maxTrySpawnCount; i++)
        {
            // 随机选一种生物预制体
            GameObject randomPrefab = oceanLifePrefabs[Random.Range(0, oceanLifePrefabs.Count)];
            OceanLife lifeCfg = randomPrefab.GetComponent<OceanLife>();
            if (lifeCfg == null) continue;

            // 再加这个生物就超一半水量 → 停止生成
            if (nowTotalLifeArea + lifeCfg.GetLifeArea() > maxAllowLifeArea)
            {
                break;
            }

            // 找不重叠的位置
            if (TryGetValidSpawnPoint(out Vector2 spawnPos))
            {
                SpawnSingleLife(spawnPos, randomPrefab);
                spawnedPositions.Add(spawnPos);
                nowTotalLifeArea += lifeCfg.GetLifeArea();
            }
        }

        Debug.Log("生成完毕，生物总占用面积：" + nowTotalLifeArea
            + " 允许最大一半面积：" + maxAllowLifeArea);
    }

    // 获取水池当前实际面积
    float GetPoolCurrentArea()
    {
        // 圆形面积公式 S = π * r²
        return poolReduce.GetCurrentArea();
    }

    // 找不重叠随机点
    bool TryGetValidSpawnPoint(out Vector2 point)
    {
        point = Vector2.zero;
        float poolRadius = transform.lossyScale.x / 2f;

        int maxAttempts = 100;

        for (int a = 0; a < maxAttempts; a++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float r = Random.Range(0f, poolRadius);

            float x = Mathf.Cos(angle) * r;
            float y = Mathf.Sin(angle) * r;

            Vector2 randomPos = (Vector2)transform.position + new Vector2(x, y);

            bool overlap = false;
            foreach (var pos in spawnedPositions)
            {
                if (Vector2.Distance(randomPos, pos) < minDistanceBetweenLives)
                {
                    overlap = true;
                    break;
                }
            }

            if (!overlap)
            {
                point = randomPos;
                return true;
            }
        }
        return false;
    }

    // 生成单个指定预制体生物
    void SpawnSingleLife(Vector2 spawnPos, GameObject prefab)
    {
        float rot = Random.Range(0f, 360f);
        GameObject life = Instantiate(prefab, spawnPos, Quaternion.Euler(0, 0, rot), transform);
        life.name = "Life_" + spawnedPositions.Count;
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
        spawnedPositions.Clear();
    }
}