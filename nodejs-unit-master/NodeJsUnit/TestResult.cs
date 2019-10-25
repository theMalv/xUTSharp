namespace Adeptik.NodeJsUnit
{
    /// <summary>
    /// The result of test execution in one file.
    /// </summary>
    internal class TestResult
    {
        /// <summary>
        /// File with tests
        /// </summary>
        public string TestFile { get; set; }

        /// <summary>
        /// Whether all the tests in test file are passed or not
        /// </summary>
        public bool Passed { get; set; }

        /// <summary>
        /// Jasmine standard output
        /// </summary>
        public string Output { get; set; }
    }
}