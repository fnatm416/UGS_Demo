using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct PlayerData
{
    public string name;
    public int level;
    public int xp;

    public List<ItemData> items;
}

[Serializable]
public struct ItemData
{
    public string name;
    public int value;
    public string icon;
}

public class CloudSaveManager : MonoBehaviour
{
    [SerializeField] private Button singleDataSaveButton;
    [SerializeField] private Button multiDataSaveButton;
    [SerializeField] private Button multiDataLoadButton;

    [SerializeField] private PlayerData playerData;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log("Player Id : " + playerId);
        };

        await SignIn();

        #region ��ư�̺�Ʈ_����
        singleDataSaveButton.onClick.AddListener(async () =>
        {
            await SaveSingleData();
        });

        multiDataSaveButton.onClick.AddListener(async () =>
        {
            await SaveMultuData("PlayerMultiData", playerData);
        });

        multiDataLoadButton.onClick.AddListener(async () =>
        {
            await LoadData<PlayerData>("PlayerMultiData");
        });
        #endregion
    }

    private async Task SignIn()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task SaveSingleData()
    {
        // ������ ������
        // Dictionary Type
        var data = new Dictionary<string, object>()
        {
            {"player_name", "son" },
            {"level", 90 },
            {"xp", 2000 }
        };

        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }

    public async Task SaveMultuData<T>(string key, T saveData)
    {
        // ��ųʸ� Ÿ������ ����
        var data = new Dictionary<string, object>
        {
            {key, saveData}
        };

        // ���� �޼ҵ�
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);

        Debug.Log("Saved Multi Data!!");
    }

    // ���� ������ �ε�
    public async Task LoadData<T>(string key)
    {
       var loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });

        loadData.TryGetValue(key, out var data);

        // JSON Ÿ������ ��ȯ���� �ٷ� �����ϴ� ���
        playerData = data.Value.GetAs<PlayerData>();
    }
}
