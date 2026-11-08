using CommunityToolkit.Mvvm.Messaging.Messages;

namespace RemnantOverseer.Models.Messages;
public class NotificationInfoMessage(string value) : ValueChangedMessage<string>(value)
{
}
