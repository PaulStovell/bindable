using System;
using Bindable.Core.Helpers;

namespace Bindable.Core.Collections
{
    /// <summary>
    /// A tuple represents an object made up of two or more components. This class provides factory methods for creating a tuple, making use of the 
    /// C# compiler's genertic type parameter inference capabilities. Tuples can safely be used as the keys for dictionaries, and tuples with 
    /// the same components can be compared for equality. Once created, a tuple is read-only.
    /// </summary>
    public static class Tuple
    {
        /// <summary>
        /// Creates a tuple for two components.
        /// </summary>
        /// <typeparam name="TA">The type of component A.</typeparam>
        /// <typeparam name="TB">The type of component B.</typeparam>
        /// <param name="a">The value of component A.</param>
        /// <param name="b">The value of component B.</param>
        /// <returns>A tuple representing the value of all components.</returns>
        public static Tuple<TA, TB> For<TA, TB>(TA a, TB b)
        {
            return new Tuple<TA, TB>(a, b);
        }

        
        /// <summary>
        /// Creates a tuple for three components.
        /// </summary>
        /// <typeparam name="TA">The type of component A.</typeparam>
        /// <typeparam name="TB">The type of component B.</typeparam>
        /// <typeparam name="TC">The type of component C.</typeparam>
        /// <param name="a">The value of component A.</param>
        /// <param name="b">The value of component B.</param>
        /// <param name="c">The value of component C.</param>
        /// <returns>A tuple representing the value of all components.</returns>
        public static Tuple<TA, TB, TC> For<TA, TB, TC>(TA a, TB b, TC c)
        {
            return new Tuple<TA, TB, TC>(a, b, c);
        }

        /// <summary>
        /// Creates a tuple for four components.
        /// </summary>
        /// <typeparam name="TA">The type of component A.</typeparam>
        /// <typeparam name="TB">The type of component B.</typeparam>
        /// <typeparam name="TC">The type of component C.</typeparam>
        /// <typeparam name="TD">The type of component D.</typeparam>
        /// <param name="a">The value of component A.</param>
        /// <param name="b">The value of component B.</param>
        /// <param name="c">The value of component C.</param>
        /// <param name="d">The value of component D.</param>
        /// <returns>A tuple representing the value of all components.</returns>
        public static Tuple<TA, TB, TC, TD> For<TA, TB, TC, TD>(TA a, TB b, TC c, TD d)
        {
            return new Tuple<TA, TB, TC, TD>(a, b, c, d);
        }

        /// <summary>
        /// Creates a tuple for five components.
        /// </summary>
        /// <typeparam name="TA">The type of component A.</typeparam>
        /// <typeparam name="TB">The type of component B.</typeparam>
        /// <typeparam name="TC">The type of component C.</typeparam>
        /// <typeparam name="TD">The type of component D.</typeparam>
        /// <typeparam name="TE">The type of component E.</typeparam>
        /// <param name="a">The value of component A.</param>
        /// <param name="b">The value of component B.</param>
        /// <param name="c">The value of component C.</param>
        /// <param name="d">The value of component D.</param>
        /// <param name="e">The value of component E.</param>
        /// <returns>A tuple representing the value of all components.</returns>
        public static Tuple<TA, TB, TC, TD, TE> For<TA, TB, TC, TD, TE>(TA a, TB b, TC c, TD d, TE e)
        {
            return new Tuple<TA, TB, TC, TD, TE>(a, b, c, d, e);
        }
    }

    /// <summary>
    /// A tuple made up of two components.
    /// </summary>
    /// <typeparam name="TComponentA">The type of the component A.</typeparam>
    /// <typeparam name="TComponentB">The type of the component B.</typeparam>
    [Serializable]
    public class Tuple<TComponentA, TComponentB> : IEquatable<Tuple<TComponentA, TComponentB>>
    {
        public Tuple(TComponentA componentA, TComponentB componentB)
        {
            ComponentA = componentA;
            ComponentB = componentB;
        }

        public TComponentA ComponentA { get; private set; }
        public TComponentB ComponentB { get; private set; }

        public bool Equals(Tuple<TComponentA, TComponentB> obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.ComponentA, ComponentA) && Equals(obj.ComponentB, ComponentB);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(Tuple<TComponentA, TComponentB>) && Equals((Tuple<TComponentA, TComponentB>)obj);
        }

        public override int GetHashCode()
        {
            return HashCodeGenerator.GenerateHashCode(ComponentA, ComponentB);
        }

        public static bool operator ==(Tuple<TComponentA, TComponentB> left, Tuple<TComponentA, TComponentB> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Tuple<TComponentA, TComponentB> left, Tuple<TComponentA, TComponentB> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return ComponentA + " : " + ComponentB;
        }
    }

    /// <summary>
    /// A tuple made up of three components.
    /// </summary>
    /// <typeparam name="TComponentA">The type of the component A.</typeparam>
    /// <typeparam name="TComponentB">The type of the component B.</typeparam>
    /// <typeparam name="TComponentC">The type of the component C.</typeparam>
    [Serializable]
    public class Tuple<TComponentA, TComponentB, TComponentC> : IEquatable<Tuple<TComponentA, TComponentB, TComponentC>>
    {
        public Tuple(TComponentA componentA, TComponentB componentB, TComponentC componentC)
        {
            ComponentA = componentA;
            ComponentB = componentB;
            ComponentC = componentC;
        }

        public TComponentA ComponentA { get; private set; }
        public TComponentB ComponentB { get; private set; }
        public TComponentC ComponentC { get; private set; }

        public bool Equals(Tuple<TComponentA, TComponentB, TComponentC> obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.ComponentA, ComponentA) && Equals(obj.ComponentB, ComponentB) && Equals(obj.ComponentC, ComponentC);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(Tuple<TComponentA, TComponentB, TComponentC>) && Equals((Tuple<TComponentA, TComponentB, TComponentC>)obj);
        }

        public override int GetHashCode()
        {
            return HashCodeGenerator.GenerateHashCode(ComponentA, ComponentB);
        }

        public static bool operator ==(Tuple<TComponentA, TComponentB, TComponentC> left, Tuple<TComponentA, TComponentB, TComponentC> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Tuple<TComponentA, TComponentB, TComponentC> left, Tuple<TComponentA, TComponentB, TComponentC> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return ComponentA + " : " + ComponentB + " : " + ComponentC;
        }
    }

    /// <summary>
    /// A tuple made up of four components.
    /// </summary>
    /// <typeparam name="TComponentA">The type of the component A.</typeparam>
    /// <typeparam name="TComponentB">The type of the component B.</typeparam>
    /// <typeparam name="TComponentC">The type of the component C.</typeparam>
    /// <typeparam name="TComponentD">The type of the component D.</typeparam>
    [Serializable]
    public class Tuple<TComponentA, TComponentB, TComponentC, TComponentD> : IEquatable<Tuple<TComponentA, TComponentB, TComponentC, TComponentD>>
    {
        public Tuple(TComponentA componentA, TComponentB componentB, TComponentC componentC, TComponentD componentD)
        {
            ComponentA = componentA;
            ComponentB = componentB;
            ComponentC = componentC;
            ComponentD = componentD;
        }

        public TComponentA ComponentA { get; private set; }
        public TComponentB ComponentB { get; private set; }
        public TComponentC ComponentC { get; private set; }
        public TComponentD ComponentD { get; private set; }

        public bool Equals(Tuple<TComponentA, TComponentB, TComponentC, TComponentD> obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.ComponentA, ComponentA) && Equals(obj.ComponentB, ComponentB) && Equals(obj.ComponentC, ComponentC) && Equals(obj.ComponentD, ComponentD);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(Tuple<TComponentA, TComponentB, TComponentC, TComponentD>) && Equals((Tuple<TComponentA, TComponentB, TComponentC, TComponentD>)obj);
        }

        public override int GetHashCode()
        {
            return HashCodeGenerator.GenerateHashCode(ComponentA, ComponentB, ComponentC, ComponentD);
        }

        public static bool operator ==(Tuple<TComponentA, TComponentB, TComponentC, TComponentD> left, Tuple<TComponentA, TComponentB, TComponentC, TComponentD> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Tuple<TComponentA, TComponentB, TComponentC, TComponentD> left, Tuple<TComponentA, TComponentB, TComponentC, TComponentD> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return ComponentA + " : " + ComponentB + " : " + ComponentC + " : " + ComponentD;
        }
    }


    /// <summary>
    /// A tuple made up of five components.
    /// </summary>
    /// <typeparam name="TComponentA">The type of the component A.</typeparam>
    /// <typeparam name="TComponentB">The type of the component B.</typeparam>
    /// <typeparam name="TComponentC">The type of the component C.</typeparam>
    /// <typeparam name="TComponentD">The type of the component D.</typeparam>
    /// <typeparam name="TComponentE">The type of the component E.</typeparam>
    [Serializable]
    public class Tuple<TComponentA, TComponentB, TComponentC, TComponentD, TComponentE> : IEquatable<Tuple<TComponentA, TComponentB, TComponentC, TComponentD, TComponentE>>
    {
        public Tuple(TComponentA componentA, TComponentB componentB, TComponentC componentC, TComponentD componentD, TComponentE componentE)
        {
            ComponentA = componentA;
            ComponentB = componentB;
            ComponentC = componentC;
            ComponentD = componentD;
            ComponentE = componentE;
        }

        public TComponentA ComponentA { get; private set; }
        public TComponentB ComponentB { get; private set; }
        public TComponentC ComponentC { get; private set; }
        public TComponentD ComponentD { get; private set; }
        public TComponentE ComponentE { get; private set; }

        public bool Equals(Tuple<TComponentA, TComponentB, TComponentC, TComponentD, TComponentE> obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.ComponentA, ComponentA) && Equals(obj.ComponentB, ComponentB) && Equals(obj.ComponentC, ComponentC) && Equals(obj.ComponentD, ComponentD) && Equals(obj.ComponentE, ComponentE);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(Tuple<TComponentA, TComponentB, TComponentC, TComponentD, TComponentE>) && Equals((Tuple<TComponentA, TComponentB, TComponentC, TComponentD, TComponentE>)obj);
        }

        public override int GetHashCode()
        {
            return HashCodeGenerator.GenerateHashCode(ComponentA, ComponentB, ComponentC, ComponentD, ComponentC);
        }

        public static bool operator ==(Tuple<TComponentA, TComponentB, TComponentC, TComponentD, TComponentE> left, Tuple<TComponentA, TComponentB, TComponentC, TComponentD, TComponentE> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Tuple<TComponentA, TComponentB, TComponentC, TComponentD, TComponentE> left, Tuple<TComponentA, TComponentB, TComponentC, TComponentD, TComponentE> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return ComponentA + " : " + ComponentB + " : " + ComponentC + " : " + ComponentD + " : " + ComponentE;
        }
    }
}
