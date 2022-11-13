using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public void GoToPlay()
    {
        // 플레이 씬으로 이동
        SceneManager.LoadScene(1);
    }
    public void GoToMain()
    {
        // 메인 씬으로 이동
        SceneManager.LoadScene(0);
    }

}
