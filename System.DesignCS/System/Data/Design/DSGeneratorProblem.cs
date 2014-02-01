namespace System.Data.Design
{
    using System;
    using System.Runtime;

    internal sealed class DSGeneratorProblem
    {
        private string message;
        private DataSourceComponent problemSource;
        private ProblemSeverity severity;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal DSGeneratorProblem(string message, ProblemSeverity severity, DataSourceComponent problemSource)
        {
            this.message = message;
            this.severity = severity;
            this.problemSource = problemSource;
        }

        internal string Message
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.message;
            }
        }

        internal DataSourceComponent ProblemSource
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.problemSource;
            }
        }

        internal ProblemSeverity Severity
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return this.severity;
            }
        }
    }
}

