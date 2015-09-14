using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using InfinniPlatform.Api.Validation.BooleanValidators;
using InfinniPlatform.Api.Validation.ObjectValidators;

namespace InfinniPlatform.Api.Validation.ValidationBuilders
{
    public static class ObjectValidationBuilderExtensions
    {
        // IsNull

        public static ObjectValidationBuilder IsNull(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new NullValidator {Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNull<T>(this ObjectValidationGenericBuilder<T> target,
            object message)
        {
            return target.Predicate(new NullValidator {Message = message});
        }

        public static ObjectValidationBuilder IsNull(this ObjectValidationBuilder target, string property,
            object message)
        {
            return target.Predicate(new NullValidator {Property = property, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNull<T, TProperty>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, TProperty>> property, object message)
        {
            return
                target.Predicate(new NullValidator {Property = Reflection.GetPropertyPath(property), Message = message});
        }

        public static ObjectValidationBuilder IsNotNull(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new NullValidator(), Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotNull<T>(this ObjectValidationGenericBuilder<T> target,
            object message)
        {
            return target.Predicate(new NotValidator {Operator = new NullValidator(), Message = message});
        }

        public static ObjectValidationBuilder IsNotNull(this ObjectValidationBuilder target, string property,
            object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new NullValidator {Property = property},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotNull<T, TProperty>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, TProperty>> property, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new NullValidator {Property = Reflection.GetPropertyPath(property)},
                    Message = message
                });
        }

        // IsDefault

        public static ObjectValidationBuilder IsDefault(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new DefaultValueValidator {Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsDefault<T>(this ObjectValidationGenericBuilder<T> target,
            object message)
        {
            return target.Predicate(new DefaultValueValidator {Message = message});
        }

        public static ObjectValidationBuilder IsDefault(this ObjectValidationBuilder target, string property,
            object message)
        {
            return target.Predicate(new DefaultValueValidator {Property = property, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsDefault<T, TProperty>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, TProperty>> property, object message)
        {
            return
                target.Predicate(new DefaultValueValidator
                {
                    Property = Reflection.GetPropertyPath(property),
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotDefault(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new DefaultValueValidator(), Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotDefault<T>(this ObjectValidationGenericBuilder<T> target,
            object message)
        {
            return target.Predicate(new NotValidator {Operator = new DefaultValueValidator(), Message = message});
        }

        public static ObjectValidationBuilder IsNotDefault(this ObjectValidationBuilder target, string property,
            object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new DefaultValueValidator {Property = property},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotDefault<T, TProperty>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, TProperty>> property, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new DefaultValueValidator {Property = Reflection.GetPropertyPath(property)},
                    Message = message
                });
        }

        // IsEqual

        public static ObjectValidationBuilder IsEqual(this ObjectValidationBuilder target, object value, object message)
        {
            return target.Predicate(new EqualValidator {Value = value, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsEqual<T>(this ObjectValidationGenericBuilder<T> target,
            T value, object message)
        {
            return target.Predicate(new EqualValidator {Value = value, Message = message});
        }

        public static ObjectValidationBuilder IsEqual(this ObjectValidationBuilder target, string property, object value,
            object message)
        {
            return target.Predicate(new EqualValidator {Property = property, Value = value, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsEqual<T, TProperty>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, TProperty>> property, TProperty value,
            object message)
        {
            return
                target.Predicate(new EqualValidator
                {
                    Property = Reflection.GetPropertyPath(property),
                    Value = value,
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotEqual(this ObjectValidationBuilder target, object value,
            object message)
        {
            return target.Predicate(new NotValidator {Operator = new EqualValidator {Value = value}, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotEqual<T>(this ObjectValidationGenericBuilder<T> target,
            T value, object message)
        {
            return target.Predicate(new NotValidator {Operator = new EqualValidator {Value = value}, Message = message});
        }

        public static ObjectValidationBuilder IsNotEqual(this ObjectValidationBuilder target, string property,
            object value, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new EqualValidator {Property = property, Value = value},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotEqual<T, TProperty>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, TProperty>> property, TProperty value,
            object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new EqualValidator {Property = Reflection.GetPropertyPath(property), Value = value},
                    Message = message
                });
        }

        // IsIn

        public static ObjectValidationBuilder IsIn(this ObjectValidationBuilder target, IEnumerable items,
            object message)
        {
            return target.Predicate(new InValidator {Items = items, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsIn<T, TProperty>(
            this ObjectValidationGenericBuilder<T> target, IEnumerable<TProperty> items, object message)
        {
            return target.Predicate(new InValidator {Items = items, Message = message});
        }

        public static ObjectValidationBuilder IsIn(this ObjectValidationBuilder target, string property,
            IEnumerable items, object message)
        {
            return target.Predicate(new InValidator {Property = property, Items = items, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsIn<T, TProperty>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, TProperty>> property,
            IEnumerable<TProperty> items, object message)
        {
            return
                target.Predicate(new InValidator
                {
                    Property = Reflection.GetPropertyPath(property),
                    Items = items,
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotIn(this ObjectValidationBuilder target, IEnumerable items,
            object message)
        {
            return target.Predicate(new NotValidator {Operator = new InValidator {Items = items}, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotIn<T, TProperty>(
            this ObjectValidationGenericBuilder<T> target, IEnumerable<TProperty> items, object message)
        {
            return target.Predicate(new NotValidator {Operator = new InValidator {Items = items}, Message = message});
        }

        public static ObjectValidationBuilder IsNotIn(this ObjectValidationBuilder target, string property,
            IEnumerable items, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new InValidator {Property = property, Items = items},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotIn<T, TProperty>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, TProperty>> property,
            IEnumerable<TProperty> items, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new InValidator {Property = Reflection.GetPropertyPath(property), Items = items},
                    Message = message
                });
        }

        // IsNullOrEmpty

        public static ObjectValidationBuilder IsNullOrEmpty(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new NullOrEmptyValidator {Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNullOrEmpty<T>(this ObjectValidationGenericBuilder<T> target,
            object message)
        {
            return target.Predicate(new NullOrEmptyValidator {Message = message});
        }

        public static ObjectValidationBuilder IsNullOrEmpty(this ObjectValidationBuilder target, string property,
            object message)
        {
            return target.Predicate(new NullOrEmptyValidator {Property = property, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNullOrEmpty<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new NullOrEmptyValidator
                {
                    Property = Reflection.GetPropertyPath(property),
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotNullOrEmpty(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new NullOrEmptyValidator(), Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotNullOrEmpty<T>(
            this ObjectValidationGenericBuilder<T> target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new NullOrEmptyValidator(), Message = message});
        }

        public static ObjectValidationBuilder IsNotNullOrEmpty(this ObjectValidationBuilder target, string property,
            object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new NullOrEmptyValidator {Property = property},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotNullOrEmpty<T>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new NullOrEmptyValidator {Property = Reflection.GetPropertyPath(property)},
                    Message = message
                });
        }

        // IsNullOrWhiteSpace

        public static ObjectValidationBuilder IsNullOrWhiteSpace(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new NullOrWhiteSpaceValidator {Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNullOrWhiteSpace<T>(
            this ObjectValidationGenericBuilder<T> target, object message)
        {
            return target.Predicate(new NullOrWhiteSpaceValidator {Message = message});
        }

        public static ObjectValidationBuilder IsNullOrWhiteSpace(this ObjectValidationBuilder target, string property,
            object message)
        {
            return target.Predicate(new NullOrWhiteSpaceValidator {Property = property, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNullOrWhiteSpace<T>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new NullOrWhiteSpaceValidator
                {
                    Property = Reflection.GetPropertyPath(property),
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotNullOrWhiteSpace(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new NullOrWhiteSpaceValidator(), Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotNullOrWhiteSpace<T>(
            this ObjectValidationGenericBuilder<T> target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new NullOrWhiteSpaceValidator(), Message = message});
        }

        public static ObjectValidationBuilder IsNotNullOrWhiteSpace(this ObjectValidationBuilder target, string property,
            object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new NullOrWhiteSpaceValidator {Property = property},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotNullOrWhiteSpace<T>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new NullOrWhiteSpaceValidator {Property = Reflection.GetPropertyPath(property)},
                    Message = message
                });
        }

        // IsStartsWith

        public static ObjectValidationBuilder IsStartsWith(this ObjectValidationBuilder target, string value,
            object message)
        {
            return target.Predicate(new StartsWithValidator {Value = value, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsStartsWith<T>(this ObjectValidationGenericBuilder<T> target,
            string value, object message)
        {
            return target.Predicate(new StartsWithValidator {Value = value, Message = message});
        }

        public static ObjectValidationBuilder IsStartsWith(this ObjectValidationBuilder target, string property,
            string value, object message)
        {
            return target.Predicate(new StartsWithValidator {Property = property, Value = value, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsStartsWith<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, string value, object message)
        {
            return
                target.Predicate(new StartsWithValidator
                {
                    Property = Reflection.GetPropertyPath(property),
                    Value = value,
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotStartsWith(this ObjectValidationBuilder target, string value,
            object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new StartsWithValidator {Value = value},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotStartsWith<T>(
            this ObjectValidationGenericBuilder<T> target, string value, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new StartsWithValidator {Value = value},
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotStartsWith(this ObjectValidationBuilder target, string property,
            string value, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new StartsWithValidator {Property = property, Value = value},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotStartsWith<T>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, string>> property, string value,
            object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new StartsWithValidator {Property = Reflection.GetPropertyPath(property), Value = value},
                    Message = message
                });
        }

        // IsEndsWith

        public static ObjectValidationBuilder IsEndsWith(this ObjectValidationBuilder target, string value,
            object message)
        {
            return target.Predicate(new EndsWithValidator {Value = value, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsEndsWith<T>(this ObjectValidationGenericBuilder<T> target,
            string value, object message)
        {
            return target.Predicate(new EndsWithValidator {Value = value, Message = message});
        }

        public static ObjectValidationBuilder IsEndsWith(this ObjectValidationBuilder target, string property,
            string value, object message)
        {
            return target.Predicate(new EndsWithValidator {Property = property, Value = value, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsEndsWith<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, string value, object message)
        {
            return
                target.Predicate(new EndsWithValidator
                {
                    Property = Reflection.GetPropertyPath(property),
                    Value = value,
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotEndsWith(this ObjectValidationBuilder target, string value,
            object message)
        {
            return
                target.Predicate(new NotValidator {Operator = new EndsWithValidator {Value = value}, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotEndsWith<T>(this ObjectValidationGenericBuilder<T> target,
            string value, object message)
        {
            return
                target.Predicate(new NotValidator {Operator = new EndsWithValidator {Value = value}, Message = message});
        }

        public static ObjectValidationBuilder IsNotEndsWith(this ObjectValidationBuilder target, string property,
            string value, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new EndsWithValidator {Property = property, Value = value},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotEndsWith<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, string value, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new EndsWithValidator {Property = Reflection.GetPropertyPath(property), Value = value},
                    Message = message
                });
        }

        // IsContains

        public static ObjectValidationBuilder IsContains(this ObjectValidationBuilder target, string value,
            object message)
        {
            return target.Predicate(new ContainsValidator {Value = value, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsContains<T>(this ObjectValidationGenericBuilder<T> target,
            string value, object message)
        {
            return target.Predicate(new ContainsValidator {Value = value, Message = message});
        }

        public static ObjectValidationBuilder IsContains(this ObjectValidationBuilder target, string property,
            string value, object message)
        {
            return target.Predicate(new ContainsValidator {Property = property, Value = value, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsContains<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, string value, object message)
        {
            return
                target.Predicate(new ContainsValidator
                {
                    Property = Reflection.GetPropertyPath(property),
                    Value = value,
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotContains(this ObjectValidationBuilder target, string value,
            object message)
        {
            return
                target.Predicate(new NotValidator {Operator = new ContainsValidator {Value = value}, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotContains<T>(this ObjectValidationGenericBuilder<T> target,
            string value, object message)
        {
            return
                target.Predicate(new NotValidator {Operator = new ContainsValidator {Value = value}, Message = message});
        }

        public static ObjectValidationBuilder IsNotContains(this ObjectValidationBuilder target, string property,
            string value, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new ContainsValidator {Property = property, Value = value},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotContains<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, string value, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new ContainsValidator {Property = Reflection.GetPropertyPath(property), Value = value},
                    Message = message
                });
        }

        // IsRegex

        public static ObjectValidationBuilder IsRegex(this ObjectValidationBuilder target, string pattern,
            object message)
        {
            return target.Predicate(new RegexValidator {Pattern = pattern, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsRegex<T>(this ObjectValidationGenericBuilder<T> target,
            string pattern, object message)
        {
            return target.Predicate(new RegexValidator {Pattern = pattern, Message = message});
        }

        public static ObjectValidationBuilder IsRegex(this ObjectValidationBuilder target, string property,
            string pattern, object message)
        {
            return target.Predicate(new RegexValidator {Property = property, Pattern = pattern, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsRegex<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, string pattern, object message)
        {
            return
                target.Predicate(new RegexValidator
                {
                    Property = Reflection.GetPropertyPath(property),
                    Pattern = pattern,
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotRegex(this ObjectValidationBuilder target, string pattern,
            object message)
        {
            return
                target.Predicate(new NotValidator {Operator = new RegexValidator {Pattern = pattern}, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotRegex<T>(this ObjectValidationGenericBuilder<T> target,
            string pattern, object message)
        {
            return
                target.Predicate(new NotValidator {Operator = new RegexValidator {Pattern = pattern}, Message = message});
        }

        public static ObjectValidationBuilder IsNotRegex(this ObjectValidationBuilder target, string property,
            string pattern, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new RegexValidator {Property = property, Pattern = pattern},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotRegex<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, string pattern, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new RegexValidator {Property = Reflection.GetPropertyPath(property), Pattern = pattern},
                    Message = message
                });
        }

        // IsGuid

        public static ObjectValidationBuilder IsGuid(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new GuidValidator {Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsGuid<T>(this ObjectValidationGenericBuilder<T> target,
            object message)
        {
            return target.Predicate(new GuidValidator {Message = message});
        }

        public static ObjectValidationBuilder IsGuid(this ObjectValidationBuilder target, string property,
            object message)
        {
            return target.Predicate(new GuidValidator {Property = property, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsGuid<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new GuidValidator {Property = Reflection.GetPropertyPath(property), Message = message});
        }

        public static ObjectValidationBuilder IsNotGuid(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new GuidValidator(), Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotGuid<T>(this ObjectValidationGenericBuilder<T> target,
            object message)
        {
            return target.Predicate(new NotValidator {Operator = new GuidValidator(), Message = message});
        }

        public static ObjectValidationBuilder IsNotGuid(this ObjectValidationBuilder target, string property,
            object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new GuidValidator {Property = property},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotGuid<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new GuidValidator {Property = Reflection.GetPropertyPath(property)},
                    Message = message
                });
        }

        // IsAbsoluteUri

        public static ObjectValidationBuilder IsAbsoluteUri(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new AbsoluteUriValidator {Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsAbsoluteUri<T>(this ObjectValidationGenericBuilder<T> target,
            object message)
        {
            return target.Predicate(new AbsoluteUriValidator {Message = message});
        }

        public static ObjectValidationBuilder IsAbsoluteUri(this ObjectValidationBuilder target, string property,
            object message)
        {
            return target.Predicate(new AbsoluteUriValidator {Property = property, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsAbsoluteUri<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new AbsoluteUriValidator
                {
                    Property = Reflection.GetPropertyPath(property),
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotAbsoluteUri(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new AbsoluteUriValidator(), Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotAbsoluteUri<T>(
            this ObjectValidationGenericBuilder<T> target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new AbsoluteUriValidator(), Message = message});
        }

        public static ObjectValidationBuilder IsNotAbsoluteUri(this ObjectValidationBuilder target, string property,
            object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new AbsoluteUriValidator {Property = property},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotAbsoluteUri<T>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new AbsoluteUriValidator {Property = Reflection.GetPropertyPath(property)},
                    Message = message
                });
        }

        // IsRelativeUri

        public static ObjectValidationBuilder IsRelativeUri(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new RelativeUriValidator {Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsRelativeUri<T>(this ObjectValidationGenericBuilder<T> target,
            object message)
        {
            return target.Predicate(new RelativeUriValidator {Message = message});
        }

        public static ObjectValidationBuilder IsRelativeUri(this ObjectValidationBuilder target, string property,
            object message)
        {
            return target.Predicate(new RelativeUriValidator {Property = property, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsRelativeUri<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new RelativeUriValidator
                {
                    Property = Reflection.GetPropertyPath(property),
                    Message = message
                });
        }

        public static ObjectValidationBuilder IsNotRelativeUri(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new RelativeUriValidator(), Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotRelativeUri<T>(
            this ObjectValidationGenericBuilder<T> target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new RelativeUriValidator(), Message = message});
        }

        public static ObjectValidationBuilder IsNotRelativeUri(this ObjectValidationBuilder target, string property,
            object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new RelativeUriValidator {Property = property},
                    Message = message
                });
        }

        public static ObjectValidationGenericBuilder<T> IsNotRelativeUri<T>(
            this ObjectValidationGenericBuilder<T> target, Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new RelativeUriValidator {Property = Reflection.GetPropertyPath(property)},
                    Message = message
                });
        }

        // IsUri

        public static ObjectValidationBuilder IsUri(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new UriValidator {Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsUri<T>(this ObjectValidationGenericBuilder<T> target,
            object message)
        {
            return target.Predicate(new UriValidator {Message = message});
        }

        public static ObjectValidationBuilder IsUri(this ObjectValidationBuilder target, string property, object message)
        {
            return target.Predicate(new UriValidator {Property = property, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsUri<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new UriValidator {Property = Reflection.GetPropertyPath(property), Message = message});
        }

        public static ObjectValidationBuilder IsNotUri(this ObjectValidationBuilder target, object message)
        {
            return target.Predicate(new NotValidator {Operator = new UriValidator(), Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotUri<T>(this ObjectValidationGenericBuilder<T> target,
            object message)
        {
            return target.Predicate(new NotValidator {Operator = new UriValidator(), Message = message});
        }

        public static ObjectValidationBuilder IsNotUri(this ObjectValidationBuilder target, string property,
            object message)
        {
            return
                target.Predicate(new NotValidator {Operator = new UriValidator {Property = property}, Message = message});
        }

        public static ObjectValidationGenericBuilder<T> IsNotUri<T>(this ObjectValidationGenericBuilder<T> target,
            Expression<Func<T, string>> property, object message)
        {
            return
                target.Predicate(new NotValidator
                {
                    Operator = new UriValidator {Property = Reflection.GetPropertyPath(property)},
                    Message = message
                });
        }
    }
}