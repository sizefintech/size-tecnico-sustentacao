using Microsoft.EntityFrameworkCore;
using size.Carrinho.Business.Interfaces.Repositories;
using size.Carrinho.Data.Context;
using size.Core.Data;

namespace size.Carrinho.Data.Repositories
{
    public class CarrinhoRepository : Repository<Business.AggregateRoots.Carrinho>, ICarrinhoRepository
    {
        private readonly CarrinhoContext _context;

        public CarrinhoRepository(CarrinhoContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Business.AggregateRoots.Carrinho> ObterPorAgregateId(string agregateId)
        {
            return await _context.Carrinhos
                .Include(c => c.Duplicatas)
                .FirstOrDefaultAsync(c => c.AgregateId == agregateId);
        }

        public async Task<Business.AggregateRoots.Carrinho> ObterCarrinhoComDuplicatas(string carrinhoId)
        {
            return await _context.Carrinhos
                .Include(c => c.Duplicatas)
                .FirstOrDefaultAsync(c => c.Id == carrinhoId);
        }

        public void Adicionar(Business.AggregateRoots.Carrinho carrinho)
        {
            _context.Carrinhos.Add(carrinho);
        }

        public void Atualizar(Business.AggregateRoots.Carrinho carrinho)
        {
            _context.Carrinhos.Update(carrinho);
        }

        public void Remover(Business.AggregateRoots.Carrinho carrinho)
        {
            _context.Carrinhos.Remove(carrinho);
        }
    }
}
