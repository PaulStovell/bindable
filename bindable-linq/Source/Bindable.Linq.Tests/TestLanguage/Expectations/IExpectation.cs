using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Tests.TestLanguage.Expectations
{
    public interface IExpectation
    {
        void Validate(IScenario scenario);
    }
}
