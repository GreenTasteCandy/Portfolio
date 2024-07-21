using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Move�� ĳ������ ���� �� �̵�,��ų �ý��� �Դϴ�

public class Move : MonoBehaviour
{

    //���� �ʱ�ȭ �κ�

    //���� ����
    public static int score = 0;

    //�̵� Ƚ��
    public int moveCount = 0;

    //�̵� ���ѽð�
    public float moveTime = 180.0f;
    public float moveTimeMax = 180.0f;

    //���� ���� ����
    public float JumpPoint = 0;
    public bool JumpOn = false;
    bool JumpOn2 = false;

    //�̵� ���� ���� ����
    public int leftOn = 0;
    public int rightOn = 0;
    public int jumpBt = 0;

    //�̵� ���� �� �浹 ����
    public bool moving = false;
    public bool crashed = false;
    int movePoint = 0;
    public static bool startHold = false;

    //��ų ���� ����
    public int SkillNum = 0;
    public float skillGauge = 0;
    public int skillPoint = 0;

    float plusTime;
    int JumpPlus;

    public bool rotate = false;

    //SE
    public AudioSource SEset;
    public AudioSource SEhit;

    //����
    public GameObject MainLight;
    public GameObject SubLight;

    //��ų : ��ȣ��
    public GameObject shieldEffect;
    public static bool shieldCheck = false;

    //��ų ����Ʈ
    public GameObject[] PsSkill;

    //ĳ���� ���͸���
    public Mesh[] PlayerMesh;
    public Material[] Characters;
    public Material[] Characters2;
    public Material[] Characters3;
    public Material[] Characters4;

    string log;

    private void Start()
    {
        startHold = false;

        //���� ��� Ȯ��
        /*
        0.Ŭ���� ���
        1.ĳ�־� ���
        2.ç���� - Ÿ�Ӿ���
        3.ç���� - �� ��å
        4.ç���� - ���� ����Ʈ
        */

        if (StartGame.seletMode == 1)
        {
            SkillNum = StartGame.seletSkill;
        }
        else if (StartGame.seletMode == 2)
        {
            MainLight.GetComponent<Light>().color = new Color(1, 0.6f, 0.3f);
            MainLight.transform.rotation = Quaternion.Euler(150, 0, 0);
            moveTime = 4000.0f;
            moveTimeMax = 4000.0f;
        }
        else if (StartGame.seletMode == 3)
        {
            MainLight.transform.rotation = Quaternion.Euler(270, 0, 0);
            RenderSettings.fogDensity = 0.04f;
            SubLight.GetComponent<Light>().enabled = true;
        }
        else if (StartGame.seletMode == 4)
        {
            MainLight.transform.rotation = Quaternion.Euler(45, 0, 0);
            MainLight.GetComponent<Light>().color = new Color(0.4f, 0.6f, 1);
            moveTime = 270.0f;
            moveTimeMax = 270.0f;
        }

        //ĳ���� ���� Ȯ��
        gameObject.GetComponent<MeshFilter>().mesh = PlayerMesh[StartGame.CharMesh];

        if (StartGame.CharMesh == 0)
            gameObject.GetComponent<MeshRenderer>().material = Characters[StartGame.CharSkin];

        else if (StartGame.CharMesh == 1)
            gameObject.GetComponent<MeshRenderer>().material = Characters2[StartGame.CharSkin];

        else if (StartGame.CharMesh == 2)
            gameObject.GetComponent<MeshRenderer>().material = Characters3[StartGame.CharSkin];

        else if (StartGame.CharMesh == 3)
            gameObject.GetComponent<MeshRenderer>().material = Characters4[StartGame.CharSkin];

        //���� �ʱ�ȭ
        score = 0;

        //�÷��̾� ȸ��
        transform.rotation = Quaternion.Euler(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359));
    }

    private void Update()
    {

        //���� ���� Ȯ��
        if (GameSystem.GameOn)
        {

            //���ѽð� ����
            if (startHold)
                moveTime -= (Time.deltaTime * 60) + plusTime;

            //���ѽð� �ʰ��� ���
            if (moveTime <= 0)
            {
                StartGame.AchievePoint[12] += 1;
                AchieveDead();
                GameSystem.GameSet = false;
            }

            //������ ���� Ȯ��
            if (JumpPoint >= 6 && JumpOn == false)
            {
                JumpOn = true;
            }

            //�̵� �� ����
            if (leftOn == 1)
            {
                MoveLeft();
                leftOn = 0;
            }
            else if (rightOn == 1)
            {
                MoveRight();
                rightOn = 0;
            }
            else if (jumpBt == 1)
            {
                MoveJump();
                jumpBt = 0;
            }

            //�׽�Ʈ�� ����
            if (Input.GetButtonDown("Left") && movePoint < 2)
                MoveLeft();
            else if (Input.GetButtonDown("Right") && movePoint > -2)
                MoveRight();
            else if (Input.GetButtonDown("Space"))
                MoveJump();

            if (Input.GetButtonDown("Fire2"))
                UseSkill();

            //���� �۵���
            if (JumpOn2 == true)
            {
                StartCoroutine(MoveBlock(new Vector3(0, -4, 0), 0.1f));
                JumpOn2 = false;

                //ĳ�־� ��ų 2�� : ��������
                if (StartGame.seletMode == 1 && SkillNum == 1)
                {
                    if (skillPoint == 1)
                    {
                        skillPoint = 0;
                    }
                    else
                    {
                        JumpPlus = (int)Random.Range(0, 3);
                        if (JumpPlus == 1 && skillPoint == 0)
                        {
                            EffectPlay(1);
                            JumpPoint = 6;
                            skillPoint = 1;
                        }
                    }
                }
            }
        }
    }

    //��ֹ� �浹 ����
    void OnTriggerEnter(Collider target)
    {
        //ȿ���� ����
        EffectPlay(4);
        SEhit.volume = StartGame.SeSet;
        SEhit.Play();

        if (target.gameObject.tag == "SkillObj")
        {
            EffectPlay(0);
            score += 3;
        }
        else if (target.gameObject.tag == "Wall")
        {
            if (StartGame.seletMode == 1)
            {
                if (SkillNum == 2 && skillPoint >= 1)
                {
                    shieldCheck = false;
                    skillPoint = 0;
                }
                else
                {
                    StartGame.AchievePoint[12] += 1;
                    AchieveDead();
                    GameSystem.GameSet = false;
                }
            }
            //ç������� : Ÿ�Ӿ��� �浹 ó��
            else if (StartGame.seletMode == 2)
            {
                moveTime -= 105;
            }
            //ç������� : ��������Ʈ ó��
            else if (StartGame.seletMode == 4)
            {
                moveTime += 45;
            }
            //�⺻ �浹 ó��
            else
            {
                StartGame.AchievePoint[12] += 1;
                AchieveDead();
                GameSystem.GameSet = false;
            }
        }
        Destroy(target.gameObject);
    }

    //���� �̵�
    public void MoveLeft()
    {
        if (moving == false)
        {
            if (StartGame.seletMode == 5)
            {
                if (movePoint > -2)
                {
                    SEset.volume = StartGame.SeSet;
                    SEset.Play();

                    if (JumpOn == false)
                        JumpPoint += 1;

                    if (skillPoint == 0)
                        skillGauge += 1;

                    movePoint -= 1;
                    StartCoroutine(MoveBlock(new Vector3(2, 0, 0), 0.1f));
                }
            }
            //���� ���� �̵�
            else
            {
                if (movePoint < 2)
                {
                    SEset.volume = StartGame.SeSet;
                    SEset.Play();

                    if (JumpOn == false)
                        JumpPoint += 1;

                    if (skillPoint == 0)
                        skillGauge += 1;

                    movePoint += 1;
                    StartCoroutine(MoveBlock(new Vector3(-2, 0, 0), 0.1f));
                }
            }
        }
    }

    //������ �̵�
    public void MoveRight()
    {
        if (moving == false)
        {
            if (StartGame.seletMode == 5)
            {
                if (movePoint < 2)
                {
                    SEset.volume = StartGame.SeSet;
                    SEset.Play();

                    if (JumpOn == false)
                        JumpPoint += 1;

                    if (skillPoint == 0)
                        skillGauge += 1;

                    movePoint += 1;
                    StartCoroutine(MoveBlock(new Vector3(-2, 0, 0), 0.1f));
                }
            }
            //���� ������ �̵�
            else
            {
                if (movePoint > -2)
                {
                    SEset.volume = StartGame.SeSet;
                    SEset.Play();

                    if (JumpOn == false)
                        JumpPoint += 1;

                    if (skillPoint == 0)
                        skillGauge += 1;

                    movePoint -= 1;
                    StartCoroutine(MoveBlock(new Vector3(2, 0, 0), 0.1f));
                }
            }
        }
    }

    //���� ��ư
    public void MoveJump()
    {
        if (moving == false && JumpOn == true)
        {
            SEset.volume = StartGame.SeSet;
            SEset.Play();

            StartCoroutine(MoveBlockJump(new Vector3(0, 4, 0), 0.1f));
            JumpPoint = 0;
            JumpOn = false;
        }
    }

    //ĳ�־� ��� : ��ų ���
    public void UseSkill()
    {
        if (moving == false && skillPoint == 0)
        {
            //3�� ��ų : ��ȣ��
            if (SkillNum == 2 && skillGauge >= 15 && shieldCheck == false)
            {
                EffectPlay(2);
                SEset.volume = StartGame.SeSet;
                SEset.Play();

                GameObject effect1 = Instantiate(shieldEffect);
                effect1.transform.position = transform.position;
                skillPoint = 5;
                skillGauge = 0;
                shieldCheck = true;
            }

            //4�� ��ų : ��üȭ
            if (SkillNum == 3 && skillGauge >= 12)
            {
                EffectPlay(3);
                SEset.volume = StartGame.SeSet;
                SEset.Play();

                gameObject.GetComponent<SphereCollider>().enabled = false;
                StartCoroutine(MoveBlock(new Vector3(0, 0, 0), 0.1f));
                skillGauge = 0;
            }
        }
    }

    //�÷��̾� �̵���
    public void MoveOn()
    {
        startHold = true;

        //Ÿ�Ӿ���,��������Ʈ �ƴ��� Ȯ��
        if (StartGame.seletMode != 2 || StartGame.seletMode != 4)
        {
            //���ѽð� ���� ����
            if (plusTime < 25.0f)
                plusTime += 0.002f;

            //���ѽð� ȸ��
            moveTime += 30.0f;

            //�߰� ����
            if (moveTime >= 170.0f)
                score += 1;
        }

        //���ѽð� �ִ�ġ �ʰ���
        if (moveTime > moveTimeMax)
            moveTime = moveTimeMax;

        //��ų : ��ȣ�� �ߵ��� ��ȣ�� ����Ƚ�� üũ
        if (SkillNum == 2 && skillPoint >= 1)
        {
            skillPoint -= 1;
            if (skillPoint <= 0)
                shieldCheck = false;
        }

        //���� �߰� �� �̵�Ƚ�� �߰�
        moveCount += 1;
        score += 1;
    }

    public void EffectPlay(int index)
    {
        GameObject particle = Instantiate(PsSkill[index]);
        particle.transform.position = transform.position;
        Destroy(particle, 1);
    }

    public void AchieveDead()
    {
        if (StartGame.AchievePoint[12] >= 10)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_34, success => log = $"{success}");
        if (StartGame.AchievePoint[12] >= 100)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_35, success => log = $"{success}");
        if (StartGame.AchievePoint[12] >= 1000)
            GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_36, success => log = $"{success}");
    }

    //�÷��̾� ���� �� �̵���� �ý���
    IEnumerator MoveBlock(Vector3 dir,float delay)
    {
        MoveOn();

        moving = true;

        float elapsedTime = 0.0f;
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = transform.position + dir;
        Quaternion currentRotation = transform.rotation;
        Quaternion plusRotation = Quaternion.Euler(75, 75, 45);

        while (elapsedTime < delay)
        {
            transform.rotation = Quaternion.Lerp(currentRotation, currentRotation * plusRotation, elapsedTime / delay);
            transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / delay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = currentRotation * plusRotation;

        if (gameObject.GetComponent<SphereCollider>().enabled == false)
        {
            moveCount += 1;
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }

        moving = false;
    }

    //������ ���� ��� �ý���
    IEnumerator MoveBlockJump(Vector3 dir, float delay)
    {
        MoveOn();

        moving = true;

        float elapsedTime = 0.0f;
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = transform.position + dir;
        Quaternion currentRotation = transform.rotation;
        Quaternion plusRotation = Quaternion.Euler(180, 180, 90);

        while (elapsedTime < delay)
        {
            transform.rotation = Quaternion.Lerp(currentRotation, currentRotation * plusRotation, elapsedTime / delay);
            transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime / delay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = currentRotation * plusRotation;

        moving = false;
        JumpOn2 = true;
    }

}