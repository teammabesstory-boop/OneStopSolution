// Path: src/Core/OneStop.Domain/Common/Primitives/IDomainEvent.cs
using MediatR;

namespace OneStop.Domain.Common.Primitives
{
    // Kita pakai INotification dari MediatR agar bisa di-broadcast
    public interface IDomainEvent : INotification
    {
    }
}