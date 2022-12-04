using SwiftCode.BBS.IService.BASE;
using SwiftCode.BBS.Models;
using SwiftCode.BBS.Repositories.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SwiftCode.BBS.Service.BASE
{
    public class BaseServices<TEntitiy> : IBaseServices<TEntitiy> where TEntitiy : class, new ()
    {

        //public IBaseRepository<TEntitiy> _baseRepository = new BaseRepository<TEntitiy>();
        public IBaseRepository<TEntitiy> _baseRepository;

        public BaseServices(IBaseRepository<TEntitiy> baseRepository)
        {
            _baseRepository = baseRepository;
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
           await _baseRepository.DeleteAsync(entity,autoSave,cancellationToken);

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
            await _baseRepository.DeleteAsync(predicete,autoSave,cancellationToken);
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
            await _baseRepository.DeleteManyAsync(entitys, autoSave, cancellationToken);
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
            return await _baseRepository.FindAsync(prediceat, cancellationToken);

        }
        /// <summary>
        /// 获取一条不存在返回null
        /// </summary>
        /// <param name="prediceat"></param>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        public Task<TEntitiy> GetAsync(Expression<Func<TEntitiy, bool>> prediceat, CancellationToken cancellationToken = default)
        {
            return  _baseRepository.GetAsync(prediceat, cancellationToken);

        }
        /// <summary>
        /// 获取一共多少条
        /// </summary>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        public Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return _baseRepository.GetCountAsync(cancellationToken);
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
            return _baseRepository.GetCountAsync(prediceat, cancellationToken);
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="cancellationToken">取消令牌（cancellationToken 为取消状态，Task内部未启动的任务不会启动新的线程）</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<TEntitiy>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return _baseRepository.GetListAsync(cancellationToken);
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
            return _baseRepository.GetListAsync(prediceat, cancellationToken);
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
            return _baseRepository.GetPageListAsync(skipCount, maxResultCount, sorting,cancellationToken);
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
            return await _baseRepository.InsertAsync(entity, autoSave, cancellationToken);
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
             await _baseRepository.InsertManyAsync(entitys, autoSave, cancellationToken);
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
            return await _baseRepository.UpdateAsync(entity, autoSave, cancellationToken);
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
             await _baseRepository.UpdateManyAsync(entitys, autoSave, cancellationToken);
        }
    }
}
