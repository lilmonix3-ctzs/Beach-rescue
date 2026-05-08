using UnityEngine;
using System.Collections.Generic;

public static class ItemRandomPlacer
{
    // 矩形区域 切格子随机排布
    /// <summary>
    /// 矩形范围随机排布子物体
    /// </summary>
    /// <param name="parent">父物体</param>
    /// <param name="targetTag">筛选标签</param>
    /// <param name="rectMin">矩形左下角</param>
    /// <param name="rectMax">矩形右上角</param>
    /// <param name="cellSize">格子大小</param>
    
    private static float offsetRatio = 0.5f;   // 偏移范围比例（0.5 = 格子的一半）
    public static void RandomPlaceInRect(Transform parent, string targetTag,
        Vector2 rectMin, Vector2 rectMax, float cellSize)
    {
        // 参数检查
        if (parent == null)
        {
            Debug.LogError("父物体不能为空");
            return;
        }

        if (cellSize <= 0.001f)
        {
            Debug.LogError("cellSize 必须大于0");
            return;
        }

        List<Transform> targets = GetTagChildren(parent, targetTag);
        if (targets.Count == 0) return;

        List<Vector2Int> gridList = new List<Vector2Int>();
        int xCount = Mathf.FloorToInt((rectMax.x - rectMin.x) / cellSize);
        int yCount = Mathf.FloorToInt((rectMax.y - rectMin.y) / cellSize);

        // 确保至少有一个格子
        if (xCount <= 0 || yCount <= 0)
        {
            Debug.LogError($"矩形区域太小，无法容纳任何格子 (xCount:{xCount}, yCount:{yCount})");
            return;
        }

        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                gridList.Add(new Vector2Int(x, y));
            }
        }

        if (gridList.Count < targets.Count)
        {
            Debug.LogError($"矩形格子数量不足，需要{targets.Count}个，实际只有{gridList.Count}个");
            return;
        }

        ShuffleGrid(gridList);
        float maxOffset = cellSize * offsetRatio;
        for (int i = 0; i < targets.Count; i++)
        {
            Vector2Int grid = gridList[i];
            float posX = rectMin.x + grid.x * cellSize + cellSize * 0.5f;
            float posY = rectMin.y + grid.y * cellSize + cellSize * 0.5f;
            float offsetX = Random.Range(-maxOffset, maxOffset);
            float offsetY = Random.Range(-maxOffset, maxOffset);
            posX += offsetX;
            posY += offsetY;

            targets[i].position = new Vector3(posX, posY, targets[i].position.z);
        }
    }

    // 圆形区域 切格子随机排布
    /// <summary>
    /// 圆形范围随机排布子物体
    /// </summary>
    /// <param name="parent">父物体</param>
    /// <param name="targetTag">筛选标签</param>
    /// <param name="circleCenter">圆心</param>
    /// <param name="radius">半径</param>
    /// <param name="cellSize">格子大小</param>
    public static void RandomPlaceInCircle(Transform parent, string targetTag,
        Vector2 circleCenter, float radius, float cellSize)
    {
        // 参数检查
        if (parent == null)
        {
            Debug.LogError("父物体不能为空");
            return;
        }

        if (cellSize <= 0.001f)
        {
            Debug.LogError("cellSize 必须大于0");
            return;
        }

        if (radius <= 0)
        {
            Debug.LogError("半径必须大于0");
            return;
        }

        List<Transform> targets = GetTagChildren(parent, targetTag);
        if (targets.Count == 0) return;

        List<Vector2Int> circleGrid = new List<Vector2Int>();
        float squareSize = radius * 2f;
        int gridCount = Mathf.CeilToInt(squareSize / cellSize);
        Vector2 squareMin = circleCenter - new Vector2(radius, radius);

        for (int x = 0; x < gridCount; x++)
        {
            for (int y = 0; y < gridCount; y++)
            {
                float worldX = squareMin.x + x * cellSize + cellSize * 0.5f;
                float worldY = squareMin.y + y * cellSize + cellSize * 0.5f;
                Vector2 gridPos = new Vector2(worldX, worldY);

                if (Vector2.Distance(gridPos, circleCenter) <= radius)
                {
                    circleGrid.Add(new Vector2Int(x, y));
                }
            }
        }

        if (circleGrid.Count < targets.Count)
        {
            Debug.LogError($"圆形内可用格子不足，需要{targets.Count}个，实际只有{circleGrid.Count}个");
            return;
        }

        ShuffleGrid(circleGrid);
        float maxOffset = cellSize * offsetRatio;
        for (int i = 0; i < targets.Count; i++)
        {
            Vector2Int grid = circleGrid[i];
            float posX = squareMin.x + grid.x * cellSize + cellSize * 0.5f;
            float posY = squareMin.y + grid.y * cellSize + cellSize * 0.5f;
            float offsetX = Random.Range(-maxOffset, maxOffset);
            float offsetY = Random.Range(-maxOffset, maxOffset);
            posX += offsetX;
            posY += offsetY;
            targets[i].position = new Vector3(posX, posY, targets[i].position.z);
        }
    }

    private static List<Transform> GetTagChildren(Transform parent, string tag)
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
                list.Add(child);
        }
        return list;
    }

    private static void ShuffleGrid(List<Vector2Int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}