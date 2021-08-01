using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication7.Dtos;
using WebApplication7.Entities;
using AutoMapperGenerator;
namespace WebApplication7.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AutoMapping(typeof(UserEntity), typeof(UserDto))]
    public class UserController : ControllerBase
    { 
        [HttpGet]
        public UserDto  Get(int id)
        {
            var userEntity = GetFromDB(id);
            var userDto = userEntity.ToUserDto();

            return userDto;
        }

        private UserEntity GetFromDB(int id)
        {
            return new UserEntity { Id = id, Name ="My IO" };
        }
    }
}
