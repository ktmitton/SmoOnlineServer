using SuperMarioOdysseyOnline.Server.Core.Connections.Packets;
using SuperMarioOdysseyOnline.Server.Core.Lobby;
using SuperMarioOdysseyOnline.Server.Core.Models;

namespace SuperMarioOdysseyOnline.Server.Lobby;

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
        switch (packet)
        {
            case ConnectPacket connectPacket:
                Name = connectPacket.Data.ClientName;

                break;
            case CappyRenderPacket cappyRenderPacket:
                Cappy = Cappy with
                {
                    Location = cappyRenderPacket.Data.Location,
                    Animation = cappyRenderPacket.Data.Animation,
                    IsThrown = cappyRenderPacket.Data.IsThrown
                };

                break;
            case CapturePacket capturePacket:
                Mario = Mario with
                {
                    CapturedEntity = capturePacket.Data.CapturedEntity
                };

                break;
            case CostumePacket costumePacket:
                Mario = Mario with
                {
                    Costume = costumePacket.Data.MariosCostume
                };

                Cappy = Cappy with
                {
                    Costume = costumePacket.Data.CappysCostume
                };

                break;
            case MarioRenderPacket marioRenderPacket:
                Mario = Mario with
                {
                    Location = marioRenderPacket.Data.Location,
                    Animation = marioRenderPacket.Data.Animation
                };

                break;
            case PlayerStagePacket playerStagePacket:
                Stage = playerStagePacket.Data.Stage;

                break;
        }
    }
}
