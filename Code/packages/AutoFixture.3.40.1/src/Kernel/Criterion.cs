﻿using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Serves as a criterion against which a value can be compared. When a
    /// given value equals the criterion, it can be said to match the 
    /// criterion; otherwise not.
    /// </summary>
    /// <typeparam name="T">The type of values being compared.</typeparam>
    /// <remarks>
    /// <para>
    /// Sometimes you need to compare various candidates against a particular
    /// value, but the comparison rules may vary. Sometimes, strict equality
    /// is warranted, but otherwise (e.g. when comparing strings) looser
    /// comparison rules can be applied (e.g. case-insensitve comparison of
    /// strings).
    /// </para>
    /// <para>
    /// The Criterion class enables you to capture the desied value (the
    /// <see cref="Target" />) together with the appropriate comparison method
    /// (the <see cref="Comparer" />) in a single object.
    /// </para>
    /// </remarks>
    public class Criterion<T> : IEquatable<T>
    {
        private readonly T target;
        private readonly IEqualityComparer<T> comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Criterion{T}" />
        /// class.
        /// </summary>
        /// <param name="target">
        /// The target value against which candidates will be compared. Can be
        /// <see langword="null" />.
        /// </param>
        /// <param name="comparer">
        /// The comparison method used to compare candidates against
        /// <paramref name="target" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer" /> is <see langword="null" />.
        /// </exception>
        /// <seealso cref="Target" />
        /// <seealso cref="Comparer" />
        public Criterion(T target, IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            this.target = target;
            this.comparer = comparer;
        }

        /// <summary>
        /// Compares a candidate value against this
        /// <see cref="Criterion{T}" />.
        /// </summary>
        /// <param name="other">
        /// The candidate to compare against this value.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="other" /> is deemed
        /// equal to this instance; otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method compares the candidate <paramref name="other" />
        /// against <see cref="Target" />, using <see cref="Comparer" /> as
        /// the comparison method. If this combination of target value and
        /// comparison method deems the candidate to be equal to this
        /// criterion, the return value is <see langword="true" />.
        /// </para>
        /// </remarks>
        public bool Equals(T other)
        {
            return this.comparer.Equals(this.target, other);
        }

        /// <summary>
        /// Determines whether this object is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare to this object.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="obj" /> is equal to this
        /// object; otherwise, <see langword="false" />.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as Criterion<T>;
            if (other == null)
                return base.Equals(obj);
            return object.Equals(this.target, other.target)
                && object.Equals(this.comparer, other.comparer);
        }

        /// <summary>
        /// The desired target value, as supplied via the constructor.
        /// </summary>
        public T Target
        {
            get { return this.target; }
        }

        /// <summary>
        /// The comparison method used to compare <see cref="Target" /> to
        /// candidates.
        /// </summary>
        public IEqualityComparer<T> Comparer
        {
            get { return this.comparer; }
        }

        /// <summary>
        /// Returns the hash code for the object.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return this.target.GetHashCode() ^ this.comparer.GetHashCode();
        }
    }
}