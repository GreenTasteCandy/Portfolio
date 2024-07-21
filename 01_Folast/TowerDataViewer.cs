using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*Ÿ�� ����â���� Ÿ���� �������� ���� �ϴ� ��ũ��Ʈ*/
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
    /*������ Ÿ�� ������ ī�޶� Ȯ���Ű�� ����*/
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

    public void OnPanel(Transform towerWeapon) //Ÿ�� ����â�� ���
    {
        /*������ Ÿ�� ������ ī�޶� Ȯ��*/
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

    public void OffPanel() //Ÿ�� ����â�� ��
    {
        /*ī�޶� ���� ����*/
        cameraTransform = cameraLocate;
        mainCamera.transform.position = cameraTransform;

        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
        ObjectDetector.touchScreen = false;
    }

    void UpdateTowerData() //Ÿ���� �������� ���
    {
        imageTower.sprite = currentTower.TowerSprite;

        if (currentTower.UnitType == UnitTypeData.Object)
        {
            textUpgradeCost.text = string.Format("{0:n}", "��ȭ �Ұ���");
            textSell.text = string.Format("{0:n}", "�����ϱ�");

            textDamage.text = "";
            textRate.text = "";
            textRange.text = "";
            textHp.text = "";

            textDamageUpgrade.text = "";
            textRateUpgrade.text = "";
            textRangeUpgrade.text = "";
            textHpUpgrade.text = "���� ��� : $50";

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

            //����
            /*
             * ������ ��� ���� ��� ���� ǥ���Ѵ�
             * ������ ��ȭ�� ���ϴ� ��� �����ۿ� ���� �ɷ�ġ�� �����ǹǷ� �ش� ���� ǥ���Ѵ�
             * ������ ���ְ� �޸� ��ų�� �����ϹǷ� ��ų ȿ���� ����Ѵ�
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

                //��������Ʈ,������,���ݼӵ�,�����Ÿ�,ü��
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

                if (currentTower.Name == "���")
                {
                    textSkillName.text = string.Format("{0:n1}", "ȭ����");
                    iconSkill.sprite = heroIconSkill[0];
                }
                else if (currentTower.Name == "�����")
                {
                    textSkillName.text = string.Format("{0:n1}", "Ÿ�츰");
                    iconSkill.sprite = heroIconSkill[1];
                }
                else if (currentTower.Name == "���÷��þ�")
                {
                    textSkillName.text = string.Format("{0:n1}", "���� �Ȱ�");
                    iconSkill.sprite = heroIconSkill[2];
                }
                else if (currentTower.Name == "�ͱ׷κ�")
                {
                    textSkillName.text = string.Format("{0:n1}", "�Ѹ� ä��");
                    iconSkill.sprite = heroIconSkill[3];
                }
                else if (currentTower.Name == "���")
                {
                    textSkillName.text = string.Format("{0:n1}", "ȫ���� �Ĺ�");
                    iconSkill.sprite = heroIconSkill[4];
                }
                else if (currentTower.Name == "�θ���")
                {
                    textSkillName.text = string.Format("{0:n1}", "�θ��� ��ġ");
                    iconSkill.sprite = heroIconSkill[5];
                }

                textDamageUpgrade.text = string.Format("{0:n1}", "");
                textRateUpgrade.text = string.Format("{0:n1}", "");
                textRangeUpgrade.text = string.Format("{0:n1}","");
                textHpUpgrade.text = string.Format("{0:n1}", "");

                textUpgradeCost.text = string.Format("{0:n}", "��ȭ �Ұ�");
                textSell.text = string.Format("{0:n}", "���� ����");
            }
            //����
            /*
             * ������ ��� ������ ���� �������� ǥ���Ѵ�
             * ������ ����â�� ���� ������ �ɷ�ġ�� ���� ǥ�����ش�
             * ������ �ִ�ġ�Ͻ� ǥ������ �ʴ´�
             */
            else
            {
                for (int i = 0; i < uiSprite.Length; i++)
                {
                    uiSprite[i].SetActive(true);
                }

                //��������Ʈ,������,���ݼӵ�,�����Ÿ�,ü��
                if (currentTower.AddedDamage > 0)
                    textDamage.text = string.Format("{0:N1}", currentTower.Damage + "+" + "<color=red>" + currentTower.AddedDamage.ToString("F1") + "</color>");
                else
                    textDamage.text = string.Format("{0:N1}", currentTower.Damage);
                textRate.text = string.Format("{0:N1}", currentTower.Rate);
                textRange.text = string.Format("{0:N1}", currentTower.Range);
                textHp.text = string.Format("{0:N1}", currentTowerHp.CurrentHP + "/" + currentTower.MaxHP);

                //������ ���� ������ Ȱ��ȭ
                for (int i = 0; i < currentTower.Level + 1; i++)
                {
                    levelimage[i].GetComponent<Image>().sprite = icons[0];
                    levelimage[i].SetActive(true);
                }

                //�������� ���� ���� �������� ��Ȱ��ȭ
                for (int i = currentTower.Level + 1; i < 5; i++)
                {
                    levelimage[i].SetActive(false);
                }

                textSkill.SetActive(false);

                //���� ���� �ɷ�ġ ������ ǥ��
                if (currentTower.Level != 4)
                {
                    int level = currentTower.Level;
                    textDamageUpgrade.text = string.Format("{0:N1}", currentTower.Damage + "��" + "<color=green>" + currentTower.TowerTemplate.weapon[level + 1].damage.ToString("F1") + "</color>");
                    textRateUpgrade.text = string.Format("{0:N1}", currentTower.Rate + "��" + "<color=green>" + currentTower.TowerTemplate.weapon[level + 1].rate.ToString("F1") + "</color>");
                    textRangeUpgrade.text = string.Format("{0:N1}", currentTower.Range + "��" + "<color=green>" + currentTower.TowerTemplate.weapon[level + 1].range.ToString("F1") + "</color>");
                    textHpUpgrade.text = string.Format("{0:N1}", currentTower.MaxHP + "��" + "<color=green>" + currentTower.TowerTemplate.weapon[level + 1].maxHp.ToString("F1") + "</color>");
                    textUpgradeCost.text = string.Format("{0:N1}", "��ȭ�ϱ� $" + currentTower.TowerTemplate.weapon[level + 1].cost.ToString("F1"));
                }
                //���� �ִ�ġ�Ͻ�
                else
                {
                    textDamageUpgrade.text = string.Format("{0:N1}", "<color=green>" + currentTower.Damage + "(�ִ�ġ)" + "</color>");
                    textRateUpgrade.text = string.Format("{0:N1}", "<color=green>" + currentTower.Rate + "(�ִ�ġ)" + "</color>");
                    textRangeUpgrade.text = string.Format("{0:N1}", "<color=green>" + currentTower.Range + "(�ִ�ġ)" + "</color>");
                    textHpUpgrade.text = string.Format("{0:N1}", "<color=green>" + currentTower.MaxHP + "(�ִ�ġ)" + "</color>");
                    textUpgradeCost.text = string.Format("{0:N}", "��ȭ �Ϸ�");
                }
                textSell.text = string.Format("{0:n}", "���� �Ǹ�");
            }
        }

        textName.text = currentTower.Name;

        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false; //�ִ� ������ �Ǹ� ��ư ��Ȱ��ȭ
    }

    public void OnClickEventTowerUpgrade() //���׷��̵� ��ư Ŭ�� �� Ÿ�� ���׷��̵�
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

    public void OnClickEventTowerSell() //�Ǹ� ��ư Ŭ�� �� Ÿ�� �Ǹ�
    {
        currentTower.Sell();
        OffPanel();
    }
}
