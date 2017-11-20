/**********************************
 *MortgageCalculator.CS
 *J.Thatcher
 *Fields, Methods, Recursive Method, Iteration, Output to Console...
 **********************************/

 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            int months = 0;
            double amount = 0.00f;
            double rate = 0.00f;

            double payment = 0.0d;            
            double totalInterestPaid = 0.00d;
            
            //Gather data for calculations....No Validation...            
            Console.Write("Enter Mortgage Amount: ");
                amount = double.Parse(Console.ReadLine());
            Console.Write("Enter Term of Mortgage in Months: ");
                months = int.Parse(Console.ReadLine());
            Console.Write("Enter Rate: ");
                rate = double.Parse(Console.ReadLine())/100;

            //Assign values
            payment = CalculatePayment(rate / 12, months, amount);
            totalInterestPaid = TotalInterestPaid(amount, rate / 12, payment, months);

            //Display Mortgage Info
            Console.Write("\nLoan Amount = " + amount.ToString("C2") + " Months = " + months.ToString("N0") + " Rate: " + rate.ToString("P") + "\n\n");            
            Console.WriteLine("Monthly Payment: " + payment.ToString("C2"));
            Console.WriteLine("Total Interest Paid: " + totalInterestPaid.ToString("C2"));
            Console.WriteLine("\nLoan Balance at 60 Months(Calc): " + CalculatedMortgageValueAtNMonths(amount, payment, (rate / 12), 60).ToString("C2"));
            Console.WriteLine("Loan Balance at 60 Months(Recursive): " + RecursiveCalculatedMortgageValueAtNMonths(amount, payment, rate / 12, 60).ToString("C2"));
            Console.WriteLine("\nLoan Balance at 120 Months(Calc): " + CalculatedMortgageValueAtNMonths(amount, payment, (rate / 12), 120).ToString("C2"));
            Console.WriteLine("Loan Balance at 120 Months(Recursive): " + RecursiveCalculatedMortgageValueAtNMonths(amount, payment, rate / 12, 120).ToString("C2"));
            Console.WriteLine("\nLoan Balance at 240 Months(Calc): " + CalculatedMortgageValueAtNMonths(amount, payment, (rate / 12), 240).ToString("C2"));
            Console.WriteLine("Loan Balance at 240 Months(Recursive): " + RecursiveCalculatedMortgageValueAtNMonths(amount, payment, rate / 12, 240).ToString("C2"));
            Console.WriteLine("\n\nPress Any Key to See Mortgage Table");
            Console.ReadKey();
            DisplayMortgageTable(amount,payment,months,rate/12);
            Console.ReadKey();
        }

        private static double RecursiveCalculatedMortgageValueAtNMonths(double P, double I, double r, int N)
        {
            if (N == 0)
            {
                return P;
            }
            else
            {
                return RecursiveCalculatedMortgageValueAtNMonths(P + P * r - I, I, r, N-1); 
            }                       
            
        }
        
        private static double CalculatedMortgageValueAtNMonths(double P, double I, double r, int N)
        {
            /* A0   = Principal(P)
             * A1   = P + P * Rate(r) - Payment (I)
             *      = P (1+r) - I
             *      = A0 (1+r) - I
             * A2   = A1 (1+r) - I
             *      = (A0 (1+r) - I)(1+r) - I               //Sub A1
             *      = A0(1+r)^2 - I(1+r) - I                //Simplify
             * A3   = A2 (1+r) - I
             *      = (A0(1+r)^2 - I(1+r) - I)(1+r) - I     //Sub A2
             *      = (A0(1+r)^3 - I(1+r)^2 - I(1+r) - I    //Simplify
             *      
             *A(N)  = A0(1+r)^N - I*(1+ (1+r) + (1+r)^2 + (1+r)^3 + (1+r)^(N-1))        //Factor out I...Polynomial Pattern 1 + X + X^2....X^N-1
             * 
             *        NOTE: P(x) = 1 + X + X^2 + X^3 + ... + X^(N-1) can be expressed as X^N - 1 / X - 1; where x = (1+r)    
             * 
             *A(N) = A0(1+r)^N - I ( (1+r)^N - 1 / (1+r) - 1)       ////Sub....Simplify. . .
             */

            double valueAtNMonths = (P*Math.Pow(1+r,N)) - (I*((Math.Pow(1+r,N) - 1) / ((1+r) - 1))); //Solve for A(N)...
            return valueAtNMonths;       
        }

        private static double CalculatePayment(double r, int N, double P)
        {
            double result = 0.0d;

            //Using --> r = interest rate; N = number of monthly payments (Term); P = principal
            // r*P*(1+r)^N / (1+r)^N - 1

            result = (r * P * Math.Pow(1 + r, N)) / (Math.Pow(1 + r, N) - 1);
            return result;
        }

        private static double TotalInterestPaid(double P, double r, double I, int N)
        {
            double calc = ((P * r) - I)*((Math.Pow(1 + r, N) - 1) / (r)) + I * N;
            return calc;
        }

        private static void DisplayMortgageTable(double P, double I, int N, double r)
        {

            Console.Clear();                        
            Console.WriteLine(String.Format("{0,-80}{1}", "Mortgage Table", DateTime.Today.ToShortDateString()));
            Console.WriteLine(String.Format("{0,-80}{1}", "***************", "**********\n\n"));
            Console.WriteLine("Loan Amount: " + P.ToString("C2"));
            Console.WriteLine("Total Interest Paid: " + TotalInterestPaid(P,r,I,N).ToString("C2"));
            Console.WriteLine("Monthly Payment: " + CalculatePayment(r,N,P).ToString("C2"));
            Console.WriteLine("\n\n");
            Console.WriteLine(String.Format("{0,-80}{1}", "***************", "**********"));

            double Pi = P;

            Console.WriteLine("\n\n" + String.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}", "Payment#","Payment","Interest","Principal","Balance"));
            Console.WriteLine(String.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}", "**********", "**********", "**********", "**********", "**********"));

            //Simple loop to write mortgage table values

            for (int i = 1; i < N+1; i++)
            {
                double interestPaid = Pi * r;
                double principalPaid = I - interestPaid;
                Pi = Pi + Pi * r - I;
                Console.WriteLine(String.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}", i.ToString("N0"), I.ToString("C2"), interestPaid.ToString("C2"), principalPaid.ToString("C2"), Pi.ToString("C2")));                  
            }

            Console.WriteLine("\n\n*********************** Mortgage Table ***********************\n\n");
            Console.WriteLine(String.Format("{0,-100}", DateTime.Today.ToShortDateString()));
            Console.WriteLine("Loan Amount: " + P.ToString("C2"));
            Console.WriteLine("Total Interest Paid: " + TotalInterestPaid(P, r, I, N).ToString("C2"));
            Console.WriteLine("Monthly Payment: " + CalculatePayment(r, N, P).ToString("C2"));
        }

    }
}
