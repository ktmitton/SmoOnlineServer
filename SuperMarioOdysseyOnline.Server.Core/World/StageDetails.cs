using System.Reflection;

namespace SuperMarioOdysseyOnline.Server.World;

public record StageDetails(Stage Stage)
{
    public Kingdom Kingdom => Stage
        .GetType()
        .GetMember(Stage.ToString())
        .First()
        .GetCustomAttribute<StageAttribute>()!.Kingdom;

    public bool IsHomeStage => Stage
        .GetType()
        .GetMember(Stage.ToString())
        .First()
        .GetCustomAttribute<StageAttribute>()!.IsHomeStage;

    public string Label
    {
        get
        {
            if (Stage == Stage.ForestWorldWoodsStage)
            {
                return "Deep Woods";
            }

            switch (Kingdom)
            {
                case Kingdom.Bowsers:
                    return "Bowser's Kingdom";
                case Kingdom.DarkSide:
                    return "Dark Side";
                case Kingdom.DarkerSide:
                    return "Darker Side";
                default:
                    return $"{Kingdom} Kingdom";
            }
        }
    }
}
