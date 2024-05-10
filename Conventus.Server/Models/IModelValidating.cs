using System.Diagnostics.Contracts;

namespace Conventus.Server.Models;

public interface IModelValidating
{
    [Pure]
    public bool IsValid();
}
