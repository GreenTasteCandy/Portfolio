using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    [SerializeField]
    TowerSpawner towerSpawner;

    public bool[] skillOn;

    public void SkillActive(TowerTemplate tower)
    {

        for (int i = 0; i < towerSpawner.TowerList.Count; i++)
        {
            TowerWeapon hero = towerSpawner.TowerList[i];
            if (hero.TowerTemplate == tower)
                hero.ChangeState(WeaponState.TryAttackSkill);
        }

    }
}
