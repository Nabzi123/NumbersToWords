using System;
using System.Collections.Generic;

namespace NumbersToWordsNS
{
    public class NumbersToWords
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the number you would like to be converted to words or vice versa or enter QUIT to finish\n");
            string getInput = Console.ReadLine();
            while (getInput != "quit")
            {
                bool isNumber = double.TryParse(getInput, out _);
                if (isNumber == true)
                {
                    if (getInput == "0")
                    {
                        Console.WriteLine("Zero only");
                    }
                    else
                    {
                        Console.WriteLine("The number in currency format is \n{0}", ConvertToWords(getInput));
                    }
                }
                else
                {
                    long output = WordsToNumbers(getInput);
                    Console.WriteLine(output);
                }
                getInput = Console.ReadLine();
            }


            Console.ReadKey();
        }
        public static long WordsToNumbers(string input)
        {
            string[] inputToLower = input.ToLower().Split(new char[] { ' ', '-', ',' });
            //Array to hold single values 1, 2, 3 etc.
            string[] ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            //Array to hold tens 10, 20, 30 etc.
            string[] tens = { "ten", "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            //Array to hold values between 10 and 20
            string[] teens = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            
            //Dictionary for multiplier values 100, 1000, 1000000, 1000000000
            Dictionary<string, long> multipliers = new Dictionary<string, long>()
            {
                {"hundred", 100},
                {"thousand", 1000},
                {"million", 1000000},
                {"billion", 1000000000}
            };

            long output = 0;
            long currentOutput = 0;
            long lastMultiplier = 1;

            foreach (string word in inputToLower)
            {
                if(multipliers.ContainsKey(word))
                {
                    lastMultiplier *= multipliers[word];
                }
                else
                {
                    long i;
                    if (lastMultiplier > 1)
                    {
                        output += currentOutput * lastMultiplier;
                        lastMultiplier = 1;
                        currentOutput = 0;
                    }

                    //check if word is in the single values. Finds the position adds 1 to get accurate position and then adds then number to output
                    if((i = Array.IndexOf(ones, word) + 1) > 0)
                    {
                        currentOutput += i;
                    }
                    //checks if word is in tens. If it is gets position then adds 1 and multiplies it by 10 for true value
                    else if((i = Array.IndexOf(tens, word) + 1) > 0)
                    {
                        currentOutput += i * 10;
                    }
                    //checks if word is between 10 and 20. Gets position adds 1 then adds 10 for true value;
                    else if((i = Array.IndexOf(teens, word) + 1 ) > 0)
                    {
                        currentOutput += i + 10;
                    }
                    else if(word != "and")
                    {
                        throw new ApplicationException("Unrecognised word: " + word);
                    }
                }

               
            }
            
            return output += currentOutput * lastMultiplier;
        }

        public static string ConvertToWords(string input)
        {
            string value = "";
            string wholeNumber = input;
            string andString = "";
            string decimalString = "";
            string endString = "Only";
            string poundString = "Pounds";
            string decimalNumbers;
            try
            {
                int decimalPoint = input.IndexOf(".");
                if (decimalPoint > 0)
                {
                    wholeNumber = input.Substring(0, decimalPoint);
                    decimalNumbers = input.Substring(decimalPoint + 1);
                    if(int.Parse(decimalNumbers) > 0)
                    {
                        andString = " and "; //Seperates whole numbers (pounds) from decimal numbers (pence)
                        endString = "Pence"; //Gets added on at the end of the string
                        decimalString = ConvertDecimals(decimalNumbers);

                    }
                    
                }
                value = String.Format("{0} {1}{2}{3} {4}", ConvertNumber(wholeNumber).Trim(), poundString, andString, decimalString, endString);
            }
            catch { }
            return value; 
        }

        private static string ConvertNumber(string input)
        {
            string word = " "; //Empty string to hold the final word
            try
            {
                bool startsWithZero = false; //Tests if string begins with a 0

                bool isDone = false; //Checks if the whole string has been translated
                double numberInput = double.Parse(input);

                if (numberInput > 0)
                {
                    startsWithZero = input.StartsWith("0");

                    int noOfDigits = input.Length; //Tests for whether or not number is in the singles, tens, hundreds etc.
                    int position = 0; //Stores groupings of digits;
                    string size = ""; //Stores name for hundred, thousand etc.
                    switch (noOfDigits)
                    {
                        //Singles
                        case 1:
                            word = Digits(input);
                            isDone = true;
                            break;

                        //Tens
                        case 2:
                            word = Tens(input);
                            isDone = true;
                            break;

                        //Hundreds
                        case 3:
                            position = (noOfDigits % 3) + 1;
                            size = " Hundred ";
                            break;

                        //Thousands
                        case 4:
                        case 5:
                        case 6:
                            position = (noOfDigits % 4) + 1;
                            size = " Thousand ";
                            break;

                        //Millions
                        case 7:
                        case 8:
                        case 9:
                            position = (noOfDigits % 7) + 1;
                            size = " Million ";
                            break;

                        default:
                            isDone = true;
                            break;


                    }

                    //Recursive function to convert whole string until it is done
                    if (!isDone)
                    {
                        //Checks if start is 0 
                        if (input.Substring(0, position) != "0" && input.Substring(position) != "0")
                        {
                            //splits string and converts number based on size 
                            word = ConvertNumber(input.Substring(0, position)) + size + ConvertNumber(input.Substring(position));
                        }
                        else
                        {
                            word = ConvertNumber(input.Substring(0, position)) + ConvertNumber(input.Substring(position));
                        }
                    }

                    if (word.Trim().Equals(size.Trim())) word = "";

                }

            }
            catch { }
            return word.Trim();
        }

        private static string Digits(string input)
        {
            int convertedInput = int.Parse(input);
            string numberAsWord = null;
            switch (convertedInput)
            {
                case 1:
                    numberAsWord = "One";
                    break;
                case 2:
                    numberAsWord = "Two";
                    break;
                case 3:
                    numberAsWord = "Three";
                    break;
                case 4:
                    numberAsWord = "Four";
                    break;
                case 5:
                    numberAsWord = "Five";
                    break;
                case 6:
                    numberAsWord = "Six";
                    break;
                case 7:
                    numberAsWord = "Seven";
                    break;
                case 8:
                    numberAsWord = "Eight";
                    break;
                case 9:
                    numberAsWord = "Nine";
                    break;

            }
            return numberAsWord;

        }

        private static string Tens(string input)
        {
            int convertedInput = int.Parse(input);
            string numberAsWord = null;
            switch (convertedInput)
            {
                case 10:
                    numberAsWord = "Ten";
                    break;

                case 11:
                    numberAsWord = "Eleven";
                    break;

                case 12:
                    numberAsWord = "Twelve";
                    break;

                case 13:
                    numberAsWord = "Thirteen";
                    break;

                case 14:
                    numberAsWord = "Fourteen";
                    break;

                case 15:
                    numberAsWord = "Fifteen";
                    break;

                case 16:
                    numberAsWord = "Sixteen";
                    break;

                case 17:
                    numberAsWord = "Seventeen";
                    break;

                case 18:
                    numberAsWord = "Eighteen";
                    break;

                case 19:
                    numberAsWord = "Nineteen";
                    break;

                case 20:
                    numberAsWord = "Twenty";
                    break;

                case 30:
                    numberAsWord = "Thirty";
                    break;

                case 40:
                    numberAsWord = "Fourty";
                    break;

                case 50:
                    numberAsWord = "Fifty";
                    break;

                case 60:
                    numberAsWord = "Sixty";
                    break;

                case 70:
                    numberAsWord = "Seventy";
                    break;

                case 80:
                    numberAsWord = "Eighty";
                    break;

                case 90:
                    numberAsWord = "Ninety";
                    break;

                default:
                    if(convertedInput > 0)
                    {
                        numberAsWord = Tens(input.Substring(0, 1) + "0") + " " + Digits(input.Substring(1)); //Splits into digits and tens
                    }
                    break;

            }
            return numberAsWord;


        }

        private static string ConvertDecimals(string input)
        {
            string word = " "; //Empty string to hold the final word
            try
            {
                bool startsWithZero = false; //Tests if string begins with a 0

                bool isDone = false; //Checks if the whole string has been translated
                double numberInput = double.Parse(input);

                if (numberInput > 0)
                {
                    startsWithZero = input.StartsWith("0");

                    int noOfDigits = input.Length; //Tests for whether or not number is in the singles, tens, hundreds etc.
                    int position = 0; //Stores groupings of digits;
                    string size = ""; //Stores name for hundred, thousand etc.
                    switch (noOfDigits)
                    {
                        //Singles
                        case 1:
                            word = Digits(input);
                            isDone = true;
                            break;

                        //Tens
                        case 2:
                            word = Tens(input);
                            isDone = true;
                            break;

                        default:
                            isDone = true;
                            break;
                    }

                    //Recursive function to convert whole string until it is done
                    if (!isDone)
                    {
                        //Checks if start is 0 
                        if (input.Substring(0, position) != "0" && input.Substring(position) != "0")
                        {
                            //splits string and converts number based on size 
                            word = ConvertNumber(input.Substring(0, position)) + size + ConvertNumber(input.Substring(position));
                        }
                        else
                        {
                            word = ConvertNumber(input.Substring(0, position)) + ConvertNumber(input.Substring(position));
                        }
                    }

                }
            }
            catch { }
            return word.Trim();
        }
    }

    
}
