namespace SuperMarioOdysseyOnline.Server.Models;

public record AnimationKeyFrame(ushort Act, ushort SubAct, AnimationWeights AnimationBlendWeights)
{
    public AnimationKeyFrame(ushort act, ushort subAct, float[] animationWeights)
        : this(act, subAct, new AnimationWeights(animationWeights))
    {
    }
}
