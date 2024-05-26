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
        questList.Add(10, new QuestData("처음주인공 등장", new int[] { 100, 200 }));
        questList.Add(20, new QuestData("해태와의 만남", new int[] { 300,  500 }));
        questList.Add(30, new QuestData("현무를 만나기위해 미르의 여름으로!!", new int[] { 800,1200, 900  }));
        questList.Add(40, new QuestData("주작을 만나기위해 미르의 가을로!!", new int[] { 1300, 1500 }));
        questList.Add(50, new QuestData("백호를 만나기위해 미르의 겨울로!!", new int[] { 1600, 2100}));
        questList.Add(60, new QuestData("황룡을 만나기위해 미르의 중앙으로!!", new int[] { 2200, 2500, 2600, 0 }));
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
                if (questActionIndex == 1)
                {
                    questObject[0].SetActive(false);
                    questObject[1].SetActive(true);
                    questObject[2].SetActive(true);
                    questObject[3].SetActive(true);
                    questObject[4].SetActive(true);
                    questObject[6].SetActive(true);
                    questObject[8].SetActive(true);
                  
                }
                else if (questActionIndex == 2)
                {
                    questObject[1].SetActive(false);
                }
                break;
            case 20:
                // 퀘스트 ID가 20인 경우, questActionIndex가 1일 때 questObject의 첫 번째 요소를 비활성화합니다.
                if (questActionIndex == 1)
                {
                    questObject[2].SetActive(false);
                    questObject[4].SetActive(false);
                    questObject[5].SetActive(true);
                }
                else if (questActionIndex == 2)
                {
                    questObject[3].SetActive(false);
                    questObject[5].SetActive(false);
                    questObject[9].SetActive(true);


                }
                else if (questActionIndex == 3)
                {
                    questObject[6].SetActive(false);
                    questObject[10].SetActive(true);

                }
              
                break;
            case 30:
                if (questActionIndex == 1)
                {
                   
                    questObject[10].SetActive(true);
                    questObject[8].SetActive(false);
                }
                    
                else if (questActionIndex == 2)
                {
                    questObject[10].SetActive(false);
                    questObject[17].SetActive(true);
                    questObject[7].SetActive(false);
                    questObject[9].SetActive(false);
                }
                else if (questActionIndex == 3)
                {
                    questObject[11].SetActive(false);
                    questObject[22].SetActive(true);
                    questObject[12].SetActive(true);
                    questObject[18].SetActive(true);
                    

                }
                break;

            case 40:
                if (questActionIndex == 1)
                {
                    questObject[22].SetActive(false);
                   
                }
                else if (questActionIndex == 2)
                {
                   
                    questObject[13].SetActive(false);
                    questObject[23].SetActive(true);
                    questObject[19].SetActive(true);
                    questObject[14].SetActive(true);
                }                   
                
                break;

            case 50:
                if (questActionIndex == 1)
                {
                    questObject[23].SetActive(false);

                }
                else if (questActionIndex == 2)
                {
                    questObject[24].SetActive(true);
                    questObject[15].SetActive(false);
                    questObject[16].SetActive(true);
                }                             
                break;
            case 60:
                if (questActionIndex == 1)
                    questObject[24].SetActive(false);
                questObject[20].SetActive(false);
                questObject[25].SetActive(true);
                questObject[21].SetActive(true);
                questObject[25].SetActive(false);
                questObject[26].SetActive(true);
                break;


        }
    }


}
