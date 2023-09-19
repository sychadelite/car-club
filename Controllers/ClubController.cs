using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppCarClub.Data;
using WebAppCarClub.Data.Interfaces;
using WebAppCarClub.Models;
using WebAppCarClub.ViewModels;

namespace WebAppCarClub.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Details(int id)
        {
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            // Identity to know who's created the data
            var currentUserId = _httpContextAccessor.HttpContext?.User?.GetUserId();
            if (currentUserId != null)
            {
                var createClubViewModel = new CreateClubViewModel
                {
                    UserId = currentUserId
                };

                return View(createClubViewModel);
            } 
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    ClubCategory = clubVM.ClubCategory,
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        State = clubVM.Address.State,
                        City = clubVM.Address.City
                    },
                    UserId = clubVM.UserId
                };

                var createdClub = await _clubRepository.DapperCreateClub(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Club Photo upload failed");
            }

            return View(clubVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);

            if (club == null)
            {
                return View("Error");
            }

            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory,
            };

            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {
                try
                {
                    if (userClub.Image != null)
                    {
                        var fileInfo = new FileInfo(userClub.Image);
                        var publicId = Path.GetFileNameWithoutExtension(fileInfo.Name);
                        await _photoService.DeletePhotoAsync(publicId);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }

                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = photoResult.Url.ToString(),
                    ClubCategory = clubVM.ClubCategory,
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address
                };

                _clubRepository.Update(club);

                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails != null)
            {
                return View(clubDetails);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost, ActionName("DeleteClub")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails != null)
            {
                _clubRepository.Delete(clubDetails);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
