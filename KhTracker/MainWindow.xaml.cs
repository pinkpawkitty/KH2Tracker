﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.IO;
using System.ComponentModel;
using Button = System.Windows.Controls.Button;
using KhTracker.Hotkeys;

namespace KhTracker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public static Data Data;
    public int Collected;
    private int total;
    public int DeathCounter = 0;

    public MainWindow()
    {
        InitializeComponent();
        InitData();
        InitImages();

        collectedChecks = new List<ImportantCheck>();
        newChecks = new List<ImportantCheck>();
        previousChecks = new List<ImportantCheck>();

        InitOptions();
        VisitLockCheck();

        OnReset(null, null);

        //hotkey stuff
        HotkeysManager.SetupSystemHook();
        LoadHotkeyBind();

        //start auto-connect if enabled
        AutoConnectOption.IsChecked = Properties.Settings.Default.AutoConnect;
        if (AutoConnectOption.IsChecked)
            InitTracker();
    }

    private void InitData()
    {
        Data = new Data();

        Data.TornPages.Add(TornPage1);
        Data.TornPages.Add(TornPage2);
        Data.TornPages.Add(TornPage3);
        Data.TornPages.Add(TornPage4);
        Data.TornPages.Add(TornPage5);

        Data.VisitLocks.Add(AuronWep);
        Data.VisitLocks.Add(MulanWep);
        Data.VisitLocks.Add(BeastWep);
        Data.VisitLocks.Add(JackWep);
        Data.VisitLocks.Add(SimbaWep);
        Data.VisitLocks.Add(SparrowWep);
        Data.VisitLocks.Add(AladdinWep);
        Data.VisitLocks.Add(TronWep);
        Data.VisitLocks.Add(MembershipCard);
        Data.VisitLocks.Add(IceCream);
        Data.VisitLocks.Add(Picture);

        Data.WorldsData.Add(
            "SorasHeart",
            new WorldData(SorasHeartTop, null, SorasHeartHint, SorasHeartGrid, 0)
        );
        Data.WorldsData.Add(
            "DriveForms",
            new WorldData(DriveFormsTop, null, DriveFormsHint, DriveFormsGrid, 0)
        );
        Data.WorldsData.Add(
            "SimulatedTwilightTown",
            new WorldData(
                SimulatedTwilightTownTop,
                SimulatedTwilightTownProgression,
                SimulatedTwilightTownHint,
                SimulatedTwilightTownGrid,
                0
            )
        );
        Data.WorldsData.Add(
            "TwilightTown",
            new WorldData(
                TwilightTownTop,
                TwilightTownProgression,
                TwilightTownHint,
                TwilightTownGrid,
                0
            )
        );
        Data.WorldsData.Add(
            "HollowBastion",
            new WorldData(
                HollowBastionTop,
                HollowBastionProgression,
                HollowBastionHint,
                HollowBastionGrid,
                0
            )
        );
        Data.WorldsData.Add(
            "BeastsCastle",
            new WorldData(
                BeastsCastleTop,
                BeastsCastleProgression,
                BeastsCastleHint,
                BeastsCastleGrid,
                0
            )
        );
        Data.WorldsData.Add(
            "OlympusColiseum",
            new WorldData(
                OlympusColiseumTop,
                OlympusColiseumProgression,
                OlympusColiseumHint,
                OlympusColiseumGrid,
                0
            )
        );
        Data.WorldsData.Add(
            "Agrabah",
            new WorldData(AgrabahTop, AgrabahProgression, AgrabahHint, AgrabahGrid, 0)
        );
        Data.WorldsData.Add(
            "LandofDragons",
            new WorldData(
                LandofDragonsTop,
                LandofDragonsProgression,
                LandofDragonsHint,
                LandofDragonsGrid,
                0
            )
        );
        Data.WorldsData.Add(
            "HundredAcreWood",
            new WorldData(
                HundredAcreWoodTop,
                HundredAcreWoodProgression,
                HundredAcreWoodHint,
                HundredAcreWoodGrid,
                0
            )
        );
        Data.WorldsData.Add(
            "PrideLands",
            new WorldData(PrideLandsTop, PrideLandsProgression, PrideLandsHint, PrideLandsGrid, 0)
        );
        Data.WorldsData.Add(
            "DisneyCastle",
            new WorldData(
                DisneyCastleTop,
                DisneyCastleProgression,
                DisneyCastleHint,
                DisneyCastleGrid,
                0
            )
        );
        Data.WorldsData.Add(
            "HalloweenTown",
            new WorldData(
                HalloweenTownTop,
                HalloweenTownProgression,
                HalloweenTownHint,
                HalloweenTownGrid,
                0
            )
        );
        Data.WorldsData.Add(
            "PortRoyal",
            new WorldData(PortRoyalTop, PortRoyalProgression, PortRoyalHint, PortRoyalGrid, 0)
        );
        Data.WorldsData.Add(
            "SpaceParanoids",
            new WorldData(
                SpaceParanoidsTop,
                SpaceParanoidsProgression,
                SpaceParanoidsHint,
                SpaceParanoidsGrid,
                0
            )
        );
        Data.WorldsData.Add(
            "TWTNW",
            new WorldData(TwtnwTop, TwtnwProgression, TwtnwHint, TwtnwGrid, 0)
        );
        Data.WorldsData.Add("GoA", new WorldData(GoATop, GoAProgression, GoAHint, GoAGrid, 0));
        Data.WorldsData.Add(
            "Atlantica",
            new WorldData(AtlanticaTop, AtlanticaProgression, AtlanticaHint, AtlanticaGrid, 0)
        );
        Data.WorldsData.Add(
            "PuzzSynth",
            new WorldData(PuzzSynthTop, null, PuzzSynthHint, PuzzSynthGrid, 0)
        );

        Data.ProgressKeys.Add(
            "SimulatedTwilightTown",
            new List<string>
            {
                "",
                "Chests",
                "Minigame",
                "TwilightThorn",
                "Axel1",
                "Struggle",
                "ComputerRoom",
                "Axel",
                "DataRoxas"
            }
        );
        Data.ProgressKeys.Add(
            "TwilightTown",
            new List<string>
            {
                "",
                "Chests",
                "Station",
                "MysteriousTower",
                "Sandlot",
                "Mansion",
                "BetwixtAndBetween",
                "DataAxel"
            }
        );
        Data.ProgressKeys.Add(
            "HollowBastion",
            new List<string>
            {
                "",
                "Chests",
                "Bailey",
                "AnsemStudy",
                "Corridor",
                "Dancers",
                "HBDemyx",
                "FinalFantasy",
                "1000Heartless",
                "Sephiroth",
                "DataDemyx",
                "SephiDemyx"
            }
        );
        Data.ProgressKeys.Add(
            "BeastsCastle",
            new List<string>
            {
                "",
                "Chests",
                "Thresholder",
                "Beast",
                "DarkThorn",
                "Dragoons",
                "Xaldin",
                "DataXaldin"
            }
        );
        Data.ProgressKeys.Add(
            "OlympusColiseum",
            new List<string>
            {
                "",
                "Chests",
                "Cerberus",
                "Urns",
                "OCDemyx",
                "OCPete",
                "Hydra",
                "AuronStatue",
                "Hades",
                "Zexion",
                "ZexionData"
            }
        );
        Data.ProgressKeys.Add(
            "Agrabah",
            new List<string>
            {
                "",
                "Chests",
                "Abu",
                "Chasm",
                "TreasureRoom",
                "Lords",
                "Carpet",
                "GenieJafar",
                "Lexaeus",
                "LexaeusData"
            }
        );
        Data.ProgressKeys.Add(
            "LandofDragons",
            new List<string>
            {
                "",
                "Chests",
                "Missions",
                "Mountain",
                "Cave",
                "Summit",
                "ShanYu",
                "ThroneRoom",
                "StormRider",
                "DataXigbar"
            }
        );
        Data.ProgressKeys.Add(
            "HundredAcreWood",
            new List<string>
            {
                "",
                "Chests",
                "Piglet",
                "Rabbit",
                "Kanga",
                "SpookyCave",
                "StarryHill"
            }
        );
        Data.ProgressKeys.Add(
            "PrideLands",
            new List<string>
            {
                "",
                "Chests",
                "Simba",
                "Hyenas1",
                "Scar",
                "Hyenas2",
                "GroundShaker",
                "DataSaix"
            }
        );
        Data.ProgressKeys.Add(
            "DisneyCastle",
            new List<string>
            {
                "",
                "Chests",
                "Minnie",
                "OldPete",
                "Windows",
                "BoatPete",
                "DCPete",
                "Marluxia",
                "MarluxiaData",
                "LingeringWill",
                "Marluxia_LingeringWill",
                "MarluxiaData_LingeringWill"
            }
        );
        Data.ProgressKeys.Add(
            "HalloweenTown",
            new List<string>
            {
                "",
                "Chests",
                "CandyCaneLane",
                "PrisonKeeper",
                "OogieBoogie",
                "Children",
                "Presents",
                "Experiment",
                "Vexen",
                "VexenData"
            }
        );
        Data.ProgressKeys.Add(
            "PortRoyal",
            new List<string>
            {
                "",
                "Chests",
                "Town",
                "1Minute",
                "Medallions",
                "Barrels",
                "Barbossa",
                "GrimReaper1",
                "Gambler",
                "GrimReaper",
                "DataLuxord"
            }
        );
        Data.ProgressKeys.Add(
            "SpaceParanoids",
            new List<string>
            {
                "",
                "Chests",
                "Screens",
                "HostileProgram",
                "SolarSailer",
                "MCP",
                "Larxene",
                "LarxeneData"
            }
        );
        Data.ProgressKeys.Add(
            "TWTNW",
            new List<string>
            {
                "",
                "Chests",
                "Roxas",
                "Xigbar",
                "Luxord",
                "Saix",
                "Xemnas1",
                "DataXemnas"
            }
        );
        Data.ProgressKeys.Add("Atlantica", new List<string> { "", "Tutorial", "Ursula", "NewDay" });
        Data.ProgressKeys.Add(
            "GoA",
            new List<string> { "", "Chests", "Fight1", "Fight2", "Transport", "Valves" }
        );

        //testing tooltip descriptions
        //(adding to existing list above cause it's easier)
        Data.ProgressKeys.Add(
            "SimulatedTwilightTownDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Part-Time Job",
                "Twilight Thorn (Boss)",
                "Axel 1 (Boss)",
                "Beat Setzer",
                "Mansion Computer Room",
                "Axel 2 (Boss)",
                "Roxas (Data)"
            }
        );
        Data.ProgressKeys.Add(
            "TwilightTownDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Train Station Fight",
                "Mysterious Tower",
                "Sandlot Fight",
                "Mansion Gate Fight",
                "Betwixt And Between",
                "Axel (Data)"
            }
        );
        Data.ProgressKeys.Add(
            "HollowBastionDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Bailey Gate Fight",
                "Ansem's Study",
                "Corridor Fight",
                "Restoration Site Fight",
                "Demyx (Boss)",
                "Final Fantasy Fights",
                "1000 Heartless",
                "Sephiroth (Boss)",
                "Demyx (Data)",
                "Sephiroth and Demyx"
            }
        );
        Data.ProgressKeys.Add(
            "BeastsCastleDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Thresholder (Boss)",
                "Beast Fight",
                "Dark Thorn (Boss)",
                "Dragoons Forced Fight",
                "Xaldin (Boss)",
                "Xaldin (Data)"
            }
        );
        Data.ProgressKeys.Add(
            "OlympusColiseumDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Cerberus (Boss)",
                "Phil's Training",
                "Demyx Fight",
                "Pete Fight",
                "Hydra (Boss)",
                "Hades' Chamber Fight",
                "Hades (Boss)",
                "Zexion (AS)",
                "Zexion (Data)"
            }
        );
        Data.ProgressKeys.Add(
            "AgrabahDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Abu Minigame",
                "Chasm of Challenges",
                "Treasure Room Fight",
                "Twin Lords (Boss)",
                "Carpet Magic Minigame",
                "Genie Jafar (Boss)",
                "Lexaeus (AS)",
                "Lexaeus (Data)"
            }
        );
        Data.ProgressKeys.Add(
            "LandofDragonsDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Mission 3 (The Search)",
                "Mountain Climb",
                "Town Cave Fight",
                "Summmit Fight",
                "Shan Yu (Boss)",
                "Throne Room",
                "Storm Rider (Boss)",
                "Xigbar (Data)"
            }
        );
        Data.ProgressKeys.Add(
            "HundredAcreWoodDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Entered Piglet's House",
                "Entered Rabbit's House",
                "Entered Kanga's House",
                "Entered Spooky Cave",
                "Entered Starry Hill"
            }
        );
        Data.ProgressKeys.Add(
            "PrideLandsDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Met Simba",
                "Hyenas Fight (1st Visit)",
                "Scar (Boss)",
                "Hyenas Fight (2nd Visit)",
                "Ground Shaker (Boss)",
                "Saix (Data)"
            }
        );
        Data.ProgressKeys.Add(
            "DisneyCastleDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Minnie Escort",
                "Past Pete Fight",
                "Windows of Time",
                "Steamboat Fight",
                "Pete (Boss)",
                "Marluxia (AS)",
                "Marluxia (Data)",
                "Lingering Will (Boss)",
                "Marluxia (AS) and Lingering Will",
                "Marluxia (Data) and Lingering Will"
            }
        );
        Data.ProgressKeys.Add(
            "HalloweenTownDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Candy Cane Lane Fight",
                "Prison Keeper (Boss)",
                "Oogie Boogie (Boss)",
                "Lock, Shock, and Barrel",
                "Made Decoy Presents",
                "The Experiment (Boss)",
                "Vexen (AS)",
                "Vexen (Data)"
            }
        );
        Data.ProgressKeys.Add(
            "PortRoyalDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Town Fight",
                "1 Minute Isle Fight",
                "Interceptor Medallion Fight",
                "Interceptor Barrels",
                "Barbossa (Boss)",
                "Grim Reaper 1 (Boss)",
                "1st Gambler Medallion",
                "Grim Reaper 2 (Boss)",
                "Luxord (Data)"
            }
        );
        Data.ProgressKeys.Add(
            "SpaceParanoidsDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Dataspace Fight",
                "Hostile Program (Boss)",
                "Solar Sailer",
                "MCP (Boss)",
                "Larxene (AS)",
                "Larxene (Data)"
            }
        );
        Data.ProgressKeys.Add(
            "TWTNWDesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Roxas (Boss)",
                "Xigbar (Boss)",
                "Luxord (Boss)",
                "Saix (Boss)",
                "Xemnas 1 (Boss)",
                "Xemnas (Data)"
            }
        );
        Data.ProgressKeys.Add(
            "AtlanticaDesc",
            new List<string> { "", "Music Tutorial", "Ursula's Revenge", "A New Day is Dawning" }
        );
        Data.ProgressKeys.Add(
            "GoADesc",
            new List<string>
            {
                "",
                "Early Checks",
                "Forced Fight 1",
                "Forced Fight 2",
                "Transport to Rememberance",
                "Steam Valves (CoR Skip)"
            }
        );

        foreach (Grid itemrow in ItemPool.Children)
        {
            foreach (var item in itemrow.Children)
            {
                if (item is Item check)
                {
                    if (!check.Name.StartsWith("Ghost_"))
                    {
                        Data.Items.Add(
                            check.Name,
                            new Tuple<Item, Grid>(check, check.Parent as Grid)
                        ); //list of all valid items
                        //data.ItemsGrid.Add(check.Parent as Grid);   //list of grids each item belongs to
                        ++total;
                        //Console.WriteLine(check.Name);
                    }
                    else
                    {
                        Data.GhostItems.Add(check.Name, check); //list of all valid ghost items (why is this a dictionary????)
                    }
                }
            }
        }
    }

    private void InitOptions()
    {
        #region Options

        TopMostOption.IsChecked = Properties.Settings.Default.TopMost;
        TopMostToggle(null, null);

        DragAndDropOption.IsChecked = Properties.Settings.Default.DragDrop;
        DragDropToggle(null, null);

        AutoSaveProgressOption.IsChecked = Properties.Settings.Default.AutoSaveProgress;
        AutoSaveProgress2Option.IsChecked = Properties.Settings.Default.AutoSaveProgress;

        #endregion

        #region Toggles

        //Items
        PromiseCharmOption.IsChecked = Properties.Settings.Default.PromiseCharm;
        PromiseCharmToggle(PromiseCharmOption.IsChecked);

        AbilitiesOption.IsChecked = Properties.Settings.Default.Abilities;
        AbilitiesToggle(AbilitiesOption.IsChecked);

        AntiFormOption.IsChecked = Properties.Settings.Default.AntiForm;
        AntiFormToggle(AntiFormOption.IsChecked);

        VisitLockOption.IsChecked = Properties.Settings.Default.WorldVisitLock;
        VisitLockToggle(VisitLockOption.IsChecked);

        TornPagesOption.IsChecked = Properties.Settings.Default.TornPages;
        TornPagesToggle(TornPagesOption.IsChecked);

        ExtraChecksOption.IsChecked = Properties.Settings.Default.ExtraChecks;
        ExtraChecksToggle(ExtraChecksOption.IsChecked);

        //Visual
        SeedHashOption.IsChecked = Properties.Settings.Default.SeedHash;
        SeedHashToggle(SeedHashOption.IsChecked);

        WorldProgressOption.IsChecked = Properties.Settings.Default.WorldProgress;
        WorldProgressToggle(null, null);

        FormsGrowthOption.IsChecked = Properties.Settings.Default.FormsGrowth;
        FormsGrowthToggle(null, null);

        //Levelvisuals
        NextLevelCheckOption.IsChecked = Properties.Settings.Default.NextLevelCheck;
        NextLevelCheckToggle(NextLevelCheckOption.IsChecked);

        DeathCounterOption.IsChecked = Properties.Settings.Default.DeathCounter;
        DeathCounterToggle(DeathCounterOption.IsChecked);

        SoraLevel01Option.IsChecked = Properties.Settings.Default.WorldLevel1;
        SoraLevel50Option.IsChecked = Properties.Settings.Default.WorldLevel50;
        SoraLevel99Option.IsChecked = Properties.Settings.Default.WorldLevel99;
        if (SoraLevel01Option.IsChecked)
            SoraLevel01Toggle(null, null);
        if (SoraLevel50Option.IsChecked)
            SoraLevel50Toggle(null, null);
        if (SoraLevel99Option.IsChecked)
            SoraLevel99Toggle(null, null);

        WorldHighlightOption.IsChecked = Properties.Settings.Default.WorldHighlight;

        //message box
        Disconnect.IsChecked = Properties.Settings.Default.Disconnect;
        DisconnectToggle(Disconnect.IsChecked);

        #endregion

        #region Visual

        NewWorldLayoutOption.IsChecked = Properties.Settings.Default.NewWorldLayout;
        if (NewWorldLayoutOption.IsChecked)
            NewWorldLayoutToggle(null, null);

        OldWorldLayoutOption.IsChecked = Properties.Settings.Default.OldWorldLayout;
        if (OldWorldLayoutOption.IsChecked)
            OldWorldLayoutToggle(null, null);

        MinWorldOption.IsChecked = Properties.Settings.Default.MinWorld;
        if (MinWorldOption.IsChecked)
            MinWorldToggle(null, null);

        OldWorldOption.IsChecked = Properties.Settings.Default.OldWorld;
        if (OldWorldOption.IsChecked)
            OldWorldToggle(null, null);

        MinCheckOption.IsChecked = Properties.Settings.Default.MinCheck;
        if (MinCheckOption.IsChecked)
            MinCheckToggle(null, null);

        OldCheckOption.IsChecked = Properties.Settings.Default.OldCheck;
        if (OldCheckOption.IsChecked)
            OldCheckToggle(null, null);

        MinProgOption.IsChecked = Properties.Settings.Default.MinProg;
        if (MinProgOption.IsChecked)
            MinProgToggle(null, null);

        OldProgOption.IsChecked = Properties.Settings.Default.OldProg;
        if (OldProgOption.IsChecked)
            OldProgToggle(null, null);

        CustomFolderOption.IsChecked = Properties.Settings.Default.CustomIcons;
        CustomImageToggle(null, null);

        ColorHintOption.IsChecked = Properties.Settings.Default.ColorHints;

        //testing background settings stuff (i thought this would be simplier than the above methods)
        //maybe i was wrong. (at least everything is done by 2 settings instead of 8)
        var mainBg = Properties.Settings.Default.MainBG;
        switch (mainBg)
        {
            case 1:
                MainDefOption.IsChecked = false;
                MainImg1Option.IsChecked = true;
                MainImg2Option.IsChecked = false;
                MainImg3Option.IsChecked = false;
                MainBG_Img1Toggle(null, null);
                break;
            case 2:
                MainDefOption.IsChecked = false;
                MainImg1Option.IsChecked = false;
                MainImg2Option.IsChecked = true;
                MainImg3Option.IsChecked = false;
                MainBG_Img2Toggle(null, null);
                break;
            case 3:
                MainDefOption.IsChecked = false;
                MainImg1Option.IsChecked = false;
                MainImg2Option.IsChecked = false;
                MainImg3Option.IsChecked = true;
                MainBG_Img3Toggle(null, null);
                break;
            default:
                MainDefOption.IsChecked = true;
                MainImg1Option.IsChecked = false;
                MainImg2Option.IsChecked = false;
                MainImg3Option.IsChecked = false;
                MainBG_DefToggle(null, null);
                break;
        }

        #endregion

        #region Worlds

        SoraHeartOption.IsChecked = Properties.Settings.Default.SoraHeart;
        SoraHeartToggle(SoraHeartOption.IsChecked);

        DrivesOption.IsChecked = Properties.Settings.Default.Drives;
        DrivesToggle(DrivesOption.IsChecked);

        SimulatedOption.IsChecked = Properties.Settings.Default.Simulated;
        SimulatedToggle(SimulatedOption.IsChecked);

        TwilightTownOption.IsChecked = Properties.Settings.Default.TwilightTown;
        TwilightTownToggle(TwilightTownOption.IsChecked);

        HollowBastionOption.IsChecked = Properties.Settings.Default.HollowBastion;
        HollowBastionToggle(HollowBastionOption.IsChecked);

        BeastCastleOption.IsChecked = Properties.Settings.Default.BeastCastle;
        BeastCastleToggle(BeastCastleOption.IsChecked);

        OlympusOption.IsChecked = Properties.Settings.Default.Olympus;
        OlympusToggle(OlympusOption.IsChecked);

        AgrabahOption.IsChecked = Properties.Settings.Default.Agrabah;
        AgrabahToggle(AgrabahOption.IsChecked);

        LandofDragonsOption.IsChecked = Properties.Settings.Default.LandofDragons;
        LandofDragonsToggle(LandofDragonsOption.IsChecked);

        DisneyCastleOption.IsChecked = Properties.Settings.Default.DisneyCastle;
        DisneyCastleToggle(DisneyCastleOption.IsChecked);

        PrideLandsOption.IsChecked = Properties.Settings.Default.PrideLands;
        PrideLandsToggle(PrideLandsOption.IsChecked);

        PortRoyalOption.IsChecked = Properties.Settings.Default.PortRoyal;
        PortRoyalToggle(PortRoyalOption.IsChecked);

        HalloweenTownOption.IsChecked = Properties.Settings.Default.HalloweenTown;
        HalloweenTownToggle(HalloweenTownOption.IsChecked);

        SpaceParanoidsOption.IsChecked = Properties.Settings.Default.SpaceParanoids;
        SpaceParanoidsToggle(SpaceParanoidsOption.IsChecked);

        TwtnwOption.IsChecked = Properties.Settings.Default.TWTNW;
        TwtnwToggle(TwtnwOption.IsChecked);

        HundredAcreWoodOption.IsChecked = Properties.Settings.Default.HundredAcre;
        HundredAcreWoodToggle(HundredAcreWoodOption.IsChecked);

        AtlanticaOption.IsChecked = Properties.Settings.Default.Atlantica;
        AtlanticaToggle(AtlanticaOption.IsChecked);

        PuzzleOption.IsChecked = Properties.Settings.Default.Puzzle;
        PuzzleToggle(PuzzleOption.IsChecked);

        SynthOption.IsChecked = Properties.Settings.Default.Synth;
        SynthToggle(SynthOption.IsChecked);

        #endregion

        Top = Properties.Settings.Default.WindowY;
        Left = Properties.Settings.Default.WindowX;

        Width = Properties.Settings.Default.Width;
        Height = Properties.Settings.Default.Height;

        EmitProofKeystrokeOption.IsChecked = Properties.Settings.Default.EmitProofKeystroke;
        if (EmitProofKeystrokeOption.IsChecked)
            EmitProofKeystrokeToggle(null, null);
    }

    ///
    /// Input Handling
    ///
    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var button = sender as Button;
        Button preButton = null;
        switch (e.ChangedButton)
        {
            case MouseButton.Left: //for changing world selection visuals
                if (Data.Selected != null) //set previousl selected world to default colors
                {
                    preButton = Data.Selected;
                    foreach (
                        var box in Data.WorldsData[
                            Data.Selected.Name
                        ].Top.Children.OfType<Rectangle>()
                    )
                    {
                        if (Math.Abs(box.Opacity - 0.9) > 0.0001 && !box.Name.EndsWith("SelWG"))
                            box.Fill = (SolidColorBrush)FindResource("DefaultRec");

                        if (box.Name.EndsWith("SelWG") && !WorldHighlightOption.IsChecked)
                            box.Visibility = Visibility.Collapsed;
                    }
                }
                Data.Selected = button;
                if (preButton != null && preButton == button)
                {
                    foreach (
                        var box in Data.WorldsData[
                            Data.Selected.Name
                        ].Top.Children.OfType<Rectangle>()
                    )
                    {
                        if (Math.Abs(box.Opacity - 0.9) > 0.0001 && !box.Name.EndsWith("SelWG"))
                            box.Fill = (SolidColorBrush)FindResource("DefaultRec");

                        if (box.Name.EndsWith("SelWG") && !WorldHighlightOption.IsChecked)
                            box.Visibility = Visibility.Collapsed;
                    }
                    Data.Selected = null;
                }
                else
                {
                    foreach (
                        var box in Data.WorldsData[button!.Name].Top.Children.OfType<Rectangle>()
                    ) //set currently selected world colors
                    {
                        if (Math.Abs(box.Opacity - 0.9) > 0.0001 && !box.Name.EndsWith("SelWG"))
                            box.Fill = (SolidColorBrush)FindResource("SelectedRec");

                        if (box.Name.EndsWith("SelWG") && !WorldHighlightOption.IsChecked)
                            box.Visibility = Visibility.Visible;
                    }
                }
                break;
            case MouseButton.Right: //for setting world cross icon
                if (Data.WorldsData.ContainsKey(button.Name))
                {
                    //string crossname = button.Name + "Cross";
                    //
                    //if (data.WorldsData[button.Name].top.FindName(crossname) is Image Cross)
                    //{
                    //    if (Cross.Visibility == Visibility.Collapsed)
                    //        Cross.Visibility = Visibility.Visible;
                    //    else
                    //        Cross.Visibility = Visibility.Collapsed;
                    //}

                    var crossname = button.Name + "Cross";
                    var questionname = button.Name + "Question";

                    if (
                        Data.WorldsData[button.Name].Top.FindName(crossname) is Image cross
                        && Data.WorldsData[button.Name].Top.FindName(questionname) is Image question
                    )
                    {
                        switch (cross.Visibility)
                        {
                            //if it's nothing, set to the cross
                            case Visibility.Collapsed
                                when question.Visibility == Visibility.Collapsed:
                                cross.Visibility = Visibility.Visible;
                                question.Visibility = Visibility.Collapsed;
                                break;
                            case Visibility.Visible
                                when question.Visibility == Visibility.Collapsed:
                                question.Visibility = Visibility.Visible;
                                cross.Visibility = Visibility.Collapsed;
                                break;
                            default:
                                cross.Visibility = Visibility.Collapsed;
                                question.Visibility = Visibility.Collapsed;
                                break;
                        }
                    }
                }
                break;
            case MouseButton.Middle: //setting world value back to "?" if not using any hints
                if (
                    Data.WorldsData.ContainsKey(button!.Name)
                    && Data.WorldsData[button.Name].Value != null
                    && Data.Mode == Mode.None
                )
                {
                    Data.WorldsData[button.Name].Value.Text = "?";
                }
                break;
        }
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var button = sender as Button;

        if (Data.WorldsData.ContainsKey(button!.Name) && Data.WorldsData[button.Name].Value != null)
        {
            ManualWorldValue(Data.WorldsData[button.Name].Value, e.Delta);
        }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.PageDown when Data.Selected != null:
            {
                if (
                    Data.WorldsData.ContainsKey(Data.Selected.Name)
                    && Data.WorldsData[Data.Selected.Name].Value != null
                )
                {
                    SetWorldValue(Data.WorldsData[Data.Selected.Name].Value, -1);
                }

                break;
            }
            case Key.PageUp when Data.Selected != null:
            {
                if (
                    Data.WorldsData.ContainsKey(Data.Selected.Name)
                    && Data.WorldsData[Data.Selected.Name].Value != null
                )
                {
                    SetWorldValue(Data.WorldsData[Data.Selected.Name].Value, 1);
                }

                break;
            }
        }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        if (AutoSaveProgressOption.IsChecked)
        {
            if (!Directory.Exists("KhTrackerAutoSaves"))
            {
                Directory.CreateDirectory("KhTrackerAutoSaves\\");
            }

            Save(
                "KhTrackerAutoSaves\\"
                    + "Tracker-Backup_"
                    + DateTime.Now.ToString("yy-MM-dd_H-m")
                    + ".tsv"
            );
        }
        Properties.Settings.Default.Save();
    }

    private void Window_LocationChanged(object sender, EventArgs e)
    {
        Properties.Settings.Default.WindowY = RestoreBounds.Top;
        Properties.Settings.Default.WindowX = RestoreBounds.Left;
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        Properties.Settings.Default.Width = RestoreBounds.Width;
        Properties.Settings.Default.Height = RestoreBounds.Height;
    }

    private void ResetSize(object sender, RoutedEventArgs e)
    {
        Width = 570;
        Height = 880;
    }

    ///
    /// Handle UI Changes
    ///

    //Used for when no hints are loaded. use the scroll wheel to change world number.
    private void ManualWorldValue(OutlinedTextBlock hint, int delta)
    {
        //return if the a hint mode is loaded
        if (Data.Mode != Mode.None && !hint.Name.Contains("GoA"))
            return;

        int num;

        //get current number
        if (hint.Text == "?")
        {
            if (delta > 0 && hint.Name.Contains("GoA"))
                num = -1; // if adding then start at -1 so next number is 0
            else
                num = 0; //if subtracting start at 0 so next number is -1
        }
        else
            num = int.Parse(hint.Text);

        if (delta > 0)
            ++num;
        else
            --num;

        hint.Text = num.ToString();
    }

    public void SetWorldValue(OutlinedTextBlock worldValue, int value)
    {
        var color = (SolidColorBrush)FindResource("DefaultWhite"); //default

        if (worldValue.Name.Contains("GoA"))
        {
            worldValue.Fill = (SolidColorBrush)FindResource("ClassicYellow");

            worldValue.Text = value == -999999 ? "?" : value.ToString();
            return;
        }

        worldValue.Fill = color;

        worldValue.Text = value == -999999 ? "?" : value.ToString();
    }

    public void SetCollected(bool add)
    {
        if (add)
            ++Collected;
        else
            --Collected;

        CollectedValue.Text = Collected.ToString();
    }

    public void SetTotal(bool add)
    {
        if (add)
            ++total;
        else
            --total;

        TotalValue.Text = total.ToString();
    }

    public void VisitLockCheck()
    {
        //we use this to check the current lock state and set lock visuals as needed while doing so
        foreach (var worldName in Data.WorldsData.Keys.ToList())
        {
            //could probably be handled better. oh well
            switch (worldName)
            {
                case "TwilightTown":
                    switch (Data.WorldsData["TwilightTown"].VisitLocks)
                    {
                        case 0:
                            TwilightTownLock1.Visibility = Visibility.Collapsed;
                            TwilightTownLock2.Visibility = Visibility.Collapsed;
                            TwilightTown.Opacity = 1;
                            break;
                        case 1:
                            TwilightTownLock1.Visibility = Visibility.Visible;
                            TwilightTownLock2.Visibility = Visibility.Collapsed;
                            TwilightTown.Opacity = 0.45;
                            break;
                        case 10:
                            TwilightTownLock1.Visibility = Visibility.Collapsed;
                            TwilightTownLock2.Visibility = Visibility.Visible;
                            TwilightTown.Opacity = 0.45;
                            break;
                        default:
                            TwilightTownLock1.Visibility = Visibility.Visible;
                            TwilightTownLock2.Visibility = Visibility.Visible;
                            TwilightTown.Opacity = 0.45;
                            break;
                    }
                    break;
                case "HollowBastion":
                    switch (Data.WorldsData["HollowBastion"].VisitLocks)
                    {
                        case 0:
                            HollowBastionLock.Visibility = Visibility.Hidden;
                            HollowBastion.Opacity = 1;
                            break;
                        default:
                            HollowBastionLock.Visibility = Visibility.Visible;
                            HollowBastion.Opacity = 0.45;
                            break;
                    }
                    break;
                case "BeastsCastle":
                    switch (Data.WorldsData["BeastsCastle"].VisitLocks)
                    {
                        case 0:
                            BeastsCastleLock.Visibility = Visibility.Hidden;
                            BeastsCastle.Opacity = 1;
                            break;
                        default:
                            BeastsCastleLock.Visibility = Visibility.Visible;
                            BeastsCastle.Opacity = 0.45;
                            break;
                    }
                    break;
                case "OlympusColiseum":
                    switch (Data.WorldsData["OlympusColiseum"].VisitLocks)
                    {
                        case 0:
                            OlympusColiseumLock.Visibility = Visibility.Hidden;
                            OlympusColiseum.Opacity = 1;
                            break;
                        default:
                            OlympusColiseumLock.Visibility = Visibility.Visible;
                            OlympusColiseum.Opacity = 0.45;
                            break;
                    }
                    break;
                case "Agrabah":
                    switch (Data.WorldsData["Agrabah"].VisitLocks)
                    {
                        case 0:
                            AgrabahLock.Visibility = Visibility.Hidden;
                            Agrabah.Opacity = 1;
                            break;
                        default:
                            AgrabahLock.Visibility = Visibility.Visible;
                            Agrabah.Opacity = 0.45;
                            break;
                    }
                    break;
                case "LandofDragons":
                    switch (Data.WorldsData["LandofDragons"].VisitLocks)
                    {
                        case 0:
                            LandofDragonsLock.Visibility = Visibility.Hidden;
                            LandofDragons.Opacity = 1;
                            break;
                        default:
                            LandofDragonsLock.Visibility = Visibility.Visible;
                            LandofDragons.Opacity = 0.45;
                            break;
                    }
                    break;
                case "PrideLands":
                    switch (Data.WorldsData["PrideLands"].VisitLocks)
                    {
                        case 0:
                            PrideLandsLock.Visibility = Visibility.Hidden;
                            PrideLands.Opacity = 1;
                            break;
                        default:
                            PrideLandsLock.Visibility = Visibility.Visible;
                            PrideLands.Opacity = 0.45;
                            break;
                    }
                    break;
                case "HalloweenTown":
                    switch (Data.WorldsData["HalloweenTown"].VisitLocks)
                    {
                        case 0:
                            HalloweenTownLock.Visibility = Visibility.Hidden;
                            HalloweenTown.Opacity = 1;
                            break;
                        default:
                            HalloweenTownLock.Visibility = Visibility.Visible;
                            HalloweenTown.Opacity = 0.45;
                            break;
                    }
                    break;
                case "PortRoyal":
                    switch (Data.WorldsData["PortRoyal"].VisitLocks)
                    {
                        case 0:
                            PortRoyalLock.Visibility = Visibility.Hidden;
                            PortRoyal.Opacity = 1;
                            break;
                        default:
                            PortRoyalLock.Visibility = Visibility.Visible;
                            PortRoyal.Opacity = 0.45;
                            break;
                    }
                    break;
                case "SpaceParanoids":
                    switch (Data.WorldsData["SpaceParanoids"].VisitLocks)
                    {
                        case 0:
                            SpaceParanoidsLock.Visibility = Visibility.Hidden;
                            SpaceParanoids.Opacity = 1;
                            break;
                        default:
                            SpaceParanoidsLock.Visibility = Visibility.Visible;
                            SpaceParanoids.Opacity = 0.45;
                            break;
                    }
                    break;
            }
        }
    }
}
