﻿using System;
using System.Linq;
using System.Text;
using SpecFlow.TestProjectGenerator.NewApi.Driver;

namespace TechTalk.SpecFlow.Specs.StepDefinitions
{
    [Binding]
    public class FeatureFileSteps
    {
        private readonly ProjectDriver _projectDriver;

        public FeatureFileSteps(ProjectDriver projectDriver)
        {
            _projectDriver = projectDriver;
        }

        [Given(@"there is a feature file in the project as")]
        public void GivenThereIsAFeatureFileInTheProjectAs(string featureFileContent)
        {
            _projectDriver.AddFeatureFile(featureFileContent);
        }

        [Given(@"a scenario '(.*)' as")]
        public void GivenAScenarioSimpleScenarioAs(string title, string scenarioContent)
        {

            _projectDriver.AddFeatureFile(string.Format(@"Feature: Feature {0}
Scenario: {1}
{2}
                ", Guid.NewGuid(), title, scenarioContent));
        }

        [Given(@"there is a feature '(.*)' with (\d+) passing (\d+) failing (\d+) pending and (\d+) ignored scenarios")]
        public void GivenThereAreScenarios(string featureTitle, int passCount, int failCount, int pendingCount, int ignoredCount)
        {
            StringBuilder featureBuilder = new StringBuilder();
            featureBuilder.AppendLine("Feature: " + featureTitle);

            foreach (var scenario in Enumerable.Range(0, passCount).Select(i => string.Format("Scenario: passing scenario nr {0}\r\nWhen the step pass in " + featureTitle, i)))
            {
                featureBuilder.AppendLine(scenario);
                featureBuilder.AppendLine();
            }

            foreach (var scenario in Enumerable.Range(0, failCount).Select(i => string.Format("Scenario: failing scenario nr {0}\r\nWhen the step fail in " + featureTitle, i)))
            {
                featureBuilder.AppendLine(scenario);
                featureBuilder.AppendLine();
            }

            foreach (var scenario in Enumerable.Range(0, pendingCount).Select(i => $"Scenario: pending scenario nr {i}\r\nWhen the step is pending"))
            {
                featureBuilder.AppendLine(scenario);
                featureBuilder.AppendLine();
            }

            foreach (var scenario in Enumerable.Range(0, ignoredCount).Select(i => $"@ignore\r\nScenario: ignored scenario nr {i}\r\nWhen the step is ignored"))
            {
                featureBuilder.AppendLine(scenario);
                featureBuilder.AppendLine();
            }

            _projectDriver.AddFeatureFile(featureBuilder.ToString());

            _projectDriver.AddStepBinding(ScenarioBlock.When.ToString(), "the step pass in " + featureTitle, "//pass", "'pass");
            _projectDriver.AddStepBinding(ScenarioBlock.When.ToString(), "the step fail in " + featureTitle, "throw new System.Exception(\"simulated failure\");", "Throw New System.Exception(\"simulated failure\")");
        }
    }
}
