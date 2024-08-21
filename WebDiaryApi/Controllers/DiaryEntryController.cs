using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDiaryApi.Data;
using WebDiaryApi.Model;

namespace WebDiaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaryEntryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DiaryEntryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaryEntry>>> GetDiaryEntries()
        {
            return await _context.DiaryEntries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiaryEntry>> GetDiaryEntry(int id)
        {
            var diaryentry = await _context.DiaryEntries.FindAsync(id);

            if (diaryentry == null)
            {
                return NotFound();
            }
            return diaryentry;
        }

        [HttpPost]
        public async Task<ActionResult<DiaryEntry>> PostDiaryEntry(DiaryEntry diaryEntry)
        {
            diaryEntry.Id = 0;

            _context.DiaryEntries.Add(diaryEntry);

            await _context.SaveChangesAsync();

            var resourceUrl = Url.Action(nameof(GetDiaryEntry), new { id = diaryEntry.Id });

            return Created(resourceUrl, diaryEntry);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutDiaryEntry(int id, [FromBody] DiaryEntry diaryEntry)
        {
            if(id != diaryEntry.Id)
            {
                return BadRequest();
            }         

            _context.Entry(diaryEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiaryEntryExist(id))
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

        private bool DiaryEntryExist(int id)
        {
            return _context.DiaryEntries.Any(e => e.Id == id);
            
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<DiaryEntry>> DeleteDiary(int id)
        {
            var diary = await _context.DiaryEntries.FindAsync(id);
            if (diary == null)
            {
                return NotFound();
            }

            _context.DiaryEntries.Remove(diary);
            await _context.SaveChangesAsync();

            return diary;
        }
    }
}
