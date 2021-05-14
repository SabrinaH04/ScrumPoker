using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PA.ScrumPoker.Data.Dtos;
using PA.ScrumPoker.Data.Dtos.Create;
using PA.ScrumPoker.Data.Dtos.Update;
using PA.ScrumPoker.Web.Repo;
using PA.ScrumPoker.Web.SignalRHub;

namespace PA.ScrumPoker.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserRepo _repo;
        private RoomRepo _roomRepo;
        private IHubContext<ProjectHub> _hub;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public UserController(IRepository<UserDto> repo, IRepository<RoomDto> roomRepo, IHubContext<ProjectHub> hub)
        {
            _repo = (UserRepo)repo;
            _roomRepo = (RoomRepo)roomRepo;
            _hub = hub;
        }

        //[HttpGet("{userID}")]
        //public IActionResult GetByID(Guid userID, Guid roomID)
        //{
        //    try
        //    {
        //        var result = _repo.GetByGuid(userID);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error($"An error occured: {ex.Message}");
        //        return BadRequest(ex.Message);
        //    }

        //}

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserDto dto)
        {
            try
            {
                //check room exists
                var room = _roomRepo.GetByGuid(dto.RoomID);

                if(room==null || room.RoomID == null)
                {
                    return BadRequest("Invalid roomID");
                }
                _hub.Clients.All.SendAsync("RefreshRoom", "Refresh Room");
                var result = _repo.Create(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occured: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{userID}")]
        public IActionResult Update([FromBody] UpdateUserDto dto)
        {
            try
            {
                var result = _repo.Update(dto);
                _hub.Clients.All.SendAsync("RefreshRoom", "Refresh Room");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occured: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
