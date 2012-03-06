﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Sdk;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace Xbehave
{
    /// <summary>
    /// Provides extensions for a fluent specification syntax
    /// </summary>
    public static class SpecificationExtensions
    {
        private const string IDisposableHintMessaage = "Your context implements IDisposable. Use ContextFixture() to have its lifecycle managed by Xbehave.";

        /// <summary>
        /// Records a context setup for this specification.
        /// </summary>
        /// <param name="message">A message describing the established context.</param>
        /// <param name="arrange">The action that will establish the context.</param>
        public static ISpecificationPrimitive Context( this string message, Action arrange )
        {
            return Core.SpecificationContext.Context( message,
                                          () =>
                                          {
                                              arrange();
                                              return DisposableAction.None;
                                          } );
        }

        /// <summary>
        /// Trap for using contexts implementing IDisposable with the wrong overload.
        /// </summary>
        /// <param name="message">A message describing the established context.</param>
        /// <param name="arrange">The action that will establish and return the context for this test.</param>
        [Obsolete( IDisposableHintMessaage )]
        public static void Context( this string message, ContextDelegate arrange )
        {
            throw new InvalidOperationException( IDisposableHintMessaage );
        }

        /// <summary>
        /// Records a disposable context for this specification. The context lifecycle will be managed by Xbehave.
        /// </summary>
        /// <param name="message">A message describing the established context.</param>
        /// <param name="arrange">The action that will establish and return the context for this test.</param>
        public static ISpecificationPrimitive ContextFixture( this string message, ContextDelegate arrange )
        {
            return Core.SpecificationContext.Context( message, arrange );
        }

        /// <summary>
        /// Records an action to be performed on the context for this specification.
        /// </summary>
        /// <param name="message">A message describing the action.</param>
        /// <param name="act">The action to perform.</param>
        public static ISpecificationPrimitive Do( this string message, Action act )
        {
            return Core.SpecificationContext.Do( message, act );
        }

        /// <summary>
        /// Records an assertion for this specification.
        /// Each assertion is executed on an isolated context.
        /// </summary>
        /// <param name="message">A message describing the expected result.</param>
        /// <param name="assert">The action that will verify the expectation.</param>
        public static ISpecificationPrimitive Assert( this string message, Action assert )
        {
            return Core.SpecificationContext.Assert( message, assert );
        }

        /// <summary>
        /// Records an observation for this specification.
        /// All observations are executed on the same context.
        /// </summary>
        /// <param name="message">A message describing the expected result.</param>
        /// <param name="observation">The action that will verify the expectation.</param>
        public static ISpecificationPrimitive Observation( this string message, Action observation )
        {
            return Core.SpecificationContext.Observation( message, observation );
        }

        /// <summary>
        /// Records a skipped assertion for this specification.
        /// </summary>
        /// <param name="message">A message describing the expected result.</param>
        /// <param name="skippedAction">The action that will verify the expectation.</param>
        public static ISpecificationPrimitive Todo( this string message, Action skippedAction )
        {
            return Core.SpecificationContext.Todo( message, skippedAction );
        }
    }
}