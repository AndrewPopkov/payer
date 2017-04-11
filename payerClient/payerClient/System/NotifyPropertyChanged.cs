using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DimensionTest.viewmodel
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие, вызывается при изменении значений свойств.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(propertyExpression.Name);
            }

            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("The expression is not a member access expression.", propertyExpression.Name);
            }

            var property = memberExpression.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("The member access expression does not access a property.", propertyExpression.Name);
            }

            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Вызывает событие PropertyChanged, уведомляющее всех подписчиков данного события об изменении свойства.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression">В качестве свойства необходимо передавать лямбда выражение, принимающее свойство, которое изменилось.</param>
        /// <example>
        /// RaisePropertyChanged(() => ChangedProperty);
        /// </example>
        public void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (PropertyChanged != null)
            {
                RaisePropertyChanged(ExtractPropertyName(propertyExpression));
            }
        }

        /// <summary>
        /// Вызывает событие PropertyChanged, уведомляющее всех подписчиков данного события об изменении свойства с именем name.
        /// </summary>
        /// <param name="name">Имя именившегося свойства.</param>
        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
