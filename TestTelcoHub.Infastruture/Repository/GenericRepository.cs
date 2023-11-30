
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Model.Data;

namespace TestTelcoHub.Infastruture.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApiDbContext _context;
        public GenericRepository(ApiDbContext context)
        {
            _context = context;
        }
        public async Task Add(T entity)
        {
           await _context.AddAsync(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                var result = await _context.Set<T>().ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAll method: {ex.Message}");
                throw;
            }
        }
        public async Task<T> GetById(Guid id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync();
            return entity!;

        }
        public void Update(T entity)
        { 
            _context.Set<T>().Update(entity);
        }
        public async Task<T> GetByGuildId(Guid id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync();
            return entity!;
        }
        public async Task<T> GetByStringId(string id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync();
            return entity!;
        }
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var result = await query.FirstOrDefaultAsync();
            if (result == null)
            {
                // Xử lý trường hợp giá trị null tại đây, có thể throw exception hoặc thực hiện hành động khác
            }

            return result!;
        }
        //public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null)
        //{
        //    IQueryable<T> query = _context.Set<T>();

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }
        //    return await query.FirstOrDefaultAsync()!;
        //}
    }
}
