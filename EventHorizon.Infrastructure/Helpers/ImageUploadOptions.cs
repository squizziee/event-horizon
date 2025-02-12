namespace EventHorizon.Infrastructure.Helpers
{
    public record ImageUploadOptions
    {
        public required string Url { get; set; }
        public required string AccessibleUrl { get; set; }
        public required IEnumerable<string> SupportedExtensions { get; set; }
    }
}
