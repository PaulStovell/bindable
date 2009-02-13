using Bindable.Linq.Dependencies.ExpressionAnalysis;
using Bindable.Linq.Dependencies.ExpressionAnalysis.Extractors;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Dependencies.PathNavigation.TokenFactories;

namespace Bindable.Linq.Configuration
{    
    /// <summary>
    /// The default binding configuration to use.
    /// </summary>
    internal sealed class DefaultBindingConfiguration : IBindingConfiguration
    {
        private readonly IExpressionAnalyzer _expressionAnalyzer;
        private readonly IPathNavigator _pathNavigator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBindingConfiguration"/> class.
        /// </summary>
        public DefaultBindingConfiguration()
        {
            _expressionAnalyzer = new ExpressionAnalyzer(new ItemDependencyExtractor(), new ExternalDependencyExtractor(), new StaticDependencyExtractor());
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