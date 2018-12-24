using System.Collections.Generic;
using System.Text;

namespace Dardanelles
{
    abstract class Problem
    {
        public abstract ProblemType Type { get; }
        public abstract decimal Difficulty { get; }
        public abstract bool AutoGrade { get; }
        public abstract string Prompt { get; protected set; }
        public abstract string Answer { get; protected set; }
    }

    internal struct ProblemParameters
    {
        public decimal Difficulty { get; set; }
        public bool AutoGrade { get; set; }
        public ProblemType Type { get; set; }
        public string AllowedLanguages { get; set; }
    }

    internal enum ProblemType
    {
        Programming,
        Shell,
        Research,
        Conceptual
    }
}