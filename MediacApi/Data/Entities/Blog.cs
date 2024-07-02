namespace MediacApi.Data.Entities
{
    public class Blog
    {
        public Guid Id { get; set; }

        public string blogName { get; set; } = string.Empty;

        public string blogDescription { get; set; } = string.Empty;

        public string blogImage { get; set; } = string.Empty;

        public bool checkFollow { get; set; } = false;

        public int followers {  get; set; }

        public ICollection<Post> posts { get; set; } = new List<Post>();

        public ICollection<Subscribe> subscribes { get; set; }
    }
}
