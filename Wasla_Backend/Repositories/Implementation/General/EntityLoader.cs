
namespace Wasla_Backend.Repositories.Implementation.General
{
    public class EntityLoader : IEntityLoader
    {
        private readonly Context _context;
        public EntityLoader(Context context) => _context = context;

        public async Task LoadReferenceAsync<TEntity, TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty?>> propertyExpression)
            where TEntity : class
            where TProperty : class
        {
            await _context.Entry(entity).Reference(propertyExpression).LoadAsync();
        }
    }
}
