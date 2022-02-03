namespace BricksWarehouse.Blazor.Components;

public partial class BricksCalcComponent
{
    private int _brickType;
    public int BrickType
    {
        get => _brickType;
        set
        {
            switch (value)
            {
                case 1:
                    BrickLength = 250; BrickWidth = 120; BrickHeight = 65;
                    break;
                case 2:
                    BrickLength = 250; BrickWidth = 120; BrickHeight = 88;
                    break;
                case 3:
                    BrickLength = 250; BrickWidth = 120; BrickHeight = 138;
                    break;
                case 4:
                    BrickLength = 250; BrickWidth = 88; BrickHeight = 65;
                    break;
            }
            _brickType = value;
        }
    }
    public int BrickLength { get; set; } = 250;
    public int BrickWidth { get; set; } = 120;
    public int BrickHeight { get; set; } = 65;

    public int WallHeight { get; set; } = 200;
    public int WallLength { get; set; } = 500;
    public int WallWidth { get; set; } = 120;
    public int OpeningSpace { get; set; }
    public bool IsPlusFivePerc { get; set; }

    public void CalculateBricks()
    {
        double wallVolume = WallHeight * WallLength * WallWidth; //sm
        double brickVolume = (BrickLength * BrickHeight * BrickWidth) / 1000; //sm
        double openingVolume = OpeningSpace * WallWidth; //sm

        double resultVolume = wallVolume - openingVolume;
        double resultCount = (wallVolume - openingVolume) / brickVolume;

        if (IsPlusFivePerc)
        {
            resultVolume *= 1.05;
            resultCount *= 1.05;
        }
        VolumeBricks = Convert.ToInt32(resultVolume / 1000000.0); //m3
        CountBricks = Convert.ToInt32(resultCount); //count
    }

    public int VolumeBricks { get; set; }
    public int CountBricks { get; set; }
}

