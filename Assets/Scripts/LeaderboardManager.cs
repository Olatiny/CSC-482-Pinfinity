using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Dashboard
    const string LeaderboardId = "Pinfinity-Leaderboard";

    const float gray = 0.5849056f;

    string VersionId { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int RangeLimit { get; set; }
    List<string> FriendIds { get; set; }

    [SerializeField]
    private TextMeshProUGUI highScoreText;

    [SerializeField]
    private List<TextMeshProUGUI> ranks;

    [SerializeField]
    private List<TextMeshProUGUI> names;

    [SerializeField]
    private List<TextMeshProUGUI> scores;

    async void Awake()
    {
        await UnityServices.InitializeAsync();

        await SignInAnonymously();
    }

    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<string> UpdatePlayerName(string playerName)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
        }
        catch (AuthenticationException)
        {
            // Debug.Log("Authentication Exception on changing name");
            return "Could not authenticate username.";
        }
        catch (RequestFailedException)
        {
            // Debug.Log("Request failed exception on changing name");
            return "Username change failed, wait a bit and/or try another username.";
        }

        return "success!";
    }

    public async void UpdateScoreBoard()
    {
        List<LeaderboardEntry> scoreBoard = (await GetScores()).Results;
        highScoreText.text = "" + await GetPlayerScoreValue();

        for (int j = 0; j < ranks.Count; j++)
        {
            names[j].text = "---";
            scores[j].text = "---";
        }

        int i = 0;
        foreach (LeaderboardEntry entry in scoreBoard)
        {
            string playerName = entry.PlayerName.Substring(0, entry.PlayerName.IndexOf('#'));
            names[i].text = (playerName.Length > 7 ? playerName.Substring(0, 7) : playerName);
            scores[i].text = "" + entry.Score;

            if (entry.PlayerId == GetPlayerId())
            {
                ranks[i].color = Color.red;
                names[i].color = Color.red;
                scores[i].color = Color.red;
            }
            else
            {
                ranks[i].color = new Color(gray, gray, gray);
                names[i].color = new Color(gray, gray, gray);
                scores[i].color = new Color(gray, gray, gray);
            }

            i++;
        }
    }

    public async Task<string> GetPlayerName()
    {
        return await AuthenticationService.Instance.GetPlayerNameAsync();
    }

    public string GetPlayerId()
    {
        return AuthenticationService.Instance.PlayerId;
    }

    public async Task<LeaderboardEntry> AddScore(int myScore)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, myScore);
        return scoreResponse;
    }

    public async Task<LeaderboardScoresPage> GetScores()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        return scoresResponse;
    }

    public async Task<double> GetPlayerScoreValue()
    {
        LeaderboardEntry scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        return scoreResponse.Score;
    }
}
