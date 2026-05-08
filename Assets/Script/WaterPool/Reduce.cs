using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Reduce : MonoBehaviour
{
    [Header("初始面积")]
    [SerializeField] private float startArea = 100f;

    [Header("每秒减少多少面积")]
    [SerializeField] private float areaReducePerSecond = 20f;//与生物量相关

    [Header("速度对面积减少的影响比例")]
    [SerializeField] private float RatioOfVelocityToAreaReduction = 0.1f; // 速度对面积减少的影响比例，生物量应限制最大速度

    //调整（fsun）新增开关
    [Header("是否开启扣水功能")]
    [SerializeField] bool isReduce = false;

    private List<OceanLife> oceanLives;

    private float currentArea;

    private float areaNeededByLife = 0f; // 当前生物需要的总面积

    public Rigidbody2D rb;
    

    void Start()
    {
        currentArea = startArea;
        rb = GetComponent<Rigidbody2D>();
        GetOceanLife();
        scaleUniform();
    }

    void Update()
    {
        DieOceanLife();
        if (isReduce)
        {
            ReduceByTime();
            ReduceByVelocity();
        }
        scaleUniform();
    }
    void ReduceByTime()
    {
        if (currentArea > 0)
        {
            // 面积线性减少
            currentArea -= areaReducePerSecond * Time.deltaTime;
            currentArea = Mathf.Max(currentArea, 0);
        }
    }
    void ReduceByVelocity()
    {
        if (currentArea > 0)
        {
            // 根据速度减少面积
            float reduceAmount = Time.deltaTime * rb.velocity.magnitude * RatioOfVelocityToAreaReduction; 
            currentArea -= reduceAmount;
            currentArea = Mathf.Max(currentArea, 0);
        }
    }

    //调整（fsun）整合等比缩放
    private void scaleUniform()
    {
        // 由面积求半径
        float radius = Mathf.Sqrt(currentArea / Mathf.PI);
        // 等比缩放
        transform.localScale = new Vector3(radius, radius, 1);
    }

    void DieOceanLife()
    {
        if (areaNeededByLife > currentArea && oceanLives.Count!=0)
        {
            Debug.Log("当前面积不足以支持所有生物");

            // 取出列表最后一个生物
            OceanLife lastLife = oceanLives[oceanLives.Count - 1];

            // 减去消耗和占用面积
            areaReducePerSecond -= lastLife.GetWaterDecreaseRate();
            areaNeededByLife -= lastLife.GetLifeArea();

            lastLife.Die();
            oceanLives.Remove(lastLife); // 列表自动变短，不会留null
        }
    }

    void GetOceanLife()
    {
        
        oceanLives = new List<OceanLife>(transform.parent.gameObject.GetComponentsInChildren<OceanLife>());

        // 快速排序
        if (oceanLives.Count > 0)
            QuickSort(oceanLives, 0, oceanLives.Count - 1);

        float areaReduceFromLife = 0f;
        areaNeededByLife = 0; // 重置

        foreach (var ol in oceanLives)
        {
            Debug.Log("找到子物体：" + ol.gameObject.name);
            areaNeededByLife += ol.GetLifeArea();
            areaReduceFromLife += ol.GetWaterDecreaseRate();
        }

        areaReducePerSecond = areaReduceFromLife;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("碰撞");
        if (collision == null) return;

        Reduce otherReduce = collision.collider.GetComponent<Reduce>();
        if (otherReduce == null || otherReduce == this) return;

        if (!otherReduce.gameObject.CompareTag("WaterPool")) return;
        Debug.Log("发生水池碰撞，尝试合并水池");
        MergeWaterPool(otherReduce);
    }

    // 将另一个水池合并到当前水池
    void MergeWaterPool(Reduce other)
    {
        if (other == null || other == this) return;

        // 确保本池的 oceanLives 列表已初始化
        if (oceanLives == null) oceanLives = new List<OceanLife>();

        // 将面积相加
        currentArea += other.currentArea;

        // 将另一个水池的生物消耗/占用数据加入（使用其他池上已统计的值，避免重复统计）
        areaReducePerSecond += other.areaReducePerSecond;
        areaNeededByLife += other.areaNeededByLife;

        // 将 other 中的 OceanLife 物体重新 parent 到当前水池，并加入本池列表（不再重复增加 areaNeeded/areaReduce）
        OceanLife[] others = other.transform.parent.gameObject.GetComponentsInChildren<OceanLife>();
        foreach (var ol in others)
        {
            // 如果已经是本池的子物体则跳过
            if (ol == null) continue;
            if (ol.transform.IsChildOf(this.transform.parent.transform)) continue;

            // 重新设置父对象（保持世界坐标）
            ol.transform.SetParent(this.transform.parent.transform, true);
            oceanLives.Add(ol);
        }

        // 重新排序海洋生物列表
        if (oceanLives.Count > 0)
            QuickSort(oceanLives, 0, oceanLives.Count - 1);

        // 更新当前缩放（根据合并后的面积）
        float radius = Mathf.Sqrt(currentArea / Mathf.PI);
        transform.localScale = new Vector3(radius, radius, 1);

        // 清理被合并的水池对象
        other.currentArea = 0f;
        other.areaNeededByLife = 0f;
        other.areaReducePerSecond = 0f;

        Destroy(other.gameObject);

    }

    //快速排序
    void QuickSort(List<OceanLife> arr, int left, int right)
    {
        if (left >= right) return;
        int pivotIndex = Partition(arr, left, right);
        QuickSort(arr, left, pivotIndex - 1);
        QuickSort(arr, pivotIndex + 1, right);
    }

    int Partition(List<OceanLife> arr, int left, int right)
    {
        OceanLife pivot = arr[left];
        float pivotRate = pivot.GetWaterDecreaseRate();
        int i = left;
        int j = right;

        while (i < j)
        {
            while (i < j && arr[j].GetWaterDecreaseRate() >= pivotRate)
                j--;

            while (i < j && arr[i].GetWaterDecreaseRate() <= pivotRate)
                i++;

            if (i < j)
            {
                OceanLife temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }

        arr[left] = arr[i];
        arr[i] = pivot;
        return i;
    }

    //调整（fsun）新增获得水域面积方法
    public float GetCurrentArea()
    {
        return currentArea;
    }
}
