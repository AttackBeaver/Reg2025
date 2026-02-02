using UnityEngine;
using System;

[Serializable]
public class VehicleData
{
    public string id; // "soda", "banana", "milk"
    public string name;
    public Vector3[] attachmentPoints; // Точки крепления предметов
    public float speed = 5f;
    public float turnSpeed = 100f;
}

[Serializable]
public class AttachmentItemData
{
    public string id; // "propeller", "rocket", "spiked_wheels", "wheels", "wings"
    public string name;
    public string type; // "speed", "attack", "special", "movement"
    public float value; // Значение эффекта
    public float cooldown = 1f;
    public bool isSingleUse = false;
}

[Serializable]
public class ItemSettings
{
    public VehicleData[] vehicles;
    public AttachmentItemData[] attachmentItems;
}
