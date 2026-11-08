using CommunityToolkit.Mvvm.Messaging.Messages;

namespace RemnantOverseer.Models.Messages;
public class NotificationErrorMessage(string value): ValueChangedMessage<string>(value)
{
}
