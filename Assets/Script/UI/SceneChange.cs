using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneChanger : MonoBehaviour
{
    public string targetSceneName;

    public void ChangeScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}