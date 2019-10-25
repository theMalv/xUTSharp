using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace Adeptik.NodeJsUnit
{
    /// <summary>
    /// NodeJS test runner using Jasmine test framework
    /// </summary>
    public static class NodeJsJasmineRunner
    {
        private static readonly HashAlgorithm Hasher = SHA256.Create();

        /// <summary>
        /// Runs jasmine on single test file
        /// </summary>
        /// <param name="testFile">Test file</param>
        /// <param name="testFileHash">Test file hash^: SHA256 in hex representation</param>
        /// <param name="output">xUnit output service write to</param>
        /// <param name="scriptsFolder">Folder containing 'JasmineTestRunner.js'</param>
        public static void Run(string testFile, string testFileHash, ITestOutputHelper output, string scriptsFolder)
        {
            var projectRoot = ResolveTestsRoot();
            var testFileFullName = Path.Combine(projectRoot, testFile);
            output.WriteLine($"Test file: '{testFileFullName}'");

            Assert.True(File.Exists(testFileFullName), "Test file not found");

            var expectedTestFileHash = BigInteger.Parse("0" + testFileHash, NumberStyles.HexNumber)
                .ToByteArray(isUnsigned: true, isBigEndian: true);
            var actualTestFileHash = Hasher.ComputeHash(File.OpenRead(testFileFullName));
            Assert.True(expectedTestFileHash.SkipWhile(x => x == 0).SequenceEqual(actualTestFileHash.SkipWhile(x => x == 0)), "Test file is invalid.");

            CheckRequiredModules(output, projectRoot);

            using var nodeServices = NodeServicesFactory.CreateNodeServices(
                new NodeServicesOptions(new ServiceCollection().BuildServiceProvider())
                {
                    ProjectPath = projectRoot
                });
            var testResult = nodeServices.InvokeAsync<TestResult>(Path.Combine(scriptsFolder, "JasmineTestRunner.js"), testFileFullName).Result;
            output.WriteLine(testResult.Output);
            Assert.True(testResult.Passed, $"Test suite in '{testResult.TestFile}' not passed. See details in test output.");
        }

        /// <summary>
        /// Check wheteher required NPM modules are installed
        /// </summary>
        /// <param name="output">xUnit output service write to</param>
        /// <param name="projectRoot">Path to project root folder</param>
        private static void CheckRequiredModules(ITestOutputHelper output, string projectRoot)
        {
            var nodeModulesFolder = Path.Combine(projectRoot, "node_modules");
            if (!Directory.Exists(nodeModulesFolder))
            {
                output.WriteLine($"node_modules folder not found in '{nodeModulesFolder}'. Are you missing restore NPM packages?");
                return;
            }

            var modules = new[] {
                new { Name= "jasmine" , Description = "This module is needed to run tests with Jasmine test framework" },
                new { Name= "ts-node", Description = "This module is needed to run typescript tests" },
                new { Name= "typescript", Description = "This module is needed to run typescript tests" },
                new { Name= $"@types{Path.DirectorySeparatorChar}jasmine", Description = "This module is needed to use a jasmine API in typescript tests" }
            };
            foreach (var module in modules)
            {
                if (!Directory.Exists(Path.Combine(nodeModulesFolder, module.Name)))
                {
                    output.WriteLine($"Required module '{module.Name}' not found. {module.Description}. Add dependency in package.json to the module.");
                }
            }
        }
        /// <summary>
        /// Extracts project path from 'NODE_JS_UNIT_TEST_PROJECT' file
        /// </summary>
        /// <returns>Path to project</returns>
        private static string ResolveTestsRoot()
        {
            var assemblyDir = Path.GetDirectoryName(typeof(NodeJsJasmineRunner).Assembly.Location);
            var projectPathFile = Path.Combine(assemblyDir, "NODE_JS_UNIT_TEST_PROJECT");
            return Path.GetDirectoryName(File.ReadLines(projectPathFile).First());
        }
    }
}
