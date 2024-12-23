﻿using Microsoft.EntityFrameworkCore;
using TreinandoPráticasApi.Data.Context;
using TreinandoPráticasApi.Entities;
using TreinandoPráticasApi.Repositories;

namespace TreinandoPráticasApi.Services
{
    public class ProdutoService : Repository<ProdutoEntity>, IProduto
    {
        public ProdutoService(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<ProdutoEntity> ProdutoPorCategoria(int id)
        {
            //Lambda
            return Get()
                   .Where(x => x.categoriaEntity.Id == id).OrderByDescending(x => x.Id);
        }

        // Testar desse jeito
        //Apenas para selects grandes você precisa executar uma select na mão
        //return _context._ProdutoEntity
        //    .FromSql($"SELECT * FROM TB_PRODUTOS WHERE pk_id = {id}")
        //    .ToList();
    }
}
