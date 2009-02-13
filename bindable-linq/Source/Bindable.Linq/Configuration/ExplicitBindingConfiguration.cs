using Bindable.Linq.Dependencies.ExpressionAnalysis;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Dependencies.PathNavigation.TokenFactories;

namespace Bindable.Linq.Configuration
{
    /// <summary>
    /// A binding configuration used when the runtime should not automatically detect dependencies.
    /// </summary>
    internal sealed class ExplicitBindingConfiguration : IBindingConfiguration
    {
        private readonly IExpressionAnalyzer _expressionAnalyzer;
        private readonly IPathNavigator _pathNavigator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplicitBindingConfiguration"/> class.
        /// </summary>
        public ExplicitBindingConfiguration()
        {
            _expressionAnalyzer = new ExpressionAnalyzer();
            _pathNavigator = new PathNavigator(new WpfMemberTokenFactory(), new SilverlightMemberTokenFactory(), new WindowsFormsMemberTokenFactory(), new ClrMemberTokenFactory());
        }

        /// <summary>
        /// Creates the expression analyzer.
        /// </summary>
        /// <returns></returns>
        public IExpressionAnalyzer CreateExpressionAnalyzer()
        {
            return _expressionAnalyzer;
        }

        /// <summary>
        /// Creates the path navigator.
        /// </summary>
        /// <returns></returns>
        public IPathNavigator CreatePathNavigator()
        {
            return _pathNavigator;
        }
    }
}