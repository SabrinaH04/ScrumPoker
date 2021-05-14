using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class RoomController : ControllerBase
    {

        private RoomRepo _repo;
        private UserRepo _userRepo;
        private IHubContext<ProjectHub> _hub;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public RoomController(IRepository<RoomDto> repo, IRepository<UserDto> userRepo, IHubContext<ProjectHub> hub)
        {
            _userRepo = (UserRepo)userRepo;
            _repo = (RoomRepo)repo;
            _hub = hub;
        }

        [HttpGet("{roomID}")]
        public IActionResult GetByID(Guid roomID)
        {
            try
            {
                var result = _repo.GetByGuid(roomID);
                return Ok(result);
            }
            catch(Exception ex)
            {
                logger.Error($"An error occured: {ex.Message}");
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{roomID}/User/{userID}")]
        public IActionResult GetByID(Guid roomID, Guid userID)
        {
            try
            {
                var result = _userRepo.GetByGuid(roomID, userID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occured: {ex.Message}");
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult Create([FromBody]CreateRoomDto dto)
        {
            try
            {
                var result = _repo.CreateRoomAndOwner(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occured: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{roomID}")]
        public IActionResult Update([FromBody] UpdateRoomDto dto)
        {
            try
            {
                 var result = _repo.Update(dto);
                //_hub.RefreshRoom("Refresh Room");
                _hub.Clients.All.SendAsync("RefreshRoom", "Refresh Room");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occured: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("reset/{roomID}")]
        public IActionResult resetRoom(Guid roomID)
        {
            try
            {
                _repo.ResetRoomAndVotes(roomID);
                //_hub.RefreshRoom("Refresh Room");
                _hub.Clients.All.SendAsync("RefreshRoom", "Refresh Room");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error($"An error occured: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
