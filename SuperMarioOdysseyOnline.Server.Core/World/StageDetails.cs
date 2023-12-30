using System.Reflection;

namespace SuperMarioOdysseyOnline.Server.World;

public record StageDetails(Stage Stage, Kingdom Kingdom)
{
    public static StageDetails FromStage(Stage stage)
        => new(
            stage,
            stage.GetType()
                .GetMember(stage.ToString())
                .First()
                .GetCustomAttributes<StageAttribute>()
                .First()
                .Kingdom
        );
}
