using BigShool_ThWeb.Models;
using BigShool_ThWeb.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigShool_ThWeb.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }



        // GET: CoursesController
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Heading = "Add Course"
            };
            return View(viewModel);

        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }
            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                CategoryId = viewModel.Category,
                Place = viewModel.Place
            };
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Where(x => !x.IsCanceled && x.DateTime > DateTime.Now).OrderBy(x => x.DateTime)
                .Include(l => l.Lecturer)
                .Include(l => l.category)
                .ToList();

            foreach (var item in courses)
            {
                item.isAttended = true;
            }

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };

            foreach (var item in viewModel.UpcommingCourses)
            {
                if (_dbContext.Attendances.Any(x => x.CourseId == item.Id && x.AttendeeId == userId))
                {
                    item.isAttended = true;
                }
                if (_dbContext.Followings.Any(x => x.FollowerId == userId && x.FolloweeId == item.LecturerId))
                {
                    item.isFollowed = true;
                }
            }
            return View(viewModel);
        }
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Courses
                .Include(x => x.Lecturer)
                .Include(x => x.category)
                .Where(x => x.LecturerId == userId && x.DateTime > DateTime.Now)
                .OrderBy(x => x.DateTime).ToList();

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);
        }
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Courses.Single(x => x.LecturerId == userId && x.Id == id);

            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Date = courses.DateTime.ToString("dd/MM/yyyy"),
                Time = courses.DateTime.ToString("HH:mm"),
                Category = courses.CategoryId,
                Place = courses.Place,
                Heading = "Edit Course",
                Id = courses.Id
            };
            return View("Create",viewModel);
        }

        [HttpPost]
        public ActionResult Update(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(c => c.Id == viewModel.Id && c.LecturerId == userId);

            course.Place = viewModel.Place;
            course.DateTime = viewModel.GetDateTime();
            course.CategoryId = viewModel.Category;

            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}