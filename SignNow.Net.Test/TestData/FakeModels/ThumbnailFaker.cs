using System;
using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Fake model for <see cref="Thumbnail"/>
    /// </summary>
    public class ThumbnailFaker : Faker<Thumbnail>
    {
        private const string DefaultThumbnailText = "signNow test";

        public ThumbnailFaker()
        {
            Rules((f, o) =>
            {
                o.Small = new Uri(f.Image.PlaceholderUrl(85, 110, DefaultThumbnailText));
                o.Medium = new Uri(f.Image.PlaceholderUrl(340, 440, DefaultThumbnailText));
                o.Large = new Uri(f.Image.PlaceholderUrl(890, 1151, DefaultThumbnailText));
            });
        }
    }
}
