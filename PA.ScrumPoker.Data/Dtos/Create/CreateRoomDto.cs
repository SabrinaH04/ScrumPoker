using System;
using System.Collections.Generic;
using System.Text;

namespace PA.ScrumPoker.Data.Dtos.Create
{
    public class CreateRoomDto : IAmADto
    {
        public string RoomName { get; set; }
        public string RoomOwnerName { get; set; }
    }
}
