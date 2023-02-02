using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPINetCore.API.DTOs;
using WebAPINetCore.API.Models;

namespace WebAPINetCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayers(int? id, string? lastName = "", string? teamName = "")
        {
            IQueryable<Player> players = _context.Players;

            //Query by ID
            if (id.HasValue)
            {
                var player = await players.Where(p => p.Id == id).FirstOrDefaultAsync();
                if (player == null)
                {
                    return NotFound();
                }
                return Ok(PlayerToDTO(player));
            }

            //Query All Players matching a given Last Name
            if (!string.IsNullOrEmpty(lastName))
            {
                players = players.Where(t => t.LastName.ToLowerInvariant() == (lastName.ToLowerInvariant()));
            }

            //Query for all Players on a Team
            if (!string.IsNullOrEmpty(teamName))
            {
                Team? team = await _context.Teams.FirstOrDefaultAsync(t => t.Name.ToLowerInvariant() == teamName.ToLowerInvariant());
                if (team == null)
                {
                    return NotFound();
                }

                players = players.Where(t => t.TeamId == team.Id);
            }


            return Ok(await players.Select(p => PlayerToDTO(p)).ToListAsync());
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(PlayerDTO playerDTO)
        {
            Player player = DTOToPlayer(playerDTO);
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        private static PlayerDTO PlayerToDTO(Player player) =>
         new PlayerDTO
         {
             Id = player.Id,
             FirstName = player.FirstName,
             LastName = player.LastName
         };
        private static Player DTOToPlayer(PlayerDTO playerDTO) =>
         new Player
         {
             Id = playerDTO.Id,
             FirstName = playerDTO.FirstName,
             LastName = playerDTO.LastName
         };
    }
}
