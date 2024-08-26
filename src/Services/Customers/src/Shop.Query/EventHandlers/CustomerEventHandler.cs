using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Customers.Core.Extensions;
using Customers.Core.SharedKernel;
using Customers.Domain.Entities.CustomerAggregate.Events;
using Customers.Query.Abstractions;
using Customers.Query.Application.Customer.Queries;
using Customers.Query.QueriesModel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Customers.Query.EventHandlers;

public class CustomerEventHandler(
    IMapper mapper,
    ISynchronizeDb synchronizeDb,
    ICacheService cacheService,
    ILogger<CustomerEventHandler> logger) :
    INotificationHandler<CustomerCreatedEvent>,
    INotificationHandler<CustomerUpdatedEvent>,
    INotificationHandler<CustomerDeletedEvent>
{
    private readonly ICacheService _cacheService = cacheService;
    private readonly ILogger<CustomerEventHandler> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly ISynchronizeDb _synchronizeDb = synchronizeDb;

    public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
    {
        LogEvent(notification);

        var customerQueryModel = _mapper.Map<CustomerQueryModel>(notification);
        await _synchronizeDb.UpsertAsync(customerQueryModel, filter => filter.Id == customerQueryModel.Id);
        await ClearCacheAsync(notification);
    }

    public async Task Handle(CustomerDeletedEvent notification, CancellationToken cancellationToken)
    {
        LogEvent(notification);

        await _synchronizeDb.DeleteAsync<CustomerQueryModel>(filter => filter.Email == notification.Email);
        await ClearCacheAsync(notification);
    }

    public async Task Handle(CustomerUpdatedEvent notification, CancellationToken cancellationToken)
    {
        LogEvent(notification);

        var customerQueryModel = _mapper.Map<CustomerQueryModel>(notification);
        await _synchronizeDb.UpsertAsync(customerQueryModel, filter => filter.Id == customerQueryModel.Id);
        await ClearCacheAsync(notification);
    }

    private async Task ClearCacheAsync(CustomerBaseEvent @event)
    {
        var cacheKeys = new[] { nameof(GetAllCustomerQuery), $"{nameof(GetCustomerByIdQuery)}_{@event.Id}" };
        await _cacheService.RemoveAsync(cacheKeys);
    }

    private void LogEvent<TEvent>(TEvent @event) where TEvent : CustomerBaseEvent =>
        _logger.LogInformation("----- Triggering the event {EventName}, model: {EventModel}", typeof(TEvent).Name, @event.ToJson());
}