using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LogItUpApi.Entities;
using LogItUpApi.Models;
using LogItUpApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LogItUpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryTypesController : MainController
    {
        public CategoryTypesController(
            UserManager<ApplicationUser> userManager,
            ILogger<CategoryTypesController> logger,
            IDataAccess dataAccess,
            IConfiguration configuration,
            IMapper mapper) : base(userManager, logger, dataAccess, configuration, mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryTypeDTO>>> Get()
        {
            List<CategoryType> categoryTypes = await _dataAccess.GetAll<CategoryType>();

            return Ok(_mapper.Map<IEnumerable<CategoryTypeDTO>>(categoryTypes));
        }
    }
}