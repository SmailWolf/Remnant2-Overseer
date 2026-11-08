using CommunityToolkit.Mvvm.ComponentModel;
using RemnantOverseer.Models.Enums;
using RemnantOverseer.Services;
using RemnantOverseer.Utilities;
using System;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;

namespace RemnantOverseer.Models;
public class Item : ObservableObject
{
    public string Id { get; set; } = string.Empty;
    private Bitmap? _thumbnail;
    private bool _thumbnailChecked;
    public Bitmap? Thumbnail
    {
        get
        {
            if (_thumbnailChecked) return _thumbnail;
            _thumbnailChecked = true;
            // IDs are catalog keys, never accept a path from the save file.
            if (string.IsNullOrWhiteSpace(Id) || Id.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || Id.Contains('/') || Id.Contains('\\')) return null;
            var file = Path.Combine(AppContext.BaseDirectory, "images", "items", Id + ".png");
            try { if (File.Exists(file)) { using var stream = File.OpenRead(file); _thumbnail = Bitmap.DecodeToWidth(stream, 96); } }
            catch (Exception) { _thumbnail = null; }
            return _thumbnail;
        }
    }
    public bool HasThumbnail => Thumbnail != null;
    public string CanonicalName { get; set; } = string.Empty;
    public string Name => CanonicalName;
    public ItemTypes Type { get; set; }
    public WeaponSubtypes? WeaponSubtype { get; set; }
    public string EquipmentSlotName { get; set; } = string.Empty;
    public string PlaceholderLabel { get; set; } = "Іконка відсутня";
    public string WorldName { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public string PlaceDetails => $"Світ: {WorldName} · Локація: {LocationName}";
    public string OriginDetails => string.IsNullOrWhiteSpace(CanonicalOriginName) ? "Джерело: невідоме" : $"Джерело: {CanonicalOriginName} ({OriginType})";
    public string Description { get; set; } = string.Empty;
    private string SourceDetails => !string.IsNullOrWhiteSpace(ItemDetailsCatalog.Get(Id)) ? ItemDetailsCatalog.Get(Id) : Description;
    public bool HasReviewedTranslation => ItemDetailsCatalog.HasReviewed(Id);
    public bool HasUnreviewedTranslation => !HasReviewedTranslation && !string.IsNullOrWhiteSpace(SourceDetails);
    public string HowToObtain => string.IsNullOrWhiteSpace(SourceDetails) ? "Умови отримання не вказані в каталозі." :
        ItemDetailsCatalog.HasReviewed(Id) ? ItemDetailsCatalog.GetReviewed(Id) : ItemDetailsCatalog.Translate(SourceDetails);
    public string OriginalHowToObtain => string.IsNullOrWhiteSpace(SourceDetails) ? "Умови отримання не вказані в каталозі." : SourceDetails;
    public bool HasDescription => !string.IsNullOrWhiteSpace(Description);
    public string? NoteTranslationLink => LocalizationService.ExternalTranslationUrl(Description);
    public bool HasNoteTranslationLink => NoteTranslationLink is not null;
    public bool HasPlainDescriptionIcon => HasDescription && !HasNoteTranslationLink;
    public OriginTypes OriginType { get; set; }
    public string OriginId { get; set; } = string.Empty;
    public string CanonicalOriginName { get; set; } = string.Empty;
    public string OriginName => LocalizationService.GameString(OriginId, CanonicalOriginName);
    public bool IsDuplicate { get; set; }
    public bool IsLooted { get; set; }
    public bool IsPrerequisiteMissing { get; set; }
    public bool HasRequiredMaterial { get; set; }
    public bool IsCoop { get; set; }
    public bool IsAccountAward { get; set; }
    public bool IsInCassShop { get; set; }

    public string TypeName => LocalizationService.ItemTypeName(Type);

    // We are only interested in a couple of types to display
    public string? OriginNameFormatted
    {
        get
        {
            return string.IsNullOrEmpty(OriginName) ? null :
            OriginType switch
            {
                // Extra spaces are a temporary workaround to https://github.com/AvaloniaUI/Avalonia/issues/17862, remove when fixed
                // It's not fixed yet, but I moved this text out of the tooltip. Removing spaces, keeping comment
                OriginTypes.Injectable or OriginTypes.Dungeon or OriginTypes.Vendor => LocalizationService.Format("Item_OriginNameFormat", LocalizationService.OriginTypeName(OriginType), OriginName),
                _ => null
            };
        }
    }

    private string LinkName => CanonicalName;

    public string? WikiLink => $"{UrlStrings.WikiUrl}/{LinkName}";

    public string? ToolkitLink => GetToolkitLink();

    public Item ShallowCopy()
    {
        return (Item)MemberwiseClone();
    }

    public void RefreshLocalizedProperties()
    {
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(OriginName));
        OnPropertyChanged(nameof(TypeName));
        OnPropertyChanged(nameof(OriginNameFormatted));
        OnPropertyChanged(nameof(NoteTranslationLink));
        OnPropertyChanged(nameof(HasNoteTranslationLink));
        OnPropertyChanged(nameof(HasPlainDescriptionIcon));
    }

    private string? GetToolkitLink()
    {
        var querySubstring = "";
        switch (Type)
        {
            case ItemTypes.Amulet:
                querySubstring = "amulets";
                break;
            case ItemTypes.Ring:
                querySubstring = "rings";
                break;
            case ItemTypes.Weapon:
                if (WeaponSubtype == null) return null;
                querySubstring = WeaponSubtype switch
                {
                    WeaponSubtypes.LongGun => "longGuns",
                    WeaponSubtypes.HandGun => "handGuns",
                    WeaponSubtypes.MeleeWeapon => "melees",
                    _ => throw new NotImplementedException()
                };
                break;
            default:
                return null;
        }
        var itemName = LinkName.Replace(" ", "+");
        return $"{UrlStrings.ToolkitUrl}?{querySubstring}={itemName}";
    }
}
