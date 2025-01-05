using Godot;

[Tool]
public partial class UpgradeData : Resource
{
    [Export(PropertyHint.File)] public string TexturePath;
    [Export] public string Name;
    [Export] public string Description;
    [Export] public int Cost;
    [Export] public bool IsBought;
    [Export] public bool IsEnabled;
    [Export] public int UpgradeStage;
    [Export] public int SqauresClicked;
    [Export(PropertyHint.File, ("*.tres"))] public string PathToResource;
}