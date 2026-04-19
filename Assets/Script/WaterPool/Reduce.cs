using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reduce : MonoBehaviour
{
    [Header("初始面积")]
    [SerializeField] private float startArea = 100f;

    [Header("每秒减少多少面积")]
    [SerializeField] private float areaReducePerSecond = 20f;//待改，应与生物量相关

    private OceanLife[] oceanLives;

    private float currentArea;

    public Rigidbody2D rb;

    private float RatioOfVelocityToAreaReduction = 0.1f; // 速度对面积减少的影响比例，生物量应限制最大速度

    void Start()
    {
        currentArea = startArea;
        rb = GetComponent<Rigidbody2D>();
        GetOceanLife();
    }

    void Update()
    {
        //CalculateOceanLife();
        ReduceByTime();
        ReduceByVelocity();
    }
    void ReduceByTime()
    {
        if (currentArea > 0)
        {
            // 面积线性减少
            currentArea -= areaReducePerSecond * Time.deltaTime;
            currentArea = Mathf.Max(currentArea, 0);

            // 由面积求半径
            float radius = Mathf.Sqrt(currentArea / Mathf.PI);

            // 等比缩放
            transform.localScale = new Vector3(radius, radius, 1);
        }
    }
    void ReduceByVelocity()
    {
        if (currentArea > 0)
        {
            // 根据速度减少面积
            float reduceAmount = areaReducePerSecond * Time.deltaTime * rb.velocity.magnitude * RatioOfVelocityToAreaReduction; 
            currentArea -= reduceAmount;
            currentArea = Mathf.Max(currentArea, 0);

            // 由面积求半径
            float radius = Mathf.Sqrt(currentArea / Mathf.PI);
            // 等比缩放
            transform.localScale = new Vector3(radius, radius, 1);
        }
    }

    void CalculateOceanLife()
    {
        float areaNeeded = 0f;
        float areaReduceFromLife = 0f;
        foreach (var ol in oceanLives)
        {
            Debug.Log("找到子物体：" + ol.gameObject.name);
            areaNeeded += ol.GetLifeArea();
            areaReduceFromLife += ol.GetWaterDecreaseRate();
        }
        if (areaNeeded > currentArea)
        {
            Debug.Log("当前面积不足以支持所有生物");

        }
        areaReducePerSecond = areaReduceFromLife;
    }
    void GetOceanLife()
    {
        oceanLives = GetComponentsInChildren<OceanLife>();
        foreach (var ol in oceanLives)
        {
                Debug.Log("找到子物体：" + ol.gameObject.name);
        }
    }
    // 快速排序主函数
    void QuickSort(OceanLife[] arr, int left, int right)
    {
        if (left >= right) return;

        // 得到基准最终位置
        int pivotIndex = Partition(arr, left, right);

        // 递归左右
        QuickSort(arr, left, pivotIndex - 1);
        QuickSort(arr, pivotIndex + 1, right);
    }

    // 分区：双指针，i 左→右，j 右→左
    int Partition(OceanLife[] arr, int left, int right)
    {
        // 取左边为基准
        OceanLife pivot = arr[left];
        float pivotRate = pivot.GetWaterDecreaseRate();

        int i = left;
        int j = right;

        while (i < j)
        {
            // j 从右往左找,比基准小的
            while (i < j && arr[j].GetWaterDecreaseRate() >= pivotRate)
            {
                j--;
            }

            // i 从左往右找,比基准大的
            while (i < j && arr[i].GetWaterDecreaseRate() <= pivotRate)
            {
                i++;
            }

            // 交换 i 和 j
            if (i < j)
            {
                OceanLife temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }

        // 最终 i == j，把基准换到中间位置
        arr[left] = arr[i];
        arr[i] = pivot;

        // 返回基准下标
        return i;
    }
}
