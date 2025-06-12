using System.Net;
using System.Net.Http.Json;
using AssetManagement.Application.DTOs.Reports;
using AssetManagement.IntegrationTests.SeedWork;
using Reqnroll;

namespace AssetManagement.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ReportStepDefinitions
    {
        private readonly AssetManagementApiFactory _factory;
        private HttpClient _client;
        private HttpResponseMessage _response;
        private List<ReportResponse> _reports;

        public ReportStepDefinitions(AssetManagementApiFactory factory)
        {
            _factory = factory;
        }
        
        [Given(@"the application is running")]
        public void GivenTheApplicationIsRunning()
        {
            _client = _factory.CreateClient();
        }
        
        [Given(@"I am authenticaed as an ""(.*)"" user with staff code ""(.*)"" and username ""(.*)""")]
        public void GivenIAmAuthenticatedAsAnUserWithStaffCodeAndUsername(string role, string staffCode, string userName)
        {
            _client = _factory.CreateAuthenticatedClient(staffCode, userName, role);
        }

        [When(@"I request the asset report sorted by ""(.*)""")]
        public async Task WhenIRequestTheAssetReportSortedBy(string orderBy)
        {
            _response = await _client.GetAsync($"/api/reports?orderBy={orderBy}");
        }
        
        [When(@"I request to export the asset report")]
        public async Task WhenIRequestToExportTheAssetReport()
        {
            _response = await _client.GetAsync("/api/reports/export");
        }

        [Then(@"the response status code should be (.*)")]
        public async Task ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            // Assert
            var expectedStatusCode = (HttpStatusCode)statusCode;
            Assert.Equal(_response.StatusCode, expectedStatusCode);
        }
        
        [Then(@"the report should contain at least (.*) categories")]
        public async Task ThenTheReportShouldContainAtLeastCategories(int minCount)
        {
            _response.EnsureSuccessStatusCode();
            _reports = await _response.Content.ReadFromJsonAsync<List<ReportResponse>>();
            
            // Assert
            Assert.NotNull(_reports);
            Assert.True(_reports.Count >= minCount);
        }

        [Then(@"the report for category ""(.*)"" should have all states:")]
        public void ThenTheReportForCategoryShouldHaveAllStates(string categoryName, Table table)
        {
            // Assert
            Assert.NotNull(_reports);
            var reportForCategory = _reports.FirstOrDefault(r => r.CategoryName == categoryName);

            Assert.NotNull(reportForCategory);
            var expectedStates = table.Rows.Select(row => row["State"]).ToList();

            foreach (var row in table.Rows)
            {
                var state = row["State"];

                switch (state)
                {
                    case "Total":
                        Assert.NotNull(reportForCategory.Total);
                        break;
                    case "Available":
                        Assert.NotNull(reportForCategory.TotalAvailable);
                        break;
                    case "Assigned":
                        Assert.NotNull(reportForCategory.TotalAssigned);
                        break;
                    case "Not_Available":
                        Assert.NotNull(reportForCategory.TotalNotAvailable);
                        break;
                    case "Waiting_For_Recycling":
                        Assert.NotNull(reportForCategory.TotalWaitingForRecycling);
                        break;
                    case "Recycled":
                        Assert.NotNull(reportForCategory.TotalRecycled);
                        break;
                    default:
                        Assert.Fail($"Unknown state '{state}' in the Gherkin table.");
                        break;
                }
            }

            if (expectedStates.Contains("Total"))
            {
                var sumOfStates = reportForCategory.TotalAvailable +
                                  reportForCategory.TotalAssigned +
                                  reportForCategory.TotalNotAvailable +
                                  reportForCategory.TotalWaitingForRecycling +
                                  reportForCategory.TotalRecycled;
                Assert.Equal(reportForCategory.Total, sumOfStates);
            }
        }

        [Then(@"the counts for each state should be non-negative")]
        public void ThenTheCountsForEachStateShouldBeNonNegative()
        {
            // Assert
            Assert.NotNull(_reports);
            foreach (var report in _reports)
            {
                Assert.True(report.Total >= 0);
                Assert.True(report.TotalAvailable >= 0);
                Assert.True(report.TotalAssigned >= 0);
                Assert.True(report.TotalNotAvailable >= 0);
                Assert.True(report.TotalWaitingForRecycling >= 0);
            }    
        }
        
        [Then(@"the categories in the report should be sorted by name in ascending order")]
        public void ThenTheCategoriesInTheReportShouldBeSortedByNameInAscendingOrder()
        {
            // Assert
            Assert.NotNull(_reports);
            var categoryNames = _reports.Select(r => r.CategoryName).ToList();
            var sortedNames = categoryNames.OrderBy(name => name).ToList();
            Assert.True(categoryNames.SequenceEqual(sortedNames), "Categories should be sorted by name in ascending order.");
        }

        [Then(@"the response content type should be ""(.*)""")]
        public void ThenTheResponseContentTypeShouldBe(string expectedContentType)
        {
            // Assert
            Assert.NotNull(_response.Content.Headers.ContentType);
            Assert.Equal(expectedContentType, _response.Content.Headers.ContentType.MediaType);
        }

        [Then(@"the response should have a file name starting with ""(.*)"" and ending with ""(.*)""")]
        public void ThenTheResponseShouldHaveAFileNameStartingWithAndEndingWith(string startsWith, string endsWith)
        {
            // Assert
            var contentDisposition = _response.Content.Headers.ContentDisposition;
            Assert.NotNull(contentDisposition);
            Assert.False(string.IsNullOrEmpty(contentDisposition.FileName));
            
            var fileName = contentDisposition.FileName.Trim('"');
            Assert.StartsWith(startsWith, fileName);
            Assert.EndsWith(endsWith, fileName);
        }
    }
}