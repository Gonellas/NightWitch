using System;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{

    public static NotificationManager Instance { get; private set; }

    AndroidNotificationChannel notifChannel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        notifChannel = new AndroidNotificationChannel()
        {
            Id = "reminder_notif_ch",
            Name = "Reminder Notification",
            Description = "Reminder to login",
            Importance = Importance.High
        };

        AndroidNotificationCenter.RegisterNotificationChannel(notifChannel);

        if (PlayerPrefs.HasKey("Display_ComeBack"))
        {
            CancelNotification(PlayerPrefs.GetInt("Display_ComeBack"));
        }

        PlayerPrefs.SetInt("Display_ComeBack", DisplayNotification("Comeback! ", "We miss you!",
            IconSelecter.icon_reminder, IconSelecter.icon_reminderbig, DateTime.Now.AddHours(36)));
    }

    public int DisplayNotification(string title, string text, IconSelecter iconSmall, IconSelecter iconLarge, DateTime fireTime)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.SmallIcon = iconSmall.ToString();
        notification.LargeIcon = iconLarge.ToString();
        notification.FireTime = fireTime;

        return AndroidNotificationCenter.SendNotification(notification, notifChannel.Id);
    }

    public void CancelNotification(int id)
    {
        AndroidNotificationCenter.CancelScheduledNotification(id);
    }
}

public enum IconSelecter
{
    icon_reminder,
    icon_reminderbig
}
