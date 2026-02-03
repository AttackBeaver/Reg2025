using UnityEngine;

public class ItemSettingsManager : MonoBehaviour
{
    private ItemSettings itemSettings;

    private void Start()
    {
        LoadItemSettings();
    }

    public void LoadItemSettings()
    {
        if (DataManager.Instance != null)
        {
            itemSettings = DataManager.Instance.LoadData<ItemSettings>("ItemSettings");

            // Если файла нет, создаем дефолтные значения
            if (itemSettings.vehicles == null || itemSettings.vehicles.Length == 0)
            {
                CreateDefaultItemSettings();
                SaveItemSettings();
            }
        }
        else
        {
            CreateDefaultItemSettings();
            Debug.LogWarning("DataManager not found, using default item settings");
        }
    }

    private void CreateDefaultItemSettings()
    {
        itemSettings = new ItemSettings();

        // Дефолтные транспортные средства
        itemSettings.vehicles = new VehicleData[]
        {
            new VehicleData {
                id = "soda",
                name = "Soda Can",
                attachmentPoints = new Vector3[] {
                    new Vector3(0, 0.5f, 0), // Для колес
                    new Vector3(0, 1f, 0),    // Для пропеллера/ракеты
                    new Vector3(0, 0.2f, 0.5f) // Для крыльев
                },
                speed = 5f,
                turnSpeed = 100f
            },
            new VehicleData {
                id = "banana",
                name = "Banana",
                attachmentPoints = new Vector3[] {
                    new Vector3(0.2f, 0.3f, 0),
                    new Vector3(-0.2f, 0.3f, 0),
                    new Vector3(0, 0.8f, 0)
                },
                speed = 6f,
                turnSpeed = 120f
            },
            new VehicleData {
                id = "milk",
                name = "Milk Carton",
                attachmentPoints = new Vector3[] {
                    new Vector3(0.3f, 0.2f, 0),
                    new Vector3(-0.3f, 0.2f, 0),
                    new Vector3(0, 0.6f, 0),
                    new Vector3(0, 0.2f, 0.3f)
                },
                speed = 4.5f,
                turnSpeed = 90f
            }
        };

        // Дефолтные предметы для установки
        itemSettings.attachmentItems = new AttachmentItemData[]
        {
            new AttachmentItemData {
                id = "propeller",
                name = "Propeller",
                type = "speed",
                value = 2f, // Ускорение
                cooldown = 0f,
                isSingleUse = false
            },
            new AttachmentItemData {
                id = "rocket",
                name = "Rocket",
                type = "attack",
                value = 1f, // Урон
                cooldown = 5f,
                isSingleUse = true
            },
            new AttachmentItemData {
                id = "spiked_wheels",
                name = "Spiked Wheels",
                type = "special",
                value = 1f, // Возможность ехать вверх ногами
                cooldown = 0f,
                isSingleUse = false
            },
            new AttachmentItemData {
                id = "wheels",
                name = "Wheels",
                type = "movement",
                value = 1f, // Базовая скорость
                cooldown = 0f,
                isSingleUse = false
            },
            new AttachmentItemData {
                id = "wings",
                name = "Wings",
                type = "special",
                value = 3f, // Высота прыжка
                cooldown = 1f,
                isSingleUse = false
            }
        };
    }

    public void SaveItemSettings()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.SaveData(itemSettings, "ItemSettings");
        }
    }

    public VehicleData[] GetVehicles()
    {
        return itemSettings.vehicles;
    }

    public AttachmentItemData[] GetAttachmentItems()
    {
        return itemSettings.attachmentItems;
    }

    public VehicleData GetVehicle(string id)
    {
        foreach (var vehicle in itemSettings.vehicles)
        {
            if (vehicle.id == id) return vehicle;
        }
        return null;
    }

    public AttachmentItemData GetAttachmentItem(string id)
    {
        foreach (var item in itemSettings.attachmentItems)
        {
            if (item.id == id) return item;
        }
        return null;
    }
}
