using MediatR;

namespace CQRS_Min.Features.Products.Notifications
{
    public record ProductCreatedNotification(Guid Id) : INotification;
}
