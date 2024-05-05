using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stiglerdiet;

namespace stiglerdiet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateController : ControllerBase
    {
        private readonly FoodsContext _context;
        private readonly StiglerDietCalculator _calculator;
        public CalculateController(FoodsContext context, StiglerDietCalculator calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        // POST: api/Calculate
        [HttpPost]
        public IActionResult PostCalculate(Calculate calculate)
        {
            var foods = _context.Foods.ToList();
            var result = _calculator.CalculateDiet(calculate, foods);
            return Ok(result);
        } 
    }
}