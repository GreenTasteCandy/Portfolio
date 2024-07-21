using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*타워 정보창에서 타워의 정보들을 띄우게 하는 스크립트*/
public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    Image imageTower;
    [SerializeField]
    TextMeshProUGUI textDamage;
    [SerializeField]
    TextMeshProUGUI textRate;
    [SerializeField]
    TextMeshProUGUI textRange;
    [SerializeField]
    TextMeshProUGUI textHp;
    [SerializeField]
    TextMeshProUGUI textName;
    [SerializeField]
    GameObject[] levelimage;
    [SerializeField]
    Sprite[] icons;

    [SerializeField]
    GameObject[] uiSprite;
    [SerializeField]
    TextMeshProUGUI textDamageUpgrade;
    [SerializeField]
    TextMeshProUGUI textRateUpgrade;
    [SerializeField]
    TextMeshProUGUI textRangeUpgrade;
    [SerializeField]
    TextMeshProUGUI textHpUpgrade;
    [SerializeField]
    TextMeshProUGUI textUpgradeCost;
    [SerializeField]
    TextMeshProUGUI textSell;

    [SerializeField]
    TowerAttackRange towerAttackRange;
    [SerializeField]
    SystemTextViewer systemTextViewer;
    [SerializeField]
    GameObject mainCamera;

    [SerializeField]
    InventorySet inven;

    [SerializeField]
    GameObject textSkill;
    [SerializeField]
    Image iconSkill;
    [SerializeField]
    Sprite[] heroIconSkill;
    [SerializeField]
    TextMeshProUGUI textSkillName;
    [SerializeField]
    TextMeshProUGUI textSkillNotice;


    public Button buttonUpgrade;

    TowerWeapon currentTower;
    EnemyHP currentTowerHp;
    /*선택한 타워 쪽으로 카메라 확대시키기 위함*/
    Vector3 cameraTransform;
    Vector3 cameraLocate;

    private void Awake()
    {
        cameraTransform = mainCamera.transform.position;
        cameraLocate = cameraTransform;
        OffPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon) //타워 정보창을 띄움
    {
        /*선택한 타워 쪽으로 카메라 확대*/
        cameraTransform.x = towerWeapon.position.x;
        cameraTransform.z = towerWeapon.position.z;
        cameraTransform.y = towerWeapon.position.y;

        Vector3 cameraDirection = mainCamera.transform.localRotation * Vector3.back * 5;

        mainCamera.transform.position = cameraTransform + cameraDirection;

        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        currentTowerHp = towerWeapon.GetComponent<EnemyHP>();
        gameObject.SetActive(true);
        UpdateTowerData();
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }

    public void OffPanel() //타워 정보창을 끔
    {
        /*카메라 원상 복구*/
        cameraTransform = cameraLocate;
        mainCamera.transform.position = cameraTransform;

        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
        ObjectDetector.touchScreen = false;
    }

    void UpdateTowerData() //타워의 정보들을 띄움
    {
        imageTower.sprite = currentTower.TowerSprite;

        if (currentTower.UnitType == UnitTypeData.Object)
        {
            textUpgradeCost.text = string.Format("{0:n}", "강화 불가능");
            textSell.text = string.Format("{0:n}", "제거하기");

            textDamage.text = "";
            textRate.text = "";
            textRange.text = "";
            textHp.text = "";

            textDamageUpgrade.text = "";
            textRateUpgrade.text = "";
            textRangeUpgrade.text = "";
            textHpUpgrade.text = "제거 비용 : $50";

            for (int i = 0; i < levelimage.Length; i++)
            {
                levelimage[i].SetActive(false);
            }

            for (int i = 0; i < uiSprite.Length; i++)
            {
                uiSprite[i].SetActive(false);
            }

            textSkill.SetActive(false);
        }
        else
        {

            //영웅
            /*
             * 영웅의 경우 레벨 대신 별을 표시한다
             * 영웅은 강화를 못하는 대신 아이템에 따라 능력치가 변동되므로 해당 값을 표시한다
             * 영웅은 유닛과 달리 스킬이 존재하므로 스킬 효과를 명시한다
             */
            if (currentTower.UnitType == UnitTypeData.Hero)
            {
                for (int i = 0; i < uiSprite.Length; i++)
                {
                    if (i < 4)
                        uiSprite[i].SetActive(true);
                    else
                        uiSprite[i].SetActive(false);
                }

                //스프라이트,데미지,공격속도,사정거리,체력
                if (currentTower.AddedDamage > 0)
                    textDamage.text = string.Format("{0:N1}", currentTower.Damage + "+" + "<color=red>" + currentTower.AddedDamage.ToString("F1") + "</color>");
                else
                {
                    if (currentTower.Damage + inven.ItemDamage < 0.05f)
                        textDamage.text = string.Format("{0:N1}", "0.05");
                    else
                        textDamage.text = string.Format("{0:N1}", currentTower.Damage + inven.ItemDamage);
                }
                textRate.text = string.Format("{0:N1}", currentTower.Rate + inven.ItemRate);
                textRange.text = string.Format("{0:N1}", currentTower.Range + inven.ItemRange);
                textHp.text = string.Format("{0:N1}", currentTowerHp.CurrentHP + "/" + currentTower.MaxHP);

                for (int i = 0; i < 5; i++)
                {
                    levelimage[i].GetComponent<Image>().sprite = icons[1];
                    levelimage[i].SetActive(true);
                }

                textSkill.SetActive(true);
                textSkillNotice.text = string.Format("{0:n1}", currentTower.Notice);

                if (currentTower.Name == "용과")
                {
                    textSkillName.text = string.Format("{0:n1}", "화룡참");
                    iconSkill.sprite = heroIconSkill[0];
                }
                else if (currentTower.Name == "백년초")
                {
                    textSkillName.text = string.Format("{0:n1}", "타우린");
                    iconSkill.sprite = heroIconSkill[1];
                }
                else if (currentTower.Name == "라플레시아")
                {
                    textSkillName.text = string.Format("{0:n1}", "독성 안개");
                    iconSkill.sprite = heroIconSkill[2];
                }
                else if (currentTower.Name == "맹그로브")
                {
                    textSkillName.text = string.Format("{0:n1}", "뿌리 채찍");
                    iconSkill.sprite = heroIconSkill[3];
                }
                else if (currentTower.Name == "사과")
                {
                    textSkillName.text = string.Format("{0:n1}", "홍옥의 파문");
                    iconSkill.sprite = heroIconSkill[4];
                }
                else if (currentTower.Name == "두리안")
                {
                    textSkillName.text = string.Format("{0:n1}", "두리안 펀치");
                    iconSkill.sprite = heroIconSkill[5];
                }

                textDamageUpgrade.text = string.Format("{0:n1}", "");
                textRateUpgrade.text = string.Format("{0:n1}", "");
                textRangeUpgrade.text = string.Format("{0:n1}","");
                textHpUpgrade.text = string.Format("{0:n1}", "");

                textUpgradeCost.text = string.Format("{0:n}", "강화 불가");
                textSell.text = string.Format("{0:n}", "영웅 복귀");
            }
            //유닛
            /*
             * 유닛의 경우 레벨에 따라 아이콘을 표시한다
             * 유닛은 상태창에 다음 레벨의 능력치를 같이 표시해준다
             * 레벨이 최대치일시 표시하지 않는다
             */
            else
            {
                for (int i = 0; i < uiSprite.Length; i++)
                {
                    uiSprite[i].SetActive(true);
                }

                //스프라이트,데미지,공격속도,사정거리,체력
                if (currentTower.AddedDamage > 0)
                    textDamage.text = string.Format("{0:N1}", currentTower.Damage + "+" + "<color=red>" + currentTower.AddedDamage.ToString("F1") + "</color>");
                else
                    textDamage.text = string.Format("{0:N1}", currentTower.Damage);
                textRate.text = string.Format("{0:N1}", currentTower.Rate);
                textRange.text = string.Format("{0:N1}", currentTower.Range);
                textHp.text = string.Format("{0:N1}", currentTowerHp.CurrentHP + "/" + currentTower.MaxHP);

                //레벨에 따라 아이콘 활성화
                for (int i = 0; i < currentTower.Level + 1; i++)
                {
                    levelimage[i].GetComponent<Image>().sprite = icons[0];
                    levelimage[i].SetActive(true);
                }

                //도달하지 않은 레벨 아이콘은 비활성화
                for (int i = currentTower.Level + 1; i < 5; i++)
                {
                    levelimage[i].SetActive(false);
                }

                textSkill.SetActive(false);

                //다음 레벨 능력치 변동값 표시
                if (currentTower.Level != 4)
                {
                    int level = currentTower.Level;
                    textDamageUpgrade.text = string.Format("{0:N1}", currentTower.Damage + "→" + "<color=green>" + currentTower.TowerTemplate.weapon[level + 1].damage.ToString("F1") + "</color>");
                    textRateUpgrade.text = string.Format("{0:N1}", currentTower.Rate + "→" + "<color=green>" + currentTower.TowerTemplate.weapon[level + 1].rate.ToString("F1") + "</color>");
                    textRangeUpgrade.text = string.Format("{0:N1}", currentTower.Range + "→" + "<color=green>" + currentTower.TowerTemplate.weapon[level + 1].range.ToString("F1") + "</color>");
                    textHpUpgrade.text = string.Format("{0:N1}", currentTower.MaxHP + "→" + "<color=green>" + currentTower.TowerTemplate.weapon[level + 1].maxHp.ToString("F1") + "</color>");
                    textUpgradeCost.text = string.Format("{0:N1}", "강화하기 $" + currentTower.TowerTemplate.weapon[level + 1].cost.ToString("F1"));
                }
                //레벨 최대치일시
                else
                {
                    textDamageUpgrade.text = string.Format("{0:N1}", "<color=green>" + currentTower.Damage + "(최대치)" + "</color>");
                    textRateUpgrade.text = string.Format("{0:N1}", "<color=green>" + currentTower.Rate + "(최대치)" + "</color>");
                    textRangeUpgrade.text = string.Format("{0:N1}", "<color=green>" + currentTower.Range + "(최대치)" + "</color>");
                    textHpUpgrade.text = string.Format("{0:N1}", "<color=green>" + currentTower.MaxHP + "(최대치)" + "</color>");
                    textUpgradeCost.text = string.Format("{0:N}", "강화 완료");
                }
                textSell.text = string.Format("{0:n}", "유닛 판매");
            }
        }

        textName.text = currentTower.Name;

        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false; //최대 레벨이 되면 버튼 비활성화
    }

    public void OnClickEventTowerUpgrade() //업그레이드 버튼 클릭 시 타워 업그레이드
    {
        int isSuccess = currentTower.Upgrade();

        if (isSuccess == 1)
        {
            UpdateTowerData();
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else if (isSuccess == 0)
        {
            systemTextViewer.PrintText(SystemType.Money);
        }
        else if (isSuccess == 2)
        {
            systemTextViewer.PrintText(SystemType.Limit);
        }
    }

    public void OnClickEventTowerSell() //판매 버튼 클릭 시 타워 판매
    {
        currentTower.Sell();
        OffPanel();
    }
}
