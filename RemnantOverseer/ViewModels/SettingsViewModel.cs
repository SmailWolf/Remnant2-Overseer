using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RemnantOverseer.Models.Messages;
using RemnantOverseer.Services;
using RemnantOverseer.Utilities;
using RemnantOverseer.Models;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RemnantOverseer.ViewModels;
public partial class SettingsViewModel : ViewModelBase
{
    private readonly SaveDataService _saveDataService;
    private readonly SettingsService _settingsService;

    [ObservableProperty]
    private string? _filePath;

    [ObservableProperty]
    private string _savePathStatus = "Оберіть папку з profile.sav і save_0.sav або вставте шлях нижче.";

    [ObservableProperty]
    private bool _enableVersionCheck;

    [ObservableProperty]
    private bool _showTips;

    [ObservableProperty]
    private bool _showToolkitLinks;

    [ObservableProperty]
    private CultureOption? _selectedCulture;

    public IReadOnlyList<CultureOption> SupportedCultures { get; } = LocalizationService.SupportedCultures;

    public SettingsViewModel(SettingsService settingsService, SaveDataService saveDataService)
    {
        _settingsService = settingsService;
        _saveDataService = saveDataService;
        var settings = _settingsService.Get();
        _filePath = settings.SaveFilePath;
        _showTips = !settings.HideTips;
        _showToolkitLinks = !settings.HideToolkitLinks;
        _enableVersionCheck = !settings.DisableVersionCheck;
        _selectedCulture = SupportedCultures.FirstOrDefault(c => c.CultureName == settings.CultureName) ?? SupportedCultures[0];

        if (Design.IsDesignMode)
        {
            FilePath = @"C:\Remnant\Remnant 2: I'm Beginning to Remn";
        }
    }

    [RelayCommand]
    public async Task OpenFolder()
    {
        try
        {
            var topLevel = FileDialogManager.GetTopLevelForContext(this);
            if (topLevel == null) { SavePathStatus = "Не вдалося відкрити діалог. Вставте шлях вручну і натисніть «Завантажити»."; return; }
            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Оберіть папку зі збереженнями Remnant II", AllowMultiple = false
            });
            if (folders.Count == 0) return;
            var path = folders[0].TryGetLocalPath();
            if (string.IsNullOrWhiteSpace(path)) { SavePathStatus = "Цю папку неможливо відкрити локально. Вставте її шлях вручну."; return; }
            FilePath = path;
            await ApplySavePath();
        }
        catch (Exception ex) { SavePathStatus = "Не вдалося відкрити папку: " + ex.Message; }
    }

    [RelayCommand]
    public async Task OpenFile()
    {
        try
        {
            var topLevel = FileDialogManager.GetTopLevelForContext(this);
            if (topLevel == null) { SavePathStatus = "Не вдалося відкрити діалог. Вставте шлях вручну."; return; }
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                FileTypeFilter = [Saves], AllowMultiple = false, Title = "Оберіть файл profile.sav"
            });
            if (files.Count == 0) return;
            var file = files[0].TryGetLocalPath();
            if (string.IsNullOrWhiteSpace(file)) { SavePathStatus = "Файл недоступний локально. Вставте шлях вручну."; return; }
            FilePath = file;
            await ApplySavePath();
        }
        catch (Exception ex) { SavePathStatus = "Не вдалося вибрати файл: " + ex.Message; }
    }

    [RelayCommand]
    public async Task ApplySavePath()
    {
        try
        {
            var path = FilePath?.Trim().Trim('"');
            if (string.IsNullOrWhiteSpace(path)) { SavePathStatus = "Вкажіть папку або файл profile.sav."; return; }
            path = Path.GetFullPath(path);
            if (File.Exists(path))
            {
                if (!Path.GetFileName(path).Equals("profile.sav", StringComparison.OrdinalIgnoreCase))
                { SavePathStatus = "Оберіть саме profile.sav або папку, в якій він лежить."; return; }
                path = Path.GetDirectoryName(path)!;
            }
            if (!Directory.Exists(path) || !File.Exists(Path.Combine(path, "profile.sav")))
            { SavePathStatus = "У цій папці не знайдено profile.sav. Перевірте шлях."; return; }
            if (!Directory.EnumerateFiles(path, "save_*.sav").Any())
            { SavePathStatus = "Поруч із profile.sav не знайдено save_0.sav чи іншого save_N.sav."; return; }
            FilePath = path;
            var oldPath = _settingsService.Get().SaveFilePath;
            _settingsService.Get().SaveFilePath = path;
            SavePathStatus = "Завантаження збереження…";
            var success = await _saveDataService.SelectSaveFolder(path);
            if (!success) _settingsService.Get().SaveFilePath = oldPath;
            await _settingsService.Sync();
            SavePathStatus = success
                ? "Збереження завантажено. Відкрийте вкладку світу."
                : "Не вдалося прочитати збереження. Перевірте файли або спробуйте інший профіль.";
        }
        catch (Exception ex) { SavePathStatus = "Не вдалося відкрити збереження: " + ex.Message; }
    }

    [RelayCommand]
    public async Task OpenLog()
    {
        var topLevel = FileDialogManager.GetTopLevelForContext(this);
        if (topLevel == null) return;

        try
        {
            await topLevel.Launcher.LaunchFileInfoAsync(new FileInfo(Log.GetLogFilePath()));
        }
        catch { }
    }

    [RelayCommand]
    public async Task DumpPlayerInfo()
    {
        await Task.Run(_saveDataService.ReportPlayerInfo);
    }

    [RelayCommand]
    public async Task ExportSave()
    {
        var topLevel = FileDialogManager.GetTopLevelForContext(this);
        if (topLevel == null) return;

        var documents = await topLevel.StorageProvider.TryGetWellKnownFolderAsync(WellKnownFolder.Documents);
        var storageDir = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions()
            {
                Title = LocalizationService.Get("Settings_ExportFolderPickerTitle"),
                SuggestedStartLocation = documents,
                SuggestedFileName = $"Remnant2_export_{DateTime.Now:yyyy-MM-dd}",
                AllowMultiple = false,
            });
        if (storageDir.Count == 0) return;

        await Task.Run(() => _saveDataService.ExportSave(storageDir[0].TryGetLocalPath()));
    }

    partial void OnEnableVersionCheckChanged(bool value)
    {
        Task.Run(async () =>
        {
            _settingsService.Get().DisableVersionCheck = !value;
            await _settingsService.Sync();
            WeakReferenceMessenger.Default.Send(new DisableVersionCheckChangedMessage(!value));
        });
    }

    partial void OnShowTipsChanged(bool value)
    {
        Task.Run(async () =>
        {
            _settingsService.Get().HideTips = !value;
            await _settingsService.Sync();
            WeakReferenceMessenger.Default.Send(new HideTipsChangedMessage(!value));
        });
    }

    partial void OnShowToolkitLinksChanged(bool value)
    {
        Task.Run(async () =>
        {
            _settingsService.Get().HideToolkitLinks = !value;
            await _settingsService.Sync();
            WeakReferenceMessenger.Default.Send(new HideToolkitLinksChangedMessage(!value));
        });
    }

    partial void OnSelectedCultureChanged(CultureOption? value)
    {
        if (value is null)
        {
            return;
        }

        var settings = _settingsService.Get();
        if (settings.CultureName == value.CultureName)
        {
            return;
        }

        settings.CultureName = value.CultureName;
        LocalizationService.ApplyCulture(value.CultureName);
        WeakReferenceMessenger.Default.Send(new CultureChangedMessage());
        _ = Task.Run(_settingsService.Sync);
    }

    public static FilePickerFileType Saves => new(LocalizationService.Get("FilePicker_SaveFileTypeName"))
    {
        Patterns = ["profile.sav", "containers.index"],
    };
}
