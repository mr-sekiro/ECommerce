using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.Dtos.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public BasketController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/basket/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDto>> GetBasket(string id)
        {
            var basket = await _serviceManager.BasketService.GetBasketAsync(id);
            return basket is null ? NotFound() : Ok(basket);
        }

        // POST: api/basket
        [HttpPost]
        public async Task<ActionResult<BasketDto>> UpdateBasket(BasketDto basketDto)
        {
            var updated = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basketDto);
            return Ok(updated);
        }

        // DELETE: api/basket/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            var deleted = await _serviceManager.BasketService.DeleteBasketAsync(id);
            return deleted ? Ok(true) : NotFound(false);
        }
    }
}
