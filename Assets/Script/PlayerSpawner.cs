using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("玩家预制体")]
    public GameObject playerPrefab;

    [Header("出生点（如果为空则使用Spawner自身位置）")]
    public Transform spawnPoint;

    [Header("自动为玩家添加移动脚本")]
    public bool addMovementScript = true;

    [Header("自动查找相机并设置跟随")]
    public bool setupCameraFollow = true;

    private GameObject currentPlayer;

    void OnEnable()
    {
        if (currentPlayer != null)
            Destroy(currentPlayer);
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        // 如果已存在玩家，先销毁旧的（确保唯一性）
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        if (playerPrefab == null)
        {
            Debug.LogError("PlayerSpawner: 未指定玩家预制体！");
            return;
        }

        // 确定出生位置
        Vector3 spawnPos = (spawnPoint != null) ? spawnPoint.position : transform.position;

        // 生成玩家
        currentPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        // 添加移动脚本（如果没有）
        if (addMovementScript && currentPlayer.GetComponent<MouseDirectionMovement2D>() == null)
        {
            currentPlayer.AddComponent<MouseDirectionMovement2D>();
        }

        // 设置相机跟随
        if (setupCameraFollow)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                FollowPlayer follower = mainCam.GetComponent<FollowPlayer>();
                if (follower == null)
                    follower = mainCam.gameObject.AddComponent<FollowPlayer>();

                follower.player = currentPlayer.transform;
            }
            else
            {
                Debug.LogWarning("未找到 MainCamera，无法自动设置跟随");
            }
        }
    }

    // 提供公开方法用于重生
    public void RespawnPlayer()
    {
        SpawnPlayer();
    }

    public void RegeneratePlayer()
    {
        if (currentPlayer != null)
            Destroy(currentPlayer);
        SpawnPlayer(); // 原有生成方法
    }
}