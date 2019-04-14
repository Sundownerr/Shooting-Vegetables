using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Records
{
    public List<PlayerRecord> List = new List<PlayerRecord>();
}

[Serializable]
public class PlayerRecord
{
    [SerializeField] public int Score;
    [SerializeField] public int Position;
    [SerializeField] public string Name;
    [SerializeField] public string Date;
}
