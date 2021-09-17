using BGFolklore.Models.Gallery.ViewModels;
using BGFolklore.Services.Public.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BGFolklore.Web.Controllers
{
    public class GalleryController : BaseController
    {
        private readonly IGalleryService galleryService;

        public GalleryController(ILogger<GalleryController> logger,
            IWebHostEnvironment webHostEnvironment,
            IGalleryService galleryService) : base(logger, webHostEnvironment)
        {
            this.galleryService = galleryService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Images()
        {
            GalleryViewModel galleryViewModel = new GalleryViewModel();
            galleryViewModel.EthnoAreaViewModels = galleryService.GetAllGalleryViewModels();
            galleryViewModel.FilterViewModel = new GalleryFilterViewModel();
            return View(galleryViewModel);
        }
        public IActionResult Videos()
        {
            GalleryViewModel galleryViewModel = new GalleryViewModel();
            galleryViewModel.EthnoAreaViewModels = galleryService.GetAllGalleryViewModels();
            galleryViewModel.FilterViewModel = new GalleryFilterViewModel();

            return View(galleryViewModel);
        }
        [HttpPost]
        public IActionResult Videos(GalleryViewModel galleryViewModel)
        {
            //Трябва да го измисля как ще стане...
            GalleryFilterViewModel viewModel = new GalleryFilterViewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    var wordsToSearch = galleryViewModel.FilterViewModel.VideoSearch.Split(" ");
                    var filteredVideos = galleryService.GetFilteredVideos(wordsToSearch);
                    if (filteredVideos != null)
                    {
                    }
                }
                catch (System.Exception)
                {
                    throw;
                }
                return View(viewModel);
            }
            else
            {
                return View(viewModel);
            }

            //return View(viewModel);
        }
    }
}
