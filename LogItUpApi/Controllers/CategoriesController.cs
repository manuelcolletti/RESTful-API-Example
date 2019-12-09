using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LogItUpApi.Entities;
using LogItUpApi.Filters;
using LogItUpApi.Models;
using LogItUpApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LogItUpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : MainController
    {
        public CategoriesController(
            UserManager<ApplicationUser> userManager,
            ILogger<CategoryTypesController> logger,
            IDataAccess dataAccess,
            IConfiguration configuration,
            IMapper mapper) : base(userManager, logger, dataAccess, configuration, mapper) {
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory(CategoryDTO model)
        {
            Category newCategory = await _dataAccess.Insert(await GetUser(), _mapper.Map<Category>(model));

            return _mapper.Map<CategoryDTO>(newCategory);
        }

    }
}