using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OneSignalPush.MiniJSON;

/// <summary>
/// Integration class for scheduling Push Notifications using OneSignal
/// Requires OneSignal's Plugin to function properly. Will result in errors otherwise.
/// </summary>

public class PushHandler : MonoBehaviour 
{
    [SerializeField] protected string appKey;
    [SerializeField] protected string defaultMessage;
    [SerializeField] protected int addedSeconds;
    [SerializeField] protected int addedMinutes;
    [SerializeField] protected int addedHours;
    [SerializeField] protected bool sendPushOnStart;

    [Header("Runtime")]
    [SerializeField] protected string extraMessage;

    protected virtual void Start () 
    {
        // Enable line below to enable logging if you are having issues setting up OneSignal. (logLevel, visualLogLevel)
        // OneSignal.SetLogLevel(OneSignal.LOG_LEVEL.INFO, OneSignal.LOG_LEVEL.INFO);
        OneSignal.StartInit(appKey)
        .HandleNotificationOpened(HandleNotificationOpened)
        .EndInit();

        OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;
        if(sendPushOnStart) SendNotification();
    }

    protected virtual void Reset()
    {
        sendPushOnStart = true;
        addedMinutes = 1;
        defaultMessage = "Hey! This push works, Dan is awesome!";
    }

     protected virtual void SendNotification(string message, int addedHours, int addedMinutes, int addedSeconds)
    {
        OneSignal.IdsAvailable((userId, token) =>
        {
            if(token == null) return;

            extraMessage = "Waiting to get a OneSignal userId. Uncomment OneSignal. SetLogLevel in the 'Start' method if it hangs here to debug the issue.";

            var localTime = System.DateTime.Now.ToUniversalTime();
            var notification = new Dictionary<string, object>();
            notification["contents"] = new Dictionary<string, string>() 
            { 
                {"en", string.Format("{0} ({1}:{2}:{3})", 
                message, localTime.Hour.ToString("00"), 
                localTime.Minute.ToString("00"), 
                localTime.Second.ToString("00"))} 
            };

            // Send notification to this device.
            notification["include_player_ids"] = new List<string>() { userId };
            // Example of scheduling a notification in the future.
            notification["send_after"] = localTime.AddSeconds(addedSeconds).AddMinutes(addedMinutes).AddHours(addedHours).ToString("U");

            extraMessage = "Posting test notification now.";

            OneSignal.PostNotification(notification, (responseSuccess) => 
            {
                extraMessage = "Notification posted successful! Delayed by a predetermined time amount to give you time to press the home button to see a notification vs an in-app alert.\n" + Json.Serialize(responseSuccess);
            }, 
            (responseFailure) => 
            {
                extraMessage = "Notification failed to post:\n" + Json.Serialize(responseFailure);
            });

            OneSignal.ClearOneSignalNotifications();

        });
              
    }
    protected void SendNotification()
    {
        SendNotification(defaultMessage, addedHours, addedMinutes, addedSeconds);
    }

    public virtual void SetSubscribed(bool value)
    {
        OneSignal.SetSubscription(value);
    }

    // Gets called when the player opens the notification.
    protected static void HandleNotificationOpened(OSNotificationOpenedResult result) 
    {
        
    }

}
