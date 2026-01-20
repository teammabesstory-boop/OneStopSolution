// Path: src/Core/OneStop.Domain/Common/Primitives/Entity.cs
using System;
using System.Collections.Generic;

namespace OneStop.Domain.Common.Primitives
{
    public abstract class Entity : IEquatable<Entity>
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public Guid Id { get; private set; }

        protected Entity(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("ID cannot be empty", nameof(id));
            Id = id;
        }

        // Constructor kosong untuk EF Core nanti
        protected Entity() { }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents() => _domainEvents.Clear();

        public bool Equals(Entity? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;
            return Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return obj is Entity entity && entity.Id == Id;
        }

        public override int GetHashCode() => Id.GetHashCode() * 41;

        public static bool operator ==(Entity? a, Entity? b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Entity? a, Entity? b) => !(a == b);
    }
}