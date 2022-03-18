using Microsoft.AspNetCore.Mvc;
using Dapper;
using Confluent.Kafka;

namespace api.Controllers
{
    public enum SearchStatus
    {
        Pendente = 1,
        Pesquisando = 2,
        Salvo = 3
    }

    [ApiController]
    [Route("[controller]/[action]")]
    public class PokemonController : Controller
    {
        [HttpGet]
        public IActionResult Search(string name)
        {
            if (!PokemonSearch(name))
            {
                GoSearchPokemon(name);

                return Ok(new { Name = name, Status = SearchStatus.Pendente });
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Status(string name)
        {
            var status = PokemonStatus(name);
            
            return Ok(new {  Status = status });
        }

        [HttpGet]
        public IActionResult Info(string name)
        {
            var pokemon = PokemonInfo(name);

            return Ok(pokemon);
        }

        [HttpGet]
        public IActionResult Fila()
        {
            return Created("", SendMessageByKafka("fixa"));
        }

        private bool PokemonSearch(string name)
        {
            using (var connection = new Npgsql.NpgsqlConnection("Server=host.docker.internal;Port=5432;Database=devspace;User ID=devspace;Password=devspace;"))
            {
                connection.Open();
                dynamic dinamico = connection.Query($"SELECT 1 FROM search_pokemon WHERE name = '{name}';");
                if(dinamico.Any())
                {
                    return true;
                }
            }

            return false;
        }

        private int PokemonStatus(string name)
        {
            using (var connection = new Npgsql.NpgsqlConnection("Server=host.docker.internal;Port=5432;Database=devspace;User ID=devspace;Password=devspace;"))
            {
                connection.Open();
                dynamic dinamico = connection.Query($"SELECT status FROM search_pokemon WHERE name = '{name}';");
                if (dinamico.Any())
                {
                    return dinamico.status;
                }
            }

            throw new Exception();
        }

        private object PokemonInfo(string name)
        {
            using (var connection = new Npgsql.NpgsqlConnection("Server=host.docker.internal;Port=5432;Database=devspace;User ID=devspace;Password=devspace;"))
            {
                connection.Open();
                dynamic dinamico = connection.QueryFirst($"SELECT * FROM pokemons WHERE name = '{name}';");

                return dinamico;
            }

            throw new Exception();
        }

        private void GoSearchPokemon(string name)
        {
            using (var connection = new Npgsql.NpgsqlConnection("Server=host.docker.internal;Port=5432;Database=devspace;User ID=devspace;Password=devspace;"))
            {
                connection.Open();
                connection.Query($"INSERT INTO search_pokemon (name, status) VALUES ('{name}', {(int)SearchStatus.Pendente});");

                SendMessageByKafka(name);
            }           
        }

        private string SendMessageByKafka(string message)
        {
            var config = new ProducerConfig { BootstrapServers = "host.docker.internal:9094" };

            
            using(var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var sendResult = producer.ProduceAsync(
                        "tweets",
                        new Message<Null, string> { Value = message }
                    ).GetAwaiter().GetResult();

                    return $"Mensagem: '{sendResult.Value}' de '{sendResult.TopicPartitionOffset}'";
                }
                catch (Exception)
                {
                    Console.WriteLine("Deu ruim!!!");
                }
            }

            return String.Empty;
        }
    }
}
