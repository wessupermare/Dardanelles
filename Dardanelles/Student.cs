using System;

namespace Dardanelles
{
    class Student
    {
        public SkillSet Scores { get; protected set; }
        public string Name { get; protected set; }

        public Student(string name, SkillSet BaseScores)
        {
            Name = name.ToLower().Trim();
            Scores = BaseScores;
        }

        public Student(string name) : this(name, new SkillSet()) { }

        public Student(Newtonsoft.Json.Linq.JToken student) : this(student["Name"].ToString(), new SkillSet(student["Scores"])) { }

        public Student() : this("") { }

        private readonly Random r = new Random();
        public Problem GenerateIndividual(ProblemParameters pp)
        {
            string lang = pp.AllowedLanguages.Length > 0 ? pp.AllowedLanguages.Substring(r.Next(pp.AllowedLanguages.Length / 2) * 2, 2) : "VBC#F#".Substring(r.Next("VBC#F#".Length / 2) * 2, 2);
            decimal talentNum = lang == "VB" ? Scores.VBTalent : (lang == "C#" ? Scores.CSTalent : (lang == "F#" ? Scores.FSTalent : Scores.Talent));

            ProblemParameters passparams = new ProblemParameters
            {
                Difficulty = Math.Min(pp.Difficulty, talentNum),
                AllowedLanguages = lang,
                Type = pp.Type,
                AutoGrade = pp.AutoGrade
            };

            if (pp.Type == ProblemType.Programming)
                return new ProgrammingProblem(pp);
            else
                throw new NotImplementedException();
        }

        public void ProcessID(uint ID)
        {
            string id = ID.ToString();
            if (id[0] == ((int)ProblemType.Programming + 1).ToString()[0])
            {
                SkillSet results = Scores;
#nullable disable
                if (id[1] == '1')   // VB
                    results.VBTalent += CalcAdjustment(Scores.VBTalent, int.Parse(id[2..5]) / (decimal)10);
                else if (id[1] == '2')  // C#
                    results.CSTalent += CalcAdjustment(Scores.CSTalent, int.Parse(id[2..5]) / (decimal)10);
                else if (id[1] == '3')  // F#
                    results.FSTalent += CalcAdjustment(Scores.FSTalent, int.Parse(id[2..5]) / (decimal)10);
                else
                {
                    Console.WriteLine("Bad id.");
                    return;
                }
#nullable enable
                Scores = results;
            }
            else
                Console.WriteLine("Bad id.");
        }

        private decimal CalcAdjustment(decimal current, decimal difficulty)
        {
            return Math.Max(0,  difficulty / ((current != 0 ? current : (decimal)0.1) * 10) );
        }
    }

    internal struct SkillSet
    {
        public SkillSet(Newtonsoft.Json.Linq.JToken set)
        {
            VBTalent = decimal.Parse(set["VBTalent"].ToString());
            CSTalent = decimal.Parse(set["CSTalent"].ToString());
            FSTalent = decimal.Parse(set["FSTalent"].ToString());
            Leadership = decimal.Parse(set["Leadership"].ToString());
            Teamwork = decimal.Parse(set["Teamwork"].ToString());
            Knowledge = decimal.Parse(set["Knowledge"].ToString());
        }

        public decimal Talent => VBTalent > 0 ? (VBTalent + CSTalent + FSTalent) / (((VBTalent > 0) ? 1 : 0) + ((CSTalent > 0) ? 1 : 0) + ((FSTalent > 0) ? 1 : 0)) : 0;
        public decimal VBTalent { get; set; }
        public decimal CSTalent { get; set; }
        public decimal FSTalent { get; set; }
        public decimal Leadership { get; set; }
        public decimal Teamwork { get; set; }
        public decimal Knowledge { get; set; }
    }
}