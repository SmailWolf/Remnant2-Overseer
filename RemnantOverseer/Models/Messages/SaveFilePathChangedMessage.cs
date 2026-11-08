using CommunityToolkit.Mvvm.Messaging.Messages;

namespace RemnantOverseer.Models.Messages;
public class SaveFilePathChangedMessage(string value) : ValueChangedMessage<string>(value)
{
}