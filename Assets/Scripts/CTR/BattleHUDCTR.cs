using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations;

public class BattleHUDCTR : MonoBehaviour
{
    // UI 요소들
    public TextMeshProUGUI nameText;   // 이름을 표시하는 텍스트
    public TextMeshProUGUI levelText;  // 레벨을 표시하는 텍스트
    public TextMeshProUGUI HPText;
    public Slider hpSlider; // 체력을 표시하는 슬라이더
    public Animator animator;
    public Animator animator_SkillEffect;


    public Image g_imagePortrait; // 초상화 이미지
    public RectTransform g_imageAnimation;
    // HUD를 설정하는 메서드
    public void SetHUD(UnitEntity unit)
    {
        // 유닛의 이름을 텍스트로 설정
        nameText.text = unit.m_sUnitName;
        // 유닛의 레벨을 텍스트로 설정
        levelText.text = "Lvl " + unit.m_iUnitLevel;
        // 슬라이더의 최대값을 유닛의 최대 체력으로 설정
        hpSlider.maxValue = unit.m_iUnitHP;
        // 슬라이더의 값(체력)을 유닛의 현재 체력으로 설정
        hpSlider.value = unit.m_iCurrentHP;
        //StateText.text = unit.g_UnitState.ToString();
        HPText.text = unit.m_iCurrentHP + "/" + unit.m_iUnitHP;
    }


    public void CheckUnitNo(int Num)
    {
        animator.SetInteger("UnitNo", Num);
        Vector2 Scale = new Vector2(500, 500);
        if (Num == 2)
        {
            Scale = new Vector2(53.0f, 62.0f);

        }
        else if (Num == 3)
        {
            Scale = new Vector2(56.0f, 63.0f);
        }
        else if (Num == 4)
        {
            Scale = new Vector2(71.0f, 51.0f);
        }
        else if (Num == 5)
        {
            Scale = new Vector2(64.0f, 69.0f);
        }
        else if (Num == 6)
        {
            Scale = new Vector2(73.0f, 70.0f);

        }
        else if (Num == 7)
        {
            Scale = new Vector2(60.0f, 66.0f);
        }
        else if (Num == 8)
        {
            Scale = new Vector2(66.0f, 67.0f);
        }
        else if (Num == 9)
        {
            Scale = new Vector2(60.0f, 68.0f);
        }
        else if (Num == 10)
        {
            Scale = new Vector2(70.0f, 51.0f);
        }
        else if (Num == 11)
        {
            Scale = new Vector2(72.0f, 72.0f);
        }
        else
        {
            Scale = new Vector2(56.0f, 60.0f);
        }
        Scale *= 7;
        g_imageAnimation.sizeDelta = Scale;
    }

    // 체력을 업데이트하는 메서드
    public void SetHP(int hp)
    {
        // 슬라이더의 값(체력)을 주어진 값으로 설정
        hpSlider.value = hp;
    }
}
