using UnityEngine;

public class StoreConfigManager : MonoBehaviour
{
    private StoreConfig storeConfig;

    private void Start()
    {
        LoadStoreConfig();
    }

    public void LoadStoreConfig()
    {
        if (DataManager.Instance != null)
        {
            storeConfig = DataManager.Instance.LoadData<StoreConfig>("StoreConfig");

            // Если файла нет, создаем дефолтные значения
            if (storeConfig.coinItems == null || storeConfig.coinItems.Length == 0)
            {
                CreateDefaultStoreConfig();
                SaveStoreConfig();
            }
        }
        else
        {
            CreateDefaultStoreConfig();
            Debug.LogWarning("DataManager not found, using default store config");
        }
    }

    private void CreateDefaultStoreConfig()
    {
        storeConfig = new StoreConfig();

        // Дефолтные товары для магазина монет
        storeConfig.coinItems = new StoreItemData[]
        {
            new StoreItemData { id = "coins_500", name = "500 Coins", coinAmount = 500, price = 99, isVideoReward = false, isTimeReward = false },
            new StoreItemData { id = "coins_1200", name = "1200 Coins", coinAmount = 1200, price = 199, isVideoReward = false, isTimeReward = false },
            new StoreItemData { id = "coins_2500", name = "2500 Coins", coinAmount = 2500, price = 399, isVideoReward = false, isTimeReward = false },
            new StoreItemData { id = "free_coins_1min", name = "Free Coins", coinAmount = 100, price = 0, isVideoReward = false, isTimeReward = true, cooldownTime = 60f },
            new StoreItemData { id = "free_coins_video", name = "Watch Video", coinAmount = 100, price = 0, isVideoReward = true, isTimeReward = false, cooldownTime = 30f },
            new StoreItemData { id = "free_coins_video2", name = "Watch Video", coinAmount = 100, price = 0, isVideoReward = true, isTimeReward = false, cooldownTime = 30f }
        };

        // Дефолтные фоны
        storeConfig.backgroundItems = new BackgroundItemData[]
        {
            new BackgroundItemData { id = "bg_red", name = "Red Background", price = 500, colorHex = "#FF6B6B" },
            new BackgroundItemData { id = "bg_blue", name = "Blue Background", price = 750, colorHex = "#4ECDC4" },
            new BackgroundItemData { id = "bg_green", name = "Green Background", price = 1000, colorHex = "#45B7D1" }
        };
    }

    public void SaveStoreConfig()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.SaveData(storeConfig, "StoreConfig");
        }
    }

    public StoreItemData[] GetCoinItems()
    {
        return storeConfig.coinItems;
    }

    public BackgroundItemData[] GetBackgroundItems()
    {
        return storeConfig.backgroundItems;
    }

    public StoreItemData GetCoinItem(string id)
    {
        foreach (var item in storeConfig.coinItems)
        {
            if (item.id == id) return item;
        }
        return null;
    }

    public BackgroundItemData GetBackgroundItem(string id)
    {
        foreach (var item in storeConfig.backgroundItems)
        {
            if (item.id == id) return item;
        }
        return null;
    }
}
