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
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]

        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context.Products.Include(x => x.Category).ToListAsync();
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]  //passando como parametro o id

        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
        {
            var product = await context.Products.Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }


        [HttpGet]
        [Route("categories/{id:int}")]  //passando como parametro o id
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products = await context.Products
            .Include(x => x.Category).Where(x => x.CategoryId == id)
            .AsNoTracking()
            .ToListAsync();
            return products;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post(
              [FromServices] DataContext context, // recebendo o serviço o BD
              [FromBody] Product model // recebendo do corpo da requisição a categoria
        )
        {
            if (ModelState.IsValid) //verificando se as requisições do model foram cumpridas ( MaxLength, MinLength....)
            {
                context.Products.Add(model);
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