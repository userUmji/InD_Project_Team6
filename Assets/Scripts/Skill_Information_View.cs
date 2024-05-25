using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Skill_Information_View : MonoBehaviour
{
    public GameObject view_Ob;
    public TextMeshProUGUI name_Text;
    public TextMeshProUGUI power_Text;
    public TextMeshProUGUI count_Text;
    public TextMeshProUGUI information_Text;

    public string ex_Name;
    public string ex_Power;
    public string ex_Count;
    public string ex_Information;

    public void Show_Information()
    {
        
        view_Ob.SetActive(true);
        view_Ob.transform.position = new Vector2(transform.position.x - 370, transform.position.y);
        name_Text.text = ex_Name;
        power_Text.text = "공격력 : " + ex_Power.ToString();
        count_Text.text = "사용 최대 횟수 " + ex_Count.ToString();
        information_Text.text = ex_Information;

    }
    public void Init(int attackindex , bool isInit)
    {
        if(isInit == true)
        {
            SOAttackBase attack = GameManager.Instance.Skills[attackindex];
            ex_Name = attack.m_sAttackName;
            ex_Power = attack.m_fAttackMag.ToString();
            ex_Count = attack.m_iUseAmount.ToString();
            ex_Information = attack.m_sSkillDescription;
        }
        else
        {
            ex_Name = "???";
            ex_Power = "??";
            ex_Count = "???";
            ex_Information = "이 공격은 아직 알지 못한다.";
        }

    }
    public void Off_Information()
    {
        view_Ob.SetActive(false);
        information_Text.text = " ";
    }
}
