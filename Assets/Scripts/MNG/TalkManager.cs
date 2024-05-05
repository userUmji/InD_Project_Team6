using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 대화 및 캐릭터 초상화를 관리하는 스크립트
public class TalkManager : MonoBehaviour
{
    // 대화 데이터를 저장할 딕셔너리
    Dictionary<int, string[]> talkData;
    // 캐릭터 초상화를 저장할 딕셔너리
    Dictionary<int, Sprite> portraitData;

    // 초상화 스프라이트 배열
    public Sprite[] portraitArr;

    // 초기화 함수
    void Awake()
    {
        // 딕셔너리들을 초기화
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        // 데이터 생성 함수 호출
        GenerateData();
    }

    // 대화 및 초상화 데이터 생성 함수
    void GenerateData()
    {
        talkData.Add(30 + 100, new string[] { "지금부터", "이벤트를 실행합니다!!" });

        // 대화 데이터 추가
        talkData.Add(1000, new string[] { "안녕!:0", "만나서 반가워!:0" });
        talkData.Add(2000, new string[] { "어서와!:0", "만나서 반가워!:0" });

        // 아이템 대화 데이터 추가
        talkData.Add(200, new string[] { "아이템1 이다." });
        talkData.Add(300, new string[] { "아이템2 이다." });

        //Quest Talk
        talkData.Add(10 + 1000, new string[] { "어서와:0",
                                            "이마을에 놀라운 전설이 있다는데:0"
                                            ,"알고있니?:0" });

        talkData.Add(11 + 2000, new string[] { "어서와:0",
                                            "너도 이마을에 전설에 대해 괸심있니?:0"
                                            ,"알고싶으면 마을에 있는 도께비를 찾아와줘:0" });

        // 아이템 퀘스트 대사 ----------------------------------
        talkData.Add(20 + 1000, new string[] { "오른쪽에 있는 도께비와 이야기해줘:0" });
        talkData.Add(20 + 2000, new string[] { "찾으면 가져다말해줘:0" });
        talkData.Add(20 + 5000, new string[] { "만나서 반가워.:0" });
        talkData.Add(21 + 2000, new string[] { "데리고와줘서 고마워:0" });

        // 초상화 데이터 추가
        portraitData.Add(100 + 0, portraitArr[0]);
        portraitData.Add(5000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(2000 + 0, portraitArr[1]);
    }

    // 지정된 대화 ID와 대화 인덱스에 해당하는 대화 반환
    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
            {
                // 퀘스트 맨 처음 대사마저 없을 때 기본 대사를 반환합니다.
                return GetTalk(id - id % 100, talkIndex);
            }
            else
            {
                // 해당 퀘스트 진행 순서 대사가 없을 때 퀘스트 맨 처음 대사를 반환합니다.
                return GetTalk(id - id % 10, talkIndex);
            }
        }

        // 대화 인덱스가 대화 데이터 길이와 같으면 null을 반환합니다.
        if (talkIndex == talkData[id].Length)
        {
            return null;
        }
        else
        {
            // 그렇지 않으면 해당 대화를 반환합니다.
            return talkData[id][talkIndex];
        }
    }


    // 지정된 ID와 초상화 인덱스에 해당하는 초상화 반환
    public Sprite GetPortrait(int id, int portraitIndex)
    {
        // ID와 인덱스를 사용하여 초상화 반환
        return portraitData[id + portraitIndex];
    }
}
