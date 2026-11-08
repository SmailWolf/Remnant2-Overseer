using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using RemnantOverseer.Models;
using RemnantOverseer.Services;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RemnantOverseer.ViewModels;
internal partial class ThaenProgressViewModel: ViewModelBase, IDisposable
{
    private Dictionary<int, TimeSpan> _growthTimespans = new()
    {
        { 1, TimeSpan.FromMinutes(30) },
        { 2, TimeSpan.FromMinutes(60) },
        { 3, TimeSpan.FromMinutes(60) },
        { 4, TimeSpan.FromMinutes(120) },
        { 5, TimeSpan.FromMinutes(120) },
        { 6, TimeSpan.FromMinutes(120) },
        { 7, TimeSpan.Zero },
    };

    // Trying this out instead of a converter
    private Dictionary<int, Bitmap> _growthStageMap = new()
    {
        { 0, new Bitmap(AssetLoader.Open(new Uri($"avares://RemnantOverseer/Assets/Images/Thaen/Thaen_0_dark.PNG"))) },
        { 1, new Bitmap(AssetLoader.Open(new Uri($"avares://RemnantOverseer/Assets/Images/Thaen/Thaen_1_dark.PNG"))) },
        { 2, new Bitmap(AssetLoader.Open(new Uri($"avares://RemnantOverseer/Assets/Images/Thaen/Thaen_2_dark.PNG"))) },
        { 3, new Bitmap(AssetLoader.Open(new Uri($"avares://RemnantOverseer/Assets/Images/Thaen/Thaen_3_dark.PNG"))) },
        { 4, new Bitmap(AssetLoader.Open(new Uri($"avares://RemnantOverseer/Assets/Images/Thaen/Thaen_4_dark.PNG"))) },
        { 5, new Bitmap(AssetLoader.Open(new Uri($"avares://RemnantOverseer/Assets/Images/Thaen/Thaen_4_dark.PNG"))) },
        { 6, new Bitmap(AssetLoader.Open(new Uri($"avares://RemnantOverseer/Assets/Images/Thaen/Thaen_4_dark.PNG"))) },
    };

    private Timer _timer;
    private TimeSpan? _timeLeft;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(GrowthStageImage))]
    [NotifyPropertyChangedFor(nameof(TimeToNextStage))]
    [NotifyPropertyChangedFor(nameof(HasFruit))]
    private ThaenTree? _thaenTree;

    public Bitmap? GrowthStageImage => ThaenTree is null ? _growthStageMap[0] : _growthStageMap[ThaenTree.GrowthStage];

    public bool HasFruit => ThaenTree is not null && ThaenTree.HasFruit;

    [ObservableProperty]
    private string? _timeToNextStage;

#pragma warning disable CS8618
    public ThaenProgressViewModel(ThaenTree? thaenTree)
#pragma warning restore CS8618
    {
        //ThaenTree = new ThaenTree()
        //{
        //    GrowthStage = 1,
        //    HasFruit = false,
        //    Timestamp = DateTime.Now.AddMinutes(-10),
        //    PickedCount = 0,
        //};

        //ThaenTree = new ThaenTree()
        //{
        //    GrowthStage = 2,
        //    HasFruit = false,
        //    Timestamp = DateTime.Now.AddMinutes(-40),
        //    PickedCount = 0,
        //};

        //ThaenTree = new ThaenTree()
        //{
        //    GrowthStage = 3,
        //    HasFruit = false,
        //    Timestamp = DateTime.Now.AddMinutes(-40),
        //    PickedCount = 0,
        //};

        //ThaenTree = new ThaenTree()
        //{
        //    GrowthStage = 4,
        //    HasFruit = true,
        //    Timestamp = DateTime.Now.AddMinutes(-40),
        //    PickedCount = 0,
        //};

        //ThaenTree = new ThaenTree()
        //{
        //    GrowthStage = 5,
        //    HasFruit = true,
        //    Timestamp = DateTime.Now.AddMinutes(-20),
        //    PickedCount = 0,
        //};

        _thaenTree = thaenTree;

        if (Design.IsDesignMode)
        {
            _thaenTree = new ThaenTree()
            {
                GrowthStage = 5,
                HasFruit = true,
                Timestamp = DateTime.Now.AddMinutes(-20),
                PickedCount = 0,
            };
        }

        Log.Instance.Information($"Opening {nameof(ThaenProgressViewModel)} for tree with parameters:" +
            $" GrowthStage:{ThaenTree?.GrowthStage}," +
            $" Timestamp:{ThaenTree?.Timestamp}," +
            $" HasFruit:{ThaenTree?.HasFruit}," +
            $" PickedCount:{ThaenTree?.PickedCount}");
        SetTimer();
    }

    private TimeSpan? GetTimeToNextStage()
    {
        if (ThaenTree == null) return null;
        var timePassed = DateTime.Now - ThaenTree.Timestamp;
        // I thought about checking for 4 here too, but if the state is already corrupted, the data is not going to be trustworthy either way
        var realStage = ThaenTree.HasFruit ? ThaenTree.GrowthStage + 1 : ThaenTree.GrowthStage;
        var timeLeft = _growthTimespans[realStage] - timePassed;
        return timeLeft > TimeSpan.Zero ? timeLeft : TimeSpan.Zero;
    }

    private void SetTimer()
    {
        _timeLeft = GetTimeToNextStage();
        _timer = new Timer(UpdateRemainingTime, null, 0, 1000);
    }

    private void UpdateRemainingTime(object? state)
    {
        TimeToNextStage = _timeLeft is null ? "???" : _timeLeft.Value.ToString(@"h\:mm\:ss");
        if (_timeLeft == TimeSpan.Zero) _timer.Dispose();

        _timeLeft -= TimeSpan.FromSeconds(1);
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}
