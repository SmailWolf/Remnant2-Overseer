using CommunityToolkit.Mvvm.Messaging.Messages;

namespace RemnantOverseer.Models.Messages;
public class HideToolkitLinksChangedMessage(bool value) : ValueChangedMessage<bool>(value)
{
}
