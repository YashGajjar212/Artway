namespace Artway.Presentation.DTOs.Customers
{
    public class ArtistDto
    {
        public int ArtistId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Bio { get; set; }

        public int MyProperty { get; set; }

        public DateTime Last_login { get; set; }

        // Shadow Properties
        //Creation_date - No this can't be a shadow property
        //Last_Updated - Yes this is the best candidate for shadow property because it will be auto updated
    }
}