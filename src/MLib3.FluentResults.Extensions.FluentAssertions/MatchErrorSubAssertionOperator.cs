using FluentAssertions.Execution;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;

namespace MLib3.FluentResults.Extensions.FluentAssertions;

internal class MatchErrorSubAssertionOperator
{
    public AndWhichThatConstraint<TError, TResult, ErrorAssertions> Do<TError, TResult>(TResult subject, TError parentConstraint, string expectedMessage, Func<string, string, bool> messageComparison, string because, params object[] becauseArgs)
        where TResult : IError
    {
        messageComparison = messageComparison ?? FluentResultAssertionsConfig.MessageComparison;

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .Given(() => subject.Reasons)
            .ForCondition(errors => errors.Any(reason => messageComparison(reason.Message, expectedMessage)))
            .FailWith("Expected error to contain error with message containing {0}, but found error '{1}'", expectedMessage, subject.Reasons);

        return new AndWhichThatConstraint<TError, TResult, ErrorAssertions>(parentConstraint, subject, new ErrorAssertions(subject.Reasons.SingleOrDefault(reason => messageComparison(reason.Message, expectedMessage))));
    }
}