using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SwiftCode.BBS.IService.BASE
{
    public interface IBaseServices<TEntity> where TEntity : class
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entitys">实体集合</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task InsertManyAsync(IEnumerable<TEntity> entitys, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entitys">实体集合</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task UpdateManyAsync(IEnumerable<TEntity> entitys, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除一条实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 条件删除实体
        /// </summary>
        /// <param name="prediceat">筛选条件</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicete, bool autoSave = false, CancellationToken cancellationToken = default);


        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entitys">实体集合</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task DeleteManyAsync(IEnumerable<TEntity> entitys, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 条件查询一条数据(不存在抛异常)
        /// </summary>
        /// <param name="prediceat">筛选条件</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> prediceat, CancellationToken cancellationToken = default);

        /// <summary>
        /// 条件查询一条数据(不存在返回null)
        /// </summary>
        /// <param name="prediceat">筛选条件</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> prediceat, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="prediceat">筛选条件</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> prediceat, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="skipCount">跳过多少条</param>
        /// <param name="maxResultCount">获取多少条</param>
        /// <param name="sorting">排序字段</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task<List<TEntity>> GetPageListAsync(int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task<long> GetCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件获取总数
        /// </summary>
        /// <param name="prediceat">筛选条件</param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        Task<long> GetCountAsync(Expression<Func<TEntity, bool>> prediceat, CancellationToken cancellationToken = default);

    }
}
