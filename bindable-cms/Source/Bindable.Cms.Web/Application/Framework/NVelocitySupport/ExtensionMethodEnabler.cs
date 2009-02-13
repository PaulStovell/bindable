using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bindable.Cms.Web.Application.Helpers;
using NVelocity;
using NVelocity.Exception;

namespace Bindable.Cms.Web.Application.Framework.NVelocitySupport
{
    /// <summary>
    /// Adds the ability for NVelocity to call extension methods by implementing NVelocity's IDuck interface
    /// and making a choice between which instance or extension methods to call.
    /// </summary>
    public class ExtensionMethodEnabler : IDuck
    {
        private readonly object _this;
        private readonly DynamicDispatchMethod[] _extensionMethods;
        private readonly DynamicDispatchMethod[] _thisMethods;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionMethodEnabler"/> class.
        /// </summary>
        /// <param name="this">The @this.</param>
        /// <param name="extensionMethods">The extension methods.</param>
        public ExtensionMethodEnabler(object @this, DynamicDispatchMethod[] extensionMethods)
        {
            _this = @this;
            _extensionMethods = extensionMethods;
            _thisMethods = @this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance).Select(method => new DynamicDispatchMethod(method)).ToArray();
        }

        /// <summary>
        /// Gets the value of a property. Not supported yet.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <returns>The value of a property.</returns>
        public object GetInvoke(string propName)
        {
            return null;
        }

        /// <summary>
        /// Sets the value of a property. Not supported yet.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public void SetInvoke(string propName, object value)
        {
        }

        /// <summary>
        /// Invokes the specified method. This class will first look for instance methods matching the signature, and then if no 
        /// candidates are found, it will try extension methods.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public object Invoke(string method, params object[] args)
        {
            // Do we have an instance method we can call?
            var instanceArgumentTypes = ReflectionHelper.GetTypesFromArguments(args);
            var instanceMethodCandidate = FindBestCandidate(_thisMethods, method, "instance", instanceArgumentTypes);
            if (instanceMethodCandidate != null)
            {
                return instanceMethodCandidate.Call(_this, args);
            }

            // Try exension methods instead
            var extensionMethodArguments = new object[args.Length + 1];
            extensionMethodArguments[0] = _this;
            for (var i = 0; i < args.Length; i++)
            {
                extensionMethodArguments[i + 1] = args[i];
            }

            var extensionMethodArgumentTypes = ReflectionHelper.GetTypesFromArguments(extensionMethodArguments);
            var extensionMethodCandidate = FindBestCandidate(_extensionMethods, method, "extension", extensionMethodArgumentTypes);
            if (extensionMethodCandidate != null)
            {
                return extensionMethodCandidate.Call(null, extensionMethodArguments);
            }

            // No close matches
            var message = new StringBuilder();
            message.AppendFormat("No instance or extension methods matching the signature for method '{0}' and the provided arguments were found. ", method).AppendLine();
            message.AppendLine("The call required a signature of: ");
            message.AppendLine(BuildCallSignatureForErrors(method, instanceArgumentTypes));
            throw new MethodInvocationException(message.ToString(), null, method);
        }

        private static DynamicDispatchMethod FindBestCandidate(IEnumerable<DynamicDispatchMethod> methods, string methodName, string methodOption, Type[] arguments)
        {
            // Each dynamic dispatch method has a "MatchAndRank" method that returns a score indicating how close a match it is
            // We'll filter any with -1 rankings, since they are no match at all, and then select the highest match. If there are 
            // "ties" for the top position, we throw an error.
            var candidateGroups = (from m in methods
                                   let ranking = m.MatchAndRank(methodName, arguments)
                                   where ranking > 0
                                   orderby ranking descending
                                   select new { m, ranking }).GroupBy(r => r.ranking, r => r.m).ToList().OrderBy(group => group.Key);
            var firstGroup = candidateGroups.FirstOrDefault();
            if (firstGroup == null) return null;
            if (firstGroup.Count() == 0) return null;
            if (firstGroup.Count() > 1)
            {
                var message = new StringBuilder();
                message.AppendFormat("Ambiguous method call: multiple {0} methods matching the signature for method '{1}' and the provided arguments were found. ", methodOption, methodName).AppendLine();
                message.AppendLine("The call required a signature matching: ");
                message.AppendLine(BuildCallSignatureForErrors(methodName, arguments));
                message.AppendLine("The following close matches were found, causing the ambiguity: ");
                foreach (var candidate in firstGroup)
                {
                    message.AppendFormat("- {0}", candidate).AppendLine();
                }
                throw new MethodInvocationException(message.ToString(), null, methodName);
            }
            return firstGroup.First();
        }

        private static string BuildCallSignatureForErrors(string methodName, Type[] arguments)
        {
            var result = new StringBuilder();
            result.AppendFormat("{0}(", methodName);
            for (var i = 0; i < arguments.Length; i++)
            {
                result.Append(ReflectionHelper.DescribeType(arguments[i]));
                if (i < arguments.Length - 1)
                {
                    result.Append(", ");
                }
            }
            result.AppendLine(")");
            return result.ToString();
        }
    }
}