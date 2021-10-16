using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using WebApi_EF.Models;
using WebApi_EF.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi_EF.Controller
{

    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]

        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var categories = await context.Categories.ToListAsync();
            return categories;
        }

        [HttpPost]
        [Route("")]

        public async Task<ActionResult<Category>> Post(
              [FromServices] DataContext context, // recebendo o serviço o BD
              [FromBody] Category model // recebendo do corpo da requisição a categoria
        )
        {
            if (ModelState.IsValid) //verificando se as requisições do model foram cumpridas ( MaxLength, MinLength....)
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return model;
            }

            else
            {
                return BadRequest(ModelState);
            }
        }

    }

}