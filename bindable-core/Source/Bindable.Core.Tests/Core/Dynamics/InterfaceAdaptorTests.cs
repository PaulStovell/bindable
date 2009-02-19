using System;
using MbUnit.Framework;
using System.ComponentModel;
using Bindable.Core.Dynamics;

namespace Bindable.Core.Tests.Core.Dynamics
{
    public interface ICrappy
    {
        void DoSomething();
        void DoSomething(int a);
        void DoSomething(string a);
        void DoSomething(object a);
        void DoSomething(int a, object b);
        void DoSomething(int a, object b, out int c);
        void DoSomething(int a, object b, out string c);
        void DoSomething(int a, object b, int[] ints);
        void DoSomething(int a, object b, params string[] strings);

        string DoSomethingReturning();
        string DoSomethingReturning(int a);
        string DoSomethingReturning(string a);
        string DoSomethingReturning(object a);
        string DoSomethingReturning(int a, object b);
        string DoSomethingReturning(int a, object b, out int c);
        string DoSomethingReturning(int a, object b, out string c);
        string DoSomethingReturning(int a, object b, int[] ints);
        string DoSomethingReturning(int a, object b, params string[] strings);

        event PropertyChangedEventHandler PropertyChanged;

        void TriggerEvent();

        string Name { get; set; }
        DateTime Created { get; }
    }

    public class Unrelated : ICrappy
    {
        public void DoSomething() { Console.WriteLine("DoSomething"); }
        public void DoSomething(int a) { Console.WriteLine(a); }
        public void DoSomething(string a) { Console.WriteLine(a); }
        public void DoSomething(object a) { Console.WriteLine(a); }
        public void DoSomething(int a, object b) { Console.WriteLine(a.ToString() + b); }
        public void DoSomething(int a, object b, out int c)
        {
            Console.WriteLine(a.ToString() + b);
            c = 18;
        }

        public void DoSomething(int a, object b, out string c)
        {
            Console.WriteLine(a.ToString() + b);
            c = "Hello";
        }

        public void DoSomething(int a, object b, int[] ints)
        {
            Console.WriteLine(a.ToString() + b + ints.Length);
        }

        public void DoSomething(int a, object b, params string[] strings)
        {
            Console.WriteLine(a.ToString() + b + strings.Length);
        }

        public string DoSomethingReturning()
        {
            Console.WriteLine("DoSomethingReturning");
            return "Hello";
        }

        public string DoSomethingReturning(int a)
        {
            Console.WriteLine(a);
            return a.ToString();
        }

        public string DoSomethingReturning(string a)
        {
            Console.WriteLine(a);
            return a;
        }

        public string DoSomethingReturning(object a)
        {
            Console.WriteLine(a);
            return a.ToString();
        }

        public string DoSomethingReturning(int a, object b)
        {
            Console.WriteLine(a.ToString() + b);
            return a.ToString();
        }

        public string DoSomethingReturning(int a, object b, out int c)
        {
            Console.WriteLine(a.ToString() + b);
            c = 19;
            return c.ToString();
        }

        public string DoSomethingReturning(int a, object b, out string c)
        {
            Console.WriteLine(a.ToString() + b);
            c = "Hello";
            return c.ToString();
        }

        public string DoSomethingReturning(int a, object b, int[] ints)
        {
            Console.WriteLine(a.ToString() + b + ints.Length);
            return ints.Length.ToString();
        }

        public string DoSomethingReturning(int a, object b, params string[] strings)
        {
            Console.WriteLine(a.ToString() + b + strings.Length);
            return b.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void TriggerEvent()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Hello"));
            }
        }

        public string Name { get; set;}

        public DateTime Created
        {
            get { return DateTime.Now; }
        }
    }

    [TestFixture]
    public class InterfaceAdaptorTests
    {
        [Test]
        public void Test()
        {
            var original = new Unrelated();
            var adapted = Adaptor.Adapt<ICrappy>(original);

            var eventCount = 0;
            var handler = new PropertyChangedEventHandler((s, e) => eventCount++);
            adapted.PropertyChanged += handler;
            Assert.AreEqual(0, eventCount);
            adapted.TriggerEvent();
            Assert.AreEqual(1, eventCount);
            adapted.PropertyChanged -= handler;
            adapted.TriggerEvent();
            Assert.AreEqual(1, eventCount);
        }
    }
}