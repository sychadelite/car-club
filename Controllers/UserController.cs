using Microsoft.AspNetCore.Mvc;
using WebAppCarClub.Data.Interfaces;
using WebAppCarClub.ViewModels;

namespace WebAppCarClub.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Speed = user.Speed,
                    Mileage = user.Mileage,
                    ProfileImageUrl = user.ProfileImageUrl,
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user != null)
            {
                var userClubsAndRacesDTO = await _userRepository.DapperGetUserClubsAndRaces(user.Id);
                var userDetailViewModel = new UserDetailViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Speed = user.Speed,
                    Mileage = user.Mileage,
                    ProfileImageUrl = user.ProfileImageUrl,
                    Bio = user.Bio,
                    Clubs = userClubsAndRacesDTO.UserClubs,
                    Races = userClubsAndRacesDTO.UserRaces
                };

                ViewData["Message"] = userClubsAndRacesDTO;

                return View(userDetailViewModel);
            } 
            else
            {
                return View("Error");
            }
        }
    }
}
