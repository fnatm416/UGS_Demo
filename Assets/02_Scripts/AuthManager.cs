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
        // UGS �ʱ�ȭ
        await UnityServices.InitializeAsync();

        // ��ư �̺�Ʈ ����
        signInButton.onClick.AddListener(async () =>
        {
            await SignInAsync();
        });

        signOutButton.onClick.AddListener(() =>
        {
            // �α׾ƿ�
            AuthenticationService.Instance.SignOut();
        });

        playerNameSaveButton.onClick.AddListener(async () =>
        {
            // �г��� ����
            await SetPlayerNameAsync(playerNameIf.text);
        });

        ConfigEvents();
    }

    // �͸� �α���
    private async Task SignInAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            //�÷��̾� �̸� Ȯ��
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

    // �̺�Ʈ ó��
    private void ConfigEvents()
    {
        // �α��� �Ϸ�
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("�͸� �α��� �Ϸ�");
            messageText.text = $"Login Player ID :\n {AuthenticationService.Instance.PlayerId}";
        };

        // �α׾ƿ� �Ϸ�
        AuthenticationService.Instance.SignedOut += () =>
        {
            messageText.text = $"Logout";
        };
    }

    // �÷��̾� �̸� ����
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