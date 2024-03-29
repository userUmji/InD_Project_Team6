using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void SceneChange1()
    {
<<<<<<< Updated upstream
        SceneManager.LoadScene("DialogScene");
=======
        SceneManager.LoadScene("WorldScene");
    }

    public void SceneChange2()
    {
        SceneManager.LoadScene("BattleScene");
>>>>>>> Stashed changes
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
