namespace MediacApi.DTOs.Account
{
    public class EmailSendDto
    {
        public EmailSendDto(string To, string Body, string Subject)
        {
            this.To = To;
            
            this.Body = Body;
        }
        public string To {  get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }
    }
}
