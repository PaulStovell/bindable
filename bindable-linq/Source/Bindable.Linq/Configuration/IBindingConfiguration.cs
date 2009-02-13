using Bindable.Linq.Dependencies.ExpressionAnalysis;
using Bindable.Linq.Dependencies.PathNavigation;

namespace Bindable.Linq.Configuration
{
    /// <summary>
    /// An interface that can be implemented by objects that control what binding options
    /// are in play.
    /// </summary>
    public interface IBindingConfiguration
    {
        /// <summary>
        /// Creates the expression analyzer.
        /// </summary>
        IExpressionAnalyzer CreateExpressionAnalyzer();

        /// <summary>
        /// Creates the path navigator.
        /// </summary>
        IPathNavigator CreatePathNavigator();
    }
}