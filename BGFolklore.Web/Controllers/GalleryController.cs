using BGFolklore.Models.Gallery.ViewModels;
using BGFolklore.Services.Public.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;

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
            return View(galleryViewModel);
        }
        public IActionResult Videos()
        {
            GalleryViewModel galleryViewModel = new GalleryViewModel();
            galleryViewModel.EthnoAreaViewModels = galleryService.GetAllGalleryViewModels();
            galleryViewModel.WordsToSearch = "";

            return View(galleryViewModel);
        }
        [HttpPost]
        public IActionResult Videos(GalleryViewModel galleryViewModel)
        {
            galleryViewModel.EthnoAreaViewModels = galleryService.GetAllGalleryViewModels();
            if (ModelState.IsValid && galleryViewModel.WordsToSearch != null)
            {
                try
                {
                    var wordsToSearch = galleryViewModel.WordsToSearch.Split(" ");
                    var filteredVideos = galleryService.GetFilteredVideos(wordsToSearch);
                    if (filteredVideos != null)
                    {
                        galleryViewModel.AllVideos = filteredVideos;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return View(galleryViewModel);
            }
            else
            {
                galleryViewModel.AllVideos = null;
                return View(galleryViewModel);
            }

            //return View(viewModel);
        }
    }
}
