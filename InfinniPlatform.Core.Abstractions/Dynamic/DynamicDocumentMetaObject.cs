using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace InfinniPlatform.Dynamic
{
    /// <summary>
    /// Предоставляет методы перехвата обращений к динамическому объекту типа <see cref="DynamicDocument"/>.
    /// </summary>
    /// <remarks>
    /// По факту методы перехвата формируют <see cref="Expression"/>, который представляет собой реальную реакцию динамического объекта на перехваченное обращение.
    /// </remarks>
    public class DynamicDocumentMetaObject : DynamicMetaObject
    {
        private static readonly Type ObjectType = typeof(object);
        private static readonly MethodInfo TryGetMemberMethod;
        private static readonly MethodInfo TrySetMemberMethod;
        private static readonly MethodInfo TryInvokeMember;


        static DynamicDocumentMetaObject()
        {
            // Методы динамического объекта, которые будут вызываться при перехвате динамических обращений
            TryGetMemberMethod = ((MethodCallExpression)(((Expression<Action<DynamicDocument>>)(d => d.TryGetMember(string.Empty))).Body)).Method;
            TrySetMemberMethod = ((MethodCallExpression)(((Expression<Action<DynamicDocument>>)(d => d.TrySetMember(string.Empty, new object()))).Body)).Method;
            TryInvokeMember = ((MethodCallExpression)(((Expression<Action<DynamicDocument>>)(d => d.TryInvokeMember(string.Empty, new object[] { }))).Body)).Method;
        }


        public DynamicDocumentMetaObject(Expression expression, DynamicDocument value)
            : base(expression, BindingRestrictions.Empty, value)
        {
        }


        /// <summary>
        /// Вызываемый динамический объект.
        /// </summary>
        public Expression DynamicValue
        {
            get { return Expression.Convert(Expression, LimitType); }
        }

        /// <summary>
        /// Ограничения для проверки правильности выражения.
        /// </summary>
        public BindingRestrictions TypeRestrictions
        {
            get { return BindingRestrictions.GetTypeRestriction(Expression, LimitType); }
        }


        /// <summary>
        /// Возвращает список динамических членов.
        /// </summary>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var value = (DynamicDocument)Value;

            foreach (KeyValuePair<string, object> property in value)
            {
                yield return property.Key;
            }
        }

        /// <summary>
        /// Перехватчик получения значения члена.
        /// </summary>
        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            // DynamicDocument.TryGetMember(binder.Name)
            var tryGetMemberArgs = new Expression[] { Expression.Constant(binder.Name) };
            var tryGetMemberCall = Expression.Call(DynamicValue, TryGetMemberMethod, tryGetMemberArgs);

            return new DynamicMetaObject(tryGetMemberCall, TypeRestrictions);
        }

        /// <summary>
        /// Перехватчик установки значения члена.
        /// </summary>
        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject memberValue)
        {
            // DynamicDocument.TrySetMember(binder.Name, memberValue)
            var trySetMemberArgs = new Expression[] { Expression.Constant(binder.Name), Expression.Convert(memberValue.Expression, ObjectType) };
            var trySetMemberCall = Expression.Call(DynamicValue, TrySetMemberMethod, trySetMemberArgs);

            return new DynamicMetaObject(trySetMemberCall, TypeRestrictions);
        }

        /// <summary>
        /// Перехватчик вызова члена.
        /// </summary>
        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] invokeArguments)
        {
            // DynamicDocument.TryInvokeMember(binder.Name, invokeArguments) 
            var tryInvokeMemberArgs = new Expression[] { Expression.Constant(binder.Name), Expression.NewArrayInit(ObjectType, invokeArguments.Select(a => Expression.Convert(Expression.Constant(a.Value), ObjectType))) };
            var tryInvokeMemberCall = Expression.Call(DynamicValue, TryInvokeMember, tryInvokeMemberArgs);
            var invokeMember = new DynamicMetaObject(tryInvokeMemberCall, TypeRestrictions);

            return binder.FallbackInvokeMember(this, invokeArguments, invokeMember);
        }
    }
}