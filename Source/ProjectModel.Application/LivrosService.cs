using ProjectModel.Domain.Adapters;
using ProjectModel.Domain.Models;
using ProjectModel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectModel.Application
{
    public class LivrosService : ILivrosService
    {
        private readonly IGoogleBooksReadOnlyAdapter googleBooksReadOnlyAdapter;
        private readonly ApplicationConfiguration configuration;

        public LivrosService(IGoogleBooksReadOnlyAdapter googleBooksReadOnlyAdapter, 
            ApplicationConfiguration configuration)
        {
            this.googleBooksReadOnlyAdapter = googleBooksReadOnlyAdapter ?? throw new ArgumentNullException(nameof(googleBooksReadOnlyAdapter));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<Livro>> ObterLivroPorTituloAsync(string titulo)
        {
            if (string.IsNullOrEmpty(titulo))
                throw new ArgumentNullException(nameof(titulo));

            IEnumerable<Livro> resultado = await googleBooksReadOnlyAdapter.GetLivroPorTituloAsync(titulo);

            return resultado;
        }
    }
}
