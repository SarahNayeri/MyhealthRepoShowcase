using AutoMapper;
using ImageGallery.API.Services;
using ImageGallery.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImageGallery.API.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IGalleryRepository _galleryRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public ImagesController(
            IGalleryRepository galleryRepository,
            IWebHostEnvironment hostingEnvironment,
            IMapper mapper)
        {
            _galleryRepository = galleryRepository ?? 
                throw new ArgumentNullException(nameof(galleryRepository));
            _hostingEnvironment = hostingEnvironment ?? 
                throw new ArgumentNullException(nameof(hostingEnvironment));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        public IActionResult GetImages()
        {
            // get from repo
            var imagesFromRepo = _galleryRepository.GetImages();

            // map to model
            var imagesToReturn = _mapper.Map<IEnumerable<Model.Image>>(imagesFromRepo);

            // return
            return Ok(imagesToReturn);
        }

        [HttpGet("{id}", Name = "GetImage")]
        public IActionResult GetImage(Guid id)
        {          
            var imageFromRepo = _galleryRepository.GetImage(id);

            if (imageFromRepo == null)
            {
                return NotFound(404);
            }

            var imageToReturn = _mapper.Map<Model.Image>(imageFromRepo);

            return Ok(imageToReturn);
        }

        [HttpPost()]
        public IActionResult CreateImage([FromBody] ImageForCreation imageForCreation)
        {
            var imageEntity = _mapper.Map<Entities.Image>(imageForCreation);

            var webRootPath = _hostingEnvironment.WebRootPath;

            // create the jpg filename
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            
            var filePath = Path.Combine($"{webRootPath}/images/{fileName}");

            System.IO.File.WriteAllBytes(filePath, imageForCreation.Bytes);

            imageEntity.FileName = fileName;
            imageEntity.Id = Guid.NewGuid();

            _galleryRepository.AddImage(imageEntity);

            _galleryRepository.Save();

            var imageToReturn = _mapper.Map<Image>(imageEntity);

            return CreatedAtRoute("GetImage",
                new { id = imageToReturn.Id },
                imageToReturn);
        }
    }
}