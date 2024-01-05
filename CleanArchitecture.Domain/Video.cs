namespace CleanArchitecture.Domain
{
    public class Video
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int StreamerId { get; set; }
        public virtual Streamer Streamer { get; set; }

    }
}
