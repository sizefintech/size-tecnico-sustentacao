using Microsoft.EntityFrameworkCore;
using size.Operacao.Business.Interfaces.Repositories;
using size.Operacao.Data.Context;
using size.Core.Data;

namespace size.Operacao.Data.Repositories
{
    public class OperacaoRepository : Repository<Business.AggregateRoots.Operacao>, IOperacaoRepository
    {
        private readonly OperacaoContext _context;

        public OperacaoRepository(OperacaoContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Business.AggregateRoots.Operacao> ObterPorCodigo(string codigo)
        {
            return await _context.Operacoes
                .Include(o => o.Duplicatas)
                .FirstOrDefaultAsync(o => o.Codigo == codigo);
        }

        public async Task<Business.AggregateRoots.Operacao> ObterOperacaoComDuplicatas(string operacaoId)
        {
            return await _context.Operacoes
                .Include(o => o.Duplicatas)
                .FirstOrDefaultAsync(o => o.Id == operacaoId);
        }

        public async Task<IEnumerable<Business.AggregateRoots.Operacao>> ObterPorTomadorId(string tomadorId)
        {
            return await _context.Operacoes
                .Include(o => o.Duplicatas)
                .Where(o => o.TomadorId == tomadorId)
                .ToListAsync();
        }

        public void Adicionar(Business.AggregateRoots.Operacao operacao)
        {
            _context.Operacoes.Add(operacao);
        }

        public void Atualizar(Business.AggregateRoots.Operacao operacao)
        {
            _context.Operacoes.Update(operacao);
        }

        public void Remover(Business.AggregateRoots.Operacao operacao)
        {
            _context.Operacoes.Remove(operacao);
        }
    }
}
