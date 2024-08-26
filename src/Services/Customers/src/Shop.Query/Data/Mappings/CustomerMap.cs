using Customers.Query.Abstractions;
using Customers.Query.QueriesModel;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Customers.Query.Data.Mappings;

public class CustomerMap : IReadDbMapping
{
    public void Configure()
    {
        // TryRegisterClassMap: Registers a class map if it is not already registered.
        BsonClassMap.TryRegisterClassMap<CustomerQueryModel>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);

            classMap.MapMember(customer => customer.Id)
                .SetIsRequired(true);

            classMap.MapMember(customer => customer.FirstName)
                .SetIsRequired(true);

            classMap.MapMember(customer => customer.LastName)
                .SetIsRequired(true);

            classMap.MapMember(customer => customer.Gender)
                .SetIsRequired(true);

            classMap.MapMember(customer => customer.Email)
                .SetIsRequired(true);

            classMap.MapMember(customer => customer.DateOfBirth)
                .SetIsRequired(true)
                .SetSerializer(new DateTimeSerializer(true));

            // Ignore
            classMap.UnmapMember(customer => customer.FullName);
        });
    }
}