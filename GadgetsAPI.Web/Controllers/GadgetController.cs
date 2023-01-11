using DBAccess.Repositories;
using GadgetsAPI.Web.Exceptions;
using GadgetsAPI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GadgetsAPI.Web.Controllers
{
    [ApiController]
    [Route("gadgets")]
    public class GadgetController : ControllerBase
    {
        private IGadgetRepository repository;
        public GadgetController(IGadgetRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("")]
        public async Task<List<Gadget>> GetAllGadgets()
        {
            var models = await repository.GetAllGadgetsAsync();

            var gadgets = new List<Gadget>();

            foreach (var model in models)
            {
                gadgets.Add(new Gadget(model));
            }

            return gadgets;
        }

        [HttpGet("{id:int}")]
        public async Task<Gadget> GetGadgetById(int id)
        {
            var model = await repository.GetGadgetAsync(id);

            if (model == null)
            {
                throw new GadgetNotFoundException(id);
            }

            return new Gadget(model);
        }

        [HttpGet("category/{id:int}")]
        public async Task<List<Gadget>> GetGadgetsByCategoryId(int id)
        {
            var models = await repository.GetAllGadgetsAsync(new List<int> { id }, null, null, null);

            var gadgets = new List<Gadget>();

            foreach (var model in models)
            {
                gadgets.Add(new Gadget(model));
            }

            return gadgets;
        }

        [HttpGet("producer/{id:int}")]
        public async Task<List<Gadget>> GetGadgetsByProducerId(int id)
        {
            var models = await repository.GetAllGadgetsAsync(null, new List<int> { id }, null, null);

            var gadgets = new List<Gadget>();

            foreach (var model in models)
            {
                gadgets.Add(new Gadget(model));
            }

            return gadgets;
        }



        [HttpPost]
        public async Task<Gadget> AddGadget([FromBody] Gadget gadget)
        {
            if(string.IsNullOrWhiteSpace(gadget.Model) || gadget.Price < 0)
            {
                throw new BadRequestException();
            }

            var model = await repository.AddGadgetAsync(new DBAccess.Models.GadgetModel()
            {
                CategoryId = gadget.Category != null ? gadget.Category.Id : null,
                ProducerId = gadget.Producer != null ? gadget.Producer.Id : null,
                Model = gadget.Model,
                Price = gadget.Price
            });

            return new Gadget(model);
        }

        [HttpPut]
        public async Task<Gadget> UpdateGadget([FromBody] Gadget gadget)
        {
            if (string.IsNullOrWhiteSpace(gadget.Model) || gadget.Price < 0)
            {
                throw new BadRequestException();
            }

            var model = await repository.UpdateGadgetAsync(new DBAccess.Models.GadgetModel()
            {
                Id = gadget.Id,
                CategoryId = gadget.Category != null ? gadget.Category.Id : null,
                ProducerId = gadget.Producer != null ? gadget.Producer.Id : null,
                Model = gadget.Model,
                Price = gadget.Price
            });

            return new Gadget(model);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> RemoveGadgetById(int id)
        {
            if(await repository.RemoveGadgetAsync(id) == false)
            {
                throw new GadgetNotFoundException(id);
            }

            return true;
        }

    }
}
