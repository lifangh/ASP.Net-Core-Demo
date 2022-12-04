
using SwiftCode.BBS.IService.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using SwiftCode.BBS.EntityFramework.EFContext;
using Microsoft.EntityFrameworkCore;

namespace SwiftCode.BBS.Repositories.BASE
{
    public class BaseRepository<TEntitiy> : IBaseRepository<TEntitiy> where TEntitiy : class, new()
    {
        private readonly SwiftCodeBbsContext _context;
        public BaseRepository()
        {
            _context = new SwiftCodeBbsContext();
        }
        public BaseRepository(SwiftCodeBbsContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 暴露 DBContext 给自定义仓储使用
        /// </summary>
        /// <returns></returns>
        protected SwiftCodeBbsContext DbContext()
        {
            return _context;
        }

        /// <summary>
        /// 删除一条
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteAsync(TEntitiy entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntitiy>().Remove(entity);

            if(autoSave)
                await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// 条件删除一条
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteAsync(Expression<Func<TEntitiy, bool>> predicete, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var dbSet = _context.Set<TEntitiy>();

            var entities = await dbSet.Where(predicete).ToListAsync(cancellationToken);

            if (autoSave)
                await _context.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entitys">实体集合</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteManyAsync(IEnumerable<TEntitiy> entitys, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _context.RemoveRange(entitys);

            if(autoSave)
                await _context.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// 获取一条不存在抛异常
        /// </summary>
        /// <param name="prediceat">过滤条件</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TEntitiy> FindAsync(Expression<Func<TEntitiy, bool>> prediceat, CancellationToken cancellationToken = default)
        {
            var entitiey = await FindAsync(prediceat, cancellationToken);

            if (entitiey == null)
                throw new NotImplementedException($"{nameof(TEntitiy)}:数据不存在");

            return entitiey;

        }
        /// <summary>
        /// 获取一条不存在返回null
        /// </summary>
        /// <param name="prediceat"></param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        public  Task<TEntitiy> GetAsync(Expression<Func<TEntitiy, bool>> prediceat, CancellationToken cancellationToken = default)
        {
            return  _context.Set<TEntitiy>().Where(prediceat).SingleOrDefaultAsync(cancellationToken);

        }
        /// <summary>
        /// 获取一共多少条
        /// </summary>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        public Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return _context.Set<TEntitiy>().LongCountAsync(cancellationToken);
        }

        /// <summary>
        /// 根据条件获取数据条数
        /// </summary>
        /// <param name="prediceat">过滤条件</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<long> GetCountAsync(Expression<Func<TEntitiy, bool>> prediceat, CancellationToken cancellationToken = default)
        {
            return _context.Set<TEntitiy>().Where(prediceat).LongCountAsync(cancellationToken);
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public  Task<List<TEntitiy>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return _context.Set<TEntitiy>().ToListAsync(cancellationToken);
        }
        /// <summary>
        /// 按条件获取数据
        /// </summary>
        /// <param name="prediceat">过滤条件</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<TEntitiy>> GetListAsync(Expression<Func<TEntitiy, bool>> prediceat, CancellationToken cancellationToken = default)
        {
            return _context.Set<TEntitiy>().Where(prediceat).ToListAsync(cancellationToken);
        }
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="skipCount">跳过多少条</param>
        /// <param name="maxResultCount">获取多少条</param>
        /// <param name="sorting">排序字段</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<TEntitiy>> GetPageListAsync(int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default)
        {
            return _context.Set<TEntitiy>().OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync(cancellationToken);
        }
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        public async Task<TEntitiy> InsertAsync(TEntitiy entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var saveEntity = (await _context.Set<TEntitiy>().AddAsync(entity, cancellationToken)).Entity;

            if (autoSave)
                await _context.SaveChangesAsync(cancellationToken);

            return saveEntity;
        }
        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entitys">实体</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        public async Task InsertManyAsync(IEnumerable<TEntitiy> entitys, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entityArray = entitys.ToArray();

            await _context.Set<TEntitiy>().AddRangeAsync(entityArray, cancellationToken);

            if (autoSave)
                await _context.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        public async Task<TEntitiy> UpdateAsync(TEntitiy entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _context.Attach(entity);

            var updateEntity = _context.Update(entity).Entity;

            if(autoSave)
                await _context.SaveChangesAsync(cancellationToken);

            return updateEntity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateManyAsync(IEnumerable<TEntitiy> entitys, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntitiy>().UpdateRange(entitys);

            if (autoSave)
                await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
