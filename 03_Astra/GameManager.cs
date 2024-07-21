using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using StarterAssets;
using TMPro;
using SickscoreGames.HUDNavigationSystem;

/*
 * GameManager
 * 게임의 기본적인 시스템 및 어쩌구를 담당한다
 * 
 */

public class GameManager : MonoBehaviour
{
    public static GameManager ins;

    //플레이어 능력치
    [Header("Players")]
    public GameObject player;
    public float health;
    public float maxHealth;
    public float damage;
    public float speed;
    public float range;
    public float rate;
    public float dex;
    public float luck;
    public float stamina;

    //hud ui : gauge
    [Header("UI Gauge")]
    public Slider hpGauge;
    public Slider staminaGauge;
    public Slider gatherGauge;
    public Slider enemyHPGauge;
    public TextMeshProUGUI enemyName;
    public Image skillGauge;

    //hud ui : system
    [Header("UI System")]
    public GameObject gathering;
    public GameObject inventoryUI;
    public GameObject waterView;
    public GameObject crosshair;
    public HUDNavigationSystem hudNav;

    //카메라 모드별 뷰
    [Header("Camera View")]
    public bool isTPS;
    public UIVirtualJoystick targetStick;
    public GameObject tps;
    public GameObject topview;

    //인게임 설정
    [Header("Game Setting")]
    public Light light;
    public Inventory inven;
    public PoolManager poolManager;
    public bool isAtivite;
    public Material[] skyMat;
    public GameObject[] skyLight;
    public Transform[] playerStart;
    public UniversalRendererData urd;
    public GameObject loopShip;

    //데이터 테이블들
    [Header("DataTable")]
    public ItemData itemTable;
    public EnemyData enemyTable;
    public WeaponData weaponTable;

    public itemStruct getitem;
    public bool isWork;
    public bool isUseSkill;

    private float enemyGaugeTime;
    private float gatherValue;
    private bool isInven;

    private void Awake()
    {
        ins = this;

        int pos = Random.Range(0, playerStart.Length);
        loopShip.transform.position = playerStart[pos].position;
        player.transform.position = playerStart[pos].position;
    }

    private void Start()
    {
        int day = Random.Range(0, skyMat.Length);
        RenderSettings.skybox = skyMat[day];
        skyLight[day].SetActive(true);

        int weaponLv = GameSetting.ins.player.user.weaponLv[0] + 1;
        float weaponDmg = weaponTable.weapon[0].dmgIncrease * weaponLv;
        float weaponRate = weaponTable.weapon[0].rateIncrease * weaponLv;
        float weaponRange = weaponTable.weapon[0].rangeIncreace * weaponLv;

        luck = GameSetting.ins.player.user.helmetLv * 0.3f;
        maxHealth += GameSetting.ins.player.user.armorLv * 10;
        speed = GameSetting.ins.player.user.bootLv * 0.3f;
        dex = GameSetting.ins.player.user.gloveLv * 0.2f;

        health = maxHealth;
        damage = weaponTable.weapon[0].damage + weaponDmg;
        range = weaponTable.weapon[0].range + weaponRate;
        rate = weaponTable.weapon[0].rate + weaponRange;

        hudNav.EnableMinimap(GameSetting.isMinimap);
        hudNav.EnableCompassBar(GameSetting.isCompass);
    }

    private void Update()
    {
        if (!isWork)
        { 
            if (stamina < 100f)
                stamina += Time.deltaTime;
        }
        else
        {

            gatherValue += Time.deltaTime;
            stamina -= Time.deltaTime;
            gatherGauge.value = gatherValue / (10f - dex);

            if (gatherValue >= 10f - dex)
            {
                player.GetComponent<PlayerActing>().Gathering();
                GatherOff();
            }

        }
    }

    private void LateUpdate()
    {
        hpGauge.value = health / maxHealth;
        staminaGauge.value = stamina / 100;
    }

    private IEnumerator ShowEnemyGauge()
    {
        enemyHPGauge.gameObject.SetActive(true);

        while (enemyGaugeTime > 0)
        {
            enemyGaugeTime -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        enemyHPGauge.gameObject.SetActive(false);
    }

    private IEnumerator CoolDownSkill()
    {
        float delay = 0f;
        int num = player.GetComponent<PlayerActing>().weaponNum;

        while (delay < weaponTable.weapon[num].skillCooldown)
        {
            skillGauge.fillAmount = (weaponTable.weapon[num].skillCooldown -  delay) / weaponTable.weapon[num].skillCooldown;
            delay += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        skillGauge.gameObject.SetActive(false);
        isUseSkill = false;
    }

    public void TPSViewSet()
    {
        ThirdPersonController playerView = player.GetComponent<ThirdPersonController>();

        playerView.LockCameraPosition = false;
        playerView.CinemachineCameraTarget = tps;

        playerView.BottomClamp = 0f;

        isTPS = true;
        targetStick.invertYOutputValue = true;
    }

    public void TopViewSet()
    {
        ThirdPersonController playerView = player.GetComponent<ThirdPersonController>();

        playerView.LockCameraPosition = true;
        playerView.CinemachineCameraTarget = topview;

        playerView.BottomClamp = 0f;

        isTPS = false;
        targetStick.invertYOutputValue = false;
    }

    public void GatherOff()
    {
        gatherValue = 0;
        gatherGauge.value = 0;
        gatherGauge.gameObject.SetActive(false);
        player.GetComponent<StarterAssetsInputs>().work = false;
        player.GetComponent<StarterAssetsInputs>().workCancel = false;
        isWork = false;
    }

    public void EnemyGaugeShow(string name, float hp)
    {
        enemyName.text = name;
        enemyHPGauge.value = hp;

        if (enemyGaugeTime <= 0f)
        {
            enemyGaugeTime = 5f;
            StartCoroutine(ShowEnemyGauge());
        }
        else
            enemyGaugeTime += 0.5f;
    }

    public void StartCoolDown()
    {
        skillGauge.gameObject.SetActive(true);
        isUseSkill = true;
        StartCoroutine(CoolDownSkill());
    }

    public void InvenOn()
    {
        if (isInven)
        {
            isInven = false;
        }
        else
        {
            isInven = true;
        }

        inventoryUI.SetActive(isInven);
    }

    public void FieldExit()
    {
        hudNav.EnableMinimap(false);
        hudNav.EnableCompassBar(false);

        inven.GetSupply();
        ScriptableRendererFeature srf = urd.rendererFeatures[0];
        srf.SetActive(false);

        LoadingSystem.MoveScene("Lobby");
    }

}
