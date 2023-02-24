using Microsoft.AspNetCore.Mvc;
using V2.Models;
using V2.Services;

namespace V2.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoWorkController : ControllerBase
    {
        private IDoSomethingService _service;

        public DoWorkController(IDoSomethingService service)
        {
            _service = service;
        }

        //Из за примитивности задания, не вижу смысла корячится и подстраивать методы под ассинхрон

        //GET: api/DoWork/GenerateRandom
        [HttpGet("GenerateRandom")]
        public ActionResult<Something> GetRandomNameAndValue()
        {
            //Генерируем рандом свойство
            var randomValue = _service.GenerateRandomEnumerable();

            //Добавляем елемент в статическое свойсвто
            DoSomethingService.Somethings.Add(randomValue);
            return _service.GenerateRandomEnumerable();
        }

        //GET: api/DoWork/Response
        [HttpGet("Response")]
        public ActionResult<string> GetResponse()
        {
            return _service.GetDateAndValues();
        }
    }
}
