namespace SuperMarioOdysseyOnline.Server.Models;

public record AnimationWeights(float Weight0, float Weight1, float Weight2, float Weight3, float Weight4, float Weight5)
{
    public AnimationWeights(float[] animationWeights)
        : this(
            animationWeights[0],
            animationWeights[1],
            animationWeights[2],
            animationWeights[3],
            animationWeights[4],
            animationWeights[5]
        )
    {
    }
}
