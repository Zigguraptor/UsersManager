using MediatR;
using Microsoft.AspNetCore.Http;
using UsersManager.Application.Common.Exceptions;
using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Friendship.Commands.CancelFriend;

public class CancelFriendCommandHandler : IRequestHandler<CancelFriendCommand>
{
    private readonly IFriendshipRepository _friendshipRepository;

    public CancelFriendCommandHandler(IFriendshipRepository friendshipRepository) =>
        _friendshipRepository = friendshipRepository;

    public async Task Handle(CancelFriendCommand request, CancellationToken cancellationToken)
    {
        if (await _friendshipRepository.DeleteFriendInviteAsync(request.User1Uuid, request.User2Name) > 0)
            return;

        throw new HttpException(StatusCodes.Status404NotFound, "Запросов в друзья с этим пользователем не найдено.");
    }
}
