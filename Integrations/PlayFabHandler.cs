using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ProfilesModels;
//using PlayFab.ServerModels;

public class PlayFabHandler : ReferenceSingleton<PlayFabHandler> 
{
	[SerializeField] string characterID;
    [SerializeField] string uniqueID;
    [SerializeField] string characterLocation;
    [SerializeField] bool useCityAsLocation;

	[SerializeField] bool loggedIn;

	public Dictionary<string, string> content {get; private set;}
	public delegate void PlayFabEvent();
	public event PlayFabEvent OnUserLogin;
	public event PlayFabEvent OnContentGotten;
	
	public bool LoggedIn => loggedIn;
    public string ID => characterID;
    public string Location => characterLocation;



	void Start()
	{

	}

	public void CreateEntry(string user, Action<LoginResult> onLogin, Action<PlayFabError> onFailure)
    {
        var request = new LoginWithCustomIDRequest();
        request.TitleId = "8141";
        request.CreateAccount = true;
        request.CustomId = user;

        PlayFabClientAPI.LoginWithCustomID(request, result => OnSignInSuccess(user, result, 
            loginResult => 
            {
                var infoRequest = new GetPlayerProfileRequest();
                infoRequest.PlayFabId = loginResult.PlayFabId;
                infoRequest.ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true,
                    ShowLocations = true
                };

                PlayFabClientAPI.GetPlayerProfile(infoRequest,
                infoResult =>
                {
                    uniqueID = infoResult.PlayerProfile.PlayerId;

                    var location = infoResult.PlayerProfile.Locations.Last();
                    characterLocation = useCityAsLocation ? location.City.ToString() : location.CountryCode.ToString();
                    onLogin(loginResult);
                    OnLogin();

                }, error => onFailure(error));
                    
            }), 
            onFailure);

       
    }

	void OnSignInSuccess(string user, LoginResult result, Action<LoginResult> action)
    {
        var request = new UpdateUserTitleDisplayNameRequest();
        request.DisplayName = user;
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, null, null);

        var newResult = new GetAccountInfoResult();
        action(result);
    }

	void OnLogin()
	{
		loggedIn = true;
		OnUserLogin?.Invoke();

        var request = new GetTitleDataRequest();
        request.Keys = new List<string>();

        PlayFabClientAPI.GetTitleData(request, result =>
        {
            content = result.Data;
			OnContentGotten?.Invoke();
        }, null);

        var tagRequest = new PlayFab.ServerModels.AddPlayerTagRequest();
        tagRequest.TagName = string.Format("skin:{0}", UnityEngine.Random.Range(0, 3));

        PlayFabServerAPI.AddPlayerTag(tagRequest,
        tagResult => { Debug.Log("Tag add success"); },
        tagError => { Debug.Log("Tag failed: " + tagError.ErrorMessage); });


        //Debug.Log(string.Format("{0} has {1} tags.", PlayFabClientAPI. .DisplayName, 
        //    result.Leaderboard[i].Profile.Tags != null ? result.Leaderboard[i].Profile.Tags.Count.ToString() : "no"));


    }

    public void FetchHighscores(string loginID, Action<ScoreCompare[]> onSuccess, Action onFailure)
    {
        CreateEntry(loginID, result =>
        {
            GetScores(onSuccess, error => onFailure());
        }, 
        error => onFailure());
    }

    void GetScores(Action<ScoreCompare[]> onSuccess, Action<PlayFabError> errorAction)
    {
        var request = new GetLeaderboardRequest();
        request.MaxResultsCount = 100;
        request.StatisticName = "Worldwide Scores";
        request.ProfileConstraints = new PlayerProfileViewConstraints();
        request.ProfileConstraints.ShowLocations = true;
        request.ProfileConstraints.ShowDisplayName = true;

        var list = new List<ScoreCompare>();
        var checks = new List<bool>();

        ScoreCompare score;
        PlayFab.ClientModels.LocationModel location;
        PlayFabClientAPI.GetLeaderboard(request,
            (GetLeaderboardResult result) =>
            {
                int scoreCount = result.Leaderboard.Count;
                for (int i = 0; i < scoreCount; i++)
                {
                    score = new ScoreCompare(result.Leaderboard[i].StatValue);
                    score.name = result.Leaderboard[i].DisplayName;
                    score.ID = result.Leaderboard[i].PlayFabId;
                    location = result.Leaderboard[i].Profile.Locations.Last();
                    score.location = useCityAsLocation ? location.City.ToString() : location.CountryCode.ToString();
                    
                    list.Add(score);
                    checks.Add(false);
                    string name = score.name;
                    int index = i;

                    var infoRequest = new GetUserDataRequest();
                    infoRequest.PlayFabId = result.Leaderboard[i].PlayFabId;
                    PlayFabClientAPI.GetUserData(infoRequest, infoResult =>
                    {
                        if(infoResult.Data.ContainsKey("SkinID") && infoResult.Data.ContainsKey("Location"))
                        {
                            var value = infoResult.Data["SkinID"];
                            list[index].usedSkin = (CatSkin) (int.Parse(value.Value));
                        }
                        else
                        {
                            var r = new PlayFab.ServerModels.UpdateUserDataRequest();
                            r.Permission = PlayFab.ServerModels.UserDataPermission.Public;
                            r.Data = new Dictionary<string, string>()
                            {
                                { "SkinID", "0" },
                                { "Location", "Brazil" }
                            };
                            r.PlayFabId = infoRequest.PlayFabId;
                            PlayFabServerAPI.UpdateUserData(r, sucess =>
                            {
                                Debug.Log("SUCCESS");
                            }, failure => { Debug.Log("FAILED"); });
                        }

                        checks[index] = true;

                        if(!checks.Contains(false))
                        {
                            Debug.Log("List returned");

                            var values = list.ToArray();
                            onSuccess(values);
                        }
                    },
                    failure => Debug.Log("Get content fail: " + failure.ErrorMessage));

                }

            }, errorCallback => Debug.Log("Failed to get leaderboard"));
    }

    public void SendScore(string ID, ScoreCompare playerScore, Action<ScoreCompare[]> onSuccess, Action onFailure)
    {
        CreateEntry(ID, result =>
        {
            SendScoreRaw(ID, playerScore, onSuccess, error => onFailure());
        }, 
        error => onFailure());
    }

    void SendScoreRaw(string ID, ScoreCompare playerScore, Action<ScoreCompare[]> onSuccess, Action<PlayFabError> errorAction)
    {
        var item = new StatisticUpdate();
        item.Value = playerScore.value;
        item.StatisticName = "Worldwide Scores";

        var request = new UpdatePlayerStatisticsRequest();
        request.Statistics = new List<StatisticUpdate>() { item };
        PlayFabClientAPI.UpdatePlayerStatistics(request, 
        result =>
        {
            var r = new PlayFab.ServerModels.UpdateUserDataRequest();
            r.Permission = PlayFab.ServerModels.UserDataPermission.Public;
            r.PlayFabId = uniqueID;
            r.Data = new Dictionary<string, string>()
            {
                { "SkinID", ((int) playerScore.usedSkin).ToString() },
                { "Location", playerScore.location }
            };

            PlayFabServerAPI.UpdateUserData(r, sucess => GetScores(onSuccess, errorAction), 
            failure => { Debug.Log("Fail: " + failure.ErrorMessage); });

        },
        failure => { });
    }

}
