using Likvido.CreditRisk.DataAccess.Extensions;
using Likvido.CreditRisk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Likvido.CreditRisk.DataAccess.Builders
{
    public class SearchPredicateBuilder
    {
        public Expression<Func<TSearchable, bool>> SearchFilter<TSearchable>(string searchQuery) where TSearchable : ISearchable
        {
            Type entityType = typeof(TSearchable);
            Expression<Func<TSearchable, bool>> predicate = null;

            PropertyInfo[] searchProperties =
                entityType.GetProperties()
                          .Where(
                              pinfo =>
                              pinfo.HasAttribute<SearchPropertyAttribute>()
                              || pinfo.HasAttribute<NavigationSearchPropertyAttribute>())
                          .ToArray();

            if (searchProperties == null || searchProperties.Length == 0)
            {
                throw new InvalidOperationException(
                    "Searchable entity must have property marked with [SearchProperty] attribute");
            }

            ParameterExpression entityParameter = Expression.Parameter(entityType, "type");

            foreach (PropertyInfo searchProperty in searchProperties)
            {
                Expression<Func<TSearchable, bool>> lambda;
                if (searchProperty.HasAttribute<NavigationSearchPropertyAttribute>())
                {
                    lambda = CreateLambdaExpressionForVirtual<TSearchable>(searchQuery, entityParameter, searchProperty);
                }
                else
                {
                    lambda = CreateLambdaExpressionForProperty<TSearchable>(
                        searchQuery,
                        entityParameter,
                        searchProperty);
                }


                predicate = predicate == null
                                ? lambda
                                : Compose(predicate, lambda, Expression.Or);
            }

            return predicate;
        }

        public static Expression<T> Compose<T>(
            Expression<T> first,
            Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            Dictionary<ParameterExpression, ParameterExpression> map =
                first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            Expression secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        private static Expression<Func<TSearchable, bool>> CreateLambdaExpressionForProperty<TSearchable>(
            string searchQuery,
            ParameterExpression entityParameter,
            PropertyInfo searchProperty)
            where TSearchable : ISearchable
        {
            MemberExpression memberExpression = Expression.MakeMemberAccess(entityParameter, searchProperty);            

            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            MethodCallExpression callExpression =
                Expression.Call(memberExpression, method, Expression.Constant(searchQuery, typeof(string)));

            Expression<Func<TSearchable, bool>> lambda =
                Expression.Lambda<Func<TSearchable, bool>>(
                    callExpression,
                    entityParameter);

            return lambda;
        }

        private static Expression<Func<TSearchable, bool>> CreateLambdaExpressionForVirtual<TSearchable>(
            string searchQuery,
            ParameterExpression entityParameter,
            PropertyInfo navigationSearchProperty)
            where TSearchable : ISearchable
        {
            Expression<Func<TSearchable, bool>> result;
            if (!navigationSearchProperty.HasAttribute<NavigationSearchPropertyAttribute>())
            {
                return null;
            }

            var navigationSearchAttribute =
                navigationSearchProperty.GetCustomAttribute<NavigationSearchPropertyAttribute>();
            Type firstPropertyType = navigationSearchProperty.PropertyType;

            var rootMemberExpression = Expression.Property(
                entityParameter,
                entityParameter.Type.GetProperty(navigationSearchProperty.Name));

            var lambdas = new List<Expression<Func<TSearchable, bool>>>();

            foreach (string name in navigationSearchAttribute.Names)
            {
                MemberExpression memberExpression = Expression.Property(
                    rootMemberExpression,
                    firstPropertyType.GetProperty(name));

                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                MethodCallExpression callExpression =
                    Expression.Call(memberExpression, method, Expression.Constant(searchQuery, typeof(string)));

                Expression<Func<TSearchable, bool>> lambda =
                    Expression.Lambda<Func<TSearchable, bool>>(
                        callExpression,
                        entityParameter);

                lambdas.Add(lambda);
            }

            result = lambdas[0];
            foreach (var x in lambdas.Skip(1))
            {
                result = Compose(result, x, Expression.Or);                
            }

            return result;
        }
    }

    class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(
            Dictionary<ParameterExpression, ParameterExpression> map,
            Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            ParameterExpression replacement;

            if (this.map.TryGetValue(node, out replacement))
            {
                node = replacement;
            }

            return base.VisitParameter(node);
        }
    }
}
