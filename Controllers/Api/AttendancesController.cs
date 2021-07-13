using BigShool_ThWeb.DTOs;
using BigShool_ThWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace BigShool_ThWeb.Controllers
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _dbContext;

        public AttendancesController()
        {
            _dbContext = new ApplicationDbContext();
        }

        //[HttpPost]
        //public IHttpActionResult Attennd(int courseId)
        //{
        //    var userId = User.Identity.GetUserId();

        //    if (_dbContext.Attendances.Any(x => x.AttendeeId == userId && x.CourseId == courseId))
        //        return BadRequest("The Attendance already exist!");
        //    var attendence = new Attendance
        //    {
        //        CourseId = courseId,
        //        AttendeeId = User.Identity.GetUserId()
        //    };
        //    _dbContext.Attendances.Add(attendence);
        //    _dbContext.SaveChanges();
        //    return Ok("Bạn đã đăng kí tham gia thành công");

        //}


        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto attendanceDto)
        {
            var UserId = User.Identity.GetUserId();
            //if (_dbContext.Attendances.Any(x => x.AttendeeId == UserId && x.CourseId == attendanceDto.CourseId))
            //    return BadRequest("The Attendance already exist!");

            if (_dbContext.Attendances.Any(x => x.AttendeeId == UserId && x.CourseId == attendanceDto.CourseId))
            {
                _dbContext.Attendances.Remove(_dbContext.Attendances.Where(x => x.CourseId == attendanceDto.CourseId && x.AttendeeId == UserId).FirstOrDefault());
                _dbContext.SaveChanges();
                return Ok("Hủy đăng ký tham gia thành công!");
            }

            var attendence = new Attendance
            {
                CourseId = attendanceDto.CourseId,
                AttendeeId = UserId
            };

            _dbContext.Attendances.Add(attendence);
            _dbContext.SaveChanges();
            return Ok("Bạn đã đăng kí tham gia thành công");
        }
    }
}
