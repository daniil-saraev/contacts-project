using Core.Entities;

namespace ContactBook.Commands;

public interface IDeleteContactCommand
{
    Task Execute(Contact contact, CancellationToken cancellationToken = default);
}