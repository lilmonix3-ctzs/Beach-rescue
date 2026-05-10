using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    private void Update()
    {
        scoreText.text = "Score:  " + DataStore.Instance.Score.ToString();
    }
}
