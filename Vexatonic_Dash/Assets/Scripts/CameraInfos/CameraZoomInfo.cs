public class CameraZoomInfo : CameraControlInfo
{
    public double scale;
    
    public CameraZoomInfo(double time, double term, double scale) : base(time, term, false)
    {
        this.scale = scale;
    }
}