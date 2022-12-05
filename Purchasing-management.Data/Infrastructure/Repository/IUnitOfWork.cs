﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Purchasing_management.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;

        int Save();

        Task<int> SaveAsync(CancellationToken cancellationToken = default(CancellationToken));

        bool CheckConnection();

        DbContext GetDbContext();
    }
}