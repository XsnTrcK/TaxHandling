using System;
using System.Text;

namespace TaxHandlingLibrary.Models
{
    public class Location
    {
        public Location(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
                throw new ArgumentNullException(nameof(zipCode));
            ZipCode = zipCode;
        }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string GenerateUriQuery()
        {
            var query = new StringBuilder();
            query.Append($"zip={ZipCode}");
            if (!string.IsNullOrWhiteSpace(Country))
                query.Append($"&country={Country}");
            if (!string.IsNullOrWhiteSpace(State))
                query.Append($"&state={State}");
            if (!string.IsNullOrWhiteSpace(City))
                query.Append($"&city={City}");
            if (!string.IsNullOrWhiteSpace(Street))
                query.Append($"&street={Street}");

            return query.ToString();
        }
    }
}
