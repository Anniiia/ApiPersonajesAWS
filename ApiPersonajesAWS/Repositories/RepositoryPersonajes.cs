using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;

namespace ApiPersonajesAWS.Repositories
{
    public class RepositoryPersonajes
    {
        private PersonajesContext context;
        private IConfiguration configuration;
        public RepositoryPersonajes(PersonajesContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            return await this.context.Personajes.ToListAsync();
        }

        public async Task<Personaje> FindPersonajeAsync(int id)
        {
            return await this.context.Personajes.FirstOrDefaultAsync(x => x.IdPersonaje == id);
        }

        private async Task<int> GetMaxIdPersonajeAsync()
        {
            return await this.context.Personajes.MaxAsync(x => x.IdPersonaje) + 1;
        }

        public async Task CreatePersonajeAsync(string nombre, string imagen)
        {
            Personaje personaje = new Personaje
            {
                IdPersonaje = await this.GetMaxIdPersonajeAsync(),
                Nombre = nombre,
                Imagen = imagen
            };
            this.context.Personajes.Add(personaje);
            await this.context.SaveChangesAsync();
        }


        public async Task UpdatePersonajeAsync(int id, string nombre, string imagen)
        {
            string connectionString = this.configuration.GetConnectionString("MySqlTelevision");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("ActualizarPersonaje", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id", id);
                    command.Parameters.AddWithValue("p_nombre", nombre);
                    command.Parameters.AddWithValue("p_imagen", imagen);


                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
