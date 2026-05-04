
namespace Wasla_Backend.Repositories.Interfaces.General
{
    public interface IEntityLoader
    {
        Task LoadReferenceAsync<TEntity, TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty?>> propertyExpression)
            where TEntity : class
            where TProperty : class;
    }
}
