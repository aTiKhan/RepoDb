﻿using RepoDb.Interfaces;
using System;
using System.Data;

namespace RepoDb.Requests
{
    /// <summary>
    /// A class that holds the value of the 'Average' operation arguments.
    /// </summary>
    internal class AverageRequest : BaseRequest, IEquatable<AverageRequest>
    {
        private int? hashCode = null;

        /// <summary>
        /// Creates a new instance of <see cref="AverageRequest"/> object.
        /// </summary>
        /// <param name="type">The target type.</param>
        /// <param name="connection">The connection object.</param>
        /// <param name="transaction">The transaction object.</param>
        /// <param name="field">The field object.</param>
        /// <param name="where">The query expression.</param>
        /// <param name="hints">The hints for the table.</param>
        /// <param name="statementBuilder">The statement builder.</param>
        public AverageRequest(Type type,
            IDbConnection connection,
            IDbTransaction transaction,
            Field field = null,
            QueryGroup where = null,
            string hints = null,
            IStatementBuilder statementBuilder = null)
            : this(ClassMappedNameCache.Get(type),
                  connection,
                  transaction,
                  field,
                  where,
                  hints,
                  statementBuilder)
        {
            Type = type;
        }

        /// <summary>
        /// Creates a new instance of <see cref="AverageRequest"/> object.
        /// </summary>
        /// <param name="name">The name of the request.</param>
        /// <param name="connection">The connection object.</param>
        /// <param name="transaction">The transaction object.</param>
        /// <param name="field">The field object.</param>
        /// <param name="where">The query expression.</param>
        /// <param name="hints">The hints for the table.</param>
        /// <param name="statementBuilder">The statement builder.</param>
        public AverageRequest(string name,
            IDbConnection connection,
            IDbTransaction transaction,
            Field field = null,
            QueryGroup where = null,
            string hints = null,
            IStatementBuilder statementBuilder = null)
            : base(name,
                  connection,
                  transaction,
                  statementBuilder)
        {
            Field = field;
            Where = where;
            Hints = hints;
        }

        /// <summary>
        /// Gets the field to be averaged.
        /// </summary>
        public Field Field { get; }

        /// <summary>
        /// Gets the query expression used.
        /// </summary>
        public QueryGroup Where { get; }

        /// <summary>
        /// Gets the hints for the table.
        /// </summary>
        public string Hints { get; }

        #region Equality and comparers

        /// <summary>
        /// Returns the hashcode for this <see cref="AverageRequest"/>.
        /// </summary>
        /// <returns>The hashcode value.</returns>
        public override int GetHashCode()
        {
            // Make sure to return if it is already provided
            if (this.hashCode != null)
            {
                return this.hashCode.Value;
            }

            // Get first the entity hash code
            var hashCode = HashCode.Combine(Name, ".Average");

            // Add the field
            if (Field != null)
            {
                hashCode += Field.GetHashCode();
            }

            // Add the where
            if (Where != null)
            {
                hashCode += Where.GetHashCode();
            }

            // Add the hints
            if (!string.IsNullOrWhiteSpace(Hints))
            {
                hashCode += Hints.GetHashCode();
            }

            // Set and return the hashcode
            return (this.hashCode = hashCode).Value;
        }

        /// <summary>
        /// Compares the <see cref="AverageRequest"/> object equality against the given target object.
        /// </summary>
        /// <param name="obj">The object to be compared to the current object.</param>
        /// <returns>True if the instances are equals.</returns>
        public override bool Equals(object obj) =>
            obj?.GetHashCode() == GetHashCode();

        /// <summary>
        /// Compares the <see cref="AverageRequest"/> object equality against the given target object.
        /// </summary>
        /// <param name="other">The object to be compared to the current object.</param>
        /// <returns>True if the instances are equal.</returns>
        public bool Equals(AverageRequest other) =>
            other?.GetHashCode() == GetHashCode();

        /// <summary>
        /// Compares the equality of the two <see cref="AverageRequest"/> objects.
        /// </summary>
        /// <param name="objA">The first <see cref="AverageRequest"/> object.</param>
        /// <param name="objB">The second <see cref="AverageRequest"/> object.</param>
        /// <returns>True if the instances are equal.</returns>
        public static bool operator ==(AverageRequest objA,
            AverageRequest objB)
        {
            if (objA is null)
            {
                return objB is null;
            }
            return objB?.GetHashCode() == objA.GetHashCode();
        }

        /// <summary>
        /// Compares the inequality of the two <see cref="AverageRequest"/> objects.
        /// </summary>
        /// <param name="objA">The first <see cref="AverageRequest"/> object.</param>
        /// <param name="objB">The second <see cref="AverageRequest"/> object.</param>
        /// <returns>True if the instances are not equal.</returns>
        public static bool operator !=(AverageRequest objA,
            AverageRequest objB) =>
            (objA == objB) == false;

        #endregion
    }
}
