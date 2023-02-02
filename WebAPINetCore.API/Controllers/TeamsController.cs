using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPINetCore.API.DTOs;
using WebAPINetCore.API.Enums;
using WebAPINetCore.API.Models;
using WebAPINetCore.API.Validators;

namespace WebAPINetCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeams(int? id, string? orderBy = "")
        {
            IQueryable<Team> teams = _context.Teams;

            //Query by ID
            if (id.HasValue)
            {
                Team? team = await teams.Where(t => t.Id == id).FirstOrDefaultAsync();
                if (team == null)
                {
                    return NotFound();
                }
                return Ok(TeamToDTO(team));
            }

            //Query All Teams ordered by Name or Location
            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.ToLowerInvariant())
                {
                    case "name":
                        teams = teams.OrderBy(t => t.Name);
                        break;
                    case "location":
                        teams = teams.OrderBy(t => t.Location);
                        break;
                    default:
                        return BadRequest("Invalid value for orderBy parameter.");
                }
            }

            return Ok(await teams.Select(t => TeamToDTO(t)).ToListAsync());
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            return team;
        }

        // POST: api/Teams/1/player/2
        [HttpPut("{teamId}/player/{playerId}")]
        public async Task<IActionResult> UpdatePlayerInTeam(int teamId, int playerId, TeamUpdateType teamUpdateType)
        {
            if (!await _context.Teams.AnyAsync(x => x.Id == teamId))
            {
                return NotFound();
            }

            var player = await _context.Players.FirstOrDefaultAsync(x => x.Id == playerId);

            if (player == null)
            {
                return NotFound();
            }

            if (teamUpdateType == TeamUpdateType.Add)
            {
                player.TeamId = teamId;
                //Validate Player
                var validator = new PlayerValidator(_context);
                var validatorResult = await validator.ValidateAsync(player);

                if (!validatorResult.IsValid)
                {
                    return BadRequest(validatorResult.ToString());
                }
            }
            else if (teamUpdateType == TeamUpdateType.Remove)
            {
                if (player.TeamId != teamId) {
                    return BadRequest("Player not on team.");
                }
                player.TeamId = null;
            }
            else
            {
                return BadRequest("Invalid action.");
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Teams
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(TeamDTO teamDTO)
        {
            Team team = DTOToTeam(teamDTO);
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeam", new { id = team.Id }, team);
        }

        private static TeamDTO TeamToDTO(Team team) =>
         new TeamDTO
         {
             Id = team.Id,
             Name = team.Name,
             Location = team.Location
         };
        private static Team DTOToTeam(TeamDTO teamDTO) =>
         new Team
         {
             Id = teamDTO.Id,
             Name = teamDTO.Name,
             Location = teamDTO.Location
         };
    }
}
