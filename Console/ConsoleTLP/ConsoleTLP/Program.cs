using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTLP
{
   class Program
   {
      static void Main(string[] args)
      {
         //WorkingWithForegroundThreads();
         //WorkingWithBackgroundThreads();
         //WorkingWithTaskAndThread();
         WorkingWithParallelInvoke();

         Console.WriteLine("Main Thread: Process has finished.");         
         //Console.ReadKey();
      }

      private static void WriteHeader(string title)
      {
         Console.WriteLine(new string('-', 50));
         Console.WriteLine(title);
         Console.WriteLine(new string('-', 50));
      }

      /// <summary>
      /// Foreground Threading means all Tasks or Threads are created and executed regardless the Main Thread or Application is running or not.
      /// </summary>
      private static void WorkingWithForegroundThreads()
      {
         WriteHeader("Working with Foreground Threads");

         //Using a method reference when method doesn't have parameters.
         //Thread t1 = new Thread(DoSomeThingImportantMethodOne);
         Thread t1 = new Thread(() => DoSomeThingImportantMethodOne(1)); //Using Lamda expresion..
         Thread t2 = new Thread(() => {
            //Using a lambda expression and an anonymous method
            Console.WriteLine($"Writing from 2 first time.");
            Thread.Sleep(5000);
            Console.WriteLine($"Writing from 2 second time.");
         });       

         t1.Start();
         t2.Start();         
      }

      /// <summary>
      /// Background Threading means all Tasks or Threads are created and executed while the Main Thread or Application is running, 
      /// once this is closed or finished all Threads are cancelled.
      /// </summary>
      private static void WorkingWithBackgroundThreads()
      {
         WriteHeader("Working with Background Threads");
         //Using a method reference when method doesn't have parameters.
         //Thread t1 = new Thread(DoSomeThingImportantMethodOne);         

         Thread t1 = new Thread(() => DoSomeThingImportantMethodOne(1)); //Using Lamda expresion..
         Thread t2 = new Thread((taskNumber) => {
            //Using a lambda expression and an anonymous method
            Console.WriteLine($"Writing from 2 first time.");
            Thread.Sleep(4000);
            Console.WriteLine($"Writing from 2 second time.");
         });

         t1.IsBackground = true;
         t2.IsBackground = true;         

         t1.Start();
         t2.Start();         
      }

      /// <summary>
      /// TPL provides us the way to create Threading behavior, we can use the classes "Thread" and "Task"
      /// Thread supports the FOREGROUND and BACKGROUND Threading.
      /// Task supports only BACKGROUND Threading at least a Thread is executed in the same time.
      /// </summary>
      private static void WorkingWithTaskAndThread()
      {
         WriteHeader("Working with Tasks and Threads");

         //Using a method reference when method doesn't have parameters.
         //Task t1 = new Task(DoSomeThingImportantMethodOne);

         //Using an Action delegate and named method
         //Task t1 = new Task(new Action(DoSomeThingImportantMethodOne));         

         //Using an anonymous delegate
         Task t1 = new Task(delegate { DoSomeThingImportantMethodOne(1); });

         //Using a lambda expression and a named method
         Task t2 = new Task(() => DoSomeThingImportantMethodOne(2));

         //Using  a lambda expression and an anonymous method
         Task t3 = new Task(() => { DoSomeThingImportantMethodOne(3); });

         Thread t4 = new Thread(() => {
            //Using a lambda expression and an anonymous method
            Console.WriteLine($"Writing from 4 first time.");
            Thread.Sleep(5000);
            Console.WriteLine($"Writing from 4 second time.");
         });


         t1.Start();
         t2.Start();
         t3.Start();
         t4.Start();
      }

      /// <summary>
      /// Parallel.Invoke method manages the Threading but it is BLOCKING,
      /// It needs that all Actions finished to continue.
      /// It is similar to (Task.Wait or Task.WaitAll)
      /// </summary>
      private static void WorkingWithParallelInvoke()
      {
         WriteHeader("Working with Tasks and Threads");

         CancellationTokenSource cts = new CancellationTokenSource();
         CancellationToken ct = cts.Token;
         ParallelOptions po = new ParallelOptions { CancellationToken = ct, MaxDegreeOfParallelism = System.Environment.ProcessorCount };

         //Simple method
         Parallel.Invoke(
            new Action(() => DoSomeThingImportantMethodOne(1)),
            new Action(() => DoSomeThingImportantMethodOne(2))
            );

         //Using "ParallelOptions"
         Parallel.Invoke(po, 
            new Action(() => DoSomeThingImportantMethodOne(3)),           
            () =>
            {
               DoSomeThingImportantMethodOne(4);
               cts.Cancel();               
            },
             new Action(() => DoSomeThingImportantMethodOne(5))
            );
      }

      private static void ddd()
      {
         //Task.Wait()
         //Task.WaitAll();
         //Task.Factory.StarNew();         
         //Task.ContinueWith();
         WriteHeader("Working with Tasks and Threads");
      }

      private static void WorkingWithParallelForAndForEach()
      {
         WriteHeader("Working with Parallel For and ForEach methods");        

      }

      private static void DoSomeThingImportantMethodOne(byte taskNumber)
      {
         Console.WriteLine($"Writing from {taskNumber} first time.");
         Thread.Sleep(3000);
         Console.WriteLine($"Writing from {taskNumber} second time.");
      }
   }
}