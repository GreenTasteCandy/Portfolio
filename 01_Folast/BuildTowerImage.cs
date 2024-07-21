using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/* �� ���� �� UI���� �����̳� ���� ��ư�� ���� �� ����Ǵ� ��ũ��Ʈ */
public class BuildTowerImage : MonoBehaviour
{
    [SerializeField]
    WaveSystem waveSystem;
    [SerializeField]
    TowerSpawner towerSpanwer;
    [SerializeField]
    TextMeshProUGUI textPrice;
    [SerializeField]
    TextMeshProUGUI textCoolTime;
    [SerializeField]
    Image spriteTower;
    [SerializeField]
    int towerNum;
    [SerializeField]
    BuildTowerType unitTypeData;
    [SerializeField]
    GameObject imageSelet;
    [SerializeField]
    Image icons;

    TowerTemplate towerTemplate;
    bool checkSummon;

    public int Lv => towerTemplate.towerLevel;

    private void LateUpdate()
    {
        if (towerSpanwer.BuildTower == unitTypeData && towerSpanwer.TowerType == towerNum)
        {
            if (towerSpanwer.IsOnTowerBuild == true)
                imageSelet.SetActive(true);
            else
                imageSelet.SetActive(false);
        }
        else
            imageSelet.SetActive(false);

        //�����Ͻ� ǥ��
        if (unitTypeData == BuildTowerType.Tower)
        {
            towerTemplate = towerSpanwer.towerTemPlate[towerNum];

            if (waveSystem.WaveType == WaveType.Offense || waveSystem.WaveType == WaveType.ReadyOffense)
                textPrice.text = string.Format("{0:n0}", towerSpanwer.TowerSummon[towerNum]);
            else
            {
                int cost = towerTemplate.weapon[Lv].cost - (int)InventorySet.instance.UnitCostDown;
                cost = cost < towerTemplate.weapon[0].cost ? towerTemplate.weapon[0].cost : cost;
                textPrice.text = string.Format("{0:n0}", "$" + cost); //������ ��ġ ������ ǥ��
            }
        }

        //�����Ͻ� ǥ��
        else if (unitTypeData == BuildTowerType.Hero)
        {
            towerTemplate = towerSpanwer.heroTemPlate[towerNum];
            textPrice.text = string.Format("{0:n0}", towerTemplate.nameTag); //������ �� ������ �̸��� ǥ��

            if (towerSpanwer.IsHeroCool[towerNum] == true)
                textCoolTime.text = string.Format("{0:n0}", towerSpanwer.SkillRate[towerNum] - towerSpanwer.HeroCoolTime[towerNum]);
            else
                textCoolTime.text = "";
        }

        spriteTower.sprite = towerTemplate.weapon[Lv].sprite;
        icons.sprite = towerTemplate.typeIcon;
    }

    public void OnPointerDown(int num)
    {
        if (GameManager.instance.waveSystem.WaveType == WaveType.Offense)
        {
            if (checkSummon == false)
            {
                StopCoroutine("SummonTower");
                checkSummon = true;
                StartCoroutine(SummonTower(num));
            }
        }
        else
        {
            if (checkSummon == true)
            {
                StopCoroutine("SummonTower");
                checkSummon = false;
            }
        }
    }

    public void OnPointUp()
    {
        if (GameManager.instance.waveSystem.WaveType == WaveType.Offense)
        {
            checkSummon = false;
            StopCoroutine("SummonTower");
        }
    }

    IEnumerator SummonTower(int num)
    {
        while (checkSummon)
        {
            if (GameManager.instance.waveSystem.WaveType != WaveType.Offense)
            {
                StopCoroutine("SummonTower");
            }

            GameManager.instance.towerSpawner.ReadyToSpawnTower(num);

            yield return new WaitForSeconds(0.25f);
        }
    }
}
