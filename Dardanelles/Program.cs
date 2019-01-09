using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dardanelles
{
    class Program
    {
        static void Main(string[] args) { new Program(); }

        private readonly Config Config;
        private bool SpinFlag = false;
        public Program()
        {
            SetupSpinner();
            SpinFlag = true;
            Config = new Config("data.json");
            if (!Config.ContainsKey("SetupComplete"))
                Setup();
            else if (!(bool)Config["SetupComplete"])
            {
                Config.DeleteAll();
                Config = new Config("data.json");
                Setup();
            }
            SpinFlag = false;
            Console.Clear();
            Console.WriteLine("Welcome to the Dardenelles interface. Type 'help' for assistance.");
            RunLoop();
        }

        private void RunLoop()
        {
            while (true)
            {
                Console.Write(">");
                string input = Console.ReadLine().ToLower().Trim();

                switch (input)
                {
                    case "help":
                        Console.WriteLine("Accepted commands:\r\n\thelp\r\n\tclear\r\n\tp(roblem)\r\n\ta(nswer)\r\n\tadd\r\n\tstat\r\n\trem\r\n\texit");
                        break;

                    case "clear":
                        Console.Clear();
                        break;

                    case "p":
                        int difficulty = -1;
                        while (difficulty < 0)
                        {
                            Console.Write("Difficulty (decimal value between 0.0 & 10.0): ");
                            int.TryParse(Console.ReadLine(), out difficulty);
                        }
                        foreach (Student student in ((List<Newtonsoft.Json.Linq.JToken>)Config["Students"]).ConvertAll((t) => new Student(t)))
                        {
                            Problem problem = student.GenerateIndividual(new ProblemParameters
                            {
                                Type = ProblemType.Programming,
                                AllowedLanguages = student.Scores.VBTalent < 3 ? "VB" : (student.Scores.VBTalent < 7 || student.Scores.CSTalent < 7 ? "VBC#" : "VBC#F#"),
                                Difficulty = difficulty
                            });
                            Console.WriteLine($"ID: {problem.ID}\r\nProblem for: {student.Name}\r\n{problem.Prompt}{(problem.AutoGrade ? $"\r\n\tAnswer: {problem.Answer}" : "")}");
                        }
                        break;

                    case "a":
                        uint id = 0;
                        Console.Write("ID: ");
                        if (!uint.TryParse(Console.ReadLine(), out id) || id < 10000)
                        {
                            Console.WriteLine("Invalid id.");
                            continue;
                        }

                        Console.Write("Name: ");
                        string name = Console.ReadLine().ToLower().Trim();
                        List<Student> students = GetStudents();
                        if (students.Find((s) => s.Name == name) == null) Console.WriteLine("Unknown name."); else { students.Find((s) => s.Name == name).ProcessID(id); Config["Students"] = students; }
                        break;

                    case "add":
                        List<Student> sts = GetStudents();
                        Console.WriteLine($"Add students: Type in student names (one per line) followed by a blank line when finished.");

                        while (sts.Count == 0 || !string.IsNullOrWhiteSpace(sts[sts.Count - 1].Name))
                            sts.Add(new Student(Console.ReadLine()));

                        sts.RemoveAt(sts.Count - 1);
                        Config["Students"] = sts;
                        break;

                    case "rem":
                        List<Student> stlistforrem = GetStudents();
                        Console.Write("Student name: ");
                        string n = Console.ReadLine().ToLower().Trim();
                        Console.Write("Confirm name: ");
                        if (n != Console.ReadLine().ToLower().Trim())
                        {
                            Console.WriteLine("Confirmation failed");
                            continue;
                        }
                        if (stlistforrem.FindAll((s) => s.Name == n).Count > 0)
                        {
                            stlistforrem.RemoveAll((s) => s.Name == n);
                        }
                        break;

                    case "exit":
                        return;

                    default:
                        Console.WriteLine("Command not recognised.");
                        continue;
                }
            }
        }

        private List<Student> GetStudents()
        {
            return ((List<Newtonsoft.Json.Linq.JToken>)Config["Students"]).ConvertAll((t) => new Student(t));
        }

        /// <summary>
        /// Sets up a new config file with student info.
        /// </summary>
        private void Setup()
        {
            Config["SetupComplete"] = false;

            Console.Write("\bSetting up project Dardanelles. One moment please ");

            SpinFlag = false;

            List<Student> students = new List<Student>();
            Console.WriteLine($"\b.{Environment.NewLine}Add students: Type in student names (one per line) followed by a blank line when finished.");

            while (students.Count == 0 || !string.IsNullOrWhiteSpace(students[students.Count - 1].Name))
                students.Add(new Student(Console.ReadLine()));

            students.RemoveAt(students.Count - 1);
            Config["Students"] = students;

            SpinFlag = true;

            Config["SetupComplete"] = true;
        }

        /// <summary>
        /// Spins a bar around on-screen.
        /// </summary>
        private void SetupSpinner()
        {
            Task.Run(() =>
            {
                char c = '|';
                while (true)
                {
                    while (SpinFlag)
                    {
                        Console.Write($"{c}\b");
                        switch (c)
                        {
                            case '|': c = '/'; break;
                            case '/': c = '-'; break;
                            case '-': c = '\\'; break;
                            case '\\': c = '|'; break;
                        }
                        Thread.Sleep(100);
                    }
                    Thread.Yield();
                }
            });
        }
    }
}