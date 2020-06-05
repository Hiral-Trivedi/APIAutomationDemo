Feature: APIDemo
	To check functionalities for London weather forecast API data

Scenario: Verify the API response contains 4 days of data
	Given I have location and appid as parameters
	When I post this request to serrver
	Then the response should have 4 days of data

Scenario: Verify in the API response if all the forecast is in hourly interval
	Given I have location and appid as parameters
	When I post this request to serrver
	Then the response should have forecast in hourly interval

Scenario: Verify in the API response for all 4 days temp value is between temp_min and temp_max
	Given I have location and appid as parameters
	When I post this request to serrver
	Then in the response temp value should be in between temp_min and temp_max

Scenario: Verify in the API response description is light rain if the value for weather id is 500
	Given I have location and appid as parameters
	When I post this request to serrver
	Then description should be light rain if the value for weather id is 500

Scenario: Verify in the API response description is clear sky if the value for weather id is 800
	Given I have location and appid as parameters
	When I post this request to serrver
	Then description should be clear sky if the value for weather id is 800