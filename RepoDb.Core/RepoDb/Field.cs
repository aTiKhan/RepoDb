﻿using System.Collections.Generic;
using System.Linq;
using System;
using RepoDb.Extensions;
using System.Linq.Expressions;
using System.Reflection;
using RepoDb.Exceptions;

namespace RepoDb
{
    /// <summary>
    /// An object that signifies as data field in the query statement.
    /// </summary>
    public class Field : IEquatable<Field>
    {
        private int? m_hashCode = null;

        /// <summary>
        /// Creates a new instance of <see cref="Field"/> object.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        public Field(string name) :
            this(name, null)
        { }

        /// <summary>
        /// Creates a new instance of <see cref="Field"/> object.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="type">The type of the field.</param>
        public Field(string name,
            Type type)
        {
            // Name is required
            if (string.IsNullOrEmpty(name))
            {
                throw new NullReferenceException(name);
            }

            // Set the name
            Name = name;

            // Set the type
            Type = type;
        }

        #region Properties

        /// <summary>
        /// Gets the quoted name of the field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        public Type Type { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Stringify the current field object.
        /// </summary>
        /// <returns>The string value equivalent to the name of the field.</returns>
        public override string ToString()
        {
            return string.Concat(Name, ", ", Type?.FullName, " (", m_hashCode, ")");
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates an enumerable of <see cref="Field"/> objects that derived from the string value.
        /// </summary>
        /// <param name="name">The enumerable of string values that signifies the name of the fields (for each item).</param>
        /// <returns>An enumerable of <see cref="Field"/> object.</returns>
        public static IEnumerable<Field> From(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new NullReferenceException("The field name must be null or empty.");
            }
            return From(new[] { name });
        }

        /// <summary>
        /// Creates an enumerable of <see cref="Field"/> objects that derived from the given array of string values.
        /// </summary>
        /// <param name="fields">The enumerable of string values that signifies the name of the fields (for each item).</param>
        /// <returns>An enumerable of <see cref="Field"/> object.</returns>
        public static IEnumerable<Field> From(params string[] fields)
        {
            if (fields == null)
            {
                throw new NullReferenceException("The list of fields must not be null.");
            }
            if (fields.Any(field => string.IsNullOrEmpty(field?.Trim())))
            {
                throw new NullReferenceException("The field name must be null or empty.");
            }
            foreach (var field in fields)
            {
                yield return new Field(field);
            }
        }

        /// <summary>
        /// Parses a type and creates an enumerable of <see cref="Field"/> objects.
        /// </summary>
        /// <returns>An enumerable of <see cref="Field"/> objects.</returns>
        public static IEnumerable<Field> Parse(Type type)
        {
            if (type != null)
            {
                foreach (var property in type.GetProperties())
                {
                    yield return property.AsField();
                }
            }
        }

        /// <summary>
        /// Parses an object and creates an enumerable of <see cref="Field"/> objects.
        /// </summary>
        /// <typeparam name="TEntity">The target type.</typeparam>
        /// <returns>An enumerable of <see cref="Field"/> objects.</returns>
        public static IEnumerable<Field> Parse<TEntity>()
            where TEntity : class
        {
            return Parse(typeof(TEntity));
        }

        /// <summary>
        /// Parses an object and creates an enumerable of <see cref="Field"/> objects.
        /// </summary>
        /// <param name="obj">An object to be parsed.</param>
        /// <returns>An enumerable of <see cref="Field"/> objects.</returns>
        public static IEnumerable<Field> Parse(object obj)
        {
            return Parse(obj?.GetType());
        }

        /// <summary>
        /// Parses a property from the data entity object based on the given <see cref="Expression"/> and converts the result 
        /// to <see cref="Field"/> object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the data entity that contains the property to be parsed.</typeparam>
        /// <param name="expression">The expression to be parsed.</param>
        /// <returns>An instance of <see cref="Field"/> object.</returns>
        public static Field Parse<TEntity>(Expression<Func<TEntity, object>> expression)
            where TEntity : class
        {
            if (expression.Body.IsUnary())
            {
                return Parse<TEntity>(expression.Body.ToUnary());
            }
            else if (expression.Body.IsMember())
            {
                return Parse<TEntity>(expression.Body.ToMember());
            }
            else if (expression.Body.IsBinary())
            {
                return Parse<TEntity>(expression.Body.ToBinary());
            }
            throw new InvalidExpressionException($"Expression '{expression.ToString()}' is invalid.");
        }

        /// <summary>
        /// Parses a property from the data entity object based on the given <see cref="UnaryExpression"/> and converts the result 
        /// to <see cref="Field"/> object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the data entity that contains the property to be parsed.</typeparam>
        /// <param name="expression">The expression to be parsed.</param>
        /// <returns>An instance of <see cref="Field"/> object.</returns>
        internal static Field Parse<TEntity>(UnaryExpression expression)
            where TEntity : class
        {
            if (expression.Operand.IsMember())
            {
                return Parse<TEntity>(expression.Operand.ToMember());
            }
            else if (expression.Operand.IsBinary())
            {
                return Parse<TEntity>(expression.Operand.ToBinary());
            }
            throw new InvalidExpressionException($"Expression '{expression.ToString()}' is invalid.");
        }

        /// <summary>
        /// Parses a property from the data entity object based on the given <see cref="MemberExpression"/> and converts the result 
        /// to <see cref="Field"/> object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the data entity that contains the property to be parsed.</typeparam>
        /// <param name="expression">The expression to be parsed.</param>
        /// <returns>An instance of <see cref="Field"/> object.</returns>
        internal static Field Parse<TEntity>(MemberExpression expression)
            where TEntity : class
        {
            if (expression.Member is PropertyInfo)
            {
                return expression.Member.ToPropertyInfo().AsField();
            }
            else
            {
                return new Field(expression.Member.Name);
            }
        }

        /// <summary>
        /// Parses a property from the data entity object based on the given <see cref="BinaryExpression"/> and converts the result 
        /// to <see cref="Field"/> object.
        /// </summary>
        /// <typeparam name="TEntity">The type of the data entity that contains the property to be parsed.</typeparam>
        /// <param name="expression">The expression to be parsed.</param>
        /// <returns>An instance of <see cref="OrderField"/> object.</returns>
        internal static Field Parse<TEntity>(BinaryExpression expression)
            where TEntity : class
        {
            return new Field(expression.GetName());
        }

        #endregion

        #region Equality and comparers

        /// <summary>
        /// Returns the hashcode for this <see cref="Field"/>.
        /// </summary>
        /// <returns>The hashcode value.</returns>
        public override int GetHashCode()
        {
            if (m_hashCode != null)
            {
                return m_hashCode.Value;
            }

            var hashCode = 0;

            // Set the hash code
            hashCode = Name.GetHashCode();
            if (Type != null)
            {
                hashCode += Type.GetHashCode();
            }

            // Set and return the hashcode
            return (m_hashCode = hashCode).Value;
        }

        /// <summary>
        /// Compares the <see cref="Field"/> object equality against the given target object.
        /// </summary>
        /// <param name="obj">The object to be compared to the current object.</param>
        /// <returns>True if the instances are equals.</returns>
        public override bool Equals(object obj)
        {
            return obj?.GetHashCode() == GetHashCode();
        }

        /// <summary>
        /// Compares the <see cref="Field"/> object equality against the given target object.
        /// </summary>
        /// <param name="other">The object to be compared to the current object.</param>
        /// <returns>True if the instances are equal.</returns>
        public bool Equals(Field other)
        {
            return other?.GetHashCode() == GetHashCode();
        }

        /// <summary>
        /// Compares the equality of the two <see cref="Field"/> objects.
        /// </summary>
        /// <param name="objA">The first <see cref="Field"/> object.</param>
        /// <param name="objB">The second <see cref="Field"/> object.</param>
        /// <returns>True if the instances are equal.</returns>
        public static bool operator ==(Field objA, Field objB)
        {
            if (ReferenceEquals(null, objA))
            {
                return ReferenceEquals(null, objB);
            }
            return objB?.GetHashCode() == objA.GetHashCode();
        }

        /// <summary>
        /// Compares the inequality of the two <see cref="Field"/> objects.
        /// </summary>
        /// <param name="objA">The first <see cref="Field"/> object.</param>
        /// <param name="objB">The second <see cref="Field"/> object.</param>
        /// <returns>True if the instances are not equal.</returns>
        public static bool operator !=(Field objA, Field objB)
        {
            return (objA == objB) == false;
        }

        #endregion
    }
}
