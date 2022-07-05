using Microsoft.AspNetCore.Mvc;
using Swift.BBS.IServices;
using Swift.BBS.Services;

namespace Swift.BBS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatContorller : ControllerBase
    {
        ICalculateServices calculateServices = new CalculateServices();

        /// <summary>
        /// 求和接口
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        [HttpGet]
        public int Get(int i,int j)
        {
            return calculateServices.Sum(i, j);
        }
    }
}
