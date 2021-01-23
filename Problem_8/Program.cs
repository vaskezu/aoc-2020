using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Problem_8
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(args[1]);
            List<Instruction> is1 = GetInstructions(lines);
            List<Instruction> is2 = GetInstructions(lines);
            int acc1 = RunProgramPart1(is1);
            int acc2 = RunProgramPart2(is2);

            Console.WriteLine($"Part 1 solution: {acc1}");
            Console.WriteLine($"Part 2 solution: {acc2}");
            Console.ReadKey();
        }

        private static int RunProgramPart1(List<Instruction> instructions)
        {
            int acc = 0;
            int position = 0;
            while (true)
            {
                var i = instructions[position];

                if (i.Executed) return acc;
                ExecuteInstruction(ref acc, ref position, i);
                i.Executed = true;
            }
        }

        private static int RunProgramPart2(List<Instruction> instructions)
        {
            int acc = 0;
            int position = 0;
            int changedInstructionPos = -1;
            List<Instruction> executedInstructions = new List<Instruction>();

            while (true)
            {
                var i = instructions[position];

                if (i.Executed)
                {
                    i = TryFixProgram(instructions, ref acc, ref position, ref changedInstructionPos, ref executedInstructions);
                }

                ExecuteInstruction(ref acc, ref position, i);

                i.Executed = true;
                executedInstructions.Add(i);

                if (position >= instructions.Count)
                    return acc;
            }
        }

        private static List<Instruction> GetInstructions(string[] lines)
        {
            return lines.Select(l =>
            {
                var sl = l.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                return new Instruction(sl[0].Trim(), int.Parse(sl[1].Trim()));
            })
            .ToList();
        }

        private static void ExecuteInstruction(ref int acc, ref int position, Instruction i)
        {
            switch (i.Command)
            {
                case "nop":
                    position++;
                    break;
                case "acc":
                    acc += i.Value;
                    position++;
                    break;
                case "jmp":
                    position += i.Value;
                    break;
                default:
                    throw new InvalidOperationException($"Not supported command {i.Command}");
            }
        }

        private static Instruction TryFixProgram(
            List<Instruction> instructions,
            ref int acc,
            ref int position,
            ref int changedInstructionPos,
            ref List<Instruction> eis)
        {
            if (changedInstructionPos != -1)
            {
                var changed = eis[changedInstructionPos];
                UndoInstructions(ref acc, ref position, ref eis, changedInstructionPos);
                InvertInstructionCommand(changed);
                var result = GetCommandToChange(eis, changedInstructionPos - 1);
                UndoInstructions(ref acc, ref position, ref eis, result.Index);
                InvertInstructionCommand(result.Ei);
                changedInstructionPos = result.Index;
            }
            else
            {
                var result = GetCommandToChange(eis, eis.Count - 1);
                UndoInstructions(ref acc, ref position, ref eis, result.Index);
                InvertInstructionCommand(result.Ei);
                changedInstructionPos = result.Index;
            }

            return instructions[position];
        }

        private static void UndoInstructions(ref int acc, ref int position, ref List<Instruction> eis, int changedInstructionPos)
        {
            for (int i = eis.Count - 1; i >= changedInstructionPos; i--)
            {
                var ei = eis[i];
                UndoInstruction(ref acc, ref position, eis, i, ei);

                ei.Executed = false;
            }
        }

        private static void UndoInstruction(ref int acc, ref int position, List<Instruction> eis, int i, Instruction ei)
        {
            switch (ei.Command)
            {
                case "nop":
                    position--;
                    eis.RemoveAt(i);
                    break;
                case "acc":
                    acc -= ei.Value;
                    position--;
                    eis.RemoveAt(i);
                    break;
                case "jmp":
                    position -= ei.Value;
                    eis.RemoveAt(i);
                    break;
                default:
                    throw new InvalidOperationException($"Not supported command {ei.Command}");
            }
        }

        private static (Instruction Ei, int Index) GetCommandToChange(List<Instruction> eis, int searchFrom)
        {
            for (int j = searchFrom; j >= 0; j--)
            {
                var ei = eis[j];

                if (ei.Command == "acc")
                    continue;

                return (ei, j);
            }

            throw new InvalidOperationException("Instruction to change not found");
        }

        private static void InvertInstructionCommand(Instruction i)
        {
            i.Command = i.Command switch
            {
                "nop" => "jmp",
                "jmp" => "nop",
                _ => throw new InvalidOperationException($"Method not supported for instruction {i.Command}"),
            };
        }

        private class Instruction
        {
            public Instruction(string command, int value)
            {
                if (string.IsNullOrEmpty(command))
                {
                    throw new ArgumentException($"'{nameof(command)}' cannot be null or empty", nameof(command));
                }

                Command = command;
                Value = value;
                Executed = false;
            }

            public string Command { get; set; }

            public int Value { get; }

            public bool Executed { get; set; }
        }
    }
}