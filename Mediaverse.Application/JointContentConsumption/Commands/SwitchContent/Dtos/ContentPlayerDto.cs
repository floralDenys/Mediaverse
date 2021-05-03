namespace Mediaverse.Application.JointContentConsumption.Commands.SwitchContent.Dtos
{
    public class ContentPlayerDto
    {
        public long Width { get; set; }
        public long Height { get; set; }
        public string Html { get; set; }
        public string State { get; set; }
        public double PlayingTime { get; set; }
    }
}