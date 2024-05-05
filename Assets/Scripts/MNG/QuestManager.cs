using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public GameObject[] questObject;
    Dictionary<int, QuestData> questList;

    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(10, new QuestData("도께비들과 이야기 나누기.", new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("아이템을 획득하기.", new int[] { 5000, 2000 }));
        questList.Add(30, new QuestData("특정지역 도달", new int[] { 100 }));
        questList.Add(40, new QuestData("퀘스트 올 클리어.", new int[] { 0 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        // Control Quest Object
        ControlQuestObject();

        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        return questList[questId].questName;
    }

    public string CheckQuest() // 매개 변수에 따라 함수호출
    {
        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    void ControlQuestObject()
    {
        // 현재 퀘스트 ID에 따라서 특정 퀘스트 도중 특정 오브젝트를 활성화 또는 비활성화합니다.
        switch (questId)
        {
            case 10:
                // 퀘스트 ID가 10인 경우,
                // questActionIndex가 2일 때 questObject의 첫 번째 요소를 활성화합니다.
                if (questActionIndex == 2)
                    questObject[0].SetActive(true);
                break;
            case 20:
                // 퀘스트 ID가 20인 경우, questActionIndex가 1일 때 questObject의 첫 번째 요소를 비활성화합니다.
                if (questActionIndex == 1)
                    questObject[0].SetActive(false);
                else if (questActionIndex == 2)
                    questObject[1].SetActive(true);
                break;
            case 30:
                // 퀘스트 ID가 30인 경우, questActionIndex가 1일 때 questObject의 첫 번째 요소를 비활성화합니다.
                if (questActionIndex == 1)
                    questObject[1].SetActive(false);
                break;
        }
    }


}
