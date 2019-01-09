using System;

namespace Dardanelles
{
    internal class ProgrammingProblem : Problem
    {
        public override ProblemType Type => ProblemType.Programming;

        public override decimal Difficulty => pparams.Difficulty;
        public override bool AutoGrade => pparams.AutoGrade;

        public override string Prompt { get; protected set; }
        public override string Answer { get; protected set; }

        private ProblemParameters pparams;
        private readonly Random r = new Random();

        public ProgrammingProblem(ProblemParameters Params)
        {
            pparams = Params;

            string pr = string.Empty, ans = string.Empty;
            retry:
            switch (r.Next((int)Difficulty + 1))
            {
                case 0:     // Data-crunching.
                    pr = P0(Params);
                    break;

                case 1:     // BLT.
                    (pr, ans) = P1(Params);
                    break;

                case 2:     // Functions.
                    pr = P2(Params);
                    break;

                case 3:     // Objects & types.
                    (pr, ans) = P3(Params);
                    break;

                case 4:     // Tasks & asynchrony (> 4.5).
                    pr = P4(Params);
                    break;

                case 5:     // Event-based programming.
                    pr = P5(Params);
                    break;

                case 6:     // Recursion.
                    pr = P6(Params);
                    break;

                case 7:     // Function-based programming.
                    if (pparams.AllowedLanguages != "F#")
                        goto retry;
                    pr = P7(Params);
                    break;

                case 8:     // Generators.
                    pr = P8(Params);
                    break;

                default:    // Practical.
                    pr = PP(Params);
                    break;
            }
            Prompt = pr;
            pparams.AutoGrade = string.IsNullOrWhiteSpace(ans);
            Answer = ans;
            ID = uint.Parse($"{(int)ProblemType.Programming + 1}{(pparams.AllowedLanguages == "VB" ? 1 : (pparams.AllowedLanguages == "C#" ? 2 : 3))}{((int)(pparams.Difficulty * 10)).ToString().PadLeft(3, '0')}");
        }

        private string P0(ProblemParameters Params)             // Confirmation tasks, instructional, forced non-autograded.
        {
            string[] projTypes = new string[] { "Console App", "Class Library", "Windows Forms (WinForms) App", "WPF or UWP App" };

            double diffSet = r.NextDouble() * (double)Params.Difficulty;
            pparams.Difficulty = (decimal)Math.Min(10, diffSet);

            if (diffSet < 0.5)
                return $"Create an empty {pparams.AllowedLanguages} {projTypes[r.Next((int)(Params.Difficulty * 10))]} that builds without errors.";
            else if (diffSet < 1)
                return $"Create a new {pparams.AllowedLanguages} Console App that prints \"Hello, World!\" to the screen.";
            else if (diffSet < 2)
                return $"Create a(n) {pparams.AllowedLanguages} Console App that asks the user for their name, then greets them by name.";
            else if (diffSet < 3)
                return $"Create a \"Hello, World!\" Console App in {pparams.AllowedLanguages} that only uses one {((pparams.AllowedLanguages == "C#") ? "semicolon" : "line of logic")}.";
            else if (diffSet < 5)
                return $"Create a(n) {pparams.AllowedLanguages} Class Library that might be used for an school attendance tracking system. You should include an abstract class for all people, a teacher class, a student class, and any other classes, interfaces, or structs you think would be useful.";
            else if (diffSet < 7)
                return $"Create a(n) {pparams.AllowedLanguages} WinForms app with a button and a label. The button should say \"Click Me!\" and the label should display the number of times the button has been clicked.";
            else
                return $"Create a(n) {pparams.AllowedLanguages} graphical inventory system for a shop selling formal dinner table setting components. All prices should be configurable, a user should be able to select items and add them to a digital cart, and there should be a checkout button that provides a summary of item numbers and quanities for a shop assistant to finish the sale.";
        }

        private (string, string) P1(ProblemParameters Params)   // Every programmers favourite sandwich!
        {
            double diffSet = r.NextDouble() * (double)Params.Difficulty;
            pparams.Difficulty = (decimal)Math.Min(4, Math.Max(1, diffSet));

            if (diffSet < 2)
                return ($"Create a(n) {pparams.AllowedLanguages} Console App that will ask for a name then say hello to your name, but tells any other name to get away from your computer.", "");
            else if (diffSet < 3)
                return ($"Create a(n) {pparams.AllowedLanguages} Console App that will keep asking for a word until the string \"exit\" is entered (at which point it should end cleanly).", "");
            else if (diffSet < 4)
            {
                int sp = r.Next(short.MinValue, short.MaxValue), ep = r.Next(sp, int.MaxValue / 4);
                int sum(int start, int end) { int ans = 0; for (int i = start; i <= end; ++i) ans += i; return ans; }
                return ($"What is the sum of all integers between {sp} and {ep} inclusive? Build your program in {pparams.AllowedLanguages}.", sum(sp, ep).ToString());
            }
            else
            {
                long sp = (long)(r.NextDouble() * int.MaxValue * 2) + int.MinValue, ep = (long)(r.NextDouble() * (long.MaxValue / 2)) + sp;
                long sum(long start, long end) { long ans = 0; for (long i = start; i <= end; ++i) ans += i; return ans; }
                return ($"What is the sum of all integers between {sp} and {ep} inclusive? Build your program in {pparams.AllowedLanguages}.", sum(sp, ep).ToString());
            }
        }

        private string P2(ProblemParameters Params)   // Multi-function programs, skeletons may or may not be provided.
        {
            double diffSet = Math.Min(5, Math.Max(2, r.NextDouble() * (double)Params.Difficulty));
            pparams.Difficulty = (decimal)diffSet;

            if (diffSet < 2.5)
                return $"Write a(n) {pparams.AllowedLanguages} {(pparams.AllowedLanguages == "VB" ? "subroutine" : "function")} that will clear the console and print a message provided as a string argument.";
            else if (diffSet < 3)
                return $"Write a(n) {pparams.AllowedLanguages} function that clears the console, prints a prompt passed as an argument, and returns the users typed response.";
            else if (diffSet < 4)
                return $"Write a(n) {pparams.AllowedLanguages} class that contains functions to add, subtract, multiply, divide, and mod integers without throwing any exceptions.";
            else
            {
                if (pparams.AllowedLanguages == "F#")
                    return $"Write an F# pipeline of form (int -> int list -> int) that returns the sum of each item of the list raised to the provided int power.";
                else
                    return $"Write a collection of {pparams.AllowedLanguages} lambdas that perform logical (boolean) AND, OR, XOR, NOT, NOR, and XNOR.";
            }
        }

        private (string, string) P3(ProblemParameters Params)   // Classes, objects, typing, and casting.
        {
            double diffSet = Math.Min(9, Math.Max(3, r.NextDouble() * (double)Params.Difficulty));
            pparams.Difficulty = (decimal)diffSet;

            if (diffSet < 3.75)
            {
                double check = Math.Round(r.NextDouble() * 100, 4);
                return ($"What is {check} casted to an Integer? (Find your answer using {pparams.AllowedLanguages})", $"{(int)check}");
            }
            else if (diffSet < 5)
                return ($"Create a(n) {pparams.AllowedLanguages} class based on a real world thing with at least two public properties (one of which must be either readonly or writeonly), two private fields, one non-default constructor, and two instance methods.", $"");
            else if (diffSet < 8)
                return ($"Model a real-world system in a(n) {pparams.AllowedLanguages} class system containing at least two interfaces, two abstract classes, and six concrete classes, at least three of which must inherit from either of the abstract classes and at least one of which must implement at least two of your interfaces.", $"");
            else
                return ($"Build your own implementation of System.Collections.Generic.List<> that properly uses indexers and is backed by a normal array.", $"");
        }

        private string P4(ProblemParameters Params)   // Multi-threading, locks, background workers, check loops, spinning bars, and asynchronous functions.
        {
            double diffSet = Math.Min(6, Math.Max(4, r.NextDouble() * (double)Params.Difficulty));
            pparams.Difficulty = (decimal)diffSet;

            if (diffSet < 5)
                return $"Build a(n) {pparams.AllowedLanguages} Console App that uses multithreading to repeatedly increment a counter until the user presses the enter key, at which point the app should print the value of the counter, wait for acknowledgement, and exit.";
            else
                return $"Use a file lock to permit a PLINQ foreach extension method call to access a file in a useful way. Write your solution in {pparams.AllowedLanguages}.";
        }

        private string P5(ProblemParameters Params)   // Event handlers, UIs, callbacks, and event-loaded objects.
        {
            double diffSet = Math.Min(7.5, Math.Max(5, r.NextDouble() * (double)Params.Difficulty));
            pparams.Difficulty = (decimal)diffSet;

            if (diffSet < 6)
                return $"Build a working {pparams.AllowedLanguages} WinForms app with the proper naming conventions (i.e. FrmMain) and at least two buttons, one container, one label, and one other control.";
            else if (diffSet < 7)
                return $"Create a(n) {pparams.AllowedLanguages} WinForms app with a button and a label. The button should say \"Click Me!\" and the label should display the number of times the button has been clicked.";
            else
                return $"Create a UI-based {pparams.AllowedLanguages} function loader that can queue multiple functions (each of which is assigned to a button) for specified times in the future using timers and callbacks.";
        }

        private string P6(ProblemParameters Params)   // Recursion.
        {
            double diffSet = Math.Min(10, Math.Max(6, r.NextDouble() * (double)Params.Difficulty));
            pparams.Difficulty = (decimal)diffSet;

            if (diffSet < 8)
                return $"Build a recursive {pparams.AllowedLanguages} function that finds the nth fibonacci number, where n is an Integer parameter of the function.";
            else
                return $"Build a calculator in a(n) {pparams.AllowedLanguages} Console App that takes in a statement containing only Integers, parens/round brackets, +, -, *, /, and %. Use a recursive descent parser to allow for arbitrary complexity.";
        }

        private string P7(ProblemParameters Params)   // F#, anyone?
        {
            double diffSet = Math.Min(9, Math.Max(7, r.NextDouble() * (double)Params.Difficulty));
            pparams.Difficulty = (decimal)diffSet;

            if (diffSet < 8)
                return "Build an F# function that takes in an int list and returns the list in reverse order with each element multiplied by the average of the entire list using only one pipeline.";
            else
                return "Build an F# Console App capable of parsing a user-supplied JSON string to an F# dictionary.";
        }

        private string P8(ProblemParameters Params)   // Generators, yields, and F# lazy sequences.
        {
            pparams.Difficulty = 8;

            if (pparams.AllowedLanguages == "F#")
                return "Build an F# generator that infinitely yields the next prime number.";
            else
                return $"Build a {pparams.AllowedLanguages} enumerator class that generates prime numbers indefinitely.";
        }

        private string PP(ProblemParameters Params)             // Practical tasks of all sorts.
        {
            return $"Ask your teacher for a practical system to build in {pparams.AllowedLanguages}. It could be a shopping system, a useful tool, or something a little more relevant to you.";
        }
    }
}