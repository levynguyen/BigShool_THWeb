using BigShool_ThWeb.DTOs;
using BigShool_ThWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigShool_ThWeb.Controllers.Api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;

        public FollowingsController()
        {
            _dbContext = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto followingDto)
        {
            var userId = User.Identity.GetUserId();

            //if (_dbContext.Followings.Any(f => f.FollowerId == userId && f.FolloweeId == followingDto.FolloweeId))
            //{
            //    return BadRequest("Following already exists!");
            //}

            if (_dbContext.Followings.Any(f => f.FollowerId == userId && f.FolloweeId == followingDto.FolloweeId))
            {
                _dbContext.Followings.Remove(_dbContext.Followings.Where(f => f.FollowerId == userId && f.FolloweeId == followingDto.FolloweeId).FirstOrDefault());
                _dbContext.SaveChanges();
                return Ok("Đã hủy Follow!");
            }

            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = followingDto.FolloweeId,
            };

            _dbContext.Followings.Add(following);
            _dbContext.SaveChanges();
            return Ok("Đã Follow thành công!");
        }
    }
}
