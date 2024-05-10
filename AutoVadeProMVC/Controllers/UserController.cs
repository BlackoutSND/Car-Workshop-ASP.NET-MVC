using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using AutoVadeProMVC.Data;
using AutoVadeProMVC.Interfaces;
using AutoVadeProMVC.Models;
using AutoVadeProMVC.Services;
using AutoVadeProMVC.ViewModels;

namespace AutoVadeProMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userService;
        private readonly IPhotoService _photoService;
        public UserController(IUserRepository userService, IPhotoService photoService)
        {
            _userService = userService;
            _photoService = photoService;
        }
        public IActionResult Index()
        {
            return View();
        }
      




        public async Task<IActionResult> Detail(int id)
        {
            if (LoggedInUser.Id == -1)
                return RedirectToAction("SignIn", "User");

            User? user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        public async Task<IActionResult> List()
        {
            if (LoggedInUser.Id == -1)
                return RedirectToAction("SignIn", "User");
            var Users = await _userService.GetUsers(); //Model
            return View(Users);
        }

        public IActionResult Create()
        {
            if (LoggedInUser.Id == -1)
                return RedirectToAction("SignIn", "User");
            if (LoggedInUser.IsAdmin == false)
                return RedirectToAction("List");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel userVM)
        {
            if (await _userService.GetUserByLoginNoTracking(userVM.Login)!=null)
            {
                ModelState.AddModelError("Login", "This login is already taken!");
            }
            else if (ModelState.IsValid)
            {

                var user = new User
                {
                    Name = userVM.Name,
                    Surname = userVM.Surname,
                    Login = userVM.Login,
                    Password = userVM.Password,
                    //Image = result.Url.ToString(),
                    Wage = userVM.Wage
                };
                if (userVM.Image != null)
                {
                    var result = await _photoService.AddPhotoAsync(userVM.Image);
                    user.Image = result.Url.ToString();
                }
                _userService.Add(user);
                return RedirectToAction("List");
            }
            else
            {
                ModelState.AddModelError("Image", "Photo upload has been failed!");
            }

            return View(userVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (LoggedInUser.Id == -1)
                return RedirectToAction("SignIn", "User");
            if (LoggedInUser.IsAdmin == true)
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                    return NotFound();
                var userVM = new EditUserViewModel
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Login = user.Login,
                    Password = user.Password,
                    Wage = user.Wage,
                    IsAdmin = user.IsAdmin,
                    ImageURL = user.Image
                };
                return View(userVM);
            }
            return RedirectToAction("List");

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditUserViewModel userVM)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit User!");
                return View("Edit", userVM);
            }
            var userReq = await _userService.GetUserByIdNoTracking(id);
            if (userReq != null)
            {
                var user = new User
                {
                    Id = userVM.Id,
                    Name = userVM.Name,
                    Surname = userVM.Surname,
                    Login = userVM.Login,
                    Password = userVM.Password,
                    //Image = result.Url.ToString(),
                    Wage = userVM.Wage,
                    IsAdmin = userVM.IsAdmin,
                };
                if (userReq.Image != null)
                {
                    try
                    {
                        await _photoService.DeletePhotoAsync(userReq.Image);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Could not delete photo!");
                        return View(userVM);
                    }
                    
                }
                if(userVM.Image != null) {
                    var result = await _photoService.AddPhotoAsync(userVM.Image);
                    user.Image = result.Url.ToString();
                }
                _userService.Update(user);
                return RedirectToAction("List");
            }
            else
            {
                 return View(userVM);
            }

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (LoggedInUser.Id == -1)
                return RedirectToAction("SignIn", "User");
            if (LoggedInUser.IsAdmin == false)
                return RedirectToAction("List");
            User? userDetails = await _userService.GetUserById(id);
            if (userDetails == null)
            {
                return RedirectToAction("List");
            }
            return View(userDetails);

        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userDetails = await _userService.GetUserById(id);
            if (userDetails==null)
            {
                return NotFound();
            }
            _userService.Delete(userDetails);
            return RedirectToAction("List");
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInUserViewModel userCredsVM)
        {
            User user = new User();
            if (userCredsVM == null|| userCredsVM.Login==null|| userCredsVM.Password == null || userCredsVM.Login == "" || userCredsVM.Password == "")
            {
                return View();
            }
            user = await _userService.GetUserByLoginPassword(userCredsVM.Login, userCredsVM.Password);
            if(user != null)
            {
                LoggedInUser.Id = user.Id;
                LoggedInUser.IsAdmin = user.IsAdmin;
                return RedirectToAction("List");
            }
            ModelState.AddModelError("", "Incorrect credentials!");
            return View();
        }

        public IActionResult SignOut()
        {
            LoggedInUser.IsAdmin=false;
            LoggedInUser.Id=-1;
            return RedirectToAction("SignIn");
        }

    }


}
