using System;

namespace Dardanelles
{
    class Student
    {
        public SkillSet Scores { get; protected set; }

        public Student(SkillSet BaseScores)
        {
            Scores = BaseScores;
        }

        public Student() : this(new SkillSet()) { }

        public Problem GenerateIndividual(ProblemParameters pp)
        {
            throw new NotImplementedException();
        }
    }

    internal struct SkillSet
    {
        public decimal Talent => (VBTalent + CSTalent + FSTalent) / (((VBTalent > 0) ? 1 : 0) + ((CSTalent > 0) ? 1 : 0) + ((FSTalent > 0) ? 1 : 0));
        public decimal VBTalent { get; set; }
        public decimal CSTalent { get; set; }
        public decimal FSTalent { get; set; }
        public decimal Leadership { get; set; }
        public decimal Teamwork { get; set; }
        public decimal Knowledge { get; set; }
    }
}