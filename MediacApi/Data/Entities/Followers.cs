namespace MediacApi.Data.Entities
{
    public class Followers
    {
        public string FollowerUserId {  get; set; }

        public User FollowerUser { get; set; }

        public string FolloweeUserId {  get; set; }

        public User FolloweeUser { get; set; }
    }
}
