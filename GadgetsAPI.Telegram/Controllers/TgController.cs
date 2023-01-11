using DBAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GadgetsAPI.Tg.Controllers
{
    [ApiController]
    [Route("api/tg")]
    public class TgController : ControllerBase
    {
        private TelegramBotClient client;
        private IGadgetRepository gadgetRepository;
        private ICategoryRepository categoryRepository;
        private IProducerRepository producerRepository;

        private Dictionary<string, Func<Update, Task>> handles = new Dictionary<string, Func<Update, Task>>();

        public TgController(TelegramBotClient client, IGadgetRepository gadgetRepository, ICategoryRepository categoryRepository, IProducerRepository producerRepository)
        {
            this.client = client;
            this.gadgetRepository = gadgetRepository;
            this.categoryRepository = categoryRepository;
            this.producerRepository = producerRepository;

            handles.Add("/categories", SendCategories);
            handles.Add("/producers", SendProducers);
            handles.Add("/gadgets", SendGadgets);
            handles.Add("/filter", SendFilteredGadgets);
            handles.Add("/help", SendHelp);
        }

        [HttpPost]
        public async Task<IResult> Post(Update update)
        {
            if (update != null && update.Message != null && update.Message.Text != null)
            {
                var idx = update.Message.Text.IndexOf(' ');
                string command;

                if (idx > 0)
                {
                    command = update.Message.Text.Substring(0, idx).ToLower();
                }
                else
                {
                    command = update.Message.Text.ToLower();
                }

                if (handles.ContainsKey(command))
                {
                    await handles[command](update);
                    return Results.Ok();
                }
            }

            return Results.BadRequest();
        }

        private async Task SendHelp(Update update)
        {
            if (update != null)
            {
                if (update.Message != null && update.Message.From != null)
                {
                    string help = @"
                            /help - get list of commands
                            /categories - get list of all categories
                            /producers - get list of all producers
                            /gadgets - get list of all gadgets
                            /filter [?p producer] [?c category] [?pr min-max] - get gadgets using filters.
	                            Producer - list of producers, separated by space.
	                            Category - list of categories, separated by space.
	                            Min - Minimum price
	                            Max - maximum price";

                    await client.SendTextMessageAsync(update.Message.From.Id, help);
                }
            }


        }

        private async Task SendCategories(Update update)
        {
            if (update != null)
            {
                if (update.Message != null && update.Message.From != null)
                {
                    var categories = await categoryRepository.GetAllCategoriesAsync();

                    if (categories.Count() > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var category in categories)
                        {
                            sb.Append(category.CategoryName);
                            sb.Append(Environment.NewLine);
                        }
                        await client.SendTextMessageAsync(update.Message.From.Id, sb.ToString());
                    }

                    else
                    {
                        await client.SendTextMessageAsync(update.Message.From.Id, "Not found");
                    }
                }
            }  
        }

        private async Task SendProducers(Update update)
        {
            if (update != null)
            {
                if (update.Message != null && update.Message.From != null)
                {
                    var producers = await producerRepository.GetAllProducersAsync();

                    if (producers.Count() > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var producer in producers)
                        {
                            sb.Append(producer.ProducerName);
                            sb.Append(Environment.NewLine);
                        }
                        await client.SendTextMessageAsync(update.Message.From.Id, sb.ToString());
                    }

                    else
                    {
                        await client.SendTextMessageAsync(update.Message.From.Id, "Not found");
                    }
                }
            }
        }

        private async Task SendGadgets(Update update)
        {
            if (update != null)
            {
                if (update.Message != null && update.Message.From != null)
                {
                    var gadgets = await gadgetRepository.GetAllGadgetsAsync();

                    if (gadgets.Count() > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var gadget in gadgets)
                        {
                            sb.Append($"{(gadget.Producer != null ? gadget.Producer.ProducerName : "")} {gadget.Model} ({gadget.Price})");
                            sb.Append(Environment.NewLine);
                        }
                        await client.SendTextMessageAsync(update.Message.From.Id, sb.ToString());
                    }

                    else
                    {
                        await client.SendTextMessageAsync(update.Message.From.Id, "Not found");
                    }
                }
            }
        }

        private async Task SendFilteredGadgets(Update update)
        {
            if (update != null)
            {
                if (update.Message != null && update.Message.Text != null && update.Message.From != null)
                {
                    var filters = update.Message.Text.Split("?").Select(str => str.Trim().ToLower());
                    List<int>? producerFilter = null;
                    List<int>? categoryFilter = null;
                    decimal? minPrice = null;
                    decimal? maxPrice = null;

                    foreach(var filter in filters)
                    {
                        var values = filter.Split(" ").Select(str => str.Trim().ToLower()).ToList();

                        if (values.Count > 1)
                        {
                            if (values[0] == "p")
                            {
                                producerFilter = new List<int>();

                                for(int i = 1; i < values.Count; i++)
                                {
                                    var producer = await producerRepository.GetProducerAsync(values[i]);

                                    if (producer != null)
                                    {
                                        producerFilter.Add(producer.Id);
                                    }
                                }
                            }
                            else if (values[0] == "c")
                            {
                                categoryFilter = new List<int>();

                                for (int i = 1; i < values.Count; i++)
                                {
                                    var category = await categoryRepository.GetCategoryAsync(values[i]);

                                    if (category != null)
                                    {
                                        categoryFilter.Add(category.Id);
                                    }
                                }
                            }
                            else if (values[0] == "pr")
                            {
                                var price = values[1].Split("-").Select(str => str.Trim().ToLower()).ToList();

                                if(price.Count > 0)
                                {
                                    if (decimal.TryParse(price[0], out decimal min))
                                    {
                                        minPrice = min;
                                    }

                                    if(price.Count > 1)
                                    {
                                        if (decimal.TryParse(price[1], out decimal max))
                                        {
                                            maxPrice = max;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var gadgets = await gadgetRepository.GetAllGadgetsAsync(categoryFilter, producerFilter, minPrice, maxPrice);

                    if (gadgets.Count() > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var gadget in gadgets)
                        {
                            sb.Append($"{(gadget.Producer != null ? gadget.Producer.ProducerName : "")} {gadget.Model} ({gadget.Price})");
                            sb.Append(Environment.NewLine);
                        }
                        await client.SendTextMessageAsync(update.Message.From.Id, sb.ToString());
                    }

                    else
                    {
                        await client.SendTextMessageAsync(update.Message.From.Id, "Not found");
                    }
                }
            }
        }
    }
}
