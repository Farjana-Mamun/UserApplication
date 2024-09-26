using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserApplication.Areas.Administration.Models;
using UserApplication.Data;
using UserApplication.Models;

namespace UserApplication.Areas.Administration.Controllers
{
    
    [Area("Administration")]
    [Authorize]
    public class UserModelsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserModelsController(UserManager<ApplicationUser> userManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Administration/UserModels
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            var model = users.Select(u => new UserModel
            {
                Id = u.Id,
                Name = u.UserName,
                Email = u.Email,
                LastLogin = u.LastLogin,
                RegistrationTime = u.RegistrationTime,
                Status = u.LockoutEnd.HasValue && u.LockoutEnd.Value > DateTimeOffset.Now ? "Blocked" : "Active"
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUsers(List<string> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.LockoutEnd = DateTimeOffset.MaxValue; // Block user
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUsers(List<string> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.LockoutEnd = null; // Unblock user
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers(List<string> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                await _userManager.DeleteAsync(user); // Delete user
            }
            return RedirectToAction("Index");
        }

        // GET: Administration/UserModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        //// GET: Administration/UserModels/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Administration/UserModels/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Email,LastLogin,RegistrationTime,Status")] UserModel userModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(userModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(userModel);
        //}

        // GET: Administration/UserModels/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        // POST: Edit User
        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Email,LastLogin,RegistrationTime,Status")] UserModel userModel)
        {
            if (id != userModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserModelExists(userModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }


        //// GET: Administration/UserModels/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var userModel = await _context.UserModel
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (userModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(userModel);
        //}

        //// POST: Administration/UserModels/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var userModel = await _context.UserModel.FindAsync(id);
        //    if (userModel != null)
        //    {
        //        _context.UserModel.Remove(userModel);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool UserModelExists(string id)
        {
            return _context.UserModel.Any(e => e.Id == id);
        }
    }
}
