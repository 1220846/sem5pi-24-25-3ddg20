namespace DDDSample1.Domain.Shared
{
    public interface IValueObject
    {
        string ToString();

        bool Equals(object other);

        int GetHashCode();
    }
}