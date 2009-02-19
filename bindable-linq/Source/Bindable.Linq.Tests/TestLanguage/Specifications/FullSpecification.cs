using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using Bindable.Linq.Tests.TestLanguage.Steps;

namespace Bindable.Linq.Tests.TestLanguage.Specifications
{
    /// <summary>
    /// A specification in which both the input and output types are known.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    internal class FullSpecification<TInput>
    {
        private readonly string _title;
        private readonly List<Scenario<TInput>> _scenarios = new List<Scenario<TInput>>();
        private Func<IEnumerable<TInput>, object> _bindableLinqQueryCreator;
        private Func<IEnumerable<TInput>, object> _standardLinqQueryCreator;
        private CompatabilityLevel _compatabilityLevel = CompatabilityLevel.FullyCompatible;

        /// <summary>
        /// Initializes a new instance of the <see cref="FullSpecification&lt;TInput&gt;"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public FullSpecification(string title)
        {
            _title = title;
        }

        /// <summary>
        /// Specifies the Bindable LINQ query, resulting in a specification with known input and output types.
        /// </summary>
        /// <param name="queryCreator">The query creator.</param>
        /// <returns></returns>
        public FullSpecification<TInput> UsingBindableLinq(Func<IEnumerable<TInput>, object> queryCreator)
        {
            _bindableLinqQueryCreator = queryCreator;
            return this;
        }

        /// <summary>
        /// Specifices the standard LINQ to Objects query that the results will be compared with.
        /// </summary>
        /// <param name="queryCreator">The query creator.</param>
        /// <returns></returns>
        public FullSpecification<TInput> UsingStandardLinq(Func<IEnumerable<TInput>, object> queryCreator)
        {
            _standardLinqQueryCreator = queryCreator;
            return this;
        }

        /// <summary>
        /// Sets the expected level of compatability between the Bindable LINQ query and the standard LINQ query.
        /// </summary>
        /// <param name="level">The level.</param>
        public FullSpecification<TInput> WithCompatabilityLevel(CompatabilityLevel level)
        {
            _compatabilityLevel = level;
            return this;
        }

        /// <summary>
        /// Defines a scenario for this specification.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="inputs">The inputs.</param>
        /// <param name="steps">The steps to perform.</param>
        /// <returns></returns>
        public FullSpecification<TInput> Scenario(string title, ObservableCollection<TInput> inputs, params Func<object, Step>[] steps)
        {
            var scenario = new Scenario<TInput>(
                title,
                inputs,
                steps.Select(step => step(null)),
                _bindableLinqQueryCreator(inputs),
                _standardLinqQueryCreator(inputs));
            scenario.CompatabilityLevel = _compatabilityLevel;
            _scenarios.Add(scenario);
            return this;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Verify()
        {
            Tracer.WriteLine("Verifying specification: {0}", _title);
            using (Tracer.Indent())
            {
                foreach (var scenario in _scenarios)
                {
                    scenario.Execute();
                }
            }
        }
    }
}
