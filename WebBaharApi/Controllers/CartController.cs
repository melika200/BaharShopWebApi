using Bahar.Application.Dto.Cart;
using Bahar.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;

namespace WebBaharApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly IMapper _mapper;

        public CartController(CartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

     
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CartItemDto>))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var items = await _cartService.GetUserCartItems(userId.Value);

         
            var itemsDto = _mapper.Map<List<CartItemDto>>(items);

            return Ok(itemsDto);
        }

        [HttpPost("add")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto model)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _cartService.AddToCart(userId.Value, model.ProductId, model.Quantity);

            if (result)
                return Ok(new { success = true });
            else
                return BadRequest(new { success = false, message = "موجودی کافی نیست یا محصول یافت نشد." });
        }

        
        [HttpPost("remove")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> RemoveCartItem([FromBody] RemoveCartItemDto model)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _cartService.RemoveCartItem(model.CartItemId);

            if (result)
                return Ok(new { success = true });
            else
                return NotFound(new { success = false, message = "آیتم یافت نشد." });
        }

     
        [HttpPost("update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] UpdateCartItemDto model)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _cartService.UpdateCartItemQuantity(model.CartItemId, model.Quantity);

            if (result)
                return Ok(new { success = true });
            else
                return BadRequest(new { success = false, message = "موجودی کافی نیست یا آیتم یافت نشد." });
        }

    
        [HttpPost("checkout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto model)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _cartService.Checkout(userId.Value, model.Address, model.Mobile);

            if (result)
                return Ok(new { success = true });
            else
                return BadRequest(new { success = false, message = "خطا در ثبت سفارش." });
        }

     
        private long? GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (long.TryParse(userIdClaim, out var userId))
                return userId;

            return null;
        }
    }
}