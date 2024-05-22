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
        questList.Add(10, new QuestData("ó�����ΰ� ����", new int[] { 100, 200 }));
        questList.Add(20, new QuestData("���¿��� ����", new int[] { 300, 400, 500, 600 }));
        questList.Add(30, new QuestData("������ ���������� �̸��� ��������!!", new int[] { 700, 0 }));
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

    public string CheckQuest() // �Ű� ������ ���� �Լ�ȣ��
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
        // ���� ����Ʈ ID�� ���� Ư�� ����Ʈ ���� Ư�� ������Ʈ�� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ�մϴ�.
        switch (questId)
        {
            case 10:
                // ����Ʈ ID�� 10�� ���,
                // questActionIndex�� 2�� �� questObject�� ù ��° ��Ҹ� Ȱ��ȭ�մϴ�.
                if (questActionIndex == 1)
                {
                    questObject[0].SetActive(false);
                    questObject[1].SetActive(true);
                }
                else if (questActionIndex == 2)
                {
                    questObject[1].SetActive(false);
                }
                break;
            case 20:
                // ����Ʈ ID�� 20�� ���, questActionIndex�� 1�� �� questObject�� ù ��° ��Ҹ� ��Ȱ��ȭ�մϴ�.
                if (questActionIndex == 1)
                {
                    questObject[2].SetActive(false);
                }
                else if (questActionIndex == 2)
                {
                    questObject[5].SetActive(true);
                    questObject[4].SetActive(false);
                }
                else if (questActionIndex == 3)
                {
                    questObject[3].SetActive(false);
                    questObject[5].SetActive(false);
                }
                else if (questActionIndex == 4)
                {
                    questObject[6].SetActive(false);
                }
                break;
            case 30:
                if (questActionIndex == 1)
                    questObject[7].SetActive(false);
                break;

        }
    }


}
