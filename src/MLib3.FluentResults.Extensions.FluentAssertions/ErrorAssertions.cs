using FluentAssertions.Primitives;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;

namespace MLib3.FluentResults.Extensions.FluentAssertions;

public class ErrorAssertions : ReferenceTypeAssertions<IError, ErrorAssertions>
{
    public ErrorAssertions(IError subject) : base(subject) { }

    protected override string Identifier => nameof(IError);
    
    public AndWhichThatConstraint<ErrorAssertions, IError, ErrorAssertions> MatchError(string message, string because = "", params object[] becauseArgs)
    { 
        return new MatchErrorSubAssertionOperator().Do(Subject, this, message, MessageComparisonLogics.ActualContainsExpected, because, becauseArgs);
    }
}