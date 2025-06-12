Feature: Asset Reports
    As an Admin
    I want to view asset report about number of assets by category and state
    So that i can make decision for purchasing asset
    
Background:
    Given the application is running
    And I am authenticaed as an "Admin" user with staff code "SD0003" and username "baoh"

Scenario: Successfully retrieve asset reports sorted by category name
    When I request the asset report sorted by "categorynameasc"
    Then the response status code should be 200
    And the report should contain at least 2 categories
    And the report for category "Access Point" should have all states:
      | State                 |
      | Total                 |
      | Available             |
      | Assigned              |
      | Not_Available         |
      | Waiting_For_Recycling |
      | Recycled              |
    And the report for category "Air Conditioner" should have all states:
      | State                 |
      | Total                 |
      | Available             |
      | Assigned              |
      | Not_Available         |
      | Waiting_For_Recycling |
      | Recycled              |
    And the counts for each state should be non-negative
    And the categories in the report should be sorted by name in ascending order
    
Scenario: Export asset report to Excel
    When I request to export the asset report
    Then the response status code should be 200
    And the response content type should be "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    And the response should have a file name starting with "AssetManagement_Report_" and ending with ".xlsx"