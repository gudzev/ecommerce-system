namespace Backend.Models
{
    public class Image
    {
        public int? id { get; set; }
        public string? url { get; set; }
        public bool? is_main_image { get; set; }

        public Image()
        {

        }
        public Image(string url)
        {
            id = null;
            this.url = url;
            is_main_image = true;
        }
    }
}
