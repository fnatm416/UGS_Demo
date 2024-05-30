using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signOutButton;
    [SerializeField] private Button playerNameSaveButton;

    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_InputField playerNameIf;

    private async void Awake()
    {
        // UGS 초기화
        await UnityServices.InitializeAsync();

        // 버튼 이벤트 연결
        signInButton.onClick.AddListener(async () =>
        {
            await SignInAsync();
        });

        signOutButton.onClick.AddListener(() =>
        {
            // 로그아웃
            AuthenticationService.Instance.SignOut();
        });

        playerNameSaveButton.onClick.AddListener(async () =>
        {
            // 닉네임 저장
            await SetPlayerNameAsync(playerNameIf.text);
        });

        ConfigEvents();
    }

    // 익명 로그인
    private async Task SignInAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            //플레이어 이름 확인
            await Task.Delay(1000);
            string name = AuthenticationService.Instance.PlayerName;
            name = name.Split('#')[0];
            messageText.text += $"Saved Player Name : <color=#00ff00><b>{name}</b></color>";
        }
        catch (AuthenticationException e)
        {
            Debug.LogError(e.Message);
        }
    }

    // 이벤트 처리
    private void ConfigEvents()
    {
        // 로그인 완료
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("익명 로그인 완료");
            messageText.text = $"Login Player ID :\n {AuthenticationService.Instance.PlayerId}";
        };

        // 로그아웃 완료
        AuthenticationService.Instance.SignedOut += () =>
        {
            messageText.text = $"Logout";
        };
    }

    // 플레이어 이름 저장
    private async Task SetPlayerNameAsync(string playerName)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
        }
        catch (AuthenticationException e)
        {
            Debug.LogError(e.Message);
        }

        messageText.text = $"Player Name : {AuthenticationService.Instance.PlayerName}";
    }
}