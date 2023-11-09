using MediatR;
using Microsoft.AspNetCore.Http;
using UsersManager.Application.Common.Exceptions;
using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Friendship.Commands.FriendRequest;

public class FriendInviteCommandHandler : IRequestHandler<FriendInviteCommand>
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IUsersRepository _usersRepository;

    public FriendInviteCommandHandler(IFriendshipRepository friendshipRepository, IUsersRepository usersRepository)
    {
        _friendshipRepository = friendshipRepository;
        _usersRepository = usersRepository;
    }

    public async Task Handle(FriendInviteCommand command, CancellationToken cancellationToken)
    {
        var recipient = await _usersRepository.GetUserAsync(userName: command.ToUser2Name);
        if (recipient == null)
            throw new HttpException(StatusCodes.Status400BadRequest, "Пользователь не найден.");

        if (command.SenderUuid == recipient.Uuid)
            throw new HttpException(StatusCodes.Status400BadRequest, "Нельзя добавить в друзья себя.");

        await _friendshipRepository.FriendInviteAsync(command.SenderUuid, recipient.Uuid);
    }
}
