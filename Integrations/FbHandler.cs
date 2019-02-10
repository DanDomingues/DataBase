using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;
using UnityEngine.Events;
using System.Linq;
using Facebook.MiniJSON;

public static class FbHandler
{

    public  delegate void Action();
    static UnityAction act;

    public static int inviteCount;
    public static string inviteDebug ;

    public static bool LoggedIn { get { return FB.IsLoggedIn; } }

    public static bool Init()
    {
        if (FB.IsInitialized == false)
        {
            FB.Init();
            return false;
        }

        return true ;

    }

    public static bool LoginWithReadPermissions(UnityAction callback)
    {
        
        if (FB.IsInitialized == false)
        {
            FB.Init();
            return false;
        }

        act = callback;
        var loginResult = new FacebookDelegate<ILoginResult>(LoginAction);

        if (!FB.IsLoggedIn) FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, loginResult);

        else return true;

        return FB.IsLoggedIn && FB.IsInitialized;
    }

    public static bool LoginWithReadPermissions()
    {
        return LoginWithReadPermissions(null);
    }

    public static bool LoginWithPublishPermissions()
    {
        if (FB.IsInitialized == false)
        {
            FB.Init();
            return false;
        }

        FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, null);

        return FB.IsLoggedIn && FB.IsInitialized;

    }

    public static void GetProfileData(ref Sprite avatarSprite, ref string name)
    {
        
    }

    public static void Logout()
    {
        if (FB.IsLoggedIn) FB.LogOut();
    }

    public static void Share(string appURL, string imgURL,string title , string description, UnityAction callback)
    {

        act = callback;
        FacebookDelegate<IShareResult> shareResult = new FacebookDelegate<IShareResult>(ShareAction);

        FB.ShareLink(new Uri(appURL) , title, description, new Uri(imgURL ), shareResult);

    }

    public static void MobInvite(string appURL,  string imgURL, string title, string description, UnityAction callback)
    {
        act = callback;

        FacebookDelegate<IAppInviteResult> mobInviteResult = new FacebookDelegate<IAppInviteResult>(MobInviteAction);
        FB.Mobile.AppInvite( new Uri( appURL), new Uri(imgURL) , mobInviteResult);

    }

    public static int Invite(UnityAction callback)
    {
        act = callback;

        FacebookDelegate<IAppRequestResult> inviteResult = new FacebookDelegate<IAppRequestResult>(InviteAction);

        FB.AppRequest(
            "Come play the game!",
            OGActionType.SEND,
            "1320930214596238",
            null,null,null,
            inviteResult);

        return inviteCount;
    }

    static void LoginAction(ILoginResult result)
    {
        Debug.Log(result.Cancelled + " " + (act != null));
        if (!result.Cancelled && act != null) act();
    }


    static void ShareAction(IShareResult result)
    {
        if (!result.Cancelled && act != null) act();
    }

    static void  MobInviteAction(IAppInviteResult result)
    {

        if (result.RawResult.Contains("did_complete"))
            act();

        
    }

    static void InviteAction(IAppRequestResult result)
    {
        inviteDebug = result.RawResult;

        foreach (KeyValuePair<string, object> pair in result.ResultDictionary)
            if (pair.Key.Contains("Complete")) inviteDebug += pair.Key + " " + pair.Value + '\n';

        if (act != null) act();
    }


}
