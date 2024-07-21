using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//오브젝트 타입
public enum TypeObject { Unit = 0, Enemy, Hero, Object }

/*체력을 설정하고 그 체력의 변동 상황과 맞았을 때 이펙트를 처리하는 스크립트
 *적 말고 유닛의 체력도 여기서 처리*/
public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    TypeObject type;
    [SerializeField]
    GameObject[] hitEffect;
    [SerializeField]
    ItemTemplate equip;

    float maxHP;
    float currentHP;
    bool isDie = false;

    Enemy enemy;
    TowerWeapon towerWeapon;
    SpriteRenderer[] spriteRenderers;

    GameObject soundEffecter;
    OptionMenu soundEffect;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        soundEffecter = GameObject.Find("OptionCheck");

        soundEffect = soundEffecter.GetComponent<OptionMenu>();

        //시작시 오브젝트의 HP,필요 컴포넌트들을 설정해준다

        if (type == TypeObject.Enemy)
        {
            //적은 Enemy 컴포넌트를 선언해준다
            enemy = GetComponent<Enemy>();
        }

        //랜더러 및 공격기능 컴포넌트 설정
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        towerWeapon = GetComponent<TowerWeapon>();

        //최대 체력과 현재체력 설정
        maxHP = towerWeapon.MaxHP;
        currentHP = maxHP;
    }

    public void PlusHP(float plus)
    {
        maxHP = towerWeapon.MaxHP;

        currentHP += plus;

        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void PlusHP(float plus, GameObject effect)
    {
        maxHP = towerWeapon.MaxHP;

        currentHP += plus;

        if (currentHP > maxHP)
            currentHP = maxHP;

        Instantiate(effect, transform.position, Quaternion.Euler(new Vector3(85, 0, 0)));
    }

    public void SetMaxHP(float plusHP) //최대 체력을 추가 설정
    {
        //테이블에 저장되어 있는 HP값을 불러온다
        maxHP = towerWeapon.MaxHP;

        //아이템 능력치에 따라 최대체력 증가
        if (type == TypeObject.Hero)
        {
            maxHP += plusHP;
        }

        //현재 체력을 최대체력과 동일하게 설정
        currentHP = maxHP;
    }

    public void TakeDamage(float damage,GameObject effect) //데미지 처리시
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            Color color = spriteRenderers[i].color;

            color.a = 1.0f;
            spriteRenderers[i].color = Color.white;
        }

        //피격시 이펙트 생성
        Instantiate(effect, transform.position, Quaternion.Euler(new Vector3(85, 0, 0)));

        if (isDie == true) 
            return;

        soundEffect.SEPlay(1);

        //데미지만큼 HP 감소
        if (damage < 0.05f)
            currentHP -= 0.05f;
        else
            currentHP -= damage;

        //피격시 효과 처리
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0) //체력이 0이 되면 쥬금
        {
            isDie = true;
            if (type == TypeObject.Unit || type == TypeObject.Hero)
                towerWeapon.UnitDie();
            else if (type == TypeObject.Enemy)
                enemy.OnDie(EnemyDestroyType.Kill);
        }
    }

    private IEnumerator HitAlphaAnimation() //데미지를 받았을 때 오브젝트를 잠시 붉게 함
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            Color color = spriteRenderers[i].color;

            color.a = 0.4f;
            spriteRenderers[i].color = Color.red;
        }
        
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            Color color = spriteRenderers[i].color;

            color.a = 1.0f;
            spriteRenderers[i].color = Color.white;
        }

    }
}
