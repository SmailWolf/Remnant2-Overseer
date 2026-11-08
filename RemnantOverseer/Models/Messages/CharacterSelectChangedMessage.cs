using CommunityToolkit.Mvvm.Messaging.Messages;

namespace RemnantOverseer.Models.Messages;
public class CharacterSelectChangedMessage(int value) : ValueChangedMessage<int>(value)
{
}