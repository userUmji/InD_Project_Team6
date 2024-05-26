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
    public Skill_Information_View Info1;
    public TextMeshProUGUI Skill2;
    public Skill_Information_View Info2;
    public TextMeshProUGUI Skill3;
    public Skill_Information_View Info3;
    public TextMeshProUGUI SkillUlt;
    public Skill_Information_View Info4;
    public TextMeshProUGUI AtkExplain;
    public TextMeshProUGUI DefExplain;
    public TextMeshProUGUI SpeedExplain;


    public void Init(string name)
    {
        GameManager.Instance.SavePlayerUnit();
        var Unit = GameManager.Instance.GetUnitData(name);
        var SaveUnit = GameManager.Instance.GetUnitSaveData(name);
        if (SaveUnit.m_isCaptured)
        {
            UnitImage.sprite = Unit.m_UnitSprite;
            ExplainText.text = Unit.m_sUnitExplain;
            NameNumber.text = "NO." + Unit.m_iUnitNo + "\n" + Unit.m_sUnitName + " LV" + SaveUnit.m_iUnitLevel;
            Type.text = GameManager.Instance.TypeToString(Unit.UnitType);
            HPBar.maxValue = Unit.m_iUnitHP + ((SaveUnit.m_iUnitLevel - 5) * 2 * SaveUnit.m_iUnitLevel);
            HPBar.value = (Unit.m_iUnitHP + ((SaveUnit.m_iUnitLevel - 5) * 2 * SaveUnit.m_iUnitLevel));
            HPAmount.text = (Unit.m_iUnitHP + ((SaveUnit.m_iUnitLevel - 5) * 2 * SaveUnit.m_iUnitLevel)).ToString();
            IntimacyBar.maxValue = 10.0f;
            IntimacyBar.value = SaveUnit.m_iIntimacy;
            IntimacyAmount.text = SaveUnit.m_iIntimacy + " / " + 10;
            Skill1.text = GameManager.Instance.Skills[SaveUnit.m_AttackBehav_1].m_sAttackName;
            Info1.Init(SaveUnit.m_AttackBehav_1,true);
            Skill2.text = GameManager.Instance.Skills[SaveUnit.m_AttackBehav_2].m_sAttackName;
            Info2.Init(SaveUnit.m_AttackBehav_2,true);
            Skill3.text = GameManager.Instance.Skills[SaveUnit.m_AttackBehav_3].m_sAttackName;
            Info3.Init(SaveUnit.m_AttackBehav_3,true);
            if (SaveUnit.m_iIntimacy == 10)
            {
                SkillUlt.text = Unit.m_AttackBehav_Ult.m_sAttackName;
                Info4.Init(SaveUnit.m_AttackBehav_Ult,true);
            }
            else
            {
                SkillUlt.text = "???";
                Info4.Init(SaveUnit.m_AttackBehav_Ult, false);
            }
            CheckMod_Status(SaveUnit.m_iPermanentAtkMod + Unit.m_iUnitAtk + (3 * SaveUnit.m_iUnitLevel), AtkExplain);
            CheckMod_Status(SaveUnit.m_iPermanentDefMod + Unit.m_iUnitDef + (3 * SaveUnit.m_iUnitLevel), DefExplain);
            CheckMod_Status(SaveUnit.m_iPermanentSpeedMod + Unit.m_iUnitSpeed + (3 * SaveUnit.m_iUnitLevel), SpeedExplain);
        }
        else
        {
            UnitImage.sprite = Resources.Load<Sprite>("Images/Unit/UnknownUnit");
            ExplainText.text = "???";
            NameNumber.text = "NO." + Unit.m_iUnitNo + "\n" + "???" + " LV" + "???";
            Type.text = "???";
            HPBar.maxValue = Unit.m_iUnitHP;
            HPBar.value = Unit.m_iUnitHP;
            HPAmount.text = "???";
            IntimacyBar.maxValue = 10.0f;
            IntimacyBar.value = 0.0f;
            IntimacyAmount.text = "???";
            Skill1.text = "???";
            Info1.Init(SaveUnit.m_AttackBehav_1, false);
            Skill2.text = "???";
            Info2.Init(SaveUnit.m_AttackBehav_1, false);
            Skill3.text = "???";
            Info3.Init(SaveUnit.m_AttackBehav_1, false);
            SkillUlt.text = "???";
            AtkExplain.text = "???";
            Info4.Init(SaveUnit.m_AttackBehav_1, false);
            DefExplain.text = "???";
            SpeedExplain.text = "???";
        }

    }


    private void CheckMod_Status(int amount, TextMeshProUGUI text)
    {
        if (amount <= 40)
            text.text = "아직 부족한것같아...";
        else if (amount > 41 && amount <= 80)
            text.text = "조금 성장했다. ";
        else if (amount > 81 && amount <= 120)
            text.text = "제법 강하다. ";
        else if (amount > 121 && amount <= 160)
            text.text = "많이 강해진듯 하다. ";
        else if (amount > 161 && amount <= 180)
            text.text = "숙련되었다!";
        else
            text.text = "최고!";
    }
}
