using CommunityToolkit.Mvvm.Messaging.Messages;

namespace RemnantOverseer.Models.Messages;
public class NotificationWarningMessage(string value) : ValueChangedMessage<string>(value)
{
}
