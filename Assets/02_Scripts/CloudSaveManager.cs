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

        #region 버튼이벤트_연결
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
        // 저장할 데이터
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
        // 딕셔너리 타입으로 저장
        var data = new Dictionary<string, object>
        {
            {key, saveData}
        };

        // 저장 메소드
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);

        Debug.Log("Saved Multi Data!!");
    }

    // 복수 데이터 로드
    public async Task LoadData<T>(string key)
    {
       var loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });

        loadData.TryGetValue(key, out var data);

        // JSON 타입으로 변환없이 바로 추출하는 방식
        playerData = data.Value.GetAs<PlayerData>();
    }
}
