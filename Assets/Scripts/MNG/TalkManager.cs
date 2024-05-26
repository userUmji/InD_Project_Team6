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
      

        // 대화 데이터 추가
        talkData.Add(1000, new string[] { "안녕!:0", "만나서 반가워!:0" });
        talkData.Add(2000, new string[] { "어서와!:0", "만나서 반가워!:0" });

        // 아이템 대화 데이터 추가
        talkData.Add(10 + 100, new string[] { "쿵쿵콰광!!"});

        talkData.Add(10 + 200, new string[] {"어제밤 천둥번개가 너무 심했어..:0",
                                        "한번도 이런적이 없었는데, 무슨일이지?:0",
                                        "사신수가 노하신건가..? 일단 밖으로 나가봐야겠다.:0" });

        talkData.Add(20 + 300, new string[] {"엇 저건 도깨비잖아..?? 도깨비가 왜 이런 외진곳까지?:0",
                                        "미동이 없네.. 엇 이건 뭐지?:0",
                                         "녀석이 여기까지 이걸 가져온건가?:0",
                                        "많이 다친것같네…일단 회복해줘야겠다.:0", ""});

       

        talkData.Add(20 + 500, new string[] {
                                        "이제 조금 정신을 차린것같네:0",
                                        "근데 왜 도망가지 않는거지?:0",
                                        "(도깨비의 눈을 지긋이 바라본다.)",
                                        "너 나한테 하고싶은 말이 있니?:0",
                                        "신기한 도깨비네.. 정말 온순하기도 하고..:0",
                                        "너 내 도깨비가 되서 나랑 함께가자!:0",
                                        "(도깨비가 신난듯 품속으로 안긴다.)",
                                        "앗 넌 해태구나? 신기하네, 해태는 본건 처음이야!:0" });

        talkData.Add(600, new string[] { "일단 가장 가까운 미르의 여름에 있는 사신수 현무를 만나야겠어.:0",
                                        "비록 사신수긴 하지만..   아는게 많고 온화하니:0",
                                        "분명 이번일에 대해서도 알고있을거야 분명해.:0" });

        talkData.Add(700, new string[] { "왼쪽에 있는 장승에서는 현재 정보를 저장을 할수있고,   한번 가봤던 지역을 장승을 통해 이동할수있었지?:0",
                                        "나중에 기회가 되면 한번 사용해 보자:0" });

        talkData.Add(30 + 800, new string[] { "미르의 여름이면 분명 위쪽 길로 이동하면 됬었지?:0" });

        talkData.Add(30 + 1200, new string[] { "여기가 미르의 여름이구나..?:0",
                                                 "확실히 날씨가 더운거 같아:0"});

        talkData.Add(30 + 900, new string[] { "...미르의 여름에 이런곳이 있을줄이야..처음알았어.:0",
                                              "...!:0",
                                              "현무다.:0",
                                              "잠깐, 뭔가 이상해.:0",
                                              "뭔가에 씌인듯한...사신수같지 않아!:0",
                                              "..!:0"  });

        talkData.Add(900, new string[] { "...현무가 있는 장소야:0",
                                              "지금은 볼일이 없으니깐 다음에 오자:0"
                                               });

        talkData.Add(40 + 1300, new string[] { "", "",
                                              "후...어찌저찌 이겼네..:0",
                                              "그치만 이건 뭔가 이상해.:0",
                                              "평소 사신수라고 생각되지 않을만큼 날카롭고 적대적인 모습..:0",
                                              "그래 틀림없어. 미르에 무슨일이 생긴거야!:0",
                                              "다른 사신수들도 전부 만나봐야겠어...!:0"  });

        talkData.Add(1100, new string[] { "미르의 여름은 위쪽이었지?:0","위로 이동하자!!:0" });




        talkData.Add(40 + 1500, new string[] { "와..미르의 가을에 이런 궁중정원이 있었다니, 아름다워!:0",
                                            "역시, 주작도 똑같아. 이상한 눈을 하고있어.:0",
                                            "도대체 뭐에 씌인거지?:0",
                                            "지금은 그런걸 생각할때가 아니야.:0",
                                            "자 얘들아 가자!:0"});

        talkData.Add(1500, new string[] { "...주작이 있는 장소야:0",
                                              "지금은 볼일이 없으니깐 다음에 오자:0"
                                               });

        talkData.Add(50 + 1600, new string[] {"", "", "후..이번에도 간신히 이겼네!:0",
                                            "사신수가 전부 뭔가에 홀린다는건..그냥 도깨비의 소행은 아니야.:0",
                                            "(쿠르르쾅!!)...!번개가 또?:0",
                                            "이상해..이만한 번개를 낼수있는 도깨비는  황룡밖에 없는데..!:0",
                                            "설마 황룡이 돌아온건가?? 미르에 다시?:0",
                                             " 도대체 미르에는 무슨일이 일어난거야...:0"});


        talkData.Add(50 + 2100, new string[] { "추워..얼어버릴것만 같아.:0",
                                            "백호는 다를줄 알았는데, 역시 뭔가에 씌여있네:0",
                                            "..미르를 지키기 위해서 여기까지 달려왔어.:0",
                                            "분명 이 앞에 뭔가가 있는건 분명해!:0",
                                            "도깨비들아 가자!?:0"});

        talkData.Add(2100, new string[] { "...백호가 있는 장소야:0",
                                              "지금은 볼일이 없으니깐 다음에 오자:0"
                                               });




        talkData.Add(60 + 2200, new string[] { "", "","질뻔했어..후! :0",
                                            "드디어 사신수들을 모두 만났네....신기하지, 원래에 나였으면 상상도 못할일이야.:0",
                                            "이제 확실해졌어. 황룡이 돌아오고나서 부터 미르에 무슨일이 생긴거야.:0",
                                            "그걸 막기위해 사신수가 나섰지만, 다들 뭔가에 씌인거지.:0",
                                            "황룡을 만나러 가야겠어.:0"});

        talkData.Add(60 + 2500, new string[] { "....여긴 어디지? :0",
                                            "난 분명 황궁에 들어와서....!:0",
                                            "황룡..이다..어째서 미르를 이렇게 만든거지?:0",
                                            "...:1",
                                            "이상해..분명 미르는 평화로운 세계였어..:0",
                                            "도깨비와 사람들이 어울러 살아가는..그런곳이였어.:0",
                                            "...:1",
                                            "나와 도깨비들의 미르를 돌려받겠어.:0"});

        talkData.Add(60 + 2600, new string[] {
                                            "하..하...쓰러뜨렸나...?:0",
                                            "어째서지? 왜 자신이 만든 미르를 이렇게 만든거야?..:0",
                                            "...!:0",
                                            "쓰러지지 않았잖아..!:0",
                                            "!:0",
                                            "황룡이 아니야..?:0",
                                            "너는..:0",
                                            "분명해, '독룡'이야!!:0",
                                            "미르에 번개가 치고 사신수가 이상해진것도, 도깨비가 날뛰어 사람들이 전부 숨어버린것도,:0",
                                            "전부 네 짓이였어!!:0",
                                            "하하 그렇다 소녀여.:1",
                                            "숨길려고 황룡으로 제 아무리 변장을 한들,:1",
                                            "나의 사악한 마음은 가릴수가 없는것이구나.:1",
                                            "...!:0",
                                            "우리의 미르를 잘도 망쳤겠다!!:0",
                                            "황룡의 흉내를 낸것도 모잘라 사신수까지 홀리다니,:0",
                                            "절때 용서하지 못해!:0",
                                            "이런, 너무 열을 내는군.:1",
                                            "어린 소녀여 그대는 정령 이 세계가 옳다고 생각하는가?:1",
                                            "그게 무슨말이야? 당연하지!:0",
                                            "인간과 도깨비가 어울러 살아가는 세계라.. :1",
                                            "그래, 너같은 아이한테는 아주 아름다운 동화속 이야기일지 모르겠구나.:1",
                                            "허나, 우리는 '도깨비' 인것이다.:1",
                                            "인간들은 우리를 섬기고, 신비로워하며, 두려워해야하는것이지.:1",
                                            "너희 인간들은 어찌하여 항상, 그리 본인들만 생각하는것이냐?:1",
                                            "도깨비가 무슨 친구쯤이라도 되는줄 알았느냐?:1",
                                            "오만하기 짝이없다. :1",
                                            "그런 세계는 없어져야 마땅한것이다.:1",
                                            "확실히..나는 도깨비와 어울리며 사는게 좋지 않다고 생각했어.:0",
                                            "무섭고 두려워서 나는 절때 친구가 될수없다고 생가했거든.:0",
                                            "하지만 해태와 도깨비들을 만나면서 깨달았어.:0",
                                            "우린 서로 다르지만, 어울러 같이 살아갈수있다고.:0",
                                            "그렇기에 지금 난 여기에 서있는거야. :0",
                                            "자 너를 물리치고 미르를 돌려받겠어.:0"
                                        });










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
        portraitData.Add(200 + 0, portraitArr[2]);
        portraitData.Add(300 + 0, portraitArr[2]);
       
        portraitData.Add(500 + 0, portraitArr[2]);
        portraitData.Add(600 + 0, portraitArr[2]);
        portraitData.Add(700 + 0, portraitArr[2]);
        portraitData.Add(800 + 0, portraitArr[2]);
        portraitData.Add(900 + 0, portraitArr[2]);
        portraitData.Add(1100 + 0, portraitArr[2]);
        portraitData.Add(1200 + 0, portraitArr[2]);
        portraitData.Add(1300 + 0, portraitArr[2]);
        portraitData.Add(1500 + 0, portraitArr[2]);
        portraitData.Add(1600 + 0, portraitArr[2]);
        portraitData.Add(2100 + 0, portraitArr[2]);
        portraitData.Add(2200 + 0, portraitArr[2]);
        portraitData.Add(2500 + 0, portraitArr[2]);
        portraitData.Add(2500 + 1, portraitArr[3]);
        portraitData.Add(2600 + 0, portraitArr[2]);
        portraitData.Add(2600 + 1, portraitArr[4]);




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
