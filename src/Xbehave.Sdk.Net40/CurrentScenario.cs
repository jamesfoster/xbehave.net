﻿// <copyright file="CurrentScenario.cs" company="xBehave.net contributors">
//  Copyright (c) xBehave.net contributors. All rights reserved.
// </copyright>

namespace Xbehave.Sdk
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Xunit.Sdk;

    public static class CurrentScenario
    {
        [ThreadStatic]
        private static bool addingBackgroundSteps;

        [ThreadStatic]
        private static List<Step> steps;

        [ThreadStatic]
        private static List<Action> teardowns;

        public static bool AddingBackgroundSteps
        {
            get { return CurrentScenario.addingBackgroundSteps; }
            set { CurrentScenario.addingBackgroundSteps = value; }
        }

        private static List<Step> Steps
        {
            get { return steps ?? (steps = new List<Step>()); }
        }

        private static List<Action> Teardowns
        {
            get { return teardowns ?? (teardowns = new List<Action>()); }
        }

        public static Step AddStep(string name, Action body)
        {
            var step = new Step(addingBackgroundSteps ? "(Background) " + name : name, body);
            Steps.Add(step);
            return step;
        }

        public static void AddTeardown(Action teardown)
        {
            if (teardown != null)
            {
                Teardowns.Add(teardown);
            }
        }

        public static IEnumerable<Action> ExtractTeardowns()
        {
            try
            {
                return Teardowns;
            }
            finally
            {
                teardowns = null;
            }
        }

        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Required to prevent infinite loops in test runners (TestDrive.NET, Resharper) when they are allowed to handle exceptions.")]
        public static IEnumerable<ITestCommand> ExtractCommands(MethodCall methodCall, IEnumerable<ITestCommand> commands)
        {
            Guard.AgainstNullArgument("methodCall", methodCall);
            Guard.AgainstNullArgument("commands", commands);

            try
            {
                try
                {
                    var feature = methodCall.Method.IsStatic ? null : methodCall.Method.CreateInstance();
                    foreach (var command in commands)
                    {
                        var result = command.Execute(feature);
                        if ((result as PassedResult) == null)
                        {
                            return new ITestCommand[] { new ReplayCommand(methodCall.Method, result) };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new ITestCommand[] { new ExceptionCommand(methodCall, ex) };
                }

                var contexts = new ContextFactory().CreateContexts(methodCall, Steps).ToArray();
                return contexts.SelectMany((context, index) => context.CreateCommands(index + 1));
            }
            finally
            {
                steps = null;
            }
        }
    }
}