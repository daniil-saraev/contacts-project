using Core.Entities;

namespace ContactBook.Commands;

public interface IAddContactCommand
{
    Task Execute(Contact contact, CancellationToken cancellationToken = default);
}