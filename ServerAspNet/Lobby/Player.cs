using SuperMarioOdysseyOnline.Server.Models;
using SuperMarioOdysseyOnline.Server.Packets;

namespace SuperMarioOdysseyOnline.Server.Lobby;

public interface IPlayer
{
    bool Connected { get; }

    bool Ignored { get; }

    string Name { get; set; }

    Guid Id { get; }

    Mario Mario { get; }

    Cappy Cappy { get; }

    Stage Stage { get; }

    void HandleReceivedPacket(IPacket packet);
}

internal class Player(Guid id) : IPlayer
{
    public bool Connected { get; set; }

    public bool Ignored { get; set; }

    public Guid Id { get; } = id;

    public string Name { get; set; } = "Unknown User";

    public Mario Mario { get; private set; } = new Mario();

    public Cappy Cappy { get; private set; } = new Cappy();

    public Stage Stage { get; private set; } = new Stage();

    public void HandleReceivedPacket(IPacket packet)
    {
        if (packet.Data is ConnectData connectData)
        {
            Name = connectData.ClientName;
        }
        else if (packet.Data is CappyRenderData capData)
        {
            Cappy = Cappy with
            {
                Location = capData.Location,
                Animation = capData.Animation,
                IsThrown = capData.IsThrown
            };
        }
        else if (packet.Data is CaptureData captureData)
        {
            Mario = Mario with
            {
                CapturedEntity = captureData.CapturedEntity
            };
        }
        else if (packet.Data is CostumeData costumeData)
        {
            Mario = Mario with
            {
                Costume = costumeData.MariosCostume
            };

            Cappy = Cappy with
            {
                Costume = costumeData.CappysCostume
            };
        }
        else if (packet.Data is MarioRenderData playerData)
        {
            Mario = Mario with
            {
                Location = playerData.Location,
                Animation = playerData.Animation
            };
        }
        else if (packet.Data is PlayerStageData gameData)
        {
            Stage = gameData.Stage;
        }
    }
}
