using Likvido.CreditRisk.DataAccess.Builders;
using Likvido.CreditRisk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Likvido.CreditRisk.DataAccess.Extensions
{
    public static class DynamicQueryExtensions
    {
        public static IQueryable<TSearchable> Search<TSearchable>(
            this IQueryable<TSearchable> searchables,
            string query)
            where TSearchable : class, ISearchable
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                return searchables
                           .Where(new SearchPredicateBuilder().SearchFilter<TSearchable>(query))
                           .OrderBy(searchable => query);
            };

            return searchables;
        }

        public static IQueryable<TSource> WhereOr<TSource>(this IQueryable<TSource> source, IEnumerable<string> cles, Func<string, Expression<Func<TSource, bool>>> predicat)       
        {
            Expression<Func<TSource, bool>> clause = x => false;
            foreach (var p in cles ?? Enumerable.Empty<string>())
            {
                clause = ParameterRebinder.Or(clause, predicat(p));
            }

            return cles != null && cles.Any() ? source.Where(clause) : source;
        }
    }

    class ParameterRebinder : ExpressionVisitor
    {
        private ParameterExpression _Parametre;
        private ParameterRebinder(ParameterExpression cle)
        {
            _Parametre = cle;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _Parametre;
        }

        internal static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>> e1, Expression<Func<T, bool>> e2)
        {
            Expression<Func<T, bool>> expression = null;

            if (e1 == null)
            {
                expression = e2;
            }
            else if (e2 == null)
            {
                expression = e1;
            }
            else
            {
                var visiteur = new ParameterRebinder(e1.Parameters[0]);
                e2 = (Expression<Func<T, bool>>)visiteur.Visit(e2);

                var body = Expression.Or(e1.Body, e2.Body);
                expression = Expression.Lambda<Func<T, bool>>(body, e1.Parameters[0]);
            }

            return expression;
        }
    }
}
