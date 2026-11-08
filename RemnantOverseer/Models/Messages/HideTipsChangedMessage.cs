using CommunityToolkit.Mvvm.Messaging.Messages;

namespace RemnantOverseer.Models.Messages;
public class HideTipsChangedMessage(bool value): ValueChangedMessage<bool>(value)
{
}
