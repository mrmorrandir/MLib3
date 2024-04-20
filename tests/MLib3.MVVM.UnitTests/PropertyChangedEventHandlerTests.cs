using System.ComponentModel;
using MLib3.MVVM.UnitTests.Mocks;

namespace MLib3.MVVM.UnitTests;
//
// public class PropertyChangedEventHandlerTests
// {
//     [Fact]
//     public void ShouldRaiseEvent_WhenPropertyChanges()
//     {
//         // Arrange
//         var command = new DelegateCommand(_ => {}, _ => true);
//         var viewModel = new MockVM();
//         var canExecuteHitCounter = 0;
//         command.CanExecuteChanged += (s, e) => canExecuteHitCounter++;
//         
//         // Act
//         var handler = new PropertyChangedHandler(nameof(MockVM.Name), () => command.RaiseCanExecuteChanged());
//         handler.Observe(viewModel);
//         viewModel.Name = "Test";
//
//         // Assert
//         canExecuteHitCounter.Should().Be(1);
//     }
//     
//     [Fact]
//     public void ShouldRaiseEvent_WhenSubPropertyChanges()
//     {
//         // Arrange
//         var command = new DelegateCommand(_ => {}, _ => true);
//         var viewModel = new MockVM();
//         var canExecuteChangedHitCounter = 0;
//         command.CanExecuteChanged += (s, e) => canExecuteChangedHitCounter++;
//         
//         // Act
//         viewModel.Child = new SubMockVM();
//         var handler = new PropertyChangedHandler(nameof(MockVM.Child), () => command.RaiseCanExecuteChanged());
//         handler.AddChild(new PropertyChangedHandler(nameof(SubMockVM.SomeThing), () => command.RaiseCanExecuteChanged()));
//         handler.Observe(viewModel);
//         viewModel.Child.SomeThing = "Test";
//
//         // Assert
//         canExecuteChangedHitCounter.Should().Be(1);
//     }
//     
//     [Fact]
//     public void ShouldRaiseEvent_WhenBeginningWithNull()
//     {
//         // Arrange
//         var command = new DelegateCommand(_ => {}, _ => true);
//         var viewModel = new MockVM();
//         var canExecuteChangedHitCounter = 0;
//         command.CanExecuteChanged += (s, e) => canExecuteChangedHitCounter++;
//         
//         // Act
//         var handler = new PropertyChangedHandler(nameof(MockVM.Child), () => command.RaiseCanExecuteChanged());
//         handler.AddChild(new PropertyChangedHandler(nameof(SubMockVM.SomeThing), () => command.RaiseCanExecuteChanged()));
//         handler.Observe(viewModel);
//         viewModel.Child = new SubMockVM();
//         viewModel.Child.SomeThing = "Test";
//
//         // Assert
//         canExecuteChangedHitCounter.Should().Be(2);
//     }
// }
//
