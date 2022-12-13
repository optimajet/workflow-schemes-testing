# How to test workflow schemes

### Why testing is necessary

Automated testing simplifies the development of business logic because the behavior of workflow schemes can be verified simply by running
tests. Reliable tests ensure that scheme changes doesn't mess up existing logic. In addition to reducing development time, this lowers the
chance of bugs.

### What's in the guide

Following this guide, you'll write automated tests for the **Vacation request** workflow scheme from the Workflow
Engine [Demo](https://demo.workflowengine.io/designer) (you can learn more about the Demo in [this article](/demo-description)).
To do this, we'll create a separate project in the sample solution and use the following testing tools:

- [MSTest](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest) framework will run and process the tests.
- [Testcontainers](https://www.nuget.org/packages/Testcontainers) package will create an isolated container for the database.
- Mockups will make it easier to configure tests.

After writing the testing architecture, we'll write three data-driven tests.

See the full article [here](https://workflowengine.io/documentation/how-to-test-workflow-schemes).
