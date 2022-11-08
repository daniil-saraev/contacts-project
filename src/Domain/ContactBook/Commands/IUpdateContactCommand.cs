using Core.Entities;

namespace ContactBook.Commands;

public interface IUpdateContactCommand
{
    Task Execute(Contact updatedContact, CancellationToken cancellationToken = default);
}