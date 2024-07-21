using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Move는 캐릭터의 조작 및 이동,스킬 시스템 입니다

public class Move : MonoBehaviour
{

    //변수 초기화 부분

    //게임 점수
    public static int score = 0;

    //이동 횟수
    public int moveCount = 0;

    //이동 제한시간
    public float moveTime = 180.0f;
    public float moveTimeMax = 180.0f;

    //점프 관련 변수
    public float JumpPoint = 0;
    public bool JumpOn = false;
    bool JumpOn2 = false;

    //이동 방향 관련 변수
    public int leftOn = 0;
    public int rightOn = 0;
    public int jumpBt = 0;

    //이동 감지 및 충돌 감지
    public bool moving = false;
    public bool crashed = false;
    int movePoint = 0;
    public static bool startHold = false;

    //스킬 관련 변수
    public int SkillNum = 0;
    public float skillGauge = 0;
    public int skillPoint = 0;

    float plusTime;
    int JumpPlus;

    public bool rotate = false;

    //SE
    public AudioSource SEset;
    public AudioSource SEhit;

    //조명
    public GameObject MainLight;
    public GameObject SubLight;

    //스킬 : 보호막
    public GameObject shieldEffect;
    public static bool shieldCheck = false;

    //스킬 이펙트
    public GameObject[] PsSkill;

    //캐릭터 머터리얼
    public Mesh[] PlayerMesh;
    public Material[] Characters;
    public Material[] Characters2;
    public Material[] Characters3;
    public Material[] Characters4;

    string log;

    private void Start()
    {
        startHold = false;

        //게임 모드 확인
        /*
        0.클래식 모드
        1.캐주얼 모드
        2.챌린지 - 타임어택
        3.챌린지 - 밤 산책
        4.챌린지 - 도그 파이트
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

        //캐릭터 외형 확인
        gameObject.GetComponent<MeshFilter>().mesh = PlayerMesh[StartGame.CharMesh];

        if (StartGame.CharMesh == 0)
            gameObject.GetComponent<MeshRenderer>().material = Characters[StartGame.CharSkin];

        else if (StartGame.CharMesh == 1)
            gameObject.GetComponent<MeshRenderer>().material = Characters2[StartGame.CharSkin];

        else if (StartGame.CharMesh == 2)
            gameObject.GetComponent<MeshRenderer>().material = Characters3[StartGame.CharSkin];

        else if (StartGame.CharMesh == 3)
            gameObject.GetComponent<MeshRenderer>().material = Characters4[StartGame.CharSkin];

        //점수 초기화
        score = 0;

        //플레이어 회전
        transform.rotation = Quaternion.Euler(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359));
    }

    private void Update()
    {

        //게임 정지 확인
        if (GameSystem.GameOn)
        {

            //제한시간 감소
            if (startHold)
                moveTime -= (Time.deltaTime * 60) + plusTime;

            //제한시간 초과시 사망
            if (moveTime <= 0)
            {
                StartGame.AchievePoint[12] += 1;
                AchieveDead();
                GameSystem.GameSet = false;
            }

            //점프시 조건 확인
            if (JumpPoint >= 6 && JumpOn == false)
            {
                JumpOn = true;
            }

            //이동 및 점프
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

            //테스트용 조작
            if (Input.GetButtonDown("Left") && movePoint < 2)
                MoveLeft();
            else if (Input.GetButtonDown("Right") && movePoint > -2)
                MoveRight();
            else if (Input.GetButtonDown("Space"))
                MoveJump();

            if (Input.GetButtonDown("Fire2"))
                UseSkill();

            //점프 작동시
            if (JumpOn2 == true)
            {
                StartCoroutine(MoveBlock(new Vector3(0, -4, 0), 0.1f));
                JumpOn2 = false;

                //캐주얼 스킬 2번 : 더블점프
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

    //장애물 충돌 감지
    void OnTriggerEnter(Collider target)
    {
        //효과음 실행
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
            //챌린지모드 : 타임어택 충돌 처리
            else if (StartGame.seletMode == 2)
            {
                moveTime -= 105;
            }
            //챌린지모드 : 도그파이트 처리
            else if (StartGame.seletMode == 4)
            {
                moveTime += 45;
            }
            //기본 충돌 처리
            else
            {
                StartGame.AchievePoint[12] += 1;
                AchieveDead();
                GameSystem.GameSet = false;
            }
        }
        Destroy(target.gameObject);
    }

    //왼쪽 이동
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
            //정상 왼쪽 이동
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

    //오른쪽 이동
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
            //정상 오른쪽 이동
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

    //점프 버튼
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

    //캐주얼 모드 : 스킬 사용
    public void UseSkill()
    {
        if (moving == false && skillPoint == 0)
        {
            //3번 스킬 : 보호막
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

            //4번 스킬 : 유체화
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

    //플레이어 이동시
    public void MoveOn()
    {
        startHold = true;

        //타임어택,도그파이트 아닌지 확인
        if (StartGame.seletMode != 2 || StartGame.seletMode != 4)
        {
            //제한시간 감소 증가
            if (plusTime < 25.0f)
                plusTime += 0.002f;

            //제한시간 회복
            moveTime += 30.0f;

            //추가 점수
            if (moveTime >= 170.0f)
                score += 1;
        }

        //제한시간 최대치 초과시
        if (moveTime > moveTimeMax)
            moveTime = moveTimeMax;

        //스킬 : 보호막 발동시 보호막 제한횟수 체크
        if (SkillNum == 2 && skillPoint >= 1)
        {
            skillPoint -= 1;
            if (skillPoint <= 0)
                shieldCheck = false;
        }

        //점수 추가 및 이동횟수 추가
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

    //플레이어 조작 시 이동모션 시스템
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

    //점프시 점프 모션 시스템
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