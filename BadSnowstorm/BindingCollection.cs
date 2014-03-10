using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BadSnowstorm
{
    public class BindingCollection<TView, TViewModel> : IEnumerable<Binding<TView, TViewModel>>
        where TView : IViewBase
        where TViewModel : ViewModel
    {
        private readonly List<Binding<TView, TViewModel>> _bindings = new List<Binding<TView, TViewModel>>();

        public void Add(
            Expression<Func<TView, IContentArea>> contentAreaExpression,
            Expression<Func<TViewModel, string>> viewModelPropertyExpression)
        {
            CheckExpression(contentAreaExpression);
            CheckExpression(viewModelPropertyExpression);
            _bindings.Add(Binding<TView, TViewModel>.Create(contentAreaExpression, viewModelPropertyExpression));
        }

        public void Add(
            Expression<Func<TView, IContentArea>> contentAreaExpression,
            Expression<Func<TViewModel, Input>> viewModelPropertyExpression)
        {
            CheckExpression(contentAreaExpression);
            CheckExpression(viewModelPropertyExpression);
            _bindings.Add(Binding<TView, TViewModel>.Create(contentAreaExpression, viewModelPropertyExpression));
        }

        public void Add(
            Expression<Func<TView, IContentArea>> contentAreaExpression,
            Expression<Func<TViewModel, object>> viewModelPropertyExpression,
            Func<object, string> viewModelPropertyValueFormatter)
        {
            CheckExpression(contentAreaExpression);
            CheckExpression(viewModelPropertyExpression);
            _bindings.Add(Binding<TView, TViewModel>.Create(contentAreaExpression, viewModelPropertyExpression, viewModelPropertyValueFormatter));
        }

        internal void Add(Binding<TView, TViewModel> binding)
        {
            _bindings.Add(binding);
        }

        private static void CheckExpression<T, TMemberType>(Expression<Func<T, TMemberType>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            // check to see if the expression includes a boxing operation, since the expression's Func may have a return type of object.
            if (memberExpression == null)
            {
                var unaryBody = expression.Body as UnaryExpression;
                if (unaryBody == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            "Expression '{0}' does not represent a property or field: Expression.Body is neither a MemberExpression nor a UnaryExpression.",
                            expression));
                }

                memberExpression = unaryBody.Operand as MemberExpression;
                if (memberExpression == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            "Expression '{0}' does not represent a property or field: ((UnaryExpression)Expression.Body).Operand is not a MemberExpression.",
                            expression));
                }
            }

            var type = typeof(T);
            var rootExpression = GetRootExpression(memberExpression);

            if (!rootExpression.Member.ReflectedType.IsAssignableFrom(type))
            {
                throw new ArgumentException(
                    string.Format(
                        "Expression '{0}' refers to a property or field that is not from type: {1}.",
                        expression,
                        type));
            }
        }

        private static MemberExpression GetRootExpression(MemberExpression memberExpression)
        {
            var parentMemberExpression = memberExpression.Expression as MemberExpression;
            return parentMemberExpression != null ? GetRootExpression(parentMemberExpression) : memberExpression;
        }

        public IEnumerator<Binding<TView, TViewModel>> GetEnumerator()
        {
            return _bindings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class BindingCollectionExtensions
    {
        public static void Add<TParentView, TPartialView, TParentViewModel, TPartialViewModel>(this BindingCollection<TParentView, TParentViewModel> bindings, PartialView<TParentView, TPartialView, TParentViewModel, TPartialViewModel> partialView)
            where TParentView : View<TParentView, TParentViewModel>, IView
            where TPartialView : PartialView<TParentView, TPartialView, TParentViewModel, TPartialViewModel>
            where TParentViewModel : ViewModel
            where TPartialViewModel : ViewModel
        {
            foreach (var binding in partialView.GetBindings())
            {
                bindings.Add(binding);
            }
        }
    }
}