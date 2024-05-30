using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Button scoreSaveButton;
    [SerializeField] private TMP_InputField scoreIf;

    // 리더보드 ID
    private const string leaderboardId = "Ranking";

    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log("Player Id : " + playerId);
        };

        await SignIn();

        scoreSaveButton.onClick.AddListener(() =>
        {
            SaveScore(int.Parse(scoreIf.text));
        });
    }

    private async Task SignIn()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // 점수 기록
    public async void SaveScore(int score)
    {
        var result = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);

        Debug.Log(result.Score);
    }
}
