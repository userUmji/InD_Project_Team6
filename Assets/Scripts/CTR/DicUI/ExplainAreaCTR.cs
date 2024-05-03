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
        UnitImage.sprite = Unit.m_UnitSprite;
        ExplainText.text = Unit.m_sUnitExplain;
        NameNumber.text = "NO." + Unit.m_iUnitNo + "\n" + Unit.m_sUnitName +" LV" + SaveUnit.m_iUnitLevel;
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
        CheckMod_Status(SaveUnit.m_iPermanentAtkMod, AtkExplain);
        CheckMod_Status(SaveUnit.m_iPermanentDefMod, DefExplain);
        CheckMod_Status(SaveUnit.m_iPermanentSpeedMod, SpeedExplain);
    }

    private void CheckMod_Status(int amount,TextMeshProUGUI text)
    {
        if (amount == 5)
            text.text = "조금 강해진 것 같다.";
        else if (amount == 10)
            text.text = "확실히 강하다!";
        else if (amount == 15)
            text.text = "최강!";
        else
            text.text = "아직 부족한 것 같다";
    }
}
