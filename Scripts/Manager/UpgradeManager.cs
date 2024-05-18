using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class UpgradeManager : MonoBehaviour
{

    public static UpgradeManager instance;
    private static UpgradePreset preset;
    public static UpgradePreset Preset
    {
        get
        {
            if (!preset) preset = Resources.Load<UpgradePreset>("UpgradePreset");
            return preset;
        }
        set
        {
            preset = value;
        }
    }

    void Awake()
    {
        if (instance) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (!preset) Preset = Resources.Load<UpgradePreset>("UpgradePreset");
        }
    }

    public void Upgrade()
    {
        int needCash = Get(Preset.cashPerUpgrade);
        if (DBManager.DB.cash < needCash)
        {
            // ��ȭ ����
            print($"��ȭ ���� (����:{DBManager.DB.cash}, �ʿ�:{needCash})");
            return;
        }
        DBManager.DB.cash -= needCash;
        DBManager.DB.upgrade++;
        print($"��ȭ �Ϸ� (��ȭ �ܰ�:{DBManager.DB.upgrade})");
    }

    public static int Get(Vector2Int info)
    {
        return info.x + (DBManager.DB.upgrade * info.y);
    }

    public static float Get(Vector2 info)
    {
        return info.x + (DBManager.DB.upgrade * info.y);
    }

}
