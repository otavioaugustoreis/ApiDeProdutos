﻿using TreinandoPráticasApi.Data.Context;
using TreinandoPráticasApi.Entities;
using TreinandoPráticasApi.Repositories;

namespace TreinandoPráticasApi.Services
{
    public class CategoriaService : Repository<CategoriaEntity>, ICategoria
    {
        public CategoriaService(AppDbContext context) : base(context)
        {
        }
    }
}
