using Microsoft.EntityFrameworkCore;
using Swift.BBS.IRepositories.Base;
using Swift.BBS.Repositories.EfContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Swift.BBS.Repositories.BASE
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private SwiftBbsContext _context;
        public BaseRepository()
        {
            _context = new SwiftBbsContext();
        }

        /// <summary>
        /// 暴漏 DbContext 提供给自动逸仓储进行使用
        /// </summary>
        /// <returns></returns>
        protected SwiftBbsContext DbContext()
        {
            return _context;
        }



        /// <summary>
        /// 功能描述：添加实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（当CancellationToken 是取消状态，Task内部未启动的任务不会启动新线程）</param>
        /// <returns></returns>
        public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var savedEntity = (await _context.Set<TEntity>().AddAsync(entity, cancellationToken)).Entity;
            if (autoSave)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            return savedEntity;
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entities">实体类集合</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（当CancellationToken 是取消状态，Task内部未启动的任务不会启动新线程）</param>
        /// <returns></returns>
        public async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entityArray = entities.ToArray();
            await _context.Set<TEntity>().AddRangeAsync(entityArray, cancellationToken);
            if (autoSave)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 更新实体类数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（当CancellationToken 是取消状态，Task内部未启动的任务不会启动新线程）</param>
        /// <returns></returns>
        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            //Attach 是将一个处于 Deleted 的Entity附加到上下文，而附加到上下文后的这一 Entity 的 State 为 UnChanged。
            //传递到 Attach 方法的对象必须具有有效的 EntityKey 值
            _context.Attach(entity);
            var updatedEntity = _context.Update(entity).Entity;
            if (autoSave)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            return updatedEntity;
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体类集合</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（当CancellationToken 是取消状态，Task内部未启动的任务不会启动新线程）</param>
        /// <returns></returns>
        public async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntity>().UpdateRange(entities);
            if (autoSave)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（当CancellationToken 是取消状态，Task内部未启动的任务不会启动新线程）</param>
        /// <returns></returns>
        public async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _context.Set<TEntity>().Remove(entity);
            if (autoSave)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 根据筛选条件，删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（当CancellationToken 是取消状态，Task内部未启动的任务不会启动新线程）</param>
        /// <returns></returns>
        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var dbSet = _context.Set<TEntity>();
            var entities = await dbSet.Where(predicate).ToListAsync(cancellationToken);
            await DeleteManyAsync(entities, autoSave, cancellationToken);
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities">实体类集合</param>
        /// <param name="autoSave">是否马上更新到数据库</param>
        /// <param name="cancellationToken">取消令牌（当CancellationToken 是取消状态，Task内部未启动的任务不会启动新线程）</param>
        /// <returns></returns>
        public async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            _context.RemoveRange(entities);
            if (autoSave)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 根据删选条件，获取一条数据，不存在则返回null
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().Where(predicate).SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// 根据筛选条件获取一条数据，如果不存在，则抛出异常
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(predicate, cancellationToken);
            //数据不存在则触发异常
            if (entity == null)
            {
                throw new Exception(nameof(TEntity) + ": 数据不存在");
            }
            return entity;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 根据筛选条件获取数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="skipCount">跳过多少条</param>
        /// <param name="maxResultCount">互殴多少条</param>
        /// <param name="sorting">排序字段</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default)
        {
            // nuget Ststem.Linq.Dynamic.Core
            return await _context.Set<TEntity>().OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 获取一共有多少条数据
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().LongCountAsync(cancellationToken);
        }

        /// <summary>
        /// 根据条件筛选出数据条数
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().Where(predicate).LongCountAsync(cancellationToken);
        }


    }
}
