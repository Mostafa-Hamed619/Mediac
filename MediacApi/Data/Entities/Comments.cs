namespace MediacApi.Data.Entities
{
    public class Comments
    {
        public Guid Id { get; set; }

        public string userId {  get; set; }

        public User user { get; set; }

        public Guid postId { get; set; }

        public Post post { get; set; }

        public string Comment {  get; set; }

        public DateTime TimeCreated { get; set; }
    }
}
