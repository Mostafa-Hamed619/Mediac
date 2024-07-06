namespace MediacApi.DTOs.Subscribes
{
    public class SubscribeBlogDto
    {
        public Guid BlogId { get; set; }

        public string BlogName { get; set; }

        public string BlogImage {  get; set; }

        public string BlogDescription {  get; set; }
    }
}
