namespace Domain.ValueObjects;

public class Location : IEquatable<Location>
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string ZipCode { get; }
    public double? Latitude { get; }
    public double? Longitude { get; }

    private Location(string street, string city, string state, string country, string zipCode, double? latitude = null, double? longitude = null)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
        Latitude = latitude;
        Longitude = longitude;
    }

    public static Location Create(string street, string city, string state, string country, string zipCode, double? latitude = null, double? longitude = null)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be null or empty.", nameof(street));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be null or empty.", nameof(city));
        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State cannot be null or empty.", nameof(state));
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be null or empty.", nameof(country));
        if (string.IsNullOrWhiteSpace(zipCode))
            throw new ArgumentException("ZipCode cannot be null or empty.", nameof(zipCode));

        if (latitude.HasValue && (latitude < -90 || latitude > 90))
            throw new ArgumentException("Latitude must be between -90 and 90.", nameof(latitude));
        if (longitude.HasValue && (longitude < -180 || longitude > 180))
            throw new ArgumentException("Longitude must be between -180 and 180.", nameof(longitude));

        return new Location(street.Trim(), city.Trim(), state.Trim(), country.Trim(), zipCode.Trim(), latitude, longitude);
    }

    public string GetFullAddress() => $"{Street}, {City}, {State} {ZipCode}, {Country}";

    public override bool Equals(object? obj) => Equals(obj as Location);

    public bool Equals(Location? other) =>
        other != null && 
        Street == other.Street && 
        City == other.City && 
        State == other.State && 
        Country == other.Country && 
        ZipCode == other.ZipCode;

    public override int GetHashCode() => HashCode.Combine(Street, City, State, Country, ZipCode);

    public override string ToString() => GetFullAddress();
}
