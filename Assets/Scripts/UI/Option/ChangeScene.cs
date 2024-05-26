using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class ChangeScene : MonoBehaviour
{
    public GameObject BattleSceneChange;
    public Coroutine LoadBattleCr;
    public void SceneChange()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void SceneChange1()
    {
        SceneManager.LoadScene("WorldScene");
    }
    public void SceneChage3()
    {
        SceneManager.LoadScene("CutScene");
    }
    public void SceneChange2()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadBattleScene(string enemyBattleUnit, int lvl)
    {
        if (LoadBattleCr == null)
        {
            GameManager.Instance.InitGOs();
            GameManager.Instance.g_GameState = GameState.BATTLE;
            AsyncOperation SceneOper = SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);
            SceneOper.allowSceneActivation = false;


            LoadBattleCr = StartCoroutine(LoadBattleScene(SceneOper,enemyBattleUnit,lvl));



            //SceneOper.allowSceneActivation = true;
        }
    }
    IEnumerator LoadBattleScene(AsyncOperation oper, string enemyBattleUnit, int lvl)
    {
        int waitTime = 0;
        RectTransform rect = BattleSceneChange.GetComponent<RectTransform>();
        Vector2 TargetScale = new Vector2 (19200,10800);
        while (true)
        {
           
            rect.sizeDelta = TargetScale/60* waitTime;
            waitTime += 1;
            yield return new WaitForSeconds(0.01f);
            if(waitTime > 60) {
                break;
            }
        }
        rect.sizeDelta = Vector2.zero;
        GameManager.Instance.g_sEnemyBattleUnit = enemyBattleUnit;
        GameManager.Instance.g_iEnemyBattleLvl = lvl;
        GameManager.Instance.GetWorldCanvasGO().SetActive(false);
        oper.allowSceneActivation = true;
        LoadBattleCr = null;
    }

}
