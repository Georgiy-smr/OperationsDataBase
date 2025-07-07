using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataBaseOperationHelper.RepositoryService;
using Microsoft.Extensions.DependencyInjection;

namespace DataBaseOperationHelper.Extensions
{ public static class FilterBuilder
    {
        public static Expression<Func<T, bool>> BuildContainsOrExpression<T>(this 
                IEnumerable<string> values,
                Expression<Func<T, string>> propertySelector) 
        { 
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));

            var valueList = values.Where(v => !string.IsNullOrEmpty(v)).ToList();
            if (!valueList.Any())
                throw new ArgumentException(nameof(valueList));

            var parameter = propertySelector.Parameters[0];
            var property = propertySelector.Body;

            Expression? combinedExpression = null;

            foreach (var value in valueList)
            {
                var method = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
                var constant = Expression.Constant(value, typeof(string));
                var containsCall = Expression.Call(property, method, constant);

                if (combinedExpression == null)
                    combinedExpression = containsCall;
                else
                    combinedExpression = Expression.OrElse(combinedExpression, containsCall);
            } 
            return Expression.Lambda<Func<T, bool>>(combinedExpression!, parameter);
            }
        }
}
