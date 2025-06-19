using Microsoft.EntityFrameworkCore;
using BlogPortal.Models;
using BlogPortal.Data;


namespace BlogPortal.Repositories
{
    public class MediaFileRepository
    {
        private readonly ApplicationDbContext _context;

        public MediaFileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddFileAsync(MediaFile file)
        {
            await _context.MediaFiles.AddAsync(file);
            await _context.SaveChangesAsync();
        }
    }
}