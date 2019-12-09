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

        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> Get()
        {
            List<Category> items = await _dataAccess.GetList<Category>(await GetUser(), x => true);

            return _mapper.Map<List<CategoryDTO>>(items);
        }

        [HttpGet("{Id}", Name = "Get")]
        public async Task<ActionResult<CategoryDTO>> Get(long Id)
        {
            Category item = await _dataAccess.GetFirst<Category>(await GetUser(), x => x.Id == Id);

            return _mapper.Map<CategoryDTO>(item);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]CategoryDTO model)
        {
            Category item = await _dataAccess.Insert(await GetUser(), _mapper.Map<Category>(model));

            CategoryDTO itemDTO = _mapper.Map<CategoryDTO>(item);

            return new CreatedAtActionResult("Get", "Categories", new { item.Id}, itemDTO);
        }

        [HttpPut]
        public async Task<ActionResult> Put(long Id, [FromBody]CategoryDTO model)
        {
            var item = await _dataAccess.GetFirst<Category>(await GetUser(), x => x.Id == model.Id);

            if (item.Id != Id)
                return BadRequest();

            if (item == null)
                return NotFound();

            item.Description = model.Description;

            item.CategoryTypeId = model.CategoryTypeId;

            _dataAccess.Update(await GetUser(), _mapper.Map<Category>(model));

            return Ok();
        }

        [HttpDelete("Id")]
        public async Task<ActionResult<CategoryDTO>> Delete(long Id)
        {
            var item = await _dataAccess.GetFirst<Category>(await GetUser(), x => x.Id == Id);

            if (item == null)
                return NotFound();

            _dataAccess.Delete(await GetUser(), item);

            return Ok(_mapper.Map<CategoryDTO>(item));
        }

    }
}