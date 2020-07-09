using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warframeaccountant.database_action;

namespace Warframeaccountant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private TypeRepo typeRepo;

        public TypeController ()
        {
            typeRepo = new TypeRepo();
        }



        [HttpGet]
        public List<domain.Type> getAllType ()
        {
            Console.WriteLine("Get all type");
            return typeRepo.getAllItem();
        }
    }
}
