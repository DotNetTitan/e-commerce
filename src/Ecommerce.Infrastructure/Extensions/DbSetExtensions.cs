using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.Infrastructure.Extensions
{
    public static class DbSetExtensions
    {
        public static Task<TEntity?> FirstOrDefaultIgnoreCaseAsync<TEntity>(
            this IQueryable<TEntity> source,
            Expression<Func<TEntity, string>> propertySelector,
            string value) where TEntity : class
        {
            return source.FirstOrDefaultAsync(e => EF.Functions.Collate(propertySelector.Compile()(e), "SQL_Latin1_General_CP1_CI_AS") == value);
        }
    }
}