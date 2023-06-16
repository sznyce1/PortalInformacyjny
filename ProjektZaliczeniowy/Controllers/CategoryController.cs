using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Models;
using ProjektZaliczeniowy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _categoryService.Delete(id);
            
            return NoContent();
            
        }
        [HttpPost]
        public ActionResult CreateCategory([FromBody]CreateCategoryDto dto)
        {
            //kod wywoływany automatycznie przez atrybut api kontroller
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var name = _categoryService.Create(dto);

            return Created($"/api/category/{name}", null);
        }
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            var categories = _categoryService.GetAll();
            return Ok(categories);
        }
        [HttpGet("{name}")]
        public ActionResult<Category> Get([FromRoute] string name)
        {
            var category = _categoryService.GetByName(name);

            return Ok(category);
        }
    }
}
