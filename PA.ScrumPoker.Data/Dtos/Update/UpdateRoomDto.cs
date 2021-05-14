using System;
using System.Collections.Generic;
using System.Text;

namespace PA.ScrumPoker.Data.Dtos.Update
{
    public class UpdateRoomDto
    {
        public Guid RoomID { get; set; }
        public string RoomName { get; set; }
        public bool canVote { get; set; }
        public bool showVotes { get; set; }
    }
}
