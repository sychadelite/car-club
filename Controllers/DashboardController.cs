using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using WebAppCarClub.Data.Interfaces;
using WebAppCarClub.Models;
using WebAppCarClub.ViewModels;

namespace WebAppCarClub.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }

        private void MapUserEdit(User user, EditUserDashboardViewModel editVM, ImageUploadResult photoResult = null, string defaultPhoto = null)
        {
            user.Id = editVM.Id;
            user.Speed = editVM.Speed;
            user.Mileage = editVM.Mileage;
            user.ProfileImageUrl = photoResult != null ? photoResult.Url.ToString() : defaultPhoto;
            user.City = editVM.City;
            user.State = editVM.State;
            user.Bio = editVM.Bio;
        }

        public async Task<IActionResult> Index()
        {
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var dashboardViewModel = new DashboardViewModel()
            {
                Clubs = userClubs,
                Races = userRaces
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(currentUserId);

            if (user == null)
            {
                return View("Error");
            }

            var editUserModel = new EditUserDashboardViewModel()
            {
                Id = user.Id,
                Speed = user.Speed,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State,
                Bio = user.Bio
            };

            return View(editUserModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editVM);
            }

            User user = await _dashboardRepository.GetUserByIdNoTracking(editVM.Id);

            if (user.ProfileImageUrl == null || user.ProfileImageUrl == "")
            {
                var photoResult = editVM.Image != null ? await _photoService.AddPhotoAsync(editVM.Image) : null;

                MapUserEdit(user, editVM, photoResult);

                //_dashboardRepository.Update(user);
                await _dashboardRepository.DapperUpdateUser(user);

                return RedirectToAction("Index");
            } 
            else
            {
                try
                {
                    var fileInfo = new FileInfo(user.ProfileImageUrl);
                    var publicId = Path.GetFileNameWithoutExtension(fileInfo.Name);
                    await _photoService.DeletePhotoAsync(publicId);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }

                var photoResult = editVM.Image != null ? await _photoService.AddPhotoAsync(editVM.Image) : null;

                var userDefaultDP = await _dashboardRepository.DapperGetUserProfileImage(editVM.Id);

                MapUserEdit(user, editVM, photoResult, userDefaultDP.ProfileImageUrl);

                //_dashboardRepository.Update(user);
                await _dashboardRepository.DapperUpdateUser(user);

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserProfile([FromBody] EditUserDashboardViewModel editVM)
        {
            try
            {
                var user = await _dashboardRepository.GetUserById(editVM.Id);

                if (user == null)
                {
                    return NotFound();
                }

                await _dashboardRepository.DapperDeleteUser(editVM.Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
