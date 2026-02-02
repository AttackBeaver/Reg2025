using UnityEngine;
using System;

[Serializable]
public class StoreItemData
{
    public string id;
    public string name;
    public int coinAmount;
    public int price;
    public bool isVideoReward;
    public bool isTimeReward;
    public float cooldownTime;
}

[Serializable]
public class BackgroundItemData
{
    public string id;
    public string name;
    public int price;
    public string colorHex; // HEX цвета фона
}

[Serializable]
public class StoreConfig
{
    public StoreItemData[] coinItems;
    public BackgroundItemData[] backgroundItems;
}
