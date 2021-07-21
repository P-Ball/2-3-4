using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Original Code by Michael Audet - Used with Permission by Sri/Trent
namespace COIS2020HLab12
{
    class Program
    {
        static void Main(string[] args)
        {
          
            Tree<int> my234Tree = new Tree<int>();

            Console.WriteLine("Welcome to the 2-3-4 Tree!");
            int choice = 0;
            while (choice != -1)
            {
                Console.WriteLine(Environment.NewLine + "Here are your options:");
                Console.WriteLine("1) Add a key.");
                Console.WriteLine("2) Remove a key (to be implemented by students in A5).");
                Console.WriteLine("3) Print the tree with an in order traversal, students should format this properly on A5");
                Console.WriteLine("4) Add a number of things, see line 58 of program.cs");
                Console.WriteLine("5) Check if tree contains a value.");
                Console.WriteLine("-1) Exit.");
                choice = ReadInt("I require an integer selection");
                switch (choice)
                {
                    case 1:
                        {
          
                            int key = ReadInt("Please enter an integer key to add to the tree.");
                            if(my234Tree.Insert(key))
                            {
                                Console.WriteLine("Key successfully added to the tree.");
                            }
                            else
                            {
                                Console.WriteLine("The insert failed.  It may have already been in the tree.");
                            }
                        }
                        break;
                    case 2:
                        {
                            Console.WriteLine("Remove a given key to be implemented by student, this didn't do anything");
                        }
                        break;
                    case 3:
                        {
                            Console.WriteLine("The tree contains the following values in order:");
                            my234Tree.Print();
                        }
                        break;
                    case 4:
                        {
                            
                            int[] testValues = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
                            foreach (int key in testValues)
                            {
                                my234Tree.Insert(key);
                            }
                            Console.WriteLine("Added a bunch of stuff");

                        }
                        break;
                    case 5:
                        {
                            int key = ReadInt("Please enter an integer key to check.");
                            if(my234Tree.Contains(key))
                            {
                                Console.WriteLine("The tree contains {0}.", key);
                            }
                            else
                            {
                                Console.WriteLine("The tree does not contain {0}.", key);
                            }
                        }
                        break;

                }//end switch
            }//end big while

        }
        //Ask the user for a integer value
        //Handle bad input, only exits oince a numeric value is received
        static int ReadInt(string prompt)
        {
            int choice = -1;
            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            while (!int.TryParse(input, out choice))
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();
            }
            return choice;
        }

    }//end program
}
