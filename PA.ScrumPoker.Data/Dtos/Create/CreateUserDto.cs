using System;
using System.Collections.Generic;
using System.Text;

namespace PA.ScrumPoker.Data.Dtos.Create
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public Guid RoomID { get; set; }
    }
}
