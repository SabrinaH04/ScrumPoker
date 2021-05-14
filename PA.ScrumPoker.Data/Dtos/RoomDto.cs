using System;
using System.Collections.Generic;
using System.Text;

namespace PA.ScrumPoker.Data.Dtos
{
    public class RoomDto : IAmADto
    {
        public Guid RoomID { get; set; }
        public string RoomName { get; set; }
        public Guid OwnerID { get; set; }
        public Guid QueryID { get; set; }
        public bool canVote { get; set; }
        public bool showVotes { get; set; }

        public List<UserDto> Users { get; set; }
    }
}
