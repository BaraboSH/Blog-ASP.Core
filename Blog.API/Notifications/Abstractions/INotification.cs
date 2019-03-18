namespace Blog.API.Notifications.Abstractions
{
    public interface INotification
    {
        NotificationType NotificationType {get;set;}
    }
}