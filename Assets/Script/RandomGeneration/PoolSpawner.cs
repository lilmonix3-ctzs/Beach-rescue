using UnityEngine;

public class PoolSpawner : MonoBehaviour
{
    [Header("水池预制体")]
    public GameObject poolPrefab;

    [Header("生成数量")]
    public int spawnCount = 5;

    [Header("生成范围")]
    public Vector2 spawnArea = new Vector2(20, 10);

    [Header("水池最小间距")]
    public float minDistance = 2f;

    void Start()
    {
        SpawnPools();
    }

    void SpawnPools()
    {
        if (poolPrefab == null)
        {
            Debug.LogError("请拖入水塘预制体！");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnPos = Vector2.zero;
            bool foundPos = false;
            int tryNum = 0;

            while (!foundPos && tryNum < 200)
            {
                tryNum++;
                float x = Random.Range(-spawnArea.x, spawnArea.x);
                float y = Random.Range(-spawnArea.y, spawnArea.y);
                spawnPos = (Vector2)transform.position + new Vector2(x, y);

                foundPos = true;
                foreach (Transform child in transform)
                {
                    if (Vector2.Distance(child.position, spawnPos) < minDistance)
                    {
                        foundPos = false;
                        break;
                    }
                }
            }

            if (foundPos)
            {
                // ✅【关键】先生成，再设置父物体，且不继承缩放
                GameObject pool = Instantiate(poolPrefab);
                pool.transform.position = spawnPos;
            }
        }

        Debug.Log("生成完成！");
    }
}