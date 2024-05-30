using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class RemoteConfigManager : MonoBehaviour
{
    public float moveSpeed;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log("Player Id : " + playerId);
        };

        await SignIn();

        RemoteConfigService.Instance.FetchCompleted += OnReturnRemoteConfig;

        await GetRemoteConfigData();
    }

    private async Task SignIn()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public struct userAttributes { };
    public struct appAttributes { };

    // Remote Config 데이터 불러오기
    private async Task GetRemoteConfigData()
    {
        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
    }

    // Query Event (데이터 긁어오기)
    private void OnReturnRemoteConfig(ConfigResponse response)
    {
        Debug.Log("결과값 리턴 성공 " + response.status);

        moveSpeed = RemoteConfigService.Instance.appConfig.GetFloat("moveSpeed");
    }
}
