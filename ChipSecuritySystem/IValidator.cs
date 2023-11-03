using System.Collections.Generic;

namespace ChipSecuritySystem
{
    public interface IValidator<T>
    {
        IEnumerable<T> Validate(IEnumerable<T> value);
    }
}
