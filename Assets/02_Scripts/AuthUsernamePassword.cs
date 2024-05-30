using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class AuthUsernamePassword : MonoBehaviour
{
    /*
        UserName : 20자 [ -, @ 는 가능 ]
        Password : 8 - 30자, 대소문자 구별 / 영문대문자1, 소문자1, 특수문자1, 숫자1 포함
    */

    [SerializeField] private TMP_InputField userNameIf, passwordIf;
    [SerializeField] private Button signUpButton, signInButton;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
    }

    void OnEnable()
    {
        signUpButton.onClick.AddListener(async () =>
        {
            await SignUp(userNameIf.text, passwordIf.text);
        });

        signInButton.onClick.AddListener(async () =>
        {
            await SignIn(userNameIf.text, passwordIf.text);
        });
    }

    // 회원가입
    private async Task SignUp(string username, string password)
    {
        try
        {
            // SignUp : 회원가입, SignIn : 로그인
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
        }
        catch(AuthenticationException e)
        {
            Debug.LogError(e.Message);
        }
        catch(RequestFailedException e)
        {
            Debug.LogError(e.Message);
        }
    }

    // 로그인
    private async Task SignIn(string username, string password)
    {
        await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);

        //await AuthenticationService.Instance.AddUsernamePasswordAsync(username, password);
        //await AuthenticationService.Instance.UpdatePasswordAsync(currentPassword, changePassword);
        //await AuthenticationService.Instance.UpdatePlayerNameAsync(newPlayerName);

        await Task.Delay(2000);

        Debug.Log(AuthenticationService.Instance.PlayerName);
        Debug.Log(AuthenticationService.Instance.PlayerId);
    }
}
