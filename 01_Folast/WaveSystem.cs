using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 라운드 설정을 위한 스크립트
 */
public enum WaveType { ReadyWave = 0, Defense, ReadyOffense, Offense }
public class WaveSystem : MonoBehaviour
{
    public static WaveSystem instance;

    [SerializeField]
    int fieldNum;
    [SerializeField]
    MapTemplate[] fieldSet;
    [SerializeField]
    MapTemplate[] fieldSet2;
    [SerializeField]
    GameObject[] pathSetting;

    int fieldOffence;
    int currentWaveIndex = -1; //디펜스 라운드 인덱스
    int currentWave2Index = 0; //오펜스 라운드 인덱스
    WaveType waveType = WaveType.ReadyWave; //시작은 디펜스 라운드부터

    public int CurrentWave => currentWaveIndex + 1;
    public int CurrentWave2 => currentWave2Index;
    public int MaxWave => 25;
    public WaveType WaveType => waveType;

    public void Start()
    {
        instance = this;

        for(int i = 0; i < fieldSet.Length; i++)
        {
            fieldSet[i].gameObject.SetActive(false);
        }

        fieldNum = 0;//Random.Range(0, fieldSet.Length);
        fieldSet[fieldNum].gameObject.SetActive(true);
    }

    public void StartWave() //라운드 시작
    {
        if (GameManager.instance.enemySpawner.EnemyList.Count == 0 && currentWaveIndex < 25)
        {
            //디펜스 라운드 시작
            if (waveType == WaveType.ReadyWave)
            {
                currentWaveIndex++;
                DefenceTemplate currentWave = fieldSet[fieldNum].defenceField[currentWaveIndex];

                GameManager.instance.enemySpawner.StartWave(fieldSet[fieldNum].paths, currentWave);
                waveType = WaveType.Defense;
            }
            //오펜스 준비단계
            else if (waveType == WaveType.Defense)
            {
                fieldOffence = Random.Range(0, fieldSet2.Length);

                fieldSet[fieldNum].gameObject.SetActive(false);
                fieldSet2[fieldOffence].gameObject.SetActive(true);

                for(int i = 0; i < fieldSet2[fieldOffence].paths.Length; i++)
                {
                    pathSetting[i].SetActive(true);
                }

                OffenceTemplate currentWave = fieldSet2[fieldOffence].offenceField[currentWave2Index];

                GameManager.instance.enemySpawner.StartWave2(fieldSet2[fieldOffence].paths, currentWave);
                waveType = WaveType.ReadyOffense;
                currentWave2Index++;
            }
        }
        //오펜스 라운드 시작
        else if (waveType == WaveType.ReadyOffense)
        {
            GameManager.instance.enemySpawner.OffenseTimeCheck();
            waveType = WaveType.Offense;
        }
    }

    public void ReadyWave() //디펜스 준비단계
    {
        for (int i = 0; i < pathSetting.Length; i++)
        {
            pathSetting[i].SetActive(false);
        }

        waveType = WaveType.ReadyWave;
        fieldSet[fieldNum].gameObject.SetActive(true);
        fieldSet2[fieldOffence].gameObject.SetActive(false);
    }

    //오펜스에서 아군 생산 경로 확인
    public void OffenceLineCheck(int num)
    {
        for (int i = 0; i < fieldSet2[fieldOffence].pathLoots.Length; i++)
        {
            fieldSet2[fieldOffence].pathLoots[i].enabled = false;
        }

        fieldSet2[fieldOffence].pathLoots[num].enabled = true;
        GameManager.instance.towerSpawner.OffenseWaySet(num);
    }

}