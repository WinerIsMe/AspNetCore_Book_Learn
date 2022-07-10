using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swift.BBS.IServices;
using Swift.BBS.Services;

namespace Swift.BBS.API.Controllers
{
    [Route("api/[controller][action]")]
    [ApiController]
    public class CalculatController : ControllerBase
    {
        private readonly CalculateServices calculateServices;

        public CalculatController(CalculateServices calculateServices)
        {
            this.calculateServices = calculateServices;
        }
        /// <summary>
        /// Sum接口
        /// </summary>
        /// <param name="i">参数i</param>
        /// <param name="j">参数j</param>
        /// <returns></returns>
        [HttpGet]
        public int Get(int i, int j)
        {
            return calculateServices.Sum(i, j);
        }
    }
}
