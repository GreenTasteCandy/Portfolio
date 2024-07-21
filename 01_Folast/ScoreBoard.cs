using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScoreBoard : ScriptableObject
{
    public int score = 0;
    public int highScore;
    public int gainCoin;
}
