using UnityEngine;

public class GameSceneReset : MonoBehaviour
{
    private n_PoolSpawner poolSpawner;
    private n_OceanLifeSpawner oceanSpawner;
    private PlayerSpawner playerSpawner;

    void OnEnable()
    {
        // 每次场景激活时（包括首次加载和从其他场景返回）重置
        ResetAllGenerators();
        Time.timeScale = 1;
    }

    public void ResetAllGenerators()
    {
        // 查找生成器（如果场景中只有一个，也可以直接拖拽引用）
        poolSpawner = FindObjectOfType<n_PoolSpawner>();
        oceanSpawner = FindObjectOfType<n_OceanLifeSpawner>();
        playerSpawner = FindObjectOfType<PlayerSpawner>();

        if (poolSpawner != null) poolSpawner.RegeneratePools();
        if (oceanSpawner != null) oceanSpawner.RegenerateLives();
        if (playerSpawner != null) playerSpawner.RegeneratePlayer();
    }
}