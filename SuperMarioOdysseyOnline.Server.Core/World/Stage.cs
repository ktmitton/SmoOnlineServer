namespace SuperMarioOdysseyOnline.Server.Core.World;

[AttributeUsage(AttributeTargets.Field)]
public class StageAttribute(Kingdom kingdom, bool isHomeStage = false) : Attribute
{
    public Kingdom Kingdom => kingdom;

    public bool IsHomeStage => isHomeStage;
}

public enum Kingdom
{
    Cap,
    Cascade,
    Sand,
    Lake,
    Wooded,
    Cloud,
    Lost,
    Metro,
    Snow,
    Seaside,
    Luncheon,
    Ruined,
    Bowsers,
    Moon,
    Mushroom,
    DarkSide,
    DarkerSide,
    Odyssey,
}

public enum Stage
{
    [Stage(Kingdom.Cap, true)]
    CapWorldHomeStage,

    [Stage(Kingdom.Cap)]
    CapWorldTowerStage,

    [Stage(Kingdom.Cap)]
    FrogSearchExStage,

    [Stage(Kingdom.Cap)]
    PoisonWaveExStage,

    [Stage(Kingdom.Cap)]
    PushBlockExStage,

    [Stage(Kingdom.Cap)]
    RollingExStage,

    [Stage(Kingdom.Cascade, true)]
    WaterfallWorldHomeStage,

    [Stage(Kingdom.Cascade)]
    TrexPoppunExStage,

    [Stage(Kingdom.Cascade)]
    Lift2DExStage,

    [Stage(Kingdom.Cascade)]
    WanwanClashExStage,

    [Stage(Kingdom.Cascade)]
    CapAppearExStage,

    [Stage(Kingdom.Cascade)]
    WindBlowExStage,

    [Stage(Kingdom.Sand, true)]
    SandWorldHomeStage,

    [Stage(Kingdom.Sand)]
    SandWorldShopStage,

    [Stage(Kingdom.Sand)]
    SandWorldSlotStage,

    [Stage(Kingdom.Sand)]
    SandWorldVibrationStage,

    [Stage(Kingdom.Sand)]
    SandWorldSecretStage,

    [Stage(Kingdom.Sand)]
    SandWorldMeganeExStage,

    [Stage(Kingdom.Sand)]
    SandWorldKillerExStage,

    [Stage(Kingdom.Sand)]
    SandWorldPressExStage,

    [Stage(Kingdom.Sand)]
    SandWorldSphinxExStage,

    [Stage(Kingdom.Sand)]
    SandWorldCostumeStage,

    [Stage(Kingdom.Sand)]
    SandWorldPyramid000Stage,

    [Stage(Kingdom.Sand)]
    SandWorldPyramid001Stage,

    [Stage(Kingdom.Sand)]
    SandWorldUnderground000Stage,

    [Stage(Kingdom.Sand)]
    SandWorldUnderground001Stage,

    [Stage(Kingdom.Sand)]
    SandWorldRotateExStage,

    [Stage(Kingdom.Sand)]
    MeganeLiftExStage,

    [Stage(Kingdom.Sand)]
    RocketFlowerExStage,

    [Stage(Kingdom.Sand)]
    WaterTubeExStage,

    [Stage(Kingdom.Lake, true)]
    LakeWorldHomeStage,

    [Stage(Kingdom.Lake)]
    LakeWorldShopStage,

    [Stage(Kingdom.Lake)]
    FastenerExStage,

    [Stage(Kingdom.Lake)]
    TrampolineWallCatchExStage,

    [Stage(Kingdom.Lake)]
    GotogotonExStage,

    [Stage(Kingdom.Lake)]
    FrogPoisonExStage,

    [Stage(Kingdom.Wooded, true)]
    ForestWorldHomeStage,

    [Stage(Kingdom.Wooded)]
    ForestWorldWaterExStage,

    [Stage(Kingdom.Wooded)]
    ForestWorldTowerStage,

    [Stage(Kingdom.Wooded)]
    ForestWorldBossStage,

    [Stage(Kingdom.Wooded)]
    ForestWorldBonusStage,

    [Stage(Kingdom.Wooded)]
    ForestWorldCloudBonusExStage,

    [Stage(Kingdom.Wooded)]
    FogMountainExStage,

    [Stage(Kingdom.Wooded)]
    RailCollisionExStage,

    [Stage(Kingdom.Wooded)]
    ShootingElevatorExStage,

    [Stage(Kingdom.Wooded)]
    ForestWorldWoodsStage,

    [Stage(Kingdom.Wooded)]
    ForestWorldWoodsTreasureStage,

    [Stage(Kingdom.Wooded)]
    ForestWorldWoodsCostumeStage,

    [Stage(Kingdom.Wooded)]
    PackunPoisonExStage,

    [Stage(Kingdom.Wooded)]
    AnimalChaseExStage,

    [Stage(Kingdom.Wooded)]
    KillerRoadExStage,

    [Stage(Kingdom.Cloud, true)]
    CloudWorldHomeStage,

    [Stage(Kingdom.Cloud)]
    FukuwaraiKuriboStage,

    [Stage(Kingdom.Cloud)]
    Cube2DExStage,

    [Stage(Kingdom.Lost, true)]
    ClashWorldHomeStage,

    [Stage(Kingdom.Lost)]
    ClashWorldShopStage,

    [Stage(Kingdom.Lost)]
    ImomuPoisonExStage,

    [Stage(Kingdom.Lost)]
    JangoExStage,

    [Stage(Kingdom.Metro, true)]
    CityWorldHomeStage,

    [Stage(Kingdom.Metro)]
    CityWorldMainTowerStage,

    [Stage(Kingdom.Metro)]
    CityWorldFactoryStage,

    [Stage(Kingdom.Metro)]
    CityWorldShop01Stage,

    [Stage(Kingdom.Metro)]
    CityWorldSandSlotStage,

    [Stage(Kingdom.Metro)]
    CityPeopleRoadStage,

    [Stage(Kingdom.Metro)]
    PoleGrabCeilExStage,

    [Stage(Kingdom.Metro)]
    TrexBikeExStage,

    [Stage(Kingdom.Metro)]
    PoleKillerExStage,

    [Stage(Kingdom.Metro)]
    Note2D3DRoomExStage,

    [Stage(Kingdom.Metro)]
    ShootingCityExStage,

    [Stage(Kingdom.Metro)]
    CapRotatePackunExStage,

    [Stage(Kingdom.Metro)]
    RadioControlExStage,

    [Stage(Kingdom.Metro)]
    ElectricWireExStage,

    [Stage(Kingdom.Metro)]
    Theater2DExStage,

    [Stage(Kingdom.Metro)]
    DonsukeExStage,

    [Stage(Kingdom.Metro)]
    SwingSteelExStage,

    [Stage(Kingdom.Metro)]
    BikeSteelExStage,

    [Stage(Kingdom.Snow, true)]
    SnowWorldHomeStage,

    [Stage(Kingdom.Snow)]
    SnowWorldTownStage,

    [Stage(Kingdom.Snow)]
    SnowWorldShopStage,

    [Stage(Kingdom.Snow)]
    SnowWorldLobby000Stage,

    [Stage(Kingdom.Snow)]
    SnowWorldLobby001Stage,

    [Stage(Kingdom.Snow)]
    SnowWorldRaceTutorialStage,

    [Stage(Kingdom.Snow)]
    SnowWorldRace000Stage,

    [Stage(Kingdom.Snow)]
    SnowWorldRace001Stage,

    [Stage(Kingdom.Snow)]
    SnowWorldCostumeStage,

    [Stage(Kingdom.Snow)]
    SnowWorldCloudBonusExStage,

    [Stage(Kingdom.Snow)]
    IceWalkerExStage,

    [Stage(Kingdom.Snow)]
    IceWaterBlockExStage,

    [Stage(Kingdom.Snow)]
    ByugoPuzzleExStage,

    [Stage(Kingdom.Snow)]
    IceWaterDashExStage,

    [Stage(Kingdom.Snow)]
    SnowWorldLobbyExStage,

    [Stage(Kingdom.Snow)]
    SnowWorldRaceExStage,

    [Stage(Kingdom.Snow)]
    SnowWorldRaceHardExStage,

    [Stage(Kingdom.Snow)]
    KillerRailCollisionExStage,

    [Stage(Kingdom.Seaside, true)]
    SeaWorldHomeStage,

    [Stage(Kingdom.Seaside)]
    SeaWorldUtsuboCaveStage,

    [Stage(Kingdom.Seaside)]
    SeaWorldVibrationStage,

    [Stage(Kingdom.Seaside)]
    SeaWorldSecretStage,

    [Stage(Kingdom.Seaside)]
    SeaWorldCostumeStage,

    [Stage(Kingdom.Seaside)]
    SeaWorldSneakingManStage,

    [Stage(Kingdom.Seaside)]
    SenobiTowerExStage,

    [Stage(Kingdom.Seaside)]
    CloudExStage,

    [Stage(Kingdom.Seaside)]
    WaterValleyExStage,

    [Stage(Kingdom.Seaside)]
    ReflectBombExStage,

    [Stage(Kingdom.Seaside)]
    TogezoRotateExStage,

    [Stage(Kingdom.Luncheon, true)]
    LavaWorldHomeStage,

    [Stage(Kingdom.Luncheon)]
    LavaWorldUpDownExStage,

    [Stage(Kingdom.Luncheon)]
    LavaBonus1Zone,

    [Stage(Kingdom.Luncheon)]
    LavaWorldShopStage,

    [Stage(Kingdom.Luncheon)]
    LavaWorldCostumeStage,

    [Stage(Kingdom.Luncheon)]
    ForkExStage,

    [Stage(Kingdom.Luncheon)]
    LavaWorldExcavationExStage,

    [Stage(Kingdom.Luncheon)]
    LavaWorldClockExStage,

    [Stage(Kingdom.Luncheon)]
    LavaWorldBubbleLaneExStage,

    [Stage(Kingdom.Luncheon)]
    LavaWorldTreasureStage,

    [Stage(Kingdom.Luncheon)]
    GabuzouClockExStage,

    [Stage(Kingdom.Luncheon)]
    CapAppearLavaLiftExStage,

    [Stage(Kingdom.Luncheon)]
    LavaWorldFenceLiftExStage,

    [Stage(Kingdom.Ruined, true)]
    BossRaidWorldHomeStage,

    [Stage(Kingdom.Ruined)]
    DotTowerExStage,

    [Stage(Kingdom.Ruined)]
    BullRunExStage,

    [Stage(Kingdom.Bowsers, true)]
    SkyWorldHomeStage,

    [Stage(Kingdom.Bowsers)]
    SkyWorldShopStage,

    [Stage(Kingdom.Bowsers)]
    SkyWorldCostumeStage,

    [Stage(Kingdom.Bowsers)]
    SkyWorldCloudBonusExStage,

    [Stage(Kingdom.Bowsers)]
    SkyWorldTreasureStage,

    [Stage(Kingdom.Bowsers)]
    JizoSwitchExStage,

    [Stage(Kingdom.Bowsers)]
    TsukkunRotateExStage,

    [Stage(Kingdom.Bowsers)]
    KaronWingTowerStage,

    [Stage(Kingdom.Bowsers)]
    TsukkunClimbExStage,

    [Stage(Kingdom.Moon, true)]
    MoonWorldHomeStage,

    [Stage(Kingdom.Moon)]
    MoonWorldCaptureParadeStage,

    [Stage(Kingdom.Moon)]
    MoonWorldWeddingRoomStage,

    [Stage(Kingdom.Moon)]
    MoonWorldKoopa1Stage,

    [Stage(Kingdom.Moon)]
    MoonWorldBasementStage,

    [Stage(Kingdom.Moon)]
    MoonWorldWeddingRoom2Stage,

    [Stage(Kingdom.Moon)]
    MoonWorldKoopa2Stage,

    [Stage(Kingdom.Moon)]
    MoonWorldShopRoom,

    [Stage(Kingdom.Moon)]
    MoonWorldSphinxRoom,

    [Stage(Kingdom.Moon)]
    MoonAthleticExStage,

    [Stage(Kingdom.Moon)]
    Galaxy2DExStage,

    [Stage(Kingdom.Mushroom, true)]
    PeachWorldHomeStage,

    [Stage(Kingdom.Mushroom)]
    PeachWorldShopStage,

    [Stage(Kingdom.Mushroom)]
    PeachWorldCastleStage,

    [Stage(Kingdom.Mushroom)]
    PeachWorldCostumeStage,

    [Stage(Kingdom.Mushroom)]
    FukuwaraiMarioStage,

    [Stage(Kingdom.Mushroom)]
    DotHardExStage,

    [Stage(Kingdom.Mushroom)]
    YoshiCloudExStage,

    [Stage(Kingdom.Mushroom)]
    PeachWorldPictureBossMagmaStage,

    [Stage(Kingdom.Mushroom)]
    RevengeBossMagmaStage,

    [Stage(Kingdom.Mushroom)]
    PeachWorldPictureGiantWanderBossStage,

    [Stage(Kingdom.Mushroom)]
    RevengeGiantWanderBossStage,

    [Stage(Kingdom.Mushroom)]
    PeachWorldPictureBossKnuckleStage,

    [Stage(Kingdom.Mushroom)]
    RevengeBossKnuckleStage,

    [Stage(Kingdom.Mushroom)]
    PeachWorldPictureBossForestStage,

    [Stage(Kingdom.Mushroom)]
    RevengeForestBossStage,

    [Stage(Kingdom.Mushroom)]
    PeachWorldPictureMofumofuStage,

    [Stage(Kingdom.Mushroom)]
    RevengeMofumofuStage,

    [Stage(Kingdom.Mushroom)]
    PeachWorldPictureBossRaidStage,

    [Stage(Kingdom.Mushroom)]
    RevengeBossRaidStage,

    [Stage(Kingdom.DarkSide, true)]
    Special1WorldHomeStage,

    [Stage(Kingdom.DarkSide)]
    Special1WorldTowerStackerStage,

    [Stage(Kingdom.DarkSide)]
    Special1WorldTowerBombTailStage,

    [Stage(Kingdom.DarkSide)]
    Special1WorldTowerFireBlowerStage,

    [Stage(Kingdom.DarkSide)]
    Special1WorldTowerCapThrowerStage,

    [Stage(Kingdom.DarkSide)]
    KillerRoadNoCapExStage,

    [Stage(Kingdom.DarkSide)]
    PackunPoisonNoCapExStage,

    [Stage(Kingdom.DarkSide)]
    BikeSteelNoCapExStage,

    [Stage(Kingdom.DarkSide)]
    ShootingCityYoshiExStage,

    [Stage(Kingdom.DarkSide)]
    SenobiTowerYoshiExStage,

    [Stage(Kingdom.DarkSide)]
    LavaWorldUpDownYoshiExStage,

    [Stage(Kingdom.DarkerSide, true)]
    Special2WorldHomeStage,

    [Stage(Kingdom.DarkerSide)]
    Special2WorldLavaStage,

    [Stage(Kingdom.DarkerSide)]
    Special2WorldCloudStage,

    [Stage(Kingdom.DarkerSide)]
    Special2WorldKoopaStage,

    [Stage(Kingdom.Odyssey, true)]
    HomeShipInsideStage,
}
