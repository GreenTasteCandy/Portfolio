using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/* 
 * 부대 편성시 각 항목별로 보유한 영웅/유닛을 표시하는 스크립트
 */
public class SquadSetting : MonoBehaviour
{
    [SerializeField]
    GameObject scrollView;
    [SerializeField]
    GameObject unitStatus;
    [SerializeField]
    GameObject squadExit;
    [SerializeField]
    GameObject unitSlotPrefab;

    [SerializeField]
    Image imageTower;
    [SerializeField]
    TextMeshProUGUI textName;
    [SerializeField]
    TextMeshProUGUI textDamage;
    [SerializeField]
    TextMeshProUGUI textRate;
    [SerializeField]
    TextMeshProUGUI textRange;
    [SerializeField]
    TextMeshProUGUI textHp;
    [SerializeField]
    TextMeshProUGUI textNotice;

    int setSquad;
    TowerTemplate seletUnit;
    List<GameObject> unitNotice;
    
    public int SetSquad => setSquad;
    public TowerTemplate SeletUnit => seletUnit;

    private void Start()
    {
        unitNotice = new List<GameObject>();
    }

    private void LateUpdate()
    {
        if (seletUnit != null)
        {
            imageTower.sprite = seletUnit.weapon[0].sprite;
            textName.text = seletUnit.nameTag;
            textDamage.text = string.Format("{0:n1}", "공격력 : " + seletUnit.weapon[0].damage);
            textRate.text = string.Format("{0:n1}", "공격 속도 : " + seletUnit.weapon[0].rate);
            textRange.text = string.Format("{0:n1}", "공격 범위 : " + seletUnit.weapon[0].range);
            textHp.text = string.Format("{0:n1}", "체력 : " + seletUnit.weapon[0].maxHp);

            textNotice.text = string.Format("{0:n1}", seletUnit.notice);
        }
    }

    public void SeletSquad(int num)
    {
        if (unitNotice != null)
        { 
            for(int i = 0; i < unitNotice.Count; i++)
            {
                Destroy(unitNotice[i]);
            }    
        }
        unitNotice.Clear();

        setSquad = num;

        int squadNum = 0;

        if (setSquad == 0)
        {
            squadNum = SettingVariable.instance.UnitAllSet.Count;
        }
        else if (setSquad == 1)
        {
            squadNum = SettingVariable.instance.UnitSet.Length;
        }
        else if (setSquad == 2)
        {
            squadNum = SettingVariable.instance.UnitSet2.Length;
        }
        else if (setSquad == 3)
        {
            squadNum = SettingVariable.instance.UnitSet3.Length;
        }
        else if (setSquad == 4)
        {
            squadNum = SettingVariable.instance.HeroSet.Length;
        }

        for (int i = 0; i < squadNum; i++)
        {
            GameObject unitSlot = Instantiate(unitSlotPrefab);
            bool unlock = true;

            if (setSquad == 0)
            {
                if (i < SettingVariable.instance.UnitSet.Length)
                    unlock = SettingVariable.unitUnLock[i];
                else if (i < SettingVariable.instance.UnitSet.Length + SettingVariable.instance.UnitSet2.Length)
                    unlock = SettingVariable.unitUnLock2[i - SettingVariable.instance.UnitSet.Length];
                else if (i < SettingVariable.instance.UnitSet.Length + SettingVariable.instance.UnitSet2.Length + SettingVariable.instance.UnitSet3.Length)
                    unlock = SettingVariable.unitUnLock3[i - (SettingVariable.instance.UnitSet.Length + SettingVariable.instance.UnitSet2.Length)];
            }
            else if (setSquad == 1)
                unlock = SettingVariable.unitUnLock[i];
            else if (setSquad == 2)
                unlock = SettingVariable.unitUnLock2[i];
            else if (setSquad == 3)
                unlock = SettingVariable.unitUnLock3[i];
            else if (setSquad == 4)
                unlock = SettingVariable.heroUnLock[i];

            unitSlot.GetComponent<UnitSquad>().SetUp(this, SettingVariable.instance, i, unlock);
            unitSlot.transform.parent = scrollView.transform;
            unitNotice.Add(unitSlot);
        }
    }

    public void SeletUnitTemplate(TowerTemplate template)
    {

        unitStatus.SetActive(true);

        if (template.unitTypeData == UnitTypeData.Hero)
        {
            squadExit.GetComponent<Button>().interactable = false;

            for (int i = 0; i < SettingVariable.heroSlot.Length; i++)
            {
                if (template == SettingVariable.heroSlot[i])
                {
                    squadExit.GetComponent<Button>().interactable = true;
                    break;
                }
            }

        }
        else
        {
            squadExit.GetComponent<Button>().interactable = false;

            for (int i = 0; i < SettingVariable.unitSlot.Length; i++)
            {
                if (template == SettingVariable.unitSlot[i])
                {
                    squadExit.GetComponent<Button>().interactable = true;
                    break;
                }
            }

        }

        seletUnit = template;
    }

    public void SeletUnitExit()
    {
        if (seletUnit != null)
        {
            if (seletUnit.unitTypeData == UnitTypeData.Hero)
            {
                for (int i = 0; i < SettingVariable.heroSlot.Length; i++)
                {
                    if (seletUnit == SettingVariable.heroSlot[i])
                    {
                        SettingVariable.heroSlot[i] = null;
                        break;
                    }
                }
            }
            else 
            {
                for (int i = 0; i < SettingVariable.unitSlot.Length; i++)
                {
                    if (seletUnit == SettingVariable.unitSlot[i])
                    {
                        SettingVariable.unitSlot[i] = null;
                        break;
                    }
                }
            }
        }
    }
}
