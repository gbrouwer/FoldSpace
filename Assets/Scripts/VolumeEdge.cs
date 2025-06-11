public class VolumeEdge
{
    public VolumeAgent from;
    public VolumeAgent to;
    public float weight;

    public VolumeEdge(VolumeAgent from, VolumeAgent to, float weight = 1f)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
    }
} 
