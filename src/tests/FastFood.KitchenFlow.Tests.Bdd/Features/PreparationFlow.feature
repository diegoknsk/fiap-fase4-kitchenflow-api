Feature: Preparation Flow
	As a kitchen staff
	I want to manage the preparation flow from payment confirmation to finished preparation
	So that I can track and process orders efficiently

	Scenario: Kitchen receives payment confirmation and completes preparation successfully
		Given I have a valid order with ID "123e4567-e89b-12d3-a456-426614174000"
		And the order snapshot contains valid order data
		When I create a preparation for this order
		Then the preparation should be created with status "Received"
		And the preparation should have the correct order ID
		When I start the preparation
		Then the preparation should have status "InProgress"
		When I finish the preparation
		Then the preparation should have status "Finished"
		And a delivery should be created for this preparation
