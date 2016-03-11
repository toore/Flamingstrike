﻿using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a sequence of consecutive numbers, starting at 1.
    /// </summary>
    public class ByteSequenceGenerator : ISpecimenBuilder
    {
        private byte b;
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteSequenceGenerator"/> class.
        /// </summary>
        public ByteSequenceGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consecutive sequence.</returns>
        public byte Create()
        {
            lock (this.syncRoot)
            {
                return ++this.b;
            }
        }

        /// <summary>
        /// Creates an anonymous number.
        /// </summary>
        /// <returns>The next number in a consecutive sequence.</returns>
        /// <remarks>Obsolete: Please move over to using <see cref="Create()">Create()</see> as this method will be removed in the next release</remarks>
        [Obsolete("Please move over to using Create() as this method will be removed in the next release")]
        public byte CreateAnonymous()
        {
            return Create();
        }

        /// <summary>
        /// Creates an anonymous byte.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// The next byte in a consecutive sequence, if <paramref name="request"/> is a request
        /// for a byte; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(byte).Equals(request))
            {
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618
            }

            return this.Create();
        }
    }
}
