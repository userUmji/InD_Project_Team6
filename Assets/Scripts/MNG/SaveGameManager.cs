using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveGameManager : MonoBehaviour
{
    public GameObject player;
    public QuestManager questManager;
    public TextMeshProUGUI QuestTalk; // Text 형식 사용을 위한 선언

    public GameObject menuSet; // menuSet 변수 선언

    void Start()
    {
        GameLoad();
        QuestTalk.text = questManager.CheckQuest();
    }

    // 게임을 저장하는 함수
    public void SaveGame()
    {
        // 플레이어의 현재 위치 저장
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);

        // 퀘스트 진행 상황 저장
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);

        // PlayerPrefs 저장
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
            return;

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;
    }
}
