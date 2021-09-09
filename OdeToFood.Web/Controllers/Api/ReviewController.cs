using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private IReviewRepository _reviewRepository;
        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var reviews =  await _reviewRepository.GetAllAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>Get(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult>Post([FromBody] Review review)
        {
            if(ModelState.IsValid)
            {
                var model = await _reviewRepository.AddAsync(review);
                return CreatedAtAction(nameof(Post), model);
            }
            return BadRequest();
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Review review, int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(review.Id != id)
            {
                return BadRequest();
            }
            if(await _reviewRepository.GetByIdAsync(id) == null)
            {
                return NotFound();
            }
            await _reviewRepository.UpdateAsync(review);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _reviewRepository.GetByIdAsync(id) == null)
            {
                return NotFound();
            }
            await _reviewRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
