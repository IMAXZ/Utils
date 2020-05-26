using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.EntityFramework
{
    public interface IEFRepository<out TDbContext, TEntity>  where TDbContext : DbContext where TEntity : class
    {
        TDbContext DbContext { get; }

        /// <summary>
        /// 根据主键查询实体
        /// </summary>
        /// <param name="keyValues">keyValues</param>
        /// <returns>the entity founded, if not found, null returned</returns>
        TEntity Find(params object[] keyValues);

        /// <summary>
        /// 根据主键查询实体
        /// </summary>
        /// <param name="keyValues">keyValues</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>the entity founded, if not found, null returned</returns>
        ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken);

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="keyValues">keyValues</param>
        /// <returns>affected rows</returns>
        int Delete(params object[] keyValues);

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <param name="keyValues">entity</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>affected rows</returns>
        Task<int> DeleteAsync(object[] keyValues, CancellationToken cancellationToken);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>affected rows</returns>
        int Delete(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>affected rows</returns>
        Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        int Delete(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新实体指定字段
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="propertyNames">properties to update</param>
        /// <returns>affected rows</returns>
        int Update(TEntity entity, params string[] propertyNames);

        /// <summary>
        /// 更新实体排除指定字段
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="propertyNames">properties not to update</param>
        /// <returns>affected rows</returns>
        int UpdateWithout(TEntity entity, params string[] propertyNames);

        /// <summary>
        /// 更新实体指定字段
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="propertyNames">properties to update</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>affected rows</returns>
        Task<int> UpdateAsync(TEntity entity, string[] propertyNames, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新实体排除指定字段
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="propertyNames">properties not to update</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>affected rows</returns>
        Task<int> UpdateWithoutAsync(TEntity entity, string[] propertyNames, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新实体指定字段
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="propertyExpressions">properties to update</param>
        /// <returns>affected rows</returns>
        int Update(TEntity entity, params Expression<Func<TEntity, object>>[] propertyExpressions);

        /// <summary>
        /// 更新实体排除指定字段
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="propertyExpressions">properties not to update</param>
        /// <returns>affected rows</returns>
        int UpdateWithout(TEntity entity, params Expression<Func<TEntity, object>>[] propertyExpressions);

        /// <summary>
        /// 更新实体指定字段
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="propertyExpressions">properties to update</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>affected rows</returns>
        Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[] propertyExpressions, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新实体排除指定字段
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="propertyExpressions">properties not to update</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>affected rows</returns>
        Task<int> UpdateWithoutAsync(TEntity entity, Expression<Func<TEntity, object>>[] propertyExpressions, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新实体通过表达式指定字段
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="whereExpression"></param>
        /// <param name="propertyExpression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        int Update<TProperty>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProperty>> propertyExpression, object value);

        /// <summary>
        /// 更新实体通过字典指定字段
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="propertyValues"></param>
        /// <returns></returns>
        int Update(Expression<Func<TEntity, bool>> whereExpression, IDictionary<string, object> propertyValues);

        /// <summary>
        /// 更新实体通过表达式指定字段
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="whereExpression"></param>
        /// <param name="propertyExpression"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync<TProperty>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProperty>> propertyExpression, object value, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新实体通过字典指定字段
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="propertyValues"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> whereExpression, IDictionary<string, object> propertyValues, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the <see cref="IQueryable{TEntity}"/> based on a predicate => WithPredict
        /// </summary>
        /// <param name="queryBuilderAction">queryBuilder</param>
        /// <remarks>This method default no-tracking query.</remarks>
        IQueryable<TEntity> Query(Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null);

        /// <summary>
        /// Gets the <see cref="List{TEntity}"/> based on a predicate => WithPredict
        /// </summary>
        /// <param name="queryBuilderAction">queryBuilderAction</param>
        /// <remarks>This method default no-tracking query.</remarks>
        List<TEntity> Get(Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null);

        /// <summary>
        /// Gets the <see cref="List{TEntity}"/> based on a predicate => WithPredict
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="queryBuilderAction">queryBuilderAction</param>
        /// <remarks>This method default no-tracking query.</remarks>
        List<TResult> GetResult<TResult>(Expression<Func<TEntity, TResult>> selector, Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null);

        /// <summary>
        /// Gets the <see cref="List{TEntity}"/> based on a predicate => WithPredict
        /// </summary>
        /// <param name="queryBuilderAction">queryBuilder</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<List<TEntity>> GetAsync(Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the <see cref="List{TResult}"/> based on a predicate => WithPredict
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="queryBuilderAction">queryBuilder</param>
        /// <param name="cancellationToken"></param>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<List<TResult>> GetResultAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        List<TEntity> Select(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<List<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件查询前几条有排序
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="count"></param>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        List<TEntity> Select<TProperty>(int count, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProperty>> orderByExpression, bool ascending = false);

        /// <summary>
        /// 根据条件查询前几条有排序
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="count"></param>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="ascending"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> SelectAsync<TProperty>(int count, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProperty>> orderByExpression, bool ascending = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the <see cref="List{TEntity}"/> based on a predicate => WithPredict
        /// </summary>
        /// <param name="queryBuilderAction">queryBuilderAction</param>
        /// <remarks>This method default no-tracking query.</remarks>
        bool Any(Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null);

        /// <summary>
        /// Gets the <see cref="List{TEntity}"/> based on a predicate => WithPredict
        /// </summary>
        /// <param name="queryBuilderAction">queryBuilder</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<bool> AnyAsync(Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the <see cref="List{TEntity}"/> based on a predicate => WithPredict
        /// </summary>
        /// <param name="queryBuilderAction">queryBuilderAction</param>
        /// <remarks>This method default no-tracking query.</remarks>
        TEntity FirstOrDefault(Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null);

        /// <summary>
        /// Gets the <see cref="List{TEntity}"/> based on a predicate => WithPredict
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="queryBuilderAction">queryBuilderAction</param>
        /// <remarks>This method default no-tracking query.</remarks>
        TResult FirstOrDefaultResult<TResult>(Expression<Func<TEntity, TResult>> selector, Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null);

        /// <summary>
        /// Gets the <see cref="List{TEntity}"/> based on a predicate => WithPredict
        /// </summary>
        /// <param name="queryBuilderAction">queryBuilder</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<TEntity> FirstOrDefaultAsync(Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the <see cref="List{TResult}"/> based on a predicate => WithPredict
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="queryBuilderAction">queryBuilder</param>
        /// <param name="cancellationToken"></param>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<TResult> FirstOrDefaultResultAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the <see cref="IPagedListModel{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="queryBuilderAction">queryBuilderAction</param>
        /// <param name="pageNumber">The pageNumber of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>An <see cref="IPagedListModel{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="queryBuilderAction"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        IPagedListModel<TEntity> GetPagedList(Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null, int pageNumber = 1, int pageSize = 20);

        /// <summary>
        /// Gets the <see cref="IPagedListModel{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="queryBuilderAction">A function to test each element for a condition.</param>
        /// <param name="pageNumber">The number of page.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An <see cref="IPagedListModel{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="queryBuilderAction"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<IPagedListModel<TEntity>> GetPagedListAsync(Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the <see cref="IPagedListModel{TResult}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="queryBuilderAction">A function to test each element for a condition.</param>
        /// <param name="pageNumber">pageNumber</param>
        /// <param name="pageSize">pageSize</param>
        /// <returns>An <see cref="IPagedListModel{TResult}"/> that contains elements that satisfy the condition specified by <paramref name="queryBuilderAction"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        IPagedListModel<TResult> GetPagedListResult<TResult>(Expression<Func<TEntity, TResult>> selector, Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null,
                                                  int pageNumber = 1, int pageSize = 20);

        /// <summary>
        /// Gets the <see cref="IPagedListModel{TEntity}"/> based on a predicate, orderby delegate and page information. This method default no-tracking query.
        /// </summary>
        /// <param name="selector">The selector for projection.</param>
        /// <param name="queryBuilderAction">A function to test each element for a condition.</param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <param name="pageNumber"></param>
        /// <returns>An <see cref="IPagedListModel{TEntity}"/> that contains elements that satisfy the condition specified by <paramref name="queryBuilderAction"/>.</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        Task<IPagedListModel<TResult>> GetPagedListResultAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Action<EFRepositoryQueryBuilder<TEntity>> queryBuilderAction = null,
                                                             int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据页码、条数、查询条件、分页条件获取分页数据
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        IPagedListModel<TEntity> Paged<TProperty>(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProperty>> orderByExpression, bool ascending = false);

        /// <summary>
        /// 根据页码、条数、查询条件、分页条件获取分页数据
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="ascending"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IPagedListModel<TEntity>> PagedAsync<TProperty>(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProperty>> orderByExpression, bool ascending = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件统计条数
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 根据条件统计条数
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件统计条数
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        long LongCount(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///  根据条件统计条数
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件判断是否存在
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        bool Exist(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 根据条件判断是否存在
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件查询实体
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        TEntity Fetch(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 根据条件、排序查询第一条实体
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        TEntity Fetch<TProperty>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProperty>> orderByExpression, bool ascending = false);

        /// <summary>
        /// 根据条件查询实体
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> FetchAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件、排序查询第一条实体
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="ascending"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> FetchAsync<TProperty>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TProperty>> orderByExpression, bool ascending = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 插入实体集合
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        int Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(TEntity entity);

        /// <summary>
        /// 插入实体集合
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
