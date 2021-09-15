using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuringMachine
{
    class TuringMachine
    {
        public void WriteIntoTape(string word)
        {
            for(int i=0;i<word.Length;i++)
            {
                tape[i + 500000] = word[i];
            }
            for(int i=0;i<tape.Length;i++)
            {
                if(tape[i].ToString()==" " || tape[i]=='\0')
                {
                    tape[i] = '#';
                }
            }
            
            SizeOfWord = word.Length;
        }
        public int head = 500000;
        public char[] tape = new char[1000000];

        public List<transition> trans = new List<transition>();
        public List<state> States = new List<state>();
        public state initial = new state();
        public state finale = new state();
        public int SizeOfWord;
        public TuringMachine(List<transition> trans,List<state> States,state Initial,state finale)
        {
            this.States = States;
            this.trans = trans;
            this.initial = Initial;
            this.finale = finale;
        }

        public bool peymayesh(state currentState)
        {
            for(int i=0;i<currentState.Trans.Count;i++)
            {
                if(currentState.Trans[i].x==tape[head].ToString())
                {
                    tape[head] = currentState.Trans[i].y[0];
                    if(currentState.Trans[i].move =="R")
                    {
                        head++;
                    }
                    else
                    {
                        head--;
                    }
                    return peymayesh(currentState.Trans[i].final);
                    //if not deterministic ....

                }
            }

            if(currentState == finale)
            {
                return true;
            }
            else
            {
                //Console.WriteLine(finale.name+"   "+currentState.name + "   " + head + "   " + tape[head]);
                return false;
            }
        }
    }

    class transition
    {
        public transition(state initial,state finale,string x,string y,string move)
        {
            this.initial = initial;
            this.final = finale;
            this.x = x;
            this.y = y;
            this.move = move;
        }
        public state initial;
        public state final;
        public string x;
        public string y;
        public string move;
    }
    class state
    {
        public string name;
        public List<transition> Trans = new List<transition>();
    }
    class Program
    {
        public static string alphabet = "#abcdefghijklmnopqrstuvwxyz";
        public static List<state> states = new List<state>();
        public static List<transition> trans = new List<transition>();
        static void Main(string[] args)
        {
            string TuringMachineStr = Console.ReadLine();
            TuringMachineStr = TuringMachineStr.Replace("00", "-");
            string[] rules = TuringMachineStr.Split('-');
            
            for(int i=0;i<rules.Length;i++)
            {
                string[] Transition = rules[i].Split('0');
                int NumberOfStateIni = Transition[0].Length;

                bool IsAlreadyExists = false;
                int indexOfSiniState = -1;
                for(int j=0;j<states.Count;j++)
                {
                    if(states[j].name == "q" + NumberOfStateIni.ToString())
                    {
                        IsAlreadyExists = true;
                        indexOfSiniState = j;
                    }
                }
                if(!IsAlreadyExists)
                {
                    state initialState = new state();
                    initialState.name = "q" + NumberOfStateIni.ToString();
                    int size = states.Count;
                    states.Add(initialState);
                    indexOfSiniState = size;
                }

                string x = alphabet[Transition[1].Length - 1].ToString();

                int NumberOfStateFinale = Transition[2].Length;
                bool IsAlreadyExistsFinale = false;
                int indexOfFinaleState = -1;
                for (int j = 0; j < states.Count; j++)
                {
                    if (states[j].name == "q" + NumberOfStateFinale.ToString())
                    {
                        IsAlreadyExistsFinale = true;
                        indexOfFinaleState = j;
                    }
                }
                if (!IsAlreadyExistsFinale)
                {
                    state FinalState = new state();
                    FinalState.name = "q" + NumberOfStateFinale.ToString();
                    int size = states.Count;
                    states.Add(FinalState);
                    indexOfFinaleState = size;
                }

                string y = alphabet[Transition[3].Length - 1].ToString();

                string move = "";
                if (Transition[4] == "1")
                {
                    move = "L";
                }
                else
                {
                    move = "R";
                }

                transition newTrnas = new transition(states[indexOfSiniState],states[indexOfFinaleState],x,y,move);
                trans.Add(newTrnas);
                states[indexOfSiniState].Trans.Add(newTrnas);
            }

            state initialS = new state();

            for(int i=0;i<states.Count;i++)
            {
                if(states[i].name =="q1")
                {
                    initialS = states[i];
                }
            }

            state FinaleS = new state();
            for(int i=0;i<states.Count;i++)
            {
                if(states[i].name=="q"+states.Count)
                {
                    FinaleS = states[i];
                }
            }

            //Console.WriteLine(FinaleS.name);
            //for(int i=0;i<states.Count;i++)
            //{
            //    Console.WriteLine(states[i].name);
            //}
            //for(int i=0;i<trans.Count;i++)
            //{
            //    Console.WriteLine(trans[i].initial.name + " " + trans[i].final.name + " " + trans[i].x + " " + trans[i].y + " " + trans[i].move);
            //}
            

            int numberOfWords = int.Parse(Console.ReadLine());
            string[] answers = new string[numberOfWords];
            for(int i=0;i<numberOfWords;i++)
            {
                TuringMachine tm = new TuringMachine(trans, states, initialS, FinaleS);
                string word = Console.ReadLine();
                string DecodedWord = WordConverter(word);
                tm.WriteIntoTape(DecodedWord);
                if(tm.peymayesh(initialS))
                {
                    answers[i] = "Accepted";
                }
                else
                {
                    answers[i] = "Rejected";
                }
            }
            for(int i=0;i<numberOfWords;i++)
            {
                Console.WriteLine(answers[i]);
            }
            Console.ReadKey();
        }


        public static string WordConverter(string CodedWord)
        {
            string decodedWord = "";
            string[] alphabets = CodedWord.Split('0');
            if(alphabets.Length==1 && alphabets[0].Length==0)
            {
                return decodedWord;
            }
            else
            {
                for (int i = 0; i < alphabets.Length; i++)
                {
                    decodedWord = decodedWord + alphabet[alphabets[i].Length - 1].ToString();
                }
                return decodedWord;
            }
        }
    }
}
