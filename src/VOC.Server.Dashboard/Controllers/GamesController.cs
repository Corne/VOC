using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VOC.Server.Dashboard.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace VOC.Server.Dashboard.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly IGameStore gamestore;

        public GamesController(IGameStore gamestore)
        {
            this.gamestore = gamestore;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            return gamestore.Games;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Game game)
        {
            //todo validation
            gamestore.Add(game);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            gamestore.Remove(id);
        }
    }
}
