using MediatR;
using Microsoft.AspNetCore.Http;
using UsersManager.Application.Common.Exceptions;
using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Friendship.Commands.DeleteFriend;

public class DeleteFriendCommandHandler : IRequestHandler<DeleteFriendCommand>
{
    private readonly IFriendshipRepository _friendshipRepository;

    public DeleteFriendCommandHandler(IFriendshipRepository friendshipRepository) =>
        _friendshipRepository = friendshipRepository;

    public async Task Handle(DeleteFriendCommand request, CancellationToken cancellationToken)
    {
        if (await _friendshipRepository.DeleteFriendAsync(request.OwnerName, request.FriendName) > 0)
            return;

        throw new HttpException(StatusCodes.Status404NotFound, "Друг не найден");
    }
}
