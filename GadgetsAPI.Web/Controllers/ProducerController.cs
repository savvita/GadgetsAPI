using DBAccess.Repositories;
using GadgetsAPI.Web.Exceptions;
using GadgetsAPI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GadgetsAPI.Web.Controllers
{
    [ApiController]
    [Route("producers")]
    public class ProducerController : ControllerBase
    {
        private IProducerRepository repository;
        public ProducerController(IProducerRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("")]
        public async Task<List<Producer>> GetAllProducers()
        {
            var models = await repository.GetAllProducersAsync();

            var producers = new List<Producer>();

            foreach (var model in models)
            {
                producers.Add(new Producer()
                {
                    Id = model.Id,
                    ProducerName = model.ProducerName
                });
            }

            return producers;
        }


        [HttpGet("{id:int}")]
        public async Task<Producer> GetProducerById(int id)
        {
            var model = await repository.GetProducerAsync(id);

            if (model == null)
            {
                throw new ProducerNotFoundException(id);
            }

            return new Producer()
            {
                Id = model.Id,
                ProducerName = model.ProducerName
            };
        }

        [HttpPost()]
        public async Task<Producer> AddProducer(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BadRequestException();
            }

            var model = await repository.AddProducerAsync(new DBAccess.Models.ProducerModel()
            {
                ProducerName = name
            });

            return new Producer()
            {
                Id = model.Id,
                ProducerName = model.ProducerName
            };
        }

        [HttpPut]
        public async Task<Producer> UpdateProducer(Producer producer)
        {
            if (string.IsNullOrWhiteSpace(producer.ProducerName))
            {
                throw new BadRequestException();
            }

            var model = await repository.UpdateProducerAsync(new DBAccess.Models.ProducerModel()
            {
                Id = producer.Id,
                ProducerName = producer.ProducerName
            });

            return new Producer()
            {
                Id = model.Id,
                ProducerName = model.ProducerName
            };
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> RemoveProducerById(int id)
        {
            return await repository.RemoveProducerAsync(id);
        }
    }
}
