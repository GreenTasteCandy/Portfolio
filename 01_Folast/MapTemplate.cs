using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FieldType { Defence = 0, Offence }
public enum RoundType { Normal = 0, Boss }

public class MapTemplate : MonoBehaviour
{
    //�ʵ� ������Ʈ
    public FieldType fieldType;
    public Path[] paths;
    public LineRenderer[] pathLoots;
    public DefenceTemplate[] defenceField;
    public OffenceTemplate[] offenceField;

    //��� ������Ʈ
    [System.Serializable]
    public struct Path
    {
        public GameObject[] wayPoints;
    }


}
