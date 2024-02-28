namespace BangazonAPI.DTOs
{
    public class OrderCreateDto
    {
        public int UserId { get; set; }
        public bool IsOpen { get; set; }
        public List<int> ProductIds { get; set; }
    }

}

