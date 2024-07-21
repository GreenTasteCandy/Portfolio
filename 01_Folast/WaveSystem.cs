using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ���� ������ ���� ��ũ��Ʈ
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
    int currentWaveIndex = -1; //���潺 ���� �ε���
    int currentWave2Index = 0; //���潺 ���� �ε���
    WaveType waveType = WaveType.ReadyWave; //������ ���潺 �������

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

    public void StartWave() //���� ����
    {
        if (GameManager.instance.enemySpawner.EnemyList.Count == 0 && currentWaveIndex < 25)
        {
            //���潺 ���� ����
            if (waveType == WaveType.ReadyWave)
            {
                currentWaveIndex++;
                DefenceTemplate currentWave = fieldSet[fieldNum].defenceField[currentWaveIndex];

                GameManager.instance.enemySpawner.StartWave(fieldSet[fieldNum].paths, currentWave);
                waveType = WaveType.Defense;
            }
            //���潺 �غ�ܰ�
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
        //���潺 ���� ����
        else if (waveType == WaveType.ReadyOffense)
        {
            GameManager.instance.enemySpawner.OffenseTimeCheck();
            waveType = WaveType.Offense;
        }
    }

    public void ReadyWave() //���潺 �غ�ܰ�
    {
        for (int i = 0; i < pathSetting.Length; i++)
        {
            pathSetting[i].SetActive(false);
        }

        waveType = WaveType.ReadyWave;
        fieldSet[fieldNum].gameObject.SetActive(true);
        fieldSet2[fieldOffence].gameObject.SetActive(false);
    }

    //���潺���� �Ʊ� ���� ��� Ȯ��
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