using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public void GoToPlay()
    {
        // �÷��� ������ �̵�
        SceneManager.LoadScene(1);
    }
    public void GoToMain()
    {
        // ���� ������ �̵�
        SceneManager.LoadScene(0);
    }

}
