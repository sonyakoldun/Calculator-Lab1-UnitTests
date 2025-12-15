Feature: Delete customer from XYZ Bank
	As a bank manager
	I want to delete a customer from the system
	So that I can manage the customer database

@DeleteCustomer
Scenario: Bank manager deletes an existing customer
	Given I open XYZ Bank application
	When I log in as bank manager
	And I open Customers page
	And I search for customer "Harry" "Potter"
	And I delete the customer
	Then the customer should not be present in the customers list

