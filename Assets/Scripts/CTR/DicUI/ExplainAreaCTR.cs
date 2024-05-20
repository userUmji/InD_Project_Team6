using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ExplainAreaCTR : MonoBehaviour
{
    public Image UnitImage;
    public TextMeshProUGUI ExplainText;
    public TextMeshProUGUI NameNumber;
    public TextMeshProUGUI Type;
    public TextMeshProUGUI HPExplain;
    public TextMeshProUGUI HPAmount;
    public Slider HPBar;
    public TextMeshProUGUI IntimacyExplain;
    public TextMeshProUGUI IntimacyAmount;
    public Slider IntimacyBar;
    public TextMeshProUGUI Skill1;
    public TextMeshProUGUI Skill2;
    public TextMeshProUGUI Skill3;
    public TextMeshProUGUI SkillUlt;
    public TextMeshProUGUI AtkExplain;
    public TextMeshProUGUI DefExplain;
    public TextMeshProUGUI SpeedExplain;


    public void Init(string name)
    {
        var Unit = GameManager.Instance.GetUnitData(name);
        var SaveUnit = GameManager.Instance.GetUnitSaveData(name);
        if(SaveUnit.m_isCaptured)
        {
            UnitImage.sprite = Unit.m_UnitSprite;
            ExplainText.text = Unit.m_sUnitExplain;
            NameNumber.text = "NO." + Unit.m_iUnitNo + "\n" + Unit.m_sUnitName + " LV" + SaveUnit.m_iUnitLevel;
            Type.text = GameManager.Instance.TypeToString(Unit.UnitType);
            HPBar.maxValue = Unit.m_iUnitHP;
            HPBar.value = Unit.m_iUnitHP;
            HPAmount.text = Unit.m_iUnitHP.ToString();
            IntimacyBar.maxValue = 10.0f;
            IntimacyBar.value = SaveUnit.m_iIntimacy;
            IntimacyAmount.text = SaveUnit.m_iIntimacy + " / " + 10;
            Skill1.text = GameManager.Instance.Skills[SaveUnit.m_AttackBehav_1].m_sAttackName;
            Skill2.text = GameManager.Instance.Skills[SaveUnit.m_AttackBehav_2].m_sAttackName;
            Skill3.text = GameManager.Instance.Skills[SaveUnit.m_AttackBehav_3].m_sAttackName;
            if (SaveUnit.m_iIntimacy == 10)
                SkillUlt.text = Unit.m_AttackBehav_Ult.m_sAttackName;
            else
                SkillUlt.text = "???";
            CheckMod_Status(SaveUnit.m_iPermanentAtkMod + Unit.m_iUnitAtk, AtkExplain);
            CheckMod_Status(SaveUnit.m_iPermanentDefMod + Unit.m_iUnitDef, DefExplain);
            CheckMod_Status(SaveUnit.m_iPermanentSpeedMod + Unit.m_iUnitSpeed, SpeedExplain);
        }
        else
        {
            UnitImage.sprite = Resources.Load<Sprite>("Image/Unit/UnkownUnit");
            ExplainText.text = "???";
            NameNumber.text = "NO." + Unit.m_iUnitNo + "\n" + "???" + " LV" + "???";
            Type.text = "???";
            HPBar.maxValue = Unit.m_iUnitHP;
            HPBar.value = Unit.m_iUnitHP;
            HPAmount.text = "???";
            IntimacyBar.maxValue = 10.0f;
            IntimacyAmount.text = "???";
            Skill1.text = "???";
            Skill2.text = "???";
            Skill3.text = "???";
            SkillUlt.text = "???";
            AtkExplain.text = "???";
            DefExplain.text = "???";
            SpeedExplain.text = "???";
        }

    }

    private void CheckMod_Status(int amount,TextMeshProUGUI text)
    {
        if (amount <= 40)
            text.text = "아직 부족한 것 같아.";
        else if (amount > 41 && amount <= 80)
            text.text = "조금 성장했다.";
        else if (amount > 81 && amount <= 120)
            text.text = "제법 강하다.";
        else if (amount > 121 && amount <= 160)
            text.text = "많이 강해진 듯 하다.";
        else if (amount > 161 && amount <= 180)
            text.text = "숙련되었다!";
        else
            text.text = "최고!";
    }
}
