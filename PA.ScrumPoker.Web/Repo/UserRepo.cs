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
    public class UserRepo : IRepository<UserDto>
    {

        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private ICache<UserDto> _userCache;
        private ICache<RoomDto> _roomCache;

        public UserRepo(ICache<UserDto> userCache, ICache<RoomDto> roomCache)
        {
            _userCache = userCache;
            _roomCache = roomCache;
        }

        public UserDto Create(CreateUserDto dto)
        {
            //make sure room exists
            var room = _roomCache.GetCache("Room_" + dto.RoomID.ToString().ToLowerInvariant());
            if(room == null || room.FirstOrDefault().RoomID == null)
            {
                throw new InvalidOperationException("Error occured during User Creation. Invalid RoomID, Room must exist.");
            }

            var newUserID = Guid.NewGuid();

            var newUser = new UserDto
            {
                UserID = newUserID,
                UserName = dto.UserName,
                RoomID = dto.RoomID
                
            };
            var oldUsers = _userCache.GetCache("User_" + dto.RoomID.ToString().ToLowerInvariant()).ToList();
            if (oldUsers != null)
            {
                oldUsers.Add(newUser);
                _userCache.SetCache("User_" + newUser.RoomID.ToString().ToLowerInvariant(), oldUsers);
            }
            else
            {
                _userCache.SetCache("User_" + newUser.RoomID.ToString().ToLowerInvariant(), new List<UserDto> { newUser });
            }

            return (newUser);
        }

        //public void Delete(Guid guid)
        //{
        //    throw new NotImplementedException();
        //}

        public List<UserDto> GetByRoomID(Guid roomID)
        {
            var roomUsers = _userCache.GetCache("User_" + roomID.ToString().ToLowerInvariant());
            if (roomUsers != null && roomUsers.Any())
            {
                return roomUsers.ToList();
            }

            return null;
        }

        public UserDto GetByGuid(Guid roomID, Guid userID)
        {
            var roomUsers = _userCache.GetCache("User_" + roomID.ToString().ToLowerInvariant());
            if(roomUsers != null && roomUsers.Any())
            {
                return roomUsers.FirstOrDefault(u => u.UserID.ToString().ToLowerInvariant() == userID.ToString().ToLowerInvariant());
            }


            return null;
        }

        public UserDto Update(UpdateUserDto dto)
        {
            var oldUsers = _userCache.GetCache("User_" + dto.RoomID.ToString().ToLowerInvariant()).ToList();
            //if (oldUser == null)
            //{
            //    throw new InvalidOperationException($"User with ID {dto.UserID} does not exist");
            //}
            var newUser = new UserDto
            {
                RoomID = dto.RoomID,
                UserID = dto.UserID,
                QueryID = dto.QueryID,
                QueryVoteValue = dto.QueryVoteValue,
                UserName = dto.UserName
            };

            if (oldUsers != null)
            {
                //remove updated user
                var oldUser = oldUsers.FirstOrDefault(a => a.UserID == dto.UserID);
                if (oldUser!=null && oldUser.UserID!=null)
                {
                    oldUsers.Remove(oldUser);
                }
                oldUsers.Add(newUser);
                _userCache.SetCache("User_" + newUser.RoomID.ToString().ToLowerInvariant(), oldUsers);
            }
            else
            {
                _userCache.SetCache("User_" + newUser.RoomID.ToString().ToLowerInvariant(), new List<UserDto> { newUser });
            }

            return (newUser);
        }
    }
}
