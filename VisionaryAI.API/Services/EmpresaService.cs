using Microsoft.EntityFrameworkCore;
using VisionaryAI.API.Database;
using VisionaryAI.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisionaryAI.API.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly VisionaryAIDBContext _dbContext;

        public EmpresaService(VisionaryAIDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Empresa> BuscarPorId(int id)
        {
            return await _dbContext.Empresas
                                   .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Empresa>> BuscarTodasEmpresas()
        {
            return await _dbContext.Empresas
                                   .ToListAsync();
        }

        public async Task<Empresa> Adicionar(Empresa empresa)
        {
            await _dbContext.Empresas.AddAsync(empresa);
            await _dbContext.SaveChangesAsync();

            return empresa;
        }

        public async Task<Empresa> Atualizar(Empresa empresa, int id)
        {
            Empresa empresaPorId = await BuscarPorId(id);

            if (empresaPorId == null)
            {
                throw new Exception($"A empresa com o ID: {id} não foi encontrada no banco de dados.");
            }

            empresaPorId.Cnpj = empresa.Cnpj;
            empresaPorId.Nome = empresa.Nome;
            empresaPorId.Email = empresa.Email;
            empresaPorId.Descricao = empresa.Descricao;
            

            _dbContext.Empresas.Update(empresaPorId);
            await _dbContext.SaveChangesAsync();

            return empresaPorId;
        }

        public async Task<bool> Apagar(int id)
        {
            Empresa empresaPorId = await BuscarPorId(id);

            if (empresaPorId == null)
            {
                throw new Exception($"A empresa com o ID: {id} não foi encontrada no banco de dados.");
            }

            _dbContext.Empresas.Remove(empresaPorId);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
