﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Xunit.Sdk;

namespace Microsoft.AspNetCore.Razor.Design.IntegrationTests
{
    public class InitializeTestProjectAttribute : BeforeAfterTestAttribute
    {
        private readonly string _originalProjectName;
        private readonly string _testProjectName;
        private readonly string _baseDirectory;
        private readonly string[] _additionalProjects;

        public InitializeTestProjectAttribute(string projectName, params string[] additionalProjects)
            : this (projectName, projectName, string.Empty, additionalProjects)
        {
        }

        public InitializeTestProjectAttribute(string originalProjectName, string targetProjectName, string baseDirectory, string[] additionalProjects = null)
        {
            _originalProjectName = originalProjectName;
            _testProjectName = targetProjectName;
            _baseDirectory = baseDirectory;
            _additionalProjects = additionalProjects ?? Array.Empty<string>();
        }

        public override void Before(MethodInfo methodUnderTest)
        {
            if (!typeof(MSBuildIntegrationTestBase).GetTypeInfo().IsAssignableFrom(methodUnderTest.DeclaringType.GetTypeInfo()))
            {
                throw new InvalidOperationException($"This should be used on a class derived from {typeof(MSBuildIntegrationTestBase)}");
            }

            MSBuildIntegrationTestBase.Project = ProjectDirectory.Create(_originalProjectName, _testProjectName, _baseDirectory, _additionalProjects);
        }

        public override void After(MethodInfo methodUnderTest)
        {
            if (!typeof(MSBuildIntegrationTestBase).GetTypeInfo().IsAssignableFrom(methodUnderTest.DeclaringType.GetTypeInfo()))
            {
                throw new InvalidOperationException($"This should be used on a class derived from {typeof(MSBuildIntegrationTestBase)}");
            }

            MSBuildIntegrationTestBase.Project.Dispose();
            MSBuildIntegrationTestBase.Project = null;
        }
    }
}
