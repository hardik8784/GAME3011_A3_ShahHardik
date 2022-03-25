using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void GoToMain()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void GoToStart()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }

    public void GoToWin()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        UnityEngine.SceneManagement.SceneManager.LoadScene("Win");
    }
}