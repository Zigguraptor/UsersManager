﻿using MediatR;

namespace UsersManager.Application.Friendship.Queries;

public class UserFriendsQuery : IRequest<IEnumerable<FriendVm>>
{
    public UserFriendsQuery(string userName) => UserName = userName;

    public string UserName { get; set; }
}
