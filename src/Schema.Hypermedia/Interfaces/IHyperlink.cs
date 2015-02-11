
namespace Schema.Hypermedia
{
    public interface IHyperlink
    {
        string Rel { get; set; }
        string Href { get; set; }
        string Title { get; set; }
        string EncType { get; set; }
        string Method { get; set; }
        string Schema { get; set; }
    }
}
