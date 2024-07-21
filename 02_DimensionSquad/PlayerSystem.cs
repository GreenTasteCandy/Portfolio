using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class PlayerSystem : MonoBehaviourPun, IPunObservable
{
    [Header("# Player Systems")]
    public Rigidbody2D rb;
    public PhotonView pv;
    public AudioSource playerSE;
    public AudioSource atkSE;
    public TextMeshProUGUI nicknameText;
    public Animator anim;
    public FloatingJoystick joystick;
    public FixedJoystick joystick2;
    public PlayerStatus playerStatus;
    public GameObject aura;

    [Header("# Player Sound Effects")]
    public AudioClip[] soundEffect;

    [Header("# Charactor Systems")]
    public GameObject[] animPrefab;
    public GameObject[] animPrefab2;
    public GameObject[] animPrefab3;
    public GameObject[] animPrefab4;
    public RuntimeAnimatorController[] playerAnim;
    public RuntimeAnimatorController[] playerAnim2;
    public RuntimeAnimatorController[] playerAnim3;
    public RuntimeAnimatorController[] playerAnim4;

    public bool isLive = true;
    public float curHp;
    public float maxhp;
    public float curArmor;

    GameObject selectChar;
    float maxArmor;
    float curMp;
    float rate;
    Slider hpBar;

    Vector3 remotePos;
    Quaternion remoteRot;

    private void Start()
    {
        joystick = GameManager.ins.joystickMove;
        joystick2 = GameManager.ins.joystickAttack;

        nicknameText.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        nicknameText.color = pv.IsMine ? Color.green : Color.blue;

        if (pv.IsMine)
        {
            var CM = GameObject.Find("CMcamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
        }
    }

    public void Setup(int charType, int charNum, Slider hpbar)
    {
        playerStatus.charStat[0] = GameManager.ins.charStatus[charType].charStat[charNum];
        curHp = playerStatus.charStat[0].hp;
        maxhp = playerStatus.charStat[0].maxhp;
        curArmor = playerStatus.charStat[0].armor;
        maxArmor = playerStatus.charStat[0].armor;

        pv.RPC("SetAnim", RpcTarget.AllBuffered, charType, charNum);
        hpBar = hpbar;
    }

    public void AddItem(Status addstat)
    {
        playerStatus.charStat[0].hp += addstat.hp;
        playerStatus.charStat[0].maxhp += addstat.maxhp;
        playerStatus.charStat[0].mp += addstat.mp;
        playerStatus.charStat[0].armor += addstat.armor;
        playerStatus.charStat[0].atk += addstat.atk;
        playerStatus.charStat[0].def += addstat.def;
        playerStatus.charStat[0].atkNum += addstat.atkNum;
        playerStatus.charStat[0].atkRange += addstat.atkRange;
        playerStatus.charStat[0].atkRate += addstat.atkRate;
        playerStatus.charStat[0].skillRate += addstat.skillRate;
        playerStatus.charStat[0].speed += addstat.speed;
        playerStatus.charStat[0].shotSpeed += addstat.shotSpeed;

        if (playerStatus.charStat[0].speed <= 1f)
            playerStatus.charStat[0].speed = 1f;

        if (playerStatus.charStat[0].shotSpeed <= 1f)
            playerStatus.charStat[0].shotSpeed = 1f;

        if (playerStatus.charStat[0].atk < 0)
            playerStatus.charStat[0].atk = 0.1f;

        if (playerStatus.charStat[0].atkRange <= 1)
            playerStatus.charStat[0].atkRange = 1f;

        if (playerStatus.charStat[0].skillRate <= 1)
            playerStatus.charStat[0].skillRate = 1f;

        for (int i = 0; i < 5; i++)
        {
            playerStatus.charStat[0].auraDmg[i] += addstat.auraDmg[i];
        }

        if (playerStatus.charStat[0].auraDmg[0] > 0)
            aura.SetActive(true);

        curHp += addstat.hp;
        maxhp = playerStatus.charStat[0].maxhp;
        curArmor += playerStatus.charStat[0].armor;
    }

    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
            Vector2 dir = new Vector2(transform.position.x + joystick.Horizontal, transform.position.y + joystick.Vertical);

            if (input != Vector2.zero)
            {
                if (!playerSE.isPlaying)
                {
                    playerSE.clip = soundEffect[Random.Range(0, soundEffect.Length)];
                    playerSE.Play();
                }

                anim.SetBool("isMove", true);
            }
            else
                anim.SetBool("isMove", false);

            if (GameManager.ins.isBuffOn[0])
            {
                float speed = playerStatus.charStat[0].speed + playerStatus.charStat[0].skillSet.status[1].speed;
                transform.position = Vector2.MoveTowards(transform.position, dir, speed * Time.fixedDeltaTime);
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, dir, playerStatus.charStat[0].speed * Time.fixedDeltaTime);

            if (joystick2.input != Vector2.zero)
            {
                pv.RPC("FlipX", RpcTarget.AllBuffered, joystick2.Horizontal);
                if (GameManager.ins.isBuffOn[1])
                {
                    float atkRate = playerStatus.charStat[0].atkRate + playerStatus.charStat[0].skillSet.status[1].atkSpeed;
                    rate += Time.deltaTime * atkRate;
                }
                else
                    rate += Time.deltaTime * playerStatus.charStat[0].atkRate;

                if (rate > 1.0f)
                {
                    atkSE.clip = playerStatus.charStat[0].atkSE;
                    atkSE.Play();

                    anim.SetTrigger("isAttack");
                    for (int i = 0; i < playerStatus.charStat[0].atkNum; i++)
                    {
                        if (playerStatus.charStat[0].atkType == WeaponType.melee)
                        {
                            GameObject attack = PhotonNetwork.Instantiate(playerStatus.charStat[0].atkObj.name, rb.position + (joystick2.input.normalized * 1.5f), Quaternion.identity);
                            attack.GetComponent<BulletSystem>().Setup(playerStatus.charStat[0].atkType, joystick2.input.normalized, playerStatus);
                        }
                        else if (playerStatus.charStat[0].atkType == WeaponType.range)
                        {
                            GameObject bullet = PhotonNetwork.Instantiate(playerStatus.charStat[0].atkObj.name, rb.position, Quaternion.identity);
                            bullet.GetComponent<BulletSystem>().Setup(playerStatus.charStat[0].atkType, joystick2.input.normalized, playerStatus);
                        }
                    }

                    rate = 0;
                }
            }

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, remotePos, 10 * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, remoteRot, 10 * Time.fixedDeltaTime);
        }

    }

    public SkillData UseSkillActive()
    {
        SkillData skillData = playerStatus.charStat[0].skillSet;

        if (skillData.status[1].activeType == SkillActive.Buff)
        {
            GameObject hitbox = PhotonNetwork.Instantiate(skillData.status[1].skillobj[0].name, transform.position, Quaternion.identity);

            if (skillData.status[0].name == "¿Â≥≠≥¢ ∏π¿∫ º“≥‡" && skillData.status[0].isUse)
                skillData.status[0].isUse = false;
        }
        else
        {
            Vector2 dir = new Vector2(joystick.Horizontal, joystick.Vertical).normalized;

            if (dir == Vector2.zero)
                dir = Vector2.left;

            GameObject hitbox = PhotonNetwork.Instantiate(skillData.status[1].skillobj[0].name, transform.position, Quaternion.identity);
            hitbox.GetComponent<SkillSystem>().Setup(dir, 1, skillData);
        }

        return skillData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAtk"))
        {
            if (isLive)
            {
                GameObject atkEnemy = PhotonNetwork.Instantiate("BloodFX", transform.position, Quaternion.identity);
                GameObject hitFx = PhotonNetwork.Instantiate("Effect01", transform.position, Quaternion.identity);

                pv.RPC("TakeDamage", RpcTarget.AllBuffered, 1f);

                if (curHp <= 0)
                {
                    GameManager.ins.deadUI.SetActive(true);
                    GameManager.ins.mainUI.SetActive(false);
                    isLive = false;
                }
            }
        }
    }

    [PunRPC]
    void SetAnim(int charType, int charNum)
    {
        if (charType == 0)
        {
            animPrefab[charNum].SetActive(true);
            anim.runtimeAnimatorController = playerAnim[charNum];
            selectChar = animPrefab[charNum];
        }
        else if (charType == 1)
        {
            animPrefab2[charNum].SetActive(true);
            anim.runtimeAnimatorController = playerAnim2[charNum];
            selectChar = animPrefab2[charNum];
        }
        else if (charType == 2)
        {
            animPrefab3[charNum].SetActive(true);
            anim.runtimeAnimatorController = playerAnim3[charNum];
            selectChar = animPrefab3[charNum];
        }
        else if (charType == 3)
        {
            animPrefab4[charNum].SetActive(true);
            anim.runtimeAnimatorController = playerAnim4[charNum];
            selectChar = animPrefab4[charNum];
        }
    }


    [PunRPC]
    void FlipX(float axis)
    {
        if (axis < 0)
            selectChar.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else
            selectChar.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    [PunRPC]
    void TakeDamage(float dmg)
    {
        if (curArmor > 0)
        {
            curArmor -= dmg * 0.85f;
        }
        else
        {
            if (isLive)
            {
                curHp -= dmg;
            }
        }
    }

    public void HealPlayer(float heal)
    {
        pv.RPC("CharactorHeal", RpcTarget.AllBuffered, heal);
    }

    [PunRPC]
    void CharactorHeal(float heal)
    {
        GameObject healfx = PhotonNetwork.Instantiate("HealFX", transform.position, Quaternion.identity);

        curHp += heal;
        if (curHp > maxhp)
            curHp = maxhp;
    }

    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(curHp);
            stream.SendNext(maxhp);
            stream.SendNext(curArmor);
        }
        else
        {
            remotePos = (Vector3)stream.ReceiveNext();
            remoteRot = (Quaternion)stream.ReceiveNext();
            curHp = (float)stream.ReceiveNext();
            maxhp = (float)stream.ReceiveNext();
            curArmor = (float)stream.ReceiveNext();
        }
    }
}
