using System;

namespace RemnantOverseer.Models;
public class ThaenTree
{
    public int GrowthStage { get; set; }

    public DateTime Timestamp { get; set; }

    public bool HasFruit { get; set; }

    public int PickedCount { get; set; }
}
