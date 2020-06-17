using ImageGallery.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageGallery.API.Services
{
    public class GalleryRepository : IGalleryRepository, IDisposable
    {
        private GalleryContext _context;
        protected DbContextOptions<GalleryContext> ContextOptions { get; }

        public GalleryRepository()
        {
            ContextOptions = new DbContextOptionsBuilder<GalleryContext>()
                    .UseInMemoryDatabase("TestDatabase")
                    .Options;
            _context = new GalleryContext(ContextOptions);
        }

        public bool ImageExists(Guid id)
        {
            return _context.Images.Any(i => i.Id == id);
        }       

        public Image GetImage(Guid id)
        {
            return _context.Images.FirstOrDefault(i => i.Id == id);
        }
  
        public IEnumerable<Image> GetImages()
        {
            return _context.Images
                .OrderBy(i => i.Title).ToList();
        }
        
        public void AddImage(Image image)
        {
            _context.Images.Add(image);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }

            }
        }     
    }
}
