using FluentAssertions.Execution;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;

namespace MLib3.FluentResults.Extensions.FluentAssertions;

internal class MatchErrorAssertionOperator
{
    public AndWhichThatConstraint<TResultAssertion, TResult, ErrorAssertions> Do<TResultAssertion, TResult>(TResult subject, TResultAssertion parentConstraint, string expectedMessage, Func<string, string, bool> messageComparison, string because, params object[] becauseArgs)
        where TResult : ResultBase
    {
        messageComparison = messageComparison ?? FluentResultAssertionsConfig.MessageComparison;

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .Given(() => subject.Errors)
            .ForCondition(errors => errors.Any(reason => messageComparison(reason.Message, expectedMessage)))
            .FailWith("Expected result to contain error with message containing {0}, but found error '{1}'", expectedMessage, subject.Errors);

        return new AndWhichThatConstraint<TResultAssertion, TResult, ErrorAssertions>(parentConstraint, subject, new ErrorAssertions(subject.Errors.SingleOrDefault(reason => messageComparison(reason.Message, expectedMessage))));
    }
}