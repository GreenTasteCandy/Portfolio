using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FieldType { Defence = 0, Offence }
public enum RoundType { Normal = 0, Boss }

public class MapTemplate : MonoBehaviour
{
    //필드 오브젝트
    public FieldType fieldType;
    public Path[] paths;
    public LineRenderer[] pathLoots;
    public DefenceTemplate[] defenceField;
    public OffenceTemplate[] offenceField;

    //경로 오브젝트
    [System.Serializable]
    public struct Path
    {
        public GameObject[] wayPoints;
    }


}
