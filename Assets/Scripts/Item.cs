using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu]
public class Item : ScriptableObject
{
   // public GameObject item_Ob; // 실제로 가지고 있는 아이템
   // public int distinguishst_Seed = 0; // 씨앗마다 다른 방식으로 자라서 그걸 구별해주기 위해서 선언함
    public Sprite item_Image; // 인벤토리에 보여줄 이미지
    public string item_Name; // 아이템 이름
    //public int price; // 아이템 가격
    //public string kind; // 아이템 종류
    //public string explanation; // 아이템 설명
}

