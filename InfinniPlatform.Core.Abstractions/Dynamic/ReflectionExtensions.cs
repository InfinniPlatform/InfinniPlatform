using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace InfinniPlatform.Dynamic
{
    /// <summary>
    /// Extensions methods for reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
        private static readonly object[] EmptyObjects = { };

        // COMMON

        /// <summary>
        /// Возвращает значение по умолчанию для заданного типа.
        /// </summary>
        public static object GetDefaultValue(this Type target)
        {
            return target.GetTypeInfo().IsValueType ? Activator.CreateInstance(target) : null;
        }

        /// <summary>
        /// Определяет, является ли объект экземпляром заданного типа.
        /// </summary>
        public static bool IsInstanceOfType(this object target, Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return (target == null && !typeInfo.IsValueType) || typeInfo.IsInstanceOfType(target);
        }

        /// <summary>
        /// Возвращает имя типа.
        /// </summary>
        public static string NameOf(this Type target)
        {
            var lastIndexOfBacktick = target.Name.LastIndexOf('`');

            return (lastIndexOfBacktick == -1) ? target.Name : target.Name.Substring(0, lastIndexOfBacktick);
        }

        // ATTRIBUTE

        /// <summary>
        /// Возвращает значение атрибута.
        /// </summary>
        /// <typeparam name="TAttribute">Тип атрибута.</typeparam>
        /// <typeparam name="TResult">Тип значения.</typeparam>
        /// <param name="target">Источник для поиска атрибута.</param>
        /// <param name="valueSelector">Метод выборки значения атрибута.</param>
        /// <param name="defaultValue">Значение атрибута по умолчанию.</param>
        /// <returns>Значение атрибута.</returns>
        public static TResult GetAttributeValue<TAttribute, TResult>(this ICustomAttributeProvider target, Func<TAttribute, TResult> valueSelector, TResult defaultValue = default(TResult)) where TAttribute : Attribute
        {
            var attributes = target.GetCustomAttributes(typeof(TAttribute), false);

            if (attributes.Length > 0)
            {
                return valueSelector((TAttribute)attributes[0]);
            }

            return defaultValue;
        }

        // MEMBER

        /// <summary>
        /// Осуществляет поиск члена с заданным именем.
        /// </summary>
        public static MemberInfo FindMember(this Type target, string memberName, Func<MemberInfo, bool> memberFilter = null, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            MemberInfo result = null;

            if (!string.IsNullOrEmpty(memberName))
            {
                var members = ReflectionCache.FindMembers(target, memberName, bindingFlags);

                if (members != null)
                {
                    result = (memberFilter == null)
                        ? members.FirstOrDefault()
                        : members.FirstOrDefault(memberFilter);
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает значение статического члена с заданным именем.
        /// </summary>
        public static object GetMemberValue(this Type target, string memberName)
        {
            if (target != null)
            {
                return GetMemberValue(target, null, memberName);
            }

            return null;
        }

        /// <summary>
        /// Возвращает значение члена с заданным именем.
        /// </summary>
        public static object GetMemberValue(this object target, string memberName)
        {
            if (target != null)
            {
                return GetMemberValue(target.GetType(), target, memberName);
            }

            return null;
        }

        private static object GetMemberValue(Type type, object instance, string memberName)
        {
            var memberInfo = FindMember(type, memberName);

            if (memberInfo != null)
            {
                object memberValue = null;

                switch (memberInfo.MemberType)
                {
                    case MemberTypes.Field:
                        memberValue = GetFieldValue(instance, (FieldInfo)memberInfo);
                        break;
                    case MemberTypes.Property:
                        memberValue = GetPropertyValue(instance, (PropertyInfo)memberInfo);
                        break;
                    case MemberTypes.Method:
                        memberValue = GetMethodDelegate(instance, (MethodInfo)memberInfo);
                        break;
                    case MemberTypes.Event:
                        memberValue = GetEventDelegate(instance, (EventInfo)memberInfo);
                        break;
                }

                return memberValue;
            }

            return null;
        }

        /// <summary>
        /// Устанавливает значение статического члена с заданным именем.
        /// </summary>
        public static bool SetMemberValue(this Type target, string memberName, object memberValue)
        {
            if (target != null)
            {
                return SetMemberValue(target, null, memberName, memberValue);
            }

            return false;
        }

        /// <summary>
        /// Устанавливает значение члена с заданным именем.
        /// </summary>
        public static bool SetMemberValue(this object target, string memberName, object memberValue)
        {
            if (target != null)
            {
                return SetMemberValue(target.GetType(), target, memberName, memberValue);
            }

            return false;
        }

        private static bool SetMemberValue(Type type, object instance, string memberName, object memberValue)
        {
            var memberInfo = FindMember(type, memberName);

            if (memberInfo != null)
            {
                var setMember = false;

                switch (memberInfo.MemberType)
                {
                    case MemberTypes.Field:
                        setMember = SetFieldValue(instance, (FieldInfo)memberInfo, memberValue);
                        break;
                    case MemberTypes.Property:
                        setMember = SetPropertyValue(instance, (PropertyInfo)memberInfo, memberValue);
                        break;
                    case MemberTypes.Event:
                        setMember = SetEventDelegate(instance, (EventInfo)memberInfo, memberValue);
                        break;
                }

                return setMember;
            }

            return false;
        }

        /// <summary>
        /// Интерпретирует значение статического члена с заданным именем как делегат и вызывает его.
        /// </summary>
        public static bool InvokeMember(this Type target, string memberName, object[] invokeArguments, out object invokeResult, Type[] genericParameters = null)
        {
            if (target != null)
            {
                return InvokeMember(target, null, memberName, invokeArguments, out invokeResult, genericParameters);
            }

            invokeResult = null;
            return false;
        }

        /// <summary>
        /// Интерпретирует значение члена с заданным именем как делегат и вызывает его.
        /// </summary>
        public static bool InvokeMember(this object target, string memberName, object[] invokeArguments, out object invokeResult, Type[] genericParameters = null)
        {
            if (target != null)
            {
                return InvokeMember(target.GetType(), target, memberName, invokeArguments, out invokeResult, genericParameters);
            }

            invokeResult = null;
            return false;
        }

        private static bool InvokeMember(Type type, object instance, string memberName, object[] invokeArguments, out object invokeResult, Type[] genericParameters = null)
        {
            invokeResult = null;

            var memberInvokeArguments = invokeArguments;

            Func<MemberInfo, bool> memberFilter
                = m => (m.MemberType == MemberTypes.Field)
                       || (m.MemberType == MemberTypes.Property)
                       || (m.MemberType == MemberTypes.Event)
                       || (m.MemberType == MemberTypes.Method
                           && CanInvoke((MethodInfo)m, invokeArguments, out memberInvokeArguments, genericParameters));

            var memberInfo = FindMember(type, memberName, memberFilter);

            if (memberInfo != null)
            {
                Delegate memberDelegate = null;

                switch (memberInfo.MemberType)
                {
                    case MemberTypes.Field:
                        memberDelegate = GetFieldValue(instance, (FieldInfo)memberInfo) as Delegate;
                        break;
                    case MemberTypes.Property:
                        memberDelegate = GetPropertyValue(instance, (PropertyInfo)memberInfo) as Delegate;
                        break;
                    case MemberTypes.Event:
                        memberDelegate = GetEventDelegate(instance, (EventInfo)memberInfo);
                        break;
                    case MemberTypes.Method:
                        var methodInfo = (MethodInfo)memberInfo;

                        if (methodInfo.IsGenericMethodDefinition)
                        {
                            // ReSharper disable AssignNullToNotNullAttribute
                            methodInfo = methodInfo.MakeGenericMethod(genericParameters);
                            // ReSharper restore AssignNullToNotNullAttribute
                        }

                        invokeResult = methodInfo.Invoke(instance, memberInvokeArguments);

                        return true;
                }

                if (memberDelegate != null)
                {
                    invokeResult = memberDelegate.FastDynamicInvoke(memberInvokeArguments);
                    return true;
                }
            }

            return false;
        }

        // FIELD

        /// <summary>
        /// Возвращает значение поля с заданным именем.
        /// </summary>
        public static object GetFieldValue(this object target, FieldInfo memberInfo)
        {
            return memberInfo.GetValue(target);
        }

        /// <summary>
        /// Устанавливает значение поля с заданным именем.
        /// </summary>
        public static bool SetFieldValue(this object target, FieldInfo memberInfo, object memberValue)
        {
            if (memberValue.IsInstanceOfType(memberInfo.FieldType) && !memberInfo.IsLiteral && !memberInfo.IsInitOnly)
            {
                memberInfo.SetValue(target, memberValue);
                return true;
            }

            return false;
        }

        // PROPERTY

        /// <summary>
        /// Возвращает значение свойства с заданным именем.
        /// </summary>
        public static object GetPropertyValue(this object target, PropertyInfo memberInfo)
        {
            return memberInfo.CanRead ? memberInfo.GetValue(target) : null;
        }

        /// <summary>
        /// Устанавливает значение свойства с заданным именем.
        /// </summary>
        public static bool SetPropertyValue(this object target, PropertyInfo memberInfo, object memberValue)
        {
            if (memberValue.IsInstanceOfType(memberInfo.PropertyType) && memberInfo.CanWrite)
            {
                memberInfo.SetValue(target, memberValue);
                return true;
            }

            return false;
        }

        // METHOD

        /// <summary>
        /// Возвращает делегат для вызова метода с заданным именем.
        /// </summary>
        public static Delegate GetMethodDelegate(this object target, MethodInfo memberInfo)
        {
            var parameters = memberInfo.GetParameters().Select(p => p.ParameterType).Concat(new[] { memberInfo.ReturnType }).ToArray();
            return memberInfo.CreateDelegate(Expression.GetDelegateType(parameters), memberInfo.IsStatic ? null : target);
        }

        /// <summary>
        /// Определяет, можно ли вызвать метод с заданными параметрами.
        /// </summary>
        public static bool CanInvoke(this MethodInfo target, object[] arguments, out object[] resultArguments, Type[] genericParameters = null)
        {
            // ReSharper disable PossibleNullReferenceException
            // ReSharper disable AssignNullToNotNullAttribute

            resultArguments = arguments;

            var genericCount = (genericParameters != null) ? genericParameters.Length : 0;

            // Если задан generic-метод
            if (target.IsGenericMethod)
            {
                var genericArguments = target.GetGenericArguments();

                // Если передано неверное количество generic-параметров
                if (genericArguments.Length != genericCount)
                {
                    return false;
                }

                // Если generic-параметры не указаны
                if (target.IsGenericMethodDefinition)
                {
                    target = target.MakeGenericMethod(genericParameters);
                }
                // Если указанные generic-параметры не совпадают с переданными
                else if (genericArguments.Where((p, i) => p != genericParameters[i]).Any())
                {
                    return false;
                }
            }
            // Если задан не generic-метод, но указаны generic-параметры
            else if (genericCount > 0)
            {
                return false;
            }

            // Если определены generic-параметры
            if (genericParameters != null && genericParameters.Length > 0)
            {
                if (target.IsGenericMethod)
                {
                }
                else
                {
                    return false;
                }
            }

            var parameters = target.GetParameters();
            var parameterCount = parameters.Length;
            var argumentCount = (arguments != null) ? arguments.Length : 0;

            // Если метод не имеет параметров
            if (parameterCount <= 0)
            {
                // Он может быть вызван, если список аргументов пуст
                return (argumentCount == 0);
            }

            resultArguments = new object[parameterCount];

            // Если последний параметр может принимать набор значений
            var lastParameter = parameters[parameterCount - 1];
            var lastParameterIsArray = (lastParameter.GetCustomAttribute<ParamArrayAttribute>() != null);

            // Если число параметров метода меньше числу аргументов
            if (parameterCount <= argumentCount)
            {
                var i = 0;

                // Проверка обычных аргументов

                for (; i < parameterCount - (lastParameterIsArray ? 1 : 0); ++i)
                {
                    var p = parameters[i];
                    var a = arguments[i];

                    // Типы параметра и аргумента не совпадают
                    if (!IsInstanceOfType(a, p.ParameterType))
                    {
                        return false;
                    }

                    resultArguments[i] = a;
                }

                // Проверка вариативных аргументов

                if (lastParameterIsArray)
                {
                    var lastParameterType = lastParameter.ParameterType.GetElementType();
                    var lastArgumentSize = argumentCount - parameterCount + 1;

                    // Если вариативный аргумент один и его тип совпадает с типом аргументов
                    if (lastArgumentSize == 1 && IsInstanceOfType(arguments[i], lastParameter.ParameterType))
                    {
                        resultArguments[parameterCount - 1] = arguments[i];
                    }
                    else
                    {
                        var lastArgument = Array.CreateInstance(lastParameterType, lastArgumentSize);

                        for (var j = 0; i < argumentCount; ++i, ++j)
                        {
                            var a = arguments[i];

                            // Типы параметра и аргумента не совпадают
                            if (!IsInstanceOfType(a, lastParameterType))
                            {
                                return false;
                            }

                            lastArgument.SetValue(a, j);
                        }

                        resultArguments[parameterCount - 1] = lastArgument;
                    }
                }

                return (i == argumentCount);
            }
            // Если число параметров больше числа аргументов
            else
            {
                var i = 0;

                for (; i < argumentCount; ++i)
                {
                    var p = parameters[i];
                    var a = arguments[i];

                    // Типы параметра и аргумента не совпадают
                    if (!IsInstanceOfType(a, p.ParameterType))
                    {
                        return false;
                    }

                    resultArguments[i] = a;
                }

                for (; i < parameterCount - (lastParameterIsArray ? 1 : 0); ++i)
                {
                    var p = parameters[i];

                    // Если параметр имеет значение по умолчанию
                    if (!p.IsOptional || !p.HasDefaultValue)
                    {
                        return false;
                    }

                    resultArguments[i] = p.DefaultValue;
                }

                if (lastParameterIsArray)
                {
                    resultArguments[i] = null;
                }

                return true;
            }

            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore PossibleNullReferenceException
        }

        // EVENT

        /// <summary>
        /// Возвращает делегат для вызова обработчика события с заданным именем.
        /// </summary>
        public static Delegate GetEventDelegate(this object target, EventInfo memberInfo)
        {
            var eventFieldInfo = (FieldInfo)FindMember(target.GetType(), memberInfo.Name, null, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField);

            return (Delegate)eventFieldInfo?.GetValue(target);
        }

        /// <summary>
        /// Устанавливает делегат для вызова обработчика события с заданным именем.
        /// </summary>
        public static bool SetEventDelegate(this object target, EventInfo memberInfo, object memberValue)
        {
            var eventFieldInfo = (FieldInfo)FindMember(target.GetType(), memberInfo.Name, null, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField);

            if (eventFieldInfo != null && memberValue.IsInstanceOfType(eventFieldInfo.FieldType))
            {
                eventFieldInfo.SetValue(target, memberValue);
                return true;
            }

            return false;
        }

        // DELEGATE

        /// <summary>
        /// Представляет метод быстрого вызова делегата.
        /// </summary>
        public static object FastDynamicInvoke(this Delegate target, params object[] args)
        {
            if (target.GetMethodInfo().ReturnType == typeof(void))
            {
                // ReSharper disable CoVariantArrayConversion
                FastDynamicInvokeAction(target, args ?? EmptyObjects);
                // ReSharper restore CoVariantArrayConversion

                return null;
            }

            // ReSharper disable CoVariantArrayConversion
            return FastDynamicInvokeReturn(target, args ?? EmptyObjects);
            // ReSharper restore CoVariantArrayConversion
        }

        private static void FastDynamicInvokeAction(Delegate target, params dynamic[] args)
        {
            dynamic targetAsDynamic = target;

            switch (args.Length)
            {
                default:
                    try
                    {
                        // ReSharper disable CoVariantArrayConversion
                        target.DynamicInvoke(args);
                        // ReSharper restore CoVariantArrayConversion
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }
                    return;

                // Optimization

                case 1:
                    targetAsDynamic(args[0]);
                    return;
                case 2:
                    targetAsDynamic(args[0], args[1]);
                    return;
                case 3:
                    targetAsDynamic(args[0], args[1], args[2]);
                    return;
                case 4:
                    targetAsDynamic(args[0], args[1], args[2], args[3]);
                    return;
                case 5:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4]);
                    return;
                case 6:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5]);
                    return;
                case 7:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                    return;
                case 8:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                    return;
                case 9:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                    return;
                case 10:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                    return;
                case 11:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                    return;
                case 12:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                    return;
                case 13:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                    return;
                case 14:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                    return;
                case 15:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                    return;
                case 16:
                    targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]);
                    return;
            }
        }

        private static object FastDynamicInvokeReturn(Delegate target, params dynamic[] args)
        {
            dynamic targetAsDynamic = target;

            switch (args.Length)
            {
                default:
                    try
                    {
                        // ReSharper disable CoVariantArrayConversion
                        return target.DynamicInvoke(args);
                        // ReSharper restore CoVariantArrayConversion
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }

                // Optimization

                case 1:
                    return targetAsDynamic(args[0]);
                case 2:
                    return targetAsDynamic(args[0], args[1]);
                case 3:
                    return targetAsDynamic(args[0], args[1], args[2]);
                case 4:
                    return targetAsDynamic(args[0], args[1], args[2], args[3]);
                case 5:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4]);
                case 6:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5]);
                case 7:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                case 8:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                case 9:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                case 10:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                case 11:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10]);
                case 12:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11]);
                case 13:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
                case 14:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13]);
                case 15:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14]);
                case 16:
                    return targetAsDynamic(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12], args[13], args[14], args[15]);
            }
        }
    }
}