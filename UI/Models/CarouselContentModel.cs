namespace UI.Models
{
    public class CarouselContentModel
    {
        public string ImageUrl { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public bool IsActionContent { get; set; }
        public string? ActionText { get; set; }
        public string? ActionUrl { get; set; }
    }
}