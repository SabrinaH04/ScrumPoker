using PA.ScrumPoker.Data.Cache;
using PA.ScrumPoker.Data.Dtos;
using PA.ScrumPoker.Data.Dtos.Create;
using PA.ScrumPoker.Data.Dtos.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.ScrumPoker.Web.Repo
{
    public class RoomRepo : IRepository<RoomDto>
    {
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private ICache<RoomDto> _roomCache;
        private ICache<UserDto> _userCache;

        public RoomRepo(ICache<RoomDto> roomCache, ICache<UserDto> userCache)
        {
            _roomCache = roomCache;
            _userCache = userCache;
        }

        public RoomDto CreateRoomAndOwner(CreateRoomDto dto)
        {
            var newRoomID = Guid.NewGuid();
            var newOwnerID = Guid.NewGuid();

            var newRoom = new RoomDto
            {
                RoomID = newRoomID,
                RoomName = dto.RoomName,
                OwnerID = newOwnerID,
                canVote = false,
                showVotes = false
            };

            _roomCache.SetCache("Room_" + newRoom.RoomID.ToString().ToLowerInvariant(), new List<RoomDto> { newRoom });


            var newOwner = new UserDto
            {
                UserID = newOwnerID,
                UserName = dto.RoomOwnerName,
                RoomID = newRoomID
            };

            _userCache.SetCache("User_" + newOwner.RoomID.ToString().ToLowerInvariant(), new List<UserDto> { newOwner });

            var t = _roomCache.GetCache("Room_" + newRoom.RoomID.ToString().ToLowerInvariant());

            //var createUserDto = new CreateUserDto
            //{
            //    RoomID = newRoomID,
            //    UserName = dto.RoomOwnerName
            //};


            //var t = _userCache.GetCache(newRoomID.ToString().ToLowerInvariant());
            newRoom.Users = new List<UserDto>() { newOwner };

            return (newRoom);
        }

        public void Delete(Guid guid)
        {
            throw new NotImplementedException();
        }

        public RoomDto GetByGuid(Guid ID)
        {
            var result = _roomCache.GetCache("Room_" + ID.ToString().ToLowerInvariant()).FirstOrDefault();

            if (result != null)
            {
                var roomUsers = _userCache.GetCache("User_" + ID.ToString().ToLowerInvariant());
                if (roomUsers != null && roomUsers.Any())
                {
                    result.Users = roomUsers.ToList();
                }

                return result;
            }

            return null;
        }

        public RoomDto Update(UpdateRoomDto dto)
        {
            var oldRoom = GetByGuid(dto.RoomID);
            if (oldRoom == null || oldRoom.RoomID == Guid.Empty || oldRoom.OwnerID == Guid.Empty)
            {
                throw new InvalidOperationException($"Room with ID {dto.RoomID} does not exist");
            }

            var newRoom = new RoomDto
            {
                RoomID = dto.RoomID,
                RoomName = dto.RoomName,
                OwnerID = oldRoom.OwnerID,
                canVote = dto.canVote,
                showVotes = dto.showVotes,
            };

            _roomCache.SetCache("Room_" + newRoom.RoomID.ToString().ToLowerInvariant(), new List<RoomDto> { newRoom });

            return (newRoom);
        }

        public void ResetRoomAndVotes(Guid roomID)
        {
            var oldRoom = _roomCache.GetCache("Room_" + roomID.ToString().ToLowerInvariant()).FirstOrDefault();
            if(oldRoom == null)
            {
                throw new InvalidOperationException($"Room with ID {roomID} does not exist");
            }

            var newRoom = new RoomDto
            {
                RoomID = oldRoom.RoomID,
                RoomName = oldRoom.RoomName,
                OwnerID = oldRoom.OwnerID,
                canVote = false,
                showVotes = false,
            };

            _roomCache.SetCache("Room_" + roomID.ToString().ToLowerInvariant(), new List<RoomDto> { newRoom });

            var users = _userCache.GetCache("User_" + roomID.ToString().ToLowerInvariant()).ToList();
            users.ForEach(u =>
            {
                u.QueryVoteValue = null;
                u.QueryID = Guid.Empty;
            });

            _userCache.SetCache("User_" + roomID.ToString().ToLowerInvariant(), users);
        }

    }
}
