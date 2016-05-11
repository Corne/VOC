using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using VOC.Server.Dashboard.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace VOC.Server.Dashboard.Controllers
{
    public class GamesController : ApiController
    {
        private readonly IGameStore gamestore;

        public GamesController(IGameStore gamestore)
        {
            this.gamestore = gamestore;
        }

        // GET: api/values
        public IEnumerable<Game> Get()
        {
            return gamestore.Games;
        }


        public void Post([FromBody]Game game)
        {
            //todo validation
            gamestore.Add(game);
        }

        public void Delete(Guid id)
        {
            gamestore.Remove(id);
        }
    }
}
