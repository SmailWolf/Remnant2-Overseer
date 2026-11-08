namespace RemnantOverseer.Models.Messages;
public class SaveFileChangedMessage(bool characterCountChanged)
{
    public bool CharacterCountChanged { get; } = characterCountChanged;
}
