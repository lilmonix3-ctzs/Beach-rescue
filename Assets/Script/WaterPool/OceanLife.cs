using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanLife : MonoBehaviour
{
    [SerializeField]
    private string lifeName;
    [SerializeField]
    private int lifeValue;//计算积分用，暂时没什么用
    [SerializeField]
    private float lifeArea;//生物存活占据的面积

    [SerializeField]
    private float waterDecreaseRate = 1f;//每秒消耗的水资源

    public int GetLifeValue()
    {
        return lifeValue;
    }

    public float GetLifeArea()
    {
        return lifeArea;
    }

    public string GetLifeName()
    {
        return lifeName;
    }
    public float GetWaterDecreaseRate()
    {
        return waterDecreaseRate;
    }
    public void Die()
    {
        Destroy(gameObject);
    }

}
