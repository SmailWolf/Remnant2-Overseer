using CommunityToolkit.Mvvm.ComponentModel;
using RemnantOverseer.Models.Enums;
using RemnantOverseer.Services;
using System;
using System.Collections.Generic;

namespace RemnantOverseer.Models;
public partial class Character: ObservableObject
{
    public int Index { get; set; }
    public Archetypes Archetype { get; set; }
    public Archetypes? SubArchetype { get; set; }
    public int ObjectCount { get; set; }
    public bool IsHardcore { get; set; }
    public int PowerLevel { get; set; }
    public int SaveSlot { get; set; }
    public string TraitRank { get; set; } = "—";
    public string TraitPoints { get; set; } = "—";
    public string RelicCharges { get; set; } = "—";
    public string CampaignDifficulty { get; set; } = "—";
    public string AdventureDifficulty { get; set; } = "—";
    public List<Item> EquippedItems { get; set; } = [];
    public List<Item> EquippedSkills { get; set; } = [];
    public List<Item> EquippedTools { get; set; } = [];
    public bool HasEquippedSkills => EquippedSkills.Count > 0;
    public bool HasEquippedTools => EquippedTools.Count > 0;
    public string HardcoreLabel => IsHardcore ? "Hardcore: так" : "Hardcore: ні";
    public string InventoryLabel => $"Предметів у колекції: {ObjectCount}";
    public string SaveSlotLabel => $"Слот збереження: save_{SaveSlot}.sav";
    public string TraitLabel => $"Ранг рис: {TraitRank} · очки: {TraitPoints}";
    public string RelicLabel => $"Заряди реліквії: {RelicCharges}";
    public static string UkrainianDifficulty(string? difficulty) => difficulty?.Trim().ToLowerInvariant() switch
    {
        "survivor" => "Вцілілий",
        "veteran" => "Ветеран",
        "nightmare" => "Кошмар",
        "apocalypse" => "Апокаліпсис",
        null or "" => "невідомо",
        _ => difficulty
    };
    public string CampaignLabel => $"Кампанія: {UkrainianDifficulty(CampaignDifficulty)}";
    public string AdventureLabel => $"Пригода: {UkrainianDifficulty(AdventureDifficulty)}";
    public TimeSpan Playtime { get; set; }
    public WorldTypes ActiveWorld { get; set; }

    public string? FormattedPlaytime => (int)Playtime.TotalHours + Playtime.ToString(@"\:mm\:ss");

    public string FormattedPowerLevel => PowerLevel > 0 ? PowerLevel.ToString() : "?";

    public string ArchetypeName => LocalizationService.ArchetypeName(Archetype);

    public string? SubArchetypeName => SubArchetype is null ? null : LocalizationService.ArchetypeName(SubArchetype.Value);

    public string? Summary
    {
        get
        {
            return SubArchetypeName is null
                ? LocalizationService.Format("Character_SummarySingle", ArchetypeName, FormattedPowerLevel)
                : LocalizationService.Format("Character_SummaryDual", ArchetypeName, SubArchetypeName, FormattedPowerLevel);
        }
    }

    // Viewmodel specific
    [ObservableProperty]
    private bool _isSelected;

    public void RefreshLocalizedProperties()
    {
        OnPropertyChanged(nameof(ArchetypeName));
        OnPropertyChanged(nameof(SubArchetypeName));
        OnPropertyChanged(nameof(Summary));
    }
}
