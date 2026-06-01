using System.Collections.Generic;
using System;

[Serializable]
public class MonsterDatabase
{
    public List<MonsterData> monsters;
}

[Serializable]
public class MonsterData
{
    public string id;
    public string name;
    public string patternType;
    public float moveSpeed;
    public float detectRange;
    public float specialCooldown;
}

public enum MonsterPattern
{
    Idle,
    Stalker,
    Ambusher,
    Observer,
    Mimic
}
