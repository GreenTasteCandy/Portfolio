using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������Ʈ Ÿ��
public enum TypeObject { Unit = 0, Enemy, Hero, Object }

/*ü���� �����ϰ� �� ü���� ���� ��Ȳ�� �¾��� �� ����Ʈ�� ó���ϴ� ��ũ��Ʈ
 *�� ���� ������ ü�µ� ���⼭ ó��*/
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

        //���۽� ������Ʈ�� HP,�ʿ� ������Ʈ���� �������ش�

        if (type == TypeObject.Enemy)
        {
            //���� Enemy ������Ʈ�� �������ش�
            enemy = GetComponent<Enemy>();
        }

        //������ �� ���ݱ�� ������Ʈ ����
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        towerWeapon = GetComponent<TowerWeapon>();

        //�ִ� ü�°� ����ü�� ����
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

    public void SetMaxHP(float plusHP) //�ִ� ü���� �߰� ����
    {
        //���̺� ����Ǿ� �ִ� HP���� �ҷ��´�
        maxHP = towerWeapon.MaxHP;

        //������ �ɷ�ġ�� ���� �ִ�ü�� ����
        if (type == TypeObject.Hero)
        {
            maxHP += plusHP;
        }

        //���� ü���� �ִ�ü�°� �����ϰ� ����
        currentHP = maxHP;
    }

    public void TakeDamage(float damage,GameObject effect) //������ ó����
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            Color color = spriteRenderers[i].color;

            color.a = 1.0f;
            spriteRenderers[i].color = Color.white;
        }

        //�ǰݽ� ����Ʈ ����
        Instantiate(effect, transform.position, Quaternion.Euler(new Vector3(85, 0, 0)));

        if (isDie == true) 
            return;

        soundEffect.SEPlay(1);

        //��������ŭ HP ����
        if (damage < 0.05f)
            currentHP -= 0.05f;
        else
            currentHP -= damage;

        //�ǰݽ� ȿ�� ó��
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0) //ü���� 0�� �Ǹ� ���
        {
            isDie = true;
            if (type == TypeObject.Unit || type == TypeObject.Hero)
                towerWeapon.UnitDie();
            else if (type == TypeObject.Enemy)
                enemy.OnDie(EnemyDestroyType.Kill);
        }
    }

    private IEnumerator HitAlphaAnimation() //�������� �޾��� �� ������Ʈ�� ��� �Ӱ� ��
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
