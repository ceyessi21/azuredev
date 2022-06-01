using ConvertSqlServerToSQLite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ConvertSqlServerToSQLite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnesController : ControllerBase
    {
        private readonly sqldbContext _context;
        private readonly LiteContext _litecontext;

        public PersonnesController(sqldbContext context, LiteContext liteContext)
        {
            _context = context;
            _litecontext = liteContext;
        }

        //// GET: api/Personnes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personne>>> GetPersonnes()
        {
            return await _context.Personnes.ToListAsync();
        }

        // GET: api/Personnes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Personne>> GetPersonne(int id)
        {
            var personne = await _context.Personnes.FindAsync(id);


            if (personne == null)
            {
                return NotFound();
            }

            return personne;
        }

        // PUT: api/Personnes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonne(int id, Personne personne)
        {
            if (id != personne.Id)
            {
                return BadRequest();
            }

            _context.Entry(personne).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Personnes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonne(int id)
        {
            var personne = await _context.Personnes.FindAsync(id);
            if (personne == null)
            {
                return NotFound();
            }

            _context.Personnes.Remove(personne);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonneExists(int id)
        {
            return _context.Personnes.Any(e => e.Id == id);
        }

        //POST: api/Personnes
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> ConverteSQLServerToSQLite()
        {
            var dbname = "myDb.db";
            //string path = Environment.CurrentDirectory + "/" + dbname;

            await _litecontext.Database.EnsureDeletedAsync();
            await _litecontext.Database.EnsureCreatedAsync();

            List<Personne> p = await _context.Personnes.ToListAsync();
            await _litecontext.AddRangeAsync(p);
            await _litecontext.SaveChangesAsync();


            await _context.Database.CloseConnectionAsync();
            await _context.DisposeAsync();
            //GC.SuppressFinalize(this);

            await _litecontext.Database.CloseConnectionAsync();
             _litecontext.Dispose();
            //GC.SuppressFinalize(this);
            SqliteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var ms = new MemoryStream();

            using (var file = new FileStream(dbname, FileMode.Open, FileAccess.Read))
            {
                file.CopyTo(ms);
            }

            System.IO.File.Delete(dbname);

            ms.Seek(0, SeekOrigin.Begin);

            return File(ms, "application/octet-stream", $"fcm.db");

        }

    }
}
