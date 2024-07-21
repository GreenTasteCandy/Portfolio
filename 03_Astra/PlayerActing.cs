using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using UnityEngine.UI;


/*
 * 플레이어의 공격,채집,기타 행동을 제어하는 스크립트이다
 * 기존의 ThirdPersonController에서 해당 기능들을 분리시킨것들이 있으며
 * 
 */

public class PlayerActing : MonoBehaviour
{
    [Header("Battle System")]
    public GameObject[] weaponMelee;
    public GameObject[] weaponHipEffect;
    public GameObject[] weaponSkillEffect;
    public GameObject[] weaponRange;
    public Transform[] aimPoint;
    public AnimatorOverrideController[] anims;
    public Transform skillEffectPos;
    public Sprite[] iconSkills;
    public Image skills;

    [Header("Gathering System")]
    public GameObject[] uiObject;
    public GameObject[] toolObject;
    public LayerMask gatherMask;
    public Collider[] obj;

    // animation IDs
    private int _animIDDance;
    private int _animIDisWork;
    private int _animIDisMining;
    private int _animIDisGathering;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    private PlayerInput _playerInput;
#endif
    private Animator _animator;
    private CharacterController _controller;
    private StarterAssetsInputs _input;
    private GameObject _mainCamera;
    private ThirdPersonController _tpsPlayer;

    private const float _threshold = 0.01f;

    private bool _hasAnimator;

    public Vector3 movePoint;

    private Vector3 LookPoint;
    public int weaponNum;
    private bool isHit;
    public bool isSlash;
    private bool isSwap;
    private bool isGather;

    private int objNum = -1;
    private bool isLive = true;

    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        _tpsPlayer = GetComponent<ThirdPersonController>();

        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        //애니메이션 파라미터를 받아오는 부분
        _animIDDance = Animator.StringToHash("Dance");
        _animIDisWork = Animator.StringToHash("isWork");
        _animIDisMining = Animator.StringToHash("isMining");
        _animIDisGathering = Animator.StringToHash("isGathering");
    }

    public IEnumerator SlashEffect(bool isSkill) //근접 공격,이펙트
    {
        isSlash = true;

        int num;
        Transform bullet;
        
        if (isSkill)
        {
            num = GameManager.ins.weaponTable.weapon[weaponNum].skillObj;
            bullet = GameManager.ins.poolManager.GetPool(num).transform;
            bullet.position = skillEffectPos.position;
        }
        else
        {
            num = 2;
            bullet = GameManager.ins.poolManager.GetPool(num).transform;
            bullet.position = skillEffectPos.position;
        }

        weaponHipEffect[0].SetActive(true);
        weaponHipEffect[1].SetActive(true);
        weaponHipEffect[0].GetComponent<SkillCollider>().skillOn = isSkill;
        weaponHipEffect[1].GetComponent<SkillCollider>().skillOn = isSkill;

        yield return new WaitForSeconds(1f / GameManager.ins.rate);

        weaponHipEffect[0].SetActive(false);
        weaponHipEffect[1].SetActive(false);
        weaponHipEffect[0].GetComponent<SkillCollider>().skillOn = false;
        weaponHipEffect[1].GetComponent<SkillCollider>().skillOn = false;

        isSlash = false;
    }

    public IEnumerator SlashEffect2(bool isSkill) //근접 공격,이펙트
    {
        isSlash = true;

        int num;
        Transform bullet;
        
        if (isSkill)
        {
            num = GameManager.ins.weaponTable.weapon[weaponNum].skillObj;
            bullet = GameManager.ins.poolManager.GetPool(num).transform;
            bullet.position = skillEffectPos.position;
        }
        else
        {
            num = 2;
            bullet = GameManager.ins.poolManager.GetPool(num).transform;
            bullet.position = skillEffectPos.position;
        }

        weaponHipEffect[2].SetActive(true);
        weaponHipEffect[2].GetComponent<SkillCollider>().skillOn = isSkill;
        yield return new WaitForSeconds(1f / GameManager.ins.rate);

        weaponHipEffect[2].SetActive(false);
        weaponHipEffect[2].GetComponent<SkillCollider>().skillOn = false;

        isSlash = false;
    }

    public IEnumerator ShotBullet(Vector3 rotate, bool isSkill) //원거리 공격
    {
        isSlash = true;
        Transform bullet = null;

        if (isSkill)
        {
            int num = GameManager.ins.weaponTable.weapon[weaponNum].skillObj;
            bullet = GameManager.ins.poolManager.GetPool(num).transform;
        }
        else
        {
            bullet = GameManager.ins.poolManager.GetPool(0).transform;
        }

        bullet.position = aimPoint[weaponNum == 1 ? 0 : 1].position;

        BulletSystem setup = bullet.GetComponent<BulletSystem>();
        Rigidbody rigid = bullet.GetComponent<Rigidbody>();

        setup.Setup(GameManager.ins.range);

        rigid.velocity = Quaternion.Euler(rotate) * Vector3.forward * 15f;

        yield return new WaitForSeconds(1f / GameManager.ins.rate);

        isSlash = false;
    }

    public void SwitchWeapon() //무기 변환
    {
        if (GameManager.ins.isUseSkill)
        {
            return;
        }

        if (_tpsPlayer.Grounded)
        {

            if (_hasAnimator && _input.swap)
            {
                if (!isSwap && !isHit)
                    StartCoroutine(SwapWeapon());
            }

        }
    }

    IEnumerator SwapWeapon() //무기 변환시 동작하는 코루틴
    {
        isSlash = false;
        isSwap = true;

        switch (weaponNum)
        {
            case 0:
                weaponMelee[0].SetActive(false);
                weaponMelee[1].SetActive(false);
                weaponMelee[2].SetActive(false);
                weaponRange[0].SetActive(true);
                weaponRange[1].SetActive(false);
                break;

            case 1:
                weaponMelee[0].SetActive(false);
                weaponMelee[1].SetActive(false);
                weaponMelee[2].SetActive(true);
                weaponRange[0].SetActive(false);
                weaponRange[1].SetActive(false);
                break;

            case 2:
                weaponMelee[0].SetActive(false);
                weaponMelee[1].SetActive(false);
                weaponMelee[2].SetActive(false);
                weaponRange[0].SetActive(false);
                weaponRange[1].SetActive(true);
                break;

            case 3:
                weaponMelee[0].SetActive(true);
                weaponMelee[1].SetActive(true);
                weaponMelee[2].SetActive(false);
                weaponRange[0].SetActive(false);
                weaponRange[1].SetActive(false);
                break;
        }

        weaponNum++;
        if (weaponNum >= 4)
            weaponNum = 0;

        skills.sprite = iconSkills[weaponNum];
        int weaponLv = GameSetting.ins.player.user.weaponLv[weaponNum] + 1;
        float weaponDmg = GameManager.ins.weaponTable.weapon[weaponNum].dmgIncrease * weaponLv;
        float weaponRate = GameManager.ins.weaponTable.weapon[weaponNum].rateIncrease * weaponLv;
        float Range = GameManager.ins.weaponTable.weapon[weaponNum].rangeIncreace * weaponLv;

        GameManager.ins.damage = GameManager.ins.weaponTable.weapon[weaponNum].damage + weaponDmg;
        GameManager.ins.range = GameManager.ins.weaponTable.weapon[weaponNum].range + weaponRate;
        GameManager.ins.rate = GameManager.ins.weaponTable.weapon[weaponNum].rate + Range;

        _animator.runtimeAnimatorController = anims[weaponNum];
        yield return new WaitForSecondsRealtime(0.2f);

        isSwap = false;
    }

    public void PlayDance() // /춤
    {
        if (_tpsPlayer.Grounded)
        {
            if (_input.dance)
            {
                StartCoroutine(Dancing());
            }
        }
    }

    IEnumerator Dancing()
    {
        _animator.SetBool(_animIDDance, true);
        float anim = _animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(anim);

        _animator.SetBool(_animIDDance, false);
    }

   
    public void GatheringWork() // 채집 시스템
    {
        if (GameManager.ins.stamina < 10 - GameManager.ins.dex)
            return;

        if (_tpsPlayer.Grounded)
        {
            if (_hasAnimator && _input.work)
            {
                if (!GameManager.ins.isWork)
                {
                    WorkStart();
                }

            }
        }
    }

    public void GatheringCancel() // 채집 시스템
    {
        if (_tpsPlayer.Grounded)
        {
            if (_hasAnimator && _input.workCancel)
            {

                if (GameManager.ins.isWork)
                {
                    WorkCancel();
                    GameManager.ins.GatherOff();
                }

            }
        }
    }

    private void WorkStart() //채집 모션
    {
        weaponMelee[0].SetActive(false);
        weaponMelee[1].SetActive(false);
        weaponMelee[2].SetActive(false);
        weaponRange[0].SetActive(false);

        obj = Physics.OverlapSphere(transform.position, 2.5f, gatherMask);
        float dis = Mathf.Infinity;
        objNum = -1;

        for (int i = 0; i < obj.Length; i++)
        {
            float distanse = Vector3.Distance(obj[i].transform.position, transform.position);

            if (distanse <= dis)
            {
                dis = distanse;
                objNum = i;
            }
        }

        if (objNum != -1)
        {
            if (obj[objNum].transform.CompareTag("Trees"))
            {
                transform.LookAt(obj[objNum].transform);
                toolObject[1].SetActive(true);
                _animator.SetBool(_animIDisWork, true);
                int rand = Random.Range(2, 6);

                GameManager.ins.getitem = obj[objNum].gameObject.GetComponent<Gathering>().GatherItem();
                GameManager.ins.getitem.count = rand;
            }
            else if (obj[objNum].transform.CompareTag("Rocks"))
            {
                transform.LookAt(obj[objNum].transform);
                toolObject[0].SetActive(true);
                _animator.SetBool(_animIDisMining, true);

                int rand = Random.Range(1, 6);

                GameManager.ins.getitem = obj[objNum].gameObject.GetComponent<Gathering>().GatherItem();
                GameManager.ins.getitem.count = rand;
            }
            else if (obj[objNum].transform.CompareTag("Plants"))
            {
                transform.LookAt(obj[objNum].transform);
                toolObject[0].SetActive(true);
                _animator.SetBool(_animIDisGathering, true);

                int rand = Random.Range(3, 11);

                GameManager.ins.getitem = obj[objNum].gameObject.GetComponent<Gathering>().GatherItem();
                GameManager.ins.getitem.count = rand;
            }
            else
            {
                GameManager.ins.GatherOff();
                WorkCancel();
                return;
            }
        }
        else
        {
            GameManager.ins.GatherOff();
            WorkCancel();
            return;
        }

        GameManager.ins.isWork = true;
        GameManager.ins.gatherGauge.gameObject.SetActive(true);
        GameManager.ins.isWork = true;
        uiObject[0].SetActive(false);
        uiObject[1].SetActive(true);
    }

    public void Gathering()
    {
        GameManager.ins.inven.GetItem(GameManager.ins.getitem);
        obj[objNum].gameObject.transform.SetParent(null);
        obj[objNum].gameObject.SetActive(false);

        WorkCancel();
    }

    private void WorkCancel()
    {
        toolObject[0].SetActive(false);
        toolObject[1].SetActive(false);
        _animator.SetBool(_animIDisWork, false);
        _animator.SetBool(_animIDisMining, false);
        _animator.SetBool(_animIDisGathering, false);

        switch (weaponNum)
        {
            case 0:
                weaponMelee[0].SetActive(true);
                weaponMelee[1].SetActive(true);
                weaponMelee[2].SetActive(false);
                weaponRange[0].SetActive(false);
                break;

            case 1:
                weaponMelee[0].SetActive(false);
                weaponMelee[1].SetActive(false);
                weaponMelee[2].SetActive(false);
                weaponRange[0].SetActive(true);
                break;

            case 2:
                weaponMelee[0].SetActive(false);
                weaponMelee[1].SetActive(false);
                weaponMelee[2].SetActive(true);
                weaponRange[0].SetActive(false);
                break;
        }

        uiObject[1].SetActive(false);
        uiObject[0].SetActive(true);

    }
}
