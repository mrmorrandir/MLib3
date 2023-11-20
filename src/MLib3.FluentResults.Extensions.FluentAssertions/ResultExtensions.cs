using FluentResults;
using FluentResults.Extensions.FluentAssertions;

namespace MLib3.FluentResults.Extensions.FluentAssertions;

public static class ResultExtensions 
{
    public static AndWhichThatConstraint<ResultAssertions, Result, ErrorAssertions> MatchError(this ResultAssertions resultAssertions, string message, string because = "", params object[] becauseArgs)
    {
        return new MatchErrorAssertionOperator().Do(resultAssertions.Subject, resultAssertions, message, MessageComparisonLogics.ActualContainsExpected, because, becauseArgs);
    }
    
    public static AndWhichThatConstraint<ResultAssertions<TResult>, Result<TResult>, ErrorAssertions> MatchError<TResult>(this ResultAssertions<TResult> resultAssertions, string message, string because = "", params object[] becauseArgs)
    {
        return new MatchErrorAssertionOperator().Do(resultAssertions.Subject, resultAssertions, message, MessageComparisonLogics.ActualContainsExpected, because, becauseArgs);
    }
}