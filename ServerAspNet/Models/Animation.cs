namespace SuperMarioOdysseyOnline.Server.Models;

public record Animation(string Name, AnimationKeyFrame? CurrentKeyFrame)
{
    public Animation() : this(string.Empty, default)
    {
    }

    public Animation(string name) : this(name, default)
    {
    }

    public Animation(AnimationKeyFrame animationKeyFrame) : this(string.Empty, animationKeyFrame)
    {
    }
}
