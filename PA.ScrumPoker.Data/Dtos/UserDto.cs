using System;
using System.Collections.Generic;
using System.Text;

namespace PA.ScrumPoker.Data.Dtos
{
    public class UserDto : IAmADto
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public Guid QueryID { get; set; }
        public string QueryVoteValue { get; set; }
        public Guid RoomID { get; set; }
    }
}
